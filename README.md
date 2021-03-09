# DSP_Mods
Debug Dyson Sphere Program Mods

1. DSP_ROOT is your folder Steam\steamapps\common\Dyson Sphere Program
1. Check Unity version of the game you have.
    See Detail of DSP_ROOT\DSPGame.exe.
1. download correct version of unity https://unity3d.com/get-unity/download/archive.
    In this case, we need download 2018.4.12. Choose Unity Editor 64-bit, the download will start automatically.
1. After download done, use 7zip unpack file.
    In this case, the filename is UnitySetup64-2018.4.12f1.exe.
1. Locate to [UnpackFolder]\Editor\Data\PlaybackEngines\windowsstandalonesupport\Variations\win64_development_mono\
    Copy UnityPlayer.dll to DSP_ROOT
1. Use text editor open DSP_ROOT\DSPGAME_Data\boot.config
    Add:
    ```
    wait-for-managed-debugger=1
    player-connection-debug=1
    ```
1. Ensure you installed https://dsp.thunderstore.io/package/xiaoye97/BepInEx/
1. Rebuild mod, and place .dll, .pdb and .mdb to DSP_ROOT\BepInEx\plugins\DSP_Helmod. To obtain .mdb file execute
    ```
    DSP_Helmod\Build\pdb2mdb.exe DSP_Helmod.dll
    ```
1. Go to Visual Studio 2019, click menu Debug/Attach Unity Debugger
A "Select Unity instance" dialog will show you some thing like..

| Project       | Machine      | Type   | Port  | Information |
| ------------- | ------------ | ------ | ----- | ----------- |
| WindowsPlayer | Your_PC_Name | Player | 56593 | PID:xxxx    |
