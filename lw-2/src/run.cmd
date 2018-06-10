@echo off
start /d Frontend dotnet Frontend.dll
start /d Backend dotnet Backend.dll
start "" /wait "http://localhost:5001/"