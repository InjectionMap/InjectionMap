![wickedflame injectionmap](assets/wickedflame injectionmap - black.png)

# InjectionMap
------------------------------
InjectionMap is a very small and extremely lightweight IoC/DI container for .NET. 
InjectionMap allows loose coupling betweeen a client's dependencies and its own behaviour. InjectionMap promotes reusability, testability and maintainability of any part of an application.

- InjectionMap is a very lightweith IoC Framework that leaves no traces in the client code
- InjectionMap uses type mapping to reference the key/reference and the implementation. 
- Instances are resolved using reflection or can be provided through a callback whitch allows the creation of instances in your own code.
- It suports a fluent syntax to help keep the code simple, small and clean.
- Parameters for constructors can be injected or passed at the time of mapping as objects or as delegate expressions.
- InjectionMap is very simple and straightforward.
- Allows mapping to a custom MappinContainer. This can help to prevent the ServiceLocator anti-pattern

InjectionMap supports .Net 4.5, Silverlight 5, Windows Phone 8 or higher and Windows Store apps for Windows 8 or higher.

## Installation
------------------------------
InjectionMap can be installed from [NuGet](http://docs.nuget.org/docs/start-here/installing-nuget) through the package manager console:  

    PM > Install-Package InjectionMap

# Examples
------------------------------
There are different ways to register types/objects in InjectionMap.
The best practice is to create all mappings in one place and only once per AppDomain. The best place to do this is the application startup. In an WPF application it would be the App.xaml.cs Startup and in a ASP.NET application it would be the Global.asax file. To achieve this, you have to create a class that implements _IInjectionMapping_.
### Register Mappings using the IMapInitializer interface
IInjectionMapping only provides one Method that has to be implemented:  
```csharp
void InitializeMap(IMappingProvider container)
```
The IMappingProvider object containes two methods to register mappings:
```csharp
Map<TSvc>()
Map<TSvc,TImpl>()
```
And one method to clean existing mappings:
```csharp
Clean<T>()
```
#### Implementation
This example demonstrates the simplest form of mapping. An interface and a class with a default constructor that implements the interface. The mapping doesn't need any special cases.

```csharp
// Interface and class that will be registerd in InjectionMap
interface IInjectionMappingTest { }
class InjectionMappingTestMock : IInjectionMappingTest { }

// IInjectionMapping implementation that is used 
// to register objects/mappings in InjectionMap
class InjectionMapperMock : IMapInitializer
{
    public void InitializeMap(IMappingProvider container)
    {
        // create mapping
        container.Map<IInjectionMappingTest, InjectionMappingTestMock>();
    }
}
```
#### Initialization
In the application startup you only call _InjectionMap.InitializeMap(...)_ with the Assembly that contains _IInjectionMapping_ implementations.
All classes that implement _IMapInitializer_ will automaticaly be created and called after _InjectionMap.InitializeMap()_ gets executed. 
##### Map to the default MappingContainer
```csharp
InjectionMap.InitializeMap(Assembly assembly);
// resolve the mappings
using (var resolver = new InjectionResolver())
{
    var map = resolver.Resolve<IInjectionMappingTest>();
}
```

##### Map to a custom MappingContainer
```csharp
var container = new MappingContainer();
InjectionMap.InitializeMap(Assembly assembly, container);
// resolve the mappings
using (var resolver = new InjectionResolver(container))
{
    var map = resolver.Resolve<IInjectionMappingTest>();
}
```

### Register Mappings using InjectionMapper
Besides registering mappings using the _IInjectionMapping_ interface, mappings can be registered using a instance of _InjectionMapper_.
#### Simple mapping
```csharp
using (var mapper = new InjectionMapper())
{
    // register InjectionMappingTest to the interface IInjectionMappingTest
    mapper.Map<IInjectionMappingTest, InjectionMappingTest>();
}
```
#### Mapping with generic extensions
Alternatively mappings can be registered with _IMappingExpression&lt;TSvc&gt;.For&lt;TImpl&gt;();_
```csharp
using (var mapper = new InjectionMapper())
{
    // create mapping
    mapper.Map<IInjectionMappingTest>.For<InjectionMappingTestMock>();
}
```
#### Mapping with expression extensions
Mappings can be created to a predefined instance of an object using _IMappingExpression&lt;TSvc&gt;.For&lt;TImpl&gt;(Func&lt;TImpl&gt; predicate);_
```csharp
using (var mapper = new InjectionMapper())
{
    // create mapping to a specific instance
    mapper.Map<IInjectionMappingTest>.For(() => new InjectionMappingTestMock());
}
```
### Resolving values
The registered mappings can be resolved form InjectioMap using _InjectionResolver_. 
```csharp
using (var resolver = new InjectionResolver())
{
    // resolve the registered mapping from InjectionMap
    var map = resolver.Resolve<IInjectionMappingTest>();
}
```
This will resolve the last map that was registered to the key type from InjectionMap. To resolve all mappings that have been registered to the key type _InjectionResolver_ provides the Method _ResolveMultiple&lt;TKey&gt;()_
```csharp
using (var resolver = new InjectionResolver())
{
    // resolve all mappings that were registered with the key type from InjectionMap
    var map = resolver.ResolveMultiple<IInjectionMappingTest>();
}
```

### Resolving the constructor
1.  Find the Constructor defined in the Mapping or the Resolver  
2. Find the contructor marked with the _InjectionConstructorAttribute_.  
3. Try to resolve the constructor that matches the passed arguments.  
4. Try to resolve the default constructor.  
5. Try to resolve any constructor using the passed arguments and by resolving mappings.  

#### Select the Constructor when Mapping
Select the constructor.
```csharp
Mapper.Map<IConstuctorTest, ClassWithThreeConstuctors>()
	 .ForConstructor(cc => cc[2]) // select constructor nr. 3
	 .WithArgument(() => 2);
```
Select the constructor and set Argument values.
```csharp
Mapper.Map<IMixedConstuctor, MixedWitDefaultConstuctor>()
	.ForConstructor(cc =>
	{
		var constr = cc[2]; // select constructor nr. 3
		constr["StringParam"].Value = "String value" // select parameter by name
		constr[1].Value = 2; // select parameter by index
		return constr;
	});
```

#### Select the Constructor when Resolving
Select constructor.
```csharp
Resolver.For<IMixedConstuctor>()
	  .ForConstructor(cc => cc[2]) // select constructor nr. 3
	  .Resolve();
Select the constructor and set Argument values.
```csharp
Resolver.For<IMixedConstuctor>()
	.ForConstructor(cc =>

	{
		var constr = cc[2]; // select constructor nr. 3
		constr["StringParam"].Value = "String value" // select parameter by name
		constr[1].Value = 2; // select parameter by index
		return constr;
	})
	.Resolve();
```

#### Select the Constructor with Attribute (InjectionConstructorAttribute)
In classes with multiple constructor, the constructor that has to be used can be marked with the _InjectionConsturctoAttribute_.
The parameters can be past as arguments or can be resolved from mappings.

```csharp
public class ConstructorInjectionMock : IConstructorInjectionMock
{
    public ConstructorInjectionMock() { }
    
    [InjectionConstructor]
    public ConstructorInjectionMock(IConstructorParameter parameter) { }
}
```

#### Passing Arguments to the mapping that will be injected into the constructor
Parameters that have to be used can be passed as Arguments to the mapping.
```csharp
// class that needs a parameter in the constructor
public class ConstructorInjectionMock : IConstructorInjectionMock
{
    public ConstructorInjectionMock(int parameter) { }
}
```

Add the argument while registering
```csharp
var mapper = new InjectionMapper();
mapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>()
      .WithArgument(() => 5);
```
Optionaly the argument can be passed to a named parameter
```csharp
var mapper = new InjectionMapper();
mapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>()
      .WithArgument("parameter", () => 5);
```

#### Inject arguments by resolving registered maps
If parameters are mapped objects, they will automatically be resolved 
```csharp
public class ConstructorParameter : IConstructorParameter { }

public class ConstructorInjectionMock : IConstructorInjectionMock
{
    public ConstructorInjectionMock(IConstructorParameter parameter) { }
}
```

```csharp
var mapper = new InjectionMapper();
mapper.Map<IConstructorParameter, ConstructorParameter>();
mapper.Map<IConstructorInjectionMock, ConstructorInjectionMock>();

var resolver = new InjectionResolver();
var obj = resolver.Resolve<IConstructorInjectionMock>();
```
The parameter IConstructorParameter will automaticaly be resolved and passed to the constructor.

#### Pass Arguments to Resolver
The Resolver exposes the Methods _ExtendMap&lt;T&gt;()_ and _For&lt;T&gt;()_ which return a _IResolverExpression&lt;T&gt;_ which provides Methods to extend the map with Arguments. 
While ExtendMap extends existing mappings, For will create and extend a copy of the mapping. Mappings extended with For will be lost if _Resolve()_ is not called.

```csharp
using (var resolver = new InjectionResolver())
{
    // Extend a map
    var value1 = resolver.ExtendMap<IMapKey>().WithArgument<int>(() => 1).Resolve();
	// Copy and extend a map
    var value2 = resolver.For<IMapKey>().WithArgument<int>(() => 1).Resolve();
}
```

### Resolve unmappped types and Inject arguments
If a type has a constructor with parameters of a type that previously were mapped to InjectionMap, the type can be created with the parameters resolved from InjectionMap without first having to register it.
```csharp
// the key type for the argument to be passed
public interface ITypeArgument { }

// the type that should be resolved to the constructor
public class TypeArgument : ITypeArgument { }

// the type that has a argument that has to be resolved from InjectionMap
public class UnmappedType
{
   public UnmappedType(ITypeArgument argument) { }
}
```
Register the argument type to InjectionMap
```csharp
using (var mapper = new InjectionMapper())
{
   mapper.Map<ITypeArgument, TypeArgument>();
}
```
Pass the type that has to be resolved to the resolver. InjectionMap will resolve the object, resolve the argument and inject these to the parameter even though the type was not registered previously.
```csharp
using (var resolver = new InjectionResolver())
{
   var map = resolver.Resolve<UnmappedType>();
}
```
There is no need to create a new instance of UnmappedType or to register it to the InjectionMap. The Parameters will automatically be resolved and injected.


## Bugs, issues and features
------------------------------
Bugs, issues or feature wishes can submitted on the [issues](https://github.com/InjectionMap/InjectionMap/issues) page or feel free to fork the project and send a pull request.


InjectionMap is developed by [wickedflame](http://wicked-flame.blogspot.ch/) under the [Ms-PL License](License.txt).