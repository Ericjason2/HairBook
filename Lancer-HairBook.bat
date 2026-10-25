@echo off
cd /d "%~dp0"
echo ==============================================
echo   Nettoyage des anciens processus HairBook...
echo ==============================================
powershell -Command "Get-Process -Name '*HairBook*' -ErrorAction SilentlyContinue | Stop-Process -Force" >nul 2>&1
timeout /t 1 /nobreak >nul

echo ==============================================
echo   Lancement de HairBook Mobile (MAUI 10)
echo ==============================================
start "" "bin\Debug\net10.0-windows10.0.19041.0\win-x64\HairBookRun.exe"
