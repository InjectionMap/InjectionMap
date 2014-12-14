@echo off
echo Begin create nuget for InjectionMap

NuGet.exe pack ..\src\InjectionMap.nuspec

echo End create nuget for InjectionMap
pause