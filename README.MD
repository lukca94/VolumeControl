# Volume Control
This project aims to create a device with software for volume level control inside the Windows audio mixer

## consoleControl
Base of the project. Locates current audio processes and provides control. Allows grouping of apps and local group saving.
Uses CSCore library.

## encoder
Gets inputs from the device. Support for multiple encoders and push buttons.
Possible future enhancements: Screen, numerical keyboard, ...

## appControl
GUI for setup and configuration of the device, allows multiple audio groups, reads Serial input from the device and translates it into Windows Mixer. Runs in the background.
Built in WPF, based on consoleControl section of the project.