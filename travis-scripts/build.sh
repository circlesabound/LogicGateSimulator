#! /bin/sh

# Example build script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# Change this the name of your project. This will be the name of the final executables as well.
project="LogicDoodler"

echo "Building and running editor unit tests"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/UnitTests.log \
  -projectPath $(pwd) \
  -runEditorTests \
  -editorTestsResultFile $(pwd)/UnitTests.xml \
  -quit

unitTests=$?
echo 'Logs from editor unit tests'
cat $(pwd)/UnitTests.xml
if [ $unitTests -ne 0 ]
then
  echo 'Failed unit tests, stopping build'
  exit $unitTests
fi

echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/WindowsBuild.log \
  -projectPath $(pwd) \
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" \
  -quit

echo "Attempting to build $project for Mac OS"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/MacBuild.log \
  -projectPath $(pwd) \
  -buildOSXUniversalPlayer "$(pwd)/Build/mac/$project.app" \
  -quit

echo "Attempting to build $project for Linux"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/LinuxBuild.log \
  -projectPath $(pwd) \
  -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project.exe" \
  -quit

echo 'Logs from Windows build'
cat $(pwd)/WindowsBuild.log

echo 'Logs from Mac OS build'
cat $(pwd)/MacBuild.log

echo 'Logs from Linux build'
cat $(pwd)/LinuxBuild.log

echo ''
echo '###############################'
echo ''

if grep -Fq "Completed 'Build.Player.WindowsStandaloneSupport'" $(pwd)/WindowsBuild.log
then
  echo 'Windows build completed successfully'
  windows=0
else
  echo 'Windows build failed'
  windows=1
fi

if grep -Fq "Completed 'Build.Player.MacStandaloneSupport'" $(pwd)/MacBuild.log
then
  echo 'Mac OS build completed successfully'
  mac=0
else
  echo 'Mac OS build failed'
  mac=1
fi

if grep -Fq "Completed 'Build.Player.LinuxStandaloneSupport'" $(pwd)/LinuxBuild.log
then
  echo 'Linux build completed successfully'
  linux=0
else
  echo 'Linux build failed'
  linux=1
fi

exit `expr $windows + $mac + $linux`
