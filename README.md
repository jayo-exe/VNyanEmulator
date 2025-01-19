# VNyanEmulator

A mock implementation of the VNyan Plugin Interface inside the Unity Editor. Allows code that references the `VNyanInterface` namespace to run in the Unity Editor without throwing exceptions caused by the VNyanInterface stub containing null references to these subsystems. 

This is intended to simplify the development cycle for UI-driven VNyan plugins that interact with the Node Graph through Trigger, Parameters and Dictionaries.

## Installation
Just place the `VNyanEmulator.dll` file into your Unity project for your VNyan plugin(s) and it will work automatically.  Be sure you've already included/referenced the `VNyanInterface.dll` from your Vnyan installation into the project.

It is important this this DLL is __not__ placed into a folder named `Editor` as that will flag all scripts inside as Editor scripts, which will prevent everything from working correctly.

It is recommended that you have Unity projects specifically dedicated to VNyan plugins, as the VNyanEmulator can't be disabled or turned off, and will run whenever you enter Play Mode.

## Usage
When entering Play Mode, VNyanEmulator will properly set the references in `VnyanInterface.VNyanInterface` to mock versions of the core subsystems, allowing any code referencing the Plugin Interface to execute without throwing exceptions.
The Parameter, UI and Trigger systems are simple but properly-working implementations of these VNyan subsystems to allow you to test plugins that interact with the VNyan node Graph through these constructs. You can register plugin buttons, window prefabs and trigger listeners, as well as read and write parameters and dictionary values.
Most other subsystems just echo out to the console to clearly indicate that method calls to the VYasn interface are working and passing the coreect argumentsworking

VNyanEmulator will also create a Canvas and populate it with some basic UI to make it easier to test UI-driven plugins.  This includes a list of plugin buttons for any plugins registered through the interface, allowing you to see and use your plugins' instantiated window prefabs.  It also includes lists of all currently-set string and float parameters.