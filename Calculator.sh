#!/usr/bin/env bash
# Author : Clovis JANICOT-TIXIER
# Copyright (c) Calculator 2019
#
clear
echo -e "Hello. To run my program, follow the following instructions.\n"
folderPath=bin/Release/netcoreapp3.0
cd $folderPath
echo "First, choose your platforms at first position and the others platforms separated by commas:"
echo -e "alpine-x64        debian-x64       fedora.24-x64    gentoo-x64          linuxmint.19-x64    opensuse.13.2-x64  rhel.6-x64      sles-x64          ubuntu.15.04-arm    ubuntu.17.10-x64    win-arm64        win7-arm-aot    win81-arm
alpine.3.10-x64   debian.10-arm    fedora.25-arm64  linux-arm           linuxmint.19.1-x64  opensuse.15.0-x64  rhel.7-x64      sles.12-x64       ubuntu.15.04-x64    ubuntu.18.04-arm    win-arm64-aot    win7-arm64      win81-arm-aot
alpine.3.11-x64   debian.10-arm64  fedora.25-x64    linux-arm64         linuxmint.19.2-x64  opensuse.15.1-x64  rhel.7.0-x64    sles.12.1-x64     ubuntu.15.10-arm    ubuntu.18.04-arm64  win-x64          win7-arm64-aot  win81-arm64
alpine.3.6-x64    debian.10-x64    fedora.26-arm64  linux-musl-arm      ol-x64              opensuse.42.1-x64  rhel.7.1-x64    sles.12.2-x64     ubuntu.15.10-x64    ubuntu.18.04-x64    win-x64-aot      win7-x64        win81-arm64-aot
alpine.3.7-x64    debian.8-arm     fedora.26-x64    linux-musl-arm64    ol.7-x64            opensuse.42.2-x64  rhel.7.2-x64    sles.12.3-x64     ubuntu.16.04-arm    ubuntu.18.10-arm    win-x86          win7-x64-aot    win81-x64
alpine.3.8-x64    debian.8-arm64   fedora.27-arm64  linux-musl-x64      ol.7.0-x64          opensuse.42.3-x64  rhel.7.3-x64    sles.12.4-x64     ubuntu.16.04-arm64  ubuntu.18.10-arm64  win-x86-aot      win7-x86        win81-x64-aot
alpine.3.9-x64    debian.8-x64     fedora.27-x64    linux-x64           ol.7.1-x64          osx-x64            rhel.7.4-x64    sles.15-x64       ubuntu.16.04-x64    ubuntu.18.10-x64    win10-arm        win7-x86-aot    win81-x86
android-arm       debian.9-arm     fedora.28-arm64  linuxmint.17-x64    ol.7.2-x64          osx.10.10-x64      rhel.7.5-x64    sles.15.1-x64     ubuntu.16.10-arm    ubuntu.19.04-arm    win10-arm-aot    win8-arm        win81-x86-aot
android-arm64     debian.9-arm64   fedora.28-x64    linuxmint.17.1-x64  ol.7.3-x64          osx.10.11-x64      rhel.7.6-x64    ubuntu-arm        ubuntu.16.10-arm64  ubuntu.19.04-arm64  win10-arm64      win8-arm-aot
android.21-arm    debian.9-x64     fedora.29-arm64  linuxmint.17.2-x64  ol.7.4-x64          osx.10.12-x64      rhel.8-arm64    ubuntu-arm64      ubuntu.16.10-x64    ubuntu.19.04-x64    win10-arm64-aot  win8-arm64
android.21-arm64  fedora-arm64     fedora.29-x64    linuxmint.17.3-x64  ol.7.5-x64          osx.10.13-x64      rhel.8-x64      ubuntu-x64        ubuntu.17.04-arm    ubuntu.19.10-arm    win10-x64        win8-arm64-aot
centos-x64        fedora-x64       fedora.30-arm64  linuxmint.18-x64    ol.7.6-x64          osx.10.14-x64      rhel.8.0-arm64  ubuntu.14.04-arm  ubuntu.17.04-arm64  ubuntu.19.10-arm64  win10-x64-aot    win8-x64
centos.7-x64      fedora.23-arm64  fedora.30-x64    linuxmint.18.1-x64  ol.8-x64            osx.10.15-x64      rhel.8.0-x64    ubuntu.14.04-x64  ubuntu.17.04-x64    ubuntu.19.10-x64    win10-x86        win8-x64-aot
debian-arm        fedora.23-x64    fedora.31-arm64  linuxmint.18.2-x64  ol.8.0-x64          rhel-arm64         rhel.8.1-arm64  ubuntu.14.10-arm  ubuntu.17.10-arm    win-arm             win10-x86-aot    win8-x86
debian-arm64      fedora.24-arm64  fedora.31-x64    linuxmint.18.3-x64  opensuse-x64        rhel-x64           rhel.8.1-x64    ubuntu.14.10-x64  ubuntu.17.10-arm64  win-arm-aot         win7-arm         win8-x86-aot\n"
read platformsList
echo -e "\n"
tarFile=OthersPlatforms.tar.gz
if [ -f $tarFile ] ; then
	echo -e "UnCompressing...\n"
	tar xzf $tarFile -C ../
	rm $tarFile
fi
echo -e "Compressing..."
IFS=','
read -ra folderToExcludeArray <<< $platformsList
temporaryFile=tmp.txt
echo $temporaryFile >> $temporaryFile
echo $tarFile >> $temporaryFile
for folderToExclude in ${folderToExcludeArray[@]}; do 
	echo $folderToExclude >> $temporaryFile
done
IFS=' '
tar czf $tarFile -X $temporaryFile ../
rm $temporaryFile
echo -e "\nDeleting others folders...\n"
IFS=$'\n'
for compressedPlateform in $(tar -tf $tarFile); do
	if [[ $compressedPlateform != "netcoreapp3.0/" ]] && [[ $compressedPlateform != *publish* ]] ; then 
		rm -r $(basename $compressedPlateform)
	fi
done
IFS=' '
cd "../../.."
echo -e "\nThen, have you the last .NET Core SDK installed (Y/N)?"
read questionSDK
if [ $questionSDK == "y" ] ||  [ $questionSDK == "Y" ] ; then
	echo -e "\n"
	for platform in ${folderToExcludeArray[@]}; do
		dotnet publish -r $platform -c Release
	done
	rm -r obj
	rm -r bin/Debug
else
	echo -e "\nDo you want to install it (Y/N)?"
	read installSDK
	if [ $installSDK == "y" ] || [ $installSDK == "Y" ] ; then
		echo -e "\nInstalling from https://dotnet.microsoft.com/download/dotnet-core thanks to Microsoft...\n"
		if [[ ${folderToExcludeArray[0]} == win* ]] ; then
			curl -sSL https://dot.net/v1/dotnet-install.sh | bash powershell | powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -Channel Current"
		else
			curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel Current
		fi
	fi
fi
for folder in ${folderToExcludeArray[@]}; do
	cd $folderPath/$folder
	for file in $folderPath/$folder/*; do
		rm $(basename $file)
	done
done
cd publish
clear
./Calculator