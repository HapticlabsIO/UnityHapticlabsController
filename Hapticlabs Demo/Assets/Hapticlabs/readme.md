# Hapticlabs Unity Package

## Installation

1. Download this folder and import it into your Unity project folder. Assets for example.
2. Drag the `Hapticlabs Manager.prefab` into your scene (make sure the Serial Speed is set to 115200).
3. Done!
 
If you have any problems with Serial connectionerror CS0234: The type or namespace name `Ports' does not exist in the namespace 'System.IO'`. 
Try changing these settings: Menu > Edit > Project Settings > Player > Other Settings > API Compatibility Level: .Net 2.0 


## Usage

From anywhere in your project you can call these function to command the Satellite.

### 1. Start a track from Hapticlabs Studio
Upload the tracks to the Satellite in Hapticlabs Studio

`Hapticlabs.StartTrack("trackName");`


- First parameter is the name of the track you created in Hapticlabs studio
- Optional parameters are:
  - queue: true/false (default false) --> controls if the track stops whatever is currently playing or adds it to the queue
  - looping: true/false (default false) --> controls if the track should loop or not

### 2. Vibrate

`Hapticlabs.Vibrate("B", 0.5, 120, 200000);`

- First parameter is the channel "A", "B" or "AB"
- Second parameter is the intensity between 0 and 1
- Third parameter is the frequency between 1 and 400
- Fourth parameter is the duration in ms
- Optional parameters are:
  - queue: true/false (default false) --> controls if the vibration stops whatever is currently playing or adds it to the queue
  - looping: true/false (default false) --> controls if the vibration should loop or not
  
### 3. Pulse
Only supported for Voice Coils!

`Hapticlabs.Pulse("B", 0.5, 200000);`

 - First parameter is the channel "A", "B" or "AB"
 - Second parameter is the intensity between 0 and 1
 - Third parameter is the duration in ms
 - Optional parameters are:
   - queue: true/false (default false) --> controls if the vibration stops whatever is currently playing or adds it to the queue
   - looping: true/false (default false) --> controls if the vibration should loop or not
