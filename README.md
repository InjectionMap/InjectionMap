![wickedflame injectionmap](assets/wickedflame injectionmap - black.png)

InjectionMap
--------------------------------
InjectionMap is a very small and lightweight IoC/DI container for .NET.
InjectionMap allows loose coupling betweeen interfaces and implementations.
Compared to other containers that are lightweight, this one really is.

- InjectionMap uses type mapping to reference the key/reference and the implementation. 
- Instances are resolved using reflection or can be provided through a callback whitch allows you to create the instance in your own code.
- It suports a fluent syntax to help keep the code simple, small and clean.
- Constructors can be marked with attributes.
- Parameters for constructors can be injected or passed when mapping.
- InjectionMap is very simple and straightforward.

Install
--------------------------------
InjectionMap can be installed from [NuGet](http://docs.nuget.org/docs/start-here/installing-nuget) from the package manager console:

    PM > Install-Package InjectionMap

Created and developed by [wickedflame productions](http://wicked-flame.blogspot.ch/)  
InjectionMap is Copyright &copy; 2014 [wickedflame](http://wicked-flame.blogspot.ch/) and other contributors under the [Ms-RL License](License.txt).