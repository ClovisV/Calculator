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
echo "Then, have you .NET Core 3 SDK installed (Y/N)?"
read answer
if [ $answer == "y" ] || [ $answer == "Y" ] ; then
	dotnet publish -r $platform -c Release
	rm -r obj
	rm -r bin/Debug
else
	echo "Do you want to install it (Y/N)?"
	read answerTwo
	if [ $answerTwo == "y" ] || [ $answerTwo == "Y" ] ; then
		echo "Installation from https://dotnet.microsoft.com/download/dotnet-core thanks to Microsoft..."
		if [[ $platform == win* ]] ; then
			curl -sSL https://dot.net/v1/dotnet-install.sh | bash powershell | powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -Channel Current"
		else
			curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel Current
		fi
	fi
fi

cd $folderPath/$platform
for file in $platform/*; do rm $(basename $file); done;
cd publish
clear
./Calculator