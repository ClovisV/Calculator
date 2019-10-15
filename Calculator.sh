#!/usr/bin/env bash
# Author : Clovis JANICOT-TIXIER
# Copyright (c) Calculator 2019
#

clear
echo "Hello. To run my program, follow the following instructions."
echo " "
folderPath=bin/Release/netcoreapp3.0
cd $folderPath
echo "First, choose your platform:"
ls
echo " "
read platform
echo " "
cd "../../.."
echo "Then, have you .NET Core 3 installed (Y/N)?"
read answer
if [ $answer == "n" ] || [ $answer == "N" ] ; then
	"Installation from https://dotnet.microsoft.com/download/dotnet-core thanks to Microsoft..."
	./dotnet-install.sh -c Current
fi
dotnet publish -r $platform -c Release
rm -r obj
rm -r bin/Debug
cd $folderPath/$platform
for file in $platform/*; do rm $(basename $file); done;
cd publish
clear
./Calculator