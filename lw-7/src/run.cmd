@echo off
start "Frontend" /d Frontend dotnet Frontend.dll
start "Backend" /d Backend dotnet Backend.dll
start "TextListener" /d TextListener dotnet TextListener.dll
start "TextRankCalc" /d TextRankCalc dotnet TextRankCalc.dll

set file=config\components.json
for /f "tokens=1,2" %%i in (%file%) do (@echo %%i %%j
for /l %%n in (1, 1, %%j) do start "%%i" /d "%%i" dotnet %%i.dll
)


start "" /wait "http://localhost:5001/"