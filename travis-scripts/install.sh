#! /bin/sh

# Example install script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

echo 'Downloading main installer from http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorInstaller/Unity-2017.1.1f1.pkg: '
export ec=56 # connection reset by peer happens occasionally
while [ $ec -eq 56 ]
do
    curl -o Unity.pkg -C - 'http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorInstaller/Unity-2017.1.1f1.pkg'
    export ec=$?
done

echo 'Downloading Windows module from http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-2017.1.1f1.pkg: '
export ec=56 # connection reset by peer happens occasionally
while [ $ec -eq 56 ]
do
    curl -o UnityWindowsModule.pkg -C - 'http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-2017.1.1f1.pkg'
    export ec=$?
done

echo 'Downloading OSX module from http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorTargetInstaller/UnitySetup-Mac-Support-for-Editor-2017.1.1f1.pkg: '
export ec=56 # connection reset by peer happens occasionally
while [ $ec -eq 56 ]
do
    curl -o UnityMacModule.pkg -C - 'http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorTargetInstaller/UnitySetup-Mac-Support-for-Editor-2017.1.1f1.pkg'
    export ec=$?
done


echo 'Downloading Linux module from http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorTargetInstaller/UnitySetup-Linux-Support-for-Editor-2017.1.1f1.pkg: '
export ec=56 # connection reset by peer happens occasionally
while [ $ec -eq 56 ]
do
    curl -o UnityLinuxModule.pkg -C - 'http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorTargetInstaller/UnitySetup-Linux-Support-for-Editor-2017.1.1f1.pkg'
    export ec=$?
done


echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /

echo 'Installing UnityWindowsModule.pkg'
sudo installer -dumplog -package UnityWindowsModule.pkg -target /

echo 'Installing UnityMacModule.pkg'
sudo installer -dumplog -package UnityMacModule.pkg -target /

echo 'Installing UnityLinuxModule.pkg'
sudo installer -dumplog -package UnityLinuxModule.pkg -target /
