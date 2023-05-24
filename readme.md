# Hapticlabs Unity Package

## Setup

### In Hapticlabs Studio

1. Configure the actuator types connected to the Satellite (ERM, LRA or Voice Coil]) on channel A and/or B.
2. Once done creating tracks and naming them accordingly, upload them to the Satellite: Menu > Development > Send Tracks to Satellite
3. Disconnect your Satellite in Satellite menu. 

[More info about the Satellite menu](https://www.hapticlabs.io/article/settings-panel)

### In Unity

1. Download this folder and import it into your Unity project folder, for example Assets.
2. Drag the `Hapticlabs Manager.prefab` into your scene (make sure the Serial Speed is set to 115200).
3. Done! Playing the scene will establish a connection with the Satellite through Serial. Make sure no other application is connected or trying to connect to the Serial port, for example Hapticlabs Studio or an (Arduino) Serial Monitor.
 <img width="348" alt="Hapticlabs prefab" src="https://user-images.githubusercontent.com/34678030/235880227-780c0c75-347b-4f96-9067-2d92990c8fe9.png">

> If you have any problems with Serial connection such as error CS0234: The type or namespace name 'Ports' does not exist in the namespace 'System.IO'. 
> Try changing these settings: Menu > Edit > Project Settings > Player > Other Settings > API Compatibility Level: .Net 2.0 


## Usage (basic)

### Start a track made in Hapticlabs Studio

From anywhere in your project you can call the following function to trigger feedback on the Satellite. When using recurring haptics, we recommend not calling a function every loop as it will slow down performance, rather aim for a looped haptic and the send a new message when it changes.

Make sure to upload the tracks to the Satellite in Hapticlabs Studio before calling this function. (See Setup)

```cs
Hapticlabs.StartTrack("trackName");
```

- First parameter is the name of the track you created in Hapticlabs studio
- Optional parameters are:
  - `queue: true/false` (default false) --> controls if the track stops whatever is currently playing or adds it to the queue
  - `looping: true/false` (default false) --> controls if the track should loop or not
 
## Usage (advanced)

To create dynamic feedback linked to content within your scene, you can send custom commands:

### 1. Vibrate

```cs
Hapticlabs.Vibrate("B", 0.5, 120, 200000); // no queuing, no looping
Hapticlabs.Vibrate("B", 0.5, 120, 200000, queue: true, looping: true);
```
- First parameter is the channel `"A"`, `"B"` or `"AB"`
- Second parameter is the intensity between `0.0` and `1.0`
- Third parameter is the frequency between `1` and `400`
- Fourth parameter is the duration in ms
- Optional parameters are:
  - `queue: true/false` (default false) --> controls if the vibration stops whatever is currently playing or adds it to the queue
  - `looping: true/false` (default false) --> controls if the vibration should loop or not
  
  
### 2. Pulse
Only recommended for Voice Coils!

```cs
Hapticlabs.Pulse("B", 0.5, 200000); // no queuing, no looping
Hapticlabs.Pulse("B", 0.5, 200000, queue: true, looping: true);
```

- First parameter is the channel `"A"`, `"B"` or `"AB"`
- Second parameter is the intensity between `0.0` and `1.0`
- Third parameter is the duration in ms
- Optional parameters are:
  - `queue: true/false` (default false) --> controls if the vibration stops whatever is currently playing or adds it to the queue
  - `looping: true/false` (default false) --> controls if the vibration should loop or not


# Examples

Make sure your character or other object triggering the collisions have a Rigidbody and a Collider attached

### Collision with Object

When using a Sphere/Box/Capsule/etc. collider on an object, then you can trigger a Hapticlabs track by attaching a script to the same game object with the following code inside the class:

```cs
private void OnCollisionEnter(Collision collision) {
    Hapticlabs.StartTrack("objectHit");
}
```

### Collision with Particles from Particle Emitter

When using a particle emitter, check Colission > Send Collision Messages, then you can trigger a Hapticlabs track by attaching a script to the same game object with the following code inside the class:

```cs
private void OnParticleCollision(GameObject other) {    
    Hapticlabs.StartTrack("particlePop");
}
```

### Dynamic vibrations

When you want to vary the parameters of vibrations or pulses, you can of course fill in the function parameters with variables.

```cs
intensity = 1;
frequency = 120;
Hapticlabs.Vibrate("B", intensity, frequency, 200000, looping: true);
```
> Try to not call this function every Update(), but rather when it changes or with set intervals.

## References

Serial bridge based on the script from Pierre Rossel. [link](https://github.com/prossel/UnitySerialPort.git)

Bubble particle system from Moonflower Carnivore. [link](https://assetstore.unity.com/packages/vfx/particles/environment/jiggly-bubble-free-61236#content)
