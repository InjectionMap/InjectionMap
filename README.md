![wickedflame injectionmap](assets/wickedflame injectionmap - black.png)

### InjectionMap
InjectionMap is a very small and extremely lightweight IoC/DI container for .NET.  
InjectionMap allows loose coupling betweeen a client's dependencies and its own behaviour. InjectionMap promotes reusability, testability and maintainability of any part of an application.


- InjectionMap uses type mapping to reference the key/reference and the implementation. 
- Instances are resolved using reflection or can be provided through a callback whitch allows you to create the instance in your own code.
- It suports a fluent syntax to help keep the code simple, small and clean.
- Injection constructors can be marked with attributes.
- Parameters for constructors can be injected or passed at the time of mapping or as expressions.
- InjectionMap is very simple and straightforward.

### Install
InjectionMap can be installed from [NuGet](http://docs.nuget.org/docs/start-here/installing-nuget) with the package manager console:

PM > Install-Package InjectionMap

### Bugs, issues and features
If you find bugs, issues or have feature wishes, you can place them on the [issues](https://github.com/InjectionMap/InjectionMap/issues) page.  

  
InjectionMap is Copyright &copy; 2014 [wickedflame](http://wicked-flame.blogspot.ch/) under the [Ms-RL License](License.txt).