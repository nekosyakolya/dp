@echo off
start "Frontend" /d Frontend dotnet Frontend.dll
start "Backend" /d Backend dotnet Backend.dll
start "TextListener" /d TextListener dotnet TextListener.dll
start "TextRankCalc" /d TextRankCalc dotnet TextRankCalc.dll


start "" /wait "http://localhost:5001/"