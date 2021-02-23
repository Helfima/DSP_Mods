SETLOCAL ENABLEEXTENSIONS
SETLOCAL ENABLEDELAYEDEXPANSION

rem *********************************
REM -- Recup de l'emplacement du fichier
SET cdir=%~dp0
SET cdir=%cdir:~0,-1%

SET DSP_DIR=C:\Steam\steamapps\common\Dyson Sphere Program
SET PLUGIN_DIR=BepInEx\plugins\DSP_Helmod
SET TARGET_DIR="%DSP_DIR%/%PLUGIN_DIR%"
IF NOT EXIST %TARGET_DIR% mkdir %TARGET_DIR%
xcopy "%1" "%DSP_DIR%/%PLUGIN_DIR%" /Y

xcopy "%2icon.png" "%DSP_DIR%/%PLUGIN_DIR%" /Y
xcopy "%2manifest.json" "%DSP_DIR%/%PLUGIN_DIR%" /Y
xcopy "%2README.md" "%DSP_DIR%/%PLUGIN_DIR%" /Y
xcopy "%2changelog.txt" "%DSP_DIR%/%PLUGIN_DIR%" /Y