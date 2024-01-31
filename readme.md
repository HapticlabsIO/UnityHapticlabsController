# Hapticlabs Unity Package

## Setup

There's two ways to communicate to the Hapticlabs Satellite from Unity: Directly through a Serial connection, or connecting to Hapticlabs Studio via TCP.

### Prepare Serial communication (not needed if connecting via TCP)

1. Configure the actuator types connected to the Satellite (ERM, LRA or Voice Coil]) on channel A and/or B.
2. Once done creating tracks and naming them accordingly, upload them to the Satellite: Menu > Development > Send Tracks to Satellite
3. Disconnect your Satellite in Satellite menu. 

[More info about the Satellite menu](https://www.hapticlabs.io/article/settings-panel)

### In Unity

1. Download this folder and import it into your Unity project folder, for example Assets.
2. Drag the `Hapticlabs Manager.prefab` into your scene (if you're communicating through Serial, make sure the Serial Speed is set to 115200).
3. Done! Playing the scene will establish a connection with the Satellite through Serial or via TCP to Hapticlabs Studio, respectively. If you communicate via Serial, make sure no other application is connected or trying to connect to the Serial port, for example Hapticlabs Studio or an (Arduino) Serial Monitor.
 <img width="348" alt="Hapticlabs prefab" src="https://user-images.githubusercontent.com/34678030/235880227-780c0c75-347b-4f96-9067-2d92990c8fe9.png">

> If you have any problems with Serial connection such as error CS0234: The type or namespace name 'Ports' does not exist in the namespace 'System.IO'. 
> Try changing these settings: Menu > Edit > Project Settings > Player > Other Settings > API Compatibility Level: .Net 2.0 


## Usage (basic)

### Start a track made in Hapticlabs Studio

From anywhere in your project you can call the following function to trigger feedback on the Satellite.

If you communicate through a Serial connection, make sure to upload the tracks to the Satellite in Hapticlabs Studio before calling this function (See Prepare Serial communication). If you communicate through TCP, make sure Hapticlabs Studio is running and the track is present in the currently opened project in Hapticlabs Studio.

```cs
Hapticlabs.StartTrack("trackName");
```

- First parameter is the name of the track you created in Hapticlabs studio
- Optional parameters are:
  - `queue: true/false` (default false) --> controls if the track stops whatever is currently playing or adds it to the queue
  - `amplitude: float` (default 1.0) --> controls the track's overall amplitude scale

### Adjust the playback intensity

At any time, you can scale the amplitude of the haptic output. Values greater than 1 represent an increase in intensity over the original signal, whereas values less than 1 reperesent a decrease.

```cs
Hapticlabs.SetAmplitudeScale(amplitude);
```
- First parameter is a float representing the amplitude scale

### Abort haptic playback

At any time, you can abort haptic playback by simply calling

```cs
Hapticlabs.Stop();
```


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

### Changing vibration amplitude dynamically
The feature of queueing signals allows us to create seemingly endless loops of a track. To achieve this, we reschedule playback of the track just before the the running playback terminates. Using the queue parameter, we ensure that the newly scheduled playback will not interrupt the currently playing signal:

```cs
// The target intensity
private float intensity;
// The real intensity currently output
private float outputIntensity = 0;

// When will the current vibration terminate?
private long expectedVibrationEnd = 0;

// Should we vibrate currently?
private bool continueVibrating = false;

// Facts about the looped signal
private string trackName = "sliderVibe";
private string trackDurationMs = 700;

// Call this in the Update() function
void updateVibration() {
    if (!continueVibrating){
        // Stop the vibration
        Hapticlabs.Stop();
        expectedVibrationEnd = 0;
        return;
    }
    DateTime currentTime = DateTime.Now;
    long currentMillis = currentTime.Ticks / TimeSpan.TicksPerMillisecond;

    // Check if the current vibration is about to end
    if (currentMillis > expectedVibrationEnd - 100){
        // Let's keep the vibration alive!
        this.restartVibration();
    }

    if (outputIntensity != intensity){
        // Update the amplitude
        Hapticlabs.SetAmplitudeScale(intensity);
        outputIntensity = intensity;
    }
}

void restartVibration() {
    DateTime currentTime = DateTime.Now;
    long lastVibrationStart = currentTime.Ticks / TimeSpan.TicksPerMillisecond;

    // By using queue: true, we ensure that the vibrations will all be queued.
    // This way, we can get an infinite vibration by repeatedly queueing vibrations.
    Hapticlabs.StartTrack(trackName, queue: true, amplitudeScale: intensity);
    outputIntensity = intensity;

    // Queueing the vibrations pushes back the end of the vibration
    expectedVibrationEnd = expectedVibrationEnd == 0 ? lastVibrationStart + trackDurationMs : expectedVibrationEnd + trackDurationMs;
}
```

Now, we can simply set `continueVibrating` and `intensity` within our code and the `updateVibration()` function called in the `Update()` function handles the rest.

## References

Serial bridge based on the script from Pierre Rossel. [link](https://github.com/prossel/UnitySerialPort.git)

Bubble particle system from Moonflower Carnivore. [link](https://assetstore.unity.com/packages/vfx/particles/environment/jiggly-bubble-free-61236#content)
