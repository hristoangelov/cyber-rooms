# Cyber Rooms
Final Year Project for COMP30040 - [Co-supervised with industry] Augmented or virtual reality in security awareness
## Overview 
This project is a virtual reality experience built with Unity and C# aimed at teaching users about the basics of security awareness. It has been tested on the Oculus Quest 2 VR headset as well as locally on a laptop.
## Features
- Immersive VR environment
- Interactive learning scenarios
- Gamified elements to engage users
- Real-world security awareness topics covered
- Designed for ease of use and accessibility

## Getting Started
### Requirements
- a Windows or Mac computer with Unity installed (This project uses Unity 2022.3.11f1 LTS)
- a Meta/Oculus Quest 2 VR Headset with USB cable

### Building on Oculus Quest 2
I. Make sure the Android platform build module is installed in your Unity version because the Quest 2 is an Android device.
1. Open the Unity hub and go to Installs.
2. Select the Unity version that you’re going to use and click on the cog wheel then ‘Add Modules‘.
3. Select Platforms > Android Build Support and click ‘Install‘.

II. Open the project in Unity.

III. Change the Unity Build Target.
1. In the Unity menu bar click on File > Build Settings.
2. In the Platform list select ‘Android‘ and click on ‘Switch Platform‘.

IV. Build the project
1. Connect your Oculus Quest 2 headset to your machine with the USB cable
2. Open the Build Settings window File > Builds Settings. Select Android in the Platform list and press on ‘Refresh‘ next to ‘Run Device‘. Switch the Run Device from ‘Default device‘ to ‘Oculus Quest 2‘.
3. Click on ‘Build And Run‘
4. The headset should stay connected while Unity is building the app. When Unity is done building, the app will be installed on your Quest and will automatically start.

### Building on laptop
I. Follow steps I. and II. from <i>Building for Oculus Quest 2</i>

II. Enable device simulator
1. From the hierarchy in the Cyber-Rooms_Scene press on --XR-- -> XR Device Simulator
2. From the inspector, tick the box next to XR Device Simulator

III. Press on the play button in the top corner of the unity window

IV. Manual on how to control the player with mouse and keyboard can be found [here](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/xr-device-simulator.html)

To enable leaderboard and data base saves, you should force remove internet permission. That can be done from Edit -> Project Settings -> click on the cog wheel next to Meta Quest Support -> untick Force Remove Internet Permission

## Usage
Once the VR experience is launched, users can navigate through the virtual environment using the VR controllers or keyboard and mouse. A tutorial and various interactive scenarios and challenges related to security awareness will be present.
