![wickedflame injectionmap](assets/wickedflame injectionmap - black.png)

InjectionMap
------------------------------
InjectionMap is a very small and extremely lightweight IoC/DI container for .NET. 
InjectionMap allows loose coupling betweeen a client's dependencies and its own behaviour. InjectionMap promotes reusability, testability and maintainability of any part of an application.

- InjectionMap uses type mapping to reference the key/reference and the implementation. 
- Instances are resolved using reflection or can be provided through a callback whitch allows you to create the instance in your own code.
- It suports a fluent syntax to help keep the code simple, small and clean.
- The desired Constructors can be marked with attributes or will be selected according to the passed arguments.
- Parameters for constructors can be injected or passed at the time of mapping as objects or as delegate expressions.
- InjectionMap is very simple and straightforward.

InjectionMap supports .Net 4.5, Silverlight 5, Windows Phone 8 or higher and Windows Store apps for Windows 8 or higher.

Installation
------------------------------
InjectionMap can be installed from [NuGet](http://docs.nuget.org/docs/start-here/installing-nuget) through the package manager console:  

    PM > Install-Package InjectionMap

Bugs, issues and features
------------------------------
Bugs, issues or feature wishes can submitted on the [issues](https://github.com/InjectionMap/InjectionMap/issues) page or feel free to fork the project and send a pull request.


InjectionMap is developed by [wickedflame](http://wicked-flame.blogspot.ch/) under the [Ms-RL License](License.txt).