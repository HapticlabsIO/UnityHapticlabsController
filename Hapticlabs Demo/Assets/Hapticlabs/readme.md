# Hapticlabs Unity Package

## Setup

### In Hapticlabs Studio

1. Configure the actuator types connected to the Satellite; ERM, LRA or Voice Coil on channel A and/or B.
2. Once done creating tracks and naming them accordingly, upload them to the Satellite: Menu > Development > Send Tracks to Satellite
3. Disconnect your Satellite in Satellite Setup. 

### In Unity

1. Download this folder and import it into your Unity project folder. Assets for example.
2. Drag the `Hapticlabs Manager.prefab` into your scene (make sure the Serial Speed is set to 115200).
3. Done! Now running the game will connect to the Satellite through Serial. So make sure no other application is connected or trying to connect to the Serial port, for example Hapticlabs Studio or an (Arduino) Serial Monitor.
 
> If you have any problems with Serial connection such as error CS0234: The type or namespace name 'Ports' does not exist in the namespace 'System.IO'. 
> Try changing these settings: Menu > Edit > Project Settings > Player > Other Settings > API Compatibility Level: .Net 2.0 


## Usage

From anywhere in your project you can call these function to command the Satellite.

### 1. Start a track made in Hapticlabs Studio

Make sure to upload the tracks to the Satellite in Hapticlabs Studio before calling this function.

```cs
Hapticlabs.StartTrack("trackName");
```

- First parameter is the name of the track you created in Hapticlabs studio
- Optional parameters are:
  - `queue: true/false` (default false) --> controls if the track stops whatever is currently playing or adds it to the queue
  - `looping: true/false` (default false) --> controls if the track should loop or not
 

### 2. Vibrate

```cs
Hapticlabs.Vibrate("B", 0.5, 120, 200000);
```
- First parameter is the channel `"A"`, `"B"` or `"AB"`
- Second parameter is the intensity between `0.0` and `1.0`
- Third parameter is the frequency between `1` and `400`
- Fourth parameter is the duration in ms
- Optional parameters are:
  - `queue: true/false` (default false) --> controls if the vibration stops whatever is currently playing or adds it to the queue
  - `looping: true/false` (default false) --> controls if the vibration should loop or not
  
  
### 3. Pulse
Only supported for Voice Coils!

```cs
Hapticlabs.Pulse("B", 0.5, 200000);
```

- First parameter is the channel `"A"`, `"B"` or `"AB"`
- Second parameter is the intensity between `0.0` and `1.0`
- Third parameter is the duration in ms
- Optional parameters are:
  - `queue: true/false` (default false) --> controls if the vibration stops whatever is currently playing or adds it to the queue
  - `looping: true/false` (default false) --> controls if the vibration should loop or not
