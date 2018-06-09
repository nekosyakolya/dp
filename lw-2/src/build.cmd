@echo off
set /a EXIT_SUCCESS=0
set /a EXIT_FAIL=1

if "%~1" == "" goto noArgument

start /wait /d Frontend dotnet publish -c Release
if %ERRORLEVEL% NEQ 0 (
    goto buildError
)

start /wait /d Backend dotnet publish -c Release
if %ERRORLEVEL% NEQ 0 (
    goto buildError
)

md "..\%~1"\Frontend
md "..\%~1"\Backend
md "..\%~1"\config

start /wait xcopy "Frontend\bin\release\netcoreapp2.0\publish" "..\%~1"\"Frontend"
start /wait xcopy "Backend\bin\release\netcoreapp2.0\publish" "..\%~1"\"Backend"

start /wait xcopy config "..\%~1"\config

start /wait xcopy run.cmd "..\%~1"

start /wait xcopy stop.cmd "..\%~1"

echo "Project built"
exit /b EXIT_SUCCESS

:noArgument
echo "Argument is empty"
exit /b EXIT_FAIL

:buildError
echo "Build is failed"
exit /b EXIT_FAIL