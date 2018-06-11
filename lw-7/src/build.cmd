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

start /wait /d TextListener dotnet publish -c Release
if %ERRORLEVEL% NEQ 0 (
    goto buildError
)

start /wait /d TextRankCalc dotnet publish -c Release
if %ERRORLEVEL% NEQ 0 (
    goto buildError
)
start /wait /d VowelConsCounter dotnet publish -c Release
if %ERRORLEVEL% NEQ 0 (
    goto buildError
)

start /wait /d VowelConsRater dotnet publish -c Release
if %ERRORLEVEL% NEQ 0 (
    goto buildError
)
md "..\%~1"\Frontend
md "..\%~1"\Backend
md "..\%~1"\config
md "..\%~1"\TextListener
md "..\%~1"\TextRankCalc
md "..\%~1"\VowelConsCounter
md "..\%~1"\VowelConsRater

xcopy "Frontend\bin\release\netcoreapp2.0\publish" "..\%~1"\"Frontend"
xcopy "Backend\bin\release\netcoreapp2.0\publish" "..\%~1"\"Backend"
xcopy "TextListener\bin\release\netcoreapp2.0\publish" "..\%~1"\"TextListener"
xcopy "TextRankCalc\bin\release\netcoreapp2.0\publish" "..\%~1"\"TextRankCalc"
xcopy "VowelConsCounter\bin\release\netcoreapp2.0\publish" "..\%~1"\"VowelConsCounter"
xcopy "VowelConsRater\bin\release\netcoreapp2.0\publish" "..\%~1"\"VowelConsRater"

xcopy config "..\%~1"\config

xcopy run.cmd "..\%~1"

xcopy stop.cmd "..\%~1"

echo "Project built"
exit /b EXIT_SUCCESS

:noArgument
echo "Argument is empty"
exit /b EXIT_FAIL

:buildError
echo "Build is failed"
exit /b EXIT_FAIL