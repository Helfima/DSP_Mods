SETLOCAL ENABLEEXTENSIONS
SETLOCAL ENABLEDELAYEDEXPANSION

rem *********************************
REM -- Recup de l'emplacement du fichier
SET cdir=%~dp0
SET cdir=%cdir:~0,-1%

SET MODE=%1
SET PROJECT_DIR=%2
SET PROJECT_DIR=%PROJECT_DIR:~0,-1%
SET PATH_DLL=%3
SET PATH_MDB=%3.mdb
SET FILENAME_MDB=%~nx3.mdb

SET DSP_DIR=C:\Steam\steamapps\common\Dyson Sphere Program
SET PLUGIN_DIR=BepInEx\plugins\DSP_Helmod
SET TARGET_DIR=%DSP_DIR%\%PLUGIN_DIR%
IF NOT EXIST "%TARGET_DIR%" mkdir "%TARGET_DIR%"

xcopy "%PATH_DLL%" "%TARGET_DIR%" /Y

xcopy "%PROJECT_DIR%\icon.png" "%TARGET_DIR%" /Y
xcopy "%PROJECT_DIR%\manifest.json" "%TARGET_DIR%" /Y
xcopy "%PROJECT_DIR%\README.md" "%TARGET_DIR%" /Y
xcopy "%PROJECT_DIR%\changelog.txt" "%TARGET_DIR%" /Y

IF "%MODE%" == "Debug" (
	xcopy "%PATH_MDB%" "%TARGET_DIR%" /Y
	xcopy "%PROJECT_DIR%\Build\Debug\UnityPlayer.dll" "%DSP_DIR%" /Y
	xcopy "%PROJECT_DIR%\Build\Debug\DSPGAME_Data\boot.config" "%DSP_DIR%\DSPGAME_Data" /Y
)
IF "%MODE%" NEQ "Debug" (
	del /Q "%TARGET_DIR%\%FILENAME_MDB%"
	xcopy "%PROJECT_DIR%\Build\Release\UnityPlayer.dll" "%DSP_DIR%" /Y
	xcopy "%PROJECT_DIR%\Build\Release\DSPGAME_Data\boot.config" "%DSP_DIR%\DSPGAME_Data" /Y
)