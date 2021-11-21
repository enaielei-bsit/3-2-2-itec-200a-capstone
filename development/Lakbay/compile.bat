@echo off
set "projectPath=%cd%"
cd "C:\Program Files\Unity\Hub\Editor\2020.3.21f1\Editor"
echo [COMPILER]: Compiling...
unity.exe -projectPath %projectPath% -buildTarget Android -batchmode -quit -nographics -executeMethod Utilities.Compiler.Compile
cls
echo [COMPILER]: Successfully compiled!
cd %projectPath%
timeout 1