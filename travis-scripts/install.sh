#! /bin/sh

# Example install script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

echo 'Downloading from http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorInstaller/Unity-2017.1.1f1.pkg: '
export ec=56 # connection reset by peer happens occasionally
while [ $ec -eq 56 ]
do
    curl -O Unity.pkg -C - 'http://netstorage.unity3d.com/unity/5d30cf096e79/MacEditorInstaller/Unity-2017.1.1f1.pkg'
    export ec=$?
done

echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /
