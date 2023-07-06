/* 
 * Version 0.1.0, 2023-04-30, Kay van den Aker, Hapticlabs
 * 
 * This component helps sending data to the Hapticlabs Satellite using a Serial libraty built by Pierre Rossel. 
 * From anywhere in your project you can call these function to command the Satellite
 * I recommend using the prefab with the Serial Config attached also. Make sure you use 115200 as speed.
 * 
 * 1. Starting a track by name. 
 * -------
 * 
 *   Hapticlabs.StartTrack("trackName");
 * 
 * - upload the tracks to the Satellite in Hapticlabs Studio
 * - First parameter is the name of the track you created in Hapticlabs studio
  * - Optional parameters are:
 *   - queue: true/false (default false)
 *     controls if the track stops whatever is currently playing or adds it to the queue
 *   - looping: true/false (default false)
 *     controls if the track should loop or not
 *   
 * 2. Start a vibration
 * -------
 * 
 *   Hapticlabs.Vibrate("B", 0.5, 120, 200000);
 *
 * - First parameter is the channel "A", "B" or "AB"
 * - Second parameter is the intensity between 0 and 1
 * - Third parameter is the frequency between 1 and 400
 * - Fourth parameter is the duration in micro seconds
 * - Optional parameters are:
 *   - queue: true/false (default false)
 *     controls if the vibration stops whatever is currently playing or adds it to the queue
 *   - looping: true/false (default false)
 *     controls if the vibration should loop or not
 * 
 * 3. Start a pulse
 * -------
 * 
 *   Hapticlabs.Pulse("B", 0.5, 200000);
 *
 *   Only supported for Voice Coils!
 * - First parameter is the channel "A", "B" or "AB"
 * - Second parameter is the intensity between 0 and 1
 * - Third parameter is the duration in micro seconds
 * - Optional parameters are:
 *   - queue: true/false (default false)
 *     controls if the vibration stops whatever is currently playing or adds it to the queue
 *   - looping: true/false (default false)
 *     controls if the vibration should loop or not
 * 
 * Troubleshooting the Serial
 * ---------------
 * 
 * You may get the following error:
 *     error CS0234: The type or namespace name `Ports' does not exist in the namespace `System.IO'. 
 *     Are you missing an assembly reference?
 * Solution: 
 *     Menu Edit | Project Settings | Player | Other Settings | API Compatibility Level: .Net 2.0
 * 
 */
 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Hapticlabs : MonoBehaviour
{
    public bool logDebugInfos = true;
    public bool enableTCP = true;
    public bool enableSerial = false;
    public static bool useTCP = false;
    public static bool useSerial = false;
    private static bool h_debug;

    void Update(){
        if(h_debug != logDebugInfos){
            h_debug = logDebugInfos;
        }
        if (useSerial != enableSerial){
            useSerial = enableSerial;
            if (useSerial){
                Serial.EnableOperation();
            } else {
                Serial.DisableOperation();
            }
        }
        if (useTCP != enableTCP){
            useTCP = enableTCP;
            if (useTCP){
                Debug.Log("enabled");
                TCPClient.EnableOperation();
            } else {
                TCPClient.DisableOperation();
            }
        }
    }

    // Example: StartTrack("trackName");    --> trackName needs to be loaded on the Satellite from Hapticlabs Studio!
    public static void StartTrack(string trackName, bool queue = false, bool looping = false){
        string message = (!queue ? "a(\"s()disableLoop()\")b(\"s()disableLoop()\");" : "") + "startTrack(\"" + trackName + "\")" + (looping ? "a(\"enableLoop()\")b(\"enableLoop()\")" : "") + ";";
        if(h_debug){Debug.Log(message);}
        WriteToSatellite(message);
    }

    // Example: Hapticlabs.Vibrate("B", 0.5, 120, 200000);
    public static void Vibrate(string track, float intensity, int frequency, int duration, bool queue = false, bool looping = false){
        string message = (!queue ? (track.Contains("A") ? "a(\"s()\")" : "") + (track.Contains("B") ? "b(\"s()\")" : "") + ";" : "");
        string command = "(\"v(" + intensity + " " + frequency + " " + duration + ")" + (looping ? "enableLoop()" : "disableLoop()") + "\")";
        message += (track.Contains("A") ? "a" + command : "") + (track.Contains("B") ? "b" + command : "") + ";";
        if(h_debug){Debug.Log(message);}
        WriteToSatellite(message);
    }

    // Example: Hapticlabs.Pulse("B", 0.5, 200000);
    public static void Pulse(string track, float intensity, int duration, bool queue = false, bool looping = false){
        string message = (!queue ? (track.Contains("A") ? "a(\"s()\")" : "") + (track.Contains("B") ? "b(\"s()\")" : "") + ";" : "");
        string command = "(\"lp(" + intensity + " 50000 " + duration + ")" + (looping ? "enableLoop()" : "disableLoop()") + "\")";
        message += (track.Contains("A") ? "a" + command : "") + (track.Contains("B") ? "b" + command : "") + ";";
        if(h_debug){Debug.Log(message);}
        WriteToSatellite(message);
    }

    public static void Stop(){
        const string message = "a(\"s()\")a(\"disableLoop()\")b(\"s()\")b(\"disableLoop()\");";
        if(h_debug){Debug.Log(message);}
        WriteToSatellite(message);
    }

    private static void WriteToSatellite(string message){
        if(useTCP && TCPClient.IsConnected()){
            TCPClient.WriteLn(message);
        }
        if (useSerial && Serial.checkOpen()){
            Serial.Write(message);
        }
    }

}

// Adding the test button to the inspector GUI
[CustomEditor(typeof(Hapticlabs))]
public class HapticlabsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // var hapticlabs = target as Hapticlabs;
        if (GUILayout.Button("Test Serial connection with satellite"))
        {
            Serial.Write("a(\"s()disableLoop()\")b(\"s()disableLoop()\");a(\"v(1 120 100000)\")b(\"v(1 120 100000)\");");
            Debug.Log("Test message: a(\"s()disableLoop()\")b(\"s()disableLoop()\");a(\"v(1 120 100000)\")b(\"v(1 120 100000)\");");
        }
        if (GUILayout.Button("Test TCP connection with satellite"))
        {
            Debug.Log(TCPClient.IsConnected());
            TCPClient.WriteLn("a(\"s()disableLoop()\")b(\"s()disableLoop()\");a(\"v(1 120 100000)\")b(\"v(1 120 100000)\");");
            Debug.Log("Test message: a(\"s()disableLoop()\")b(\"s()disableLoop()\");a(\"v(1 120 100000)\")b(\"v(1 120 100000)\");");
        }
        // if (GUILayout.Button("Disconnect satellite"))
        // {
        //     Serial.Close();
        // }

        
    }
}
