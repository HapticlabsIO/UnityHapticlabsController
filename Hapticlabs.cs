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

    void Update()
    {
        if (h_debug != logDebugInfos)
        {
            h_debug = logDebugInfos;
        }
        if (useSerial != enableSerial)
        {
            useSerial = enableSerial;
            if (useSerial)
            {
                Serial.EnableOperation();
            }
            else
            {
                Serial.DisableOperation();
            }
        }
        if (useTCP != enableTCP)
        {
            useTCP = enableTCP;
            if (useTCP)
            {
                Debug.Log("enabled");
                TCPClient.EnableOperation();
            }
            else
            {
                TCPClient.DisableOperation();
            }
        }
    }

    // Example: StartTrack("trackName");    --> trackName needs to be loaded on the Satellite from Hapticlabs Studio!
    public static void StartTrack(string trackName, bool queue = false, float amplitudeScale = 1.0f)
    {
        string message = (!queue ? "stop();\n" : "") + "startTrack(\"" + trackName + "\" " + amplitudeScale + ");";
        if (h_debug) { Debug.Log(message); }
        WriteToSatellite(message);
    }

    public static void Stop()
    {
        const string message = "stop();";
        if (h_debug) { Debug.Log(message); }
        WriteToSatellite(message);
    }

    public static void SetAmplitudeScale(float amplitudeScale)
    {
        string message = "setAmplitudeScale(" + amplitudeScale + ");";
        if (h_debug) { Debug.Log(message); }
        WriteToSatellite(message);
    }

    private static void WriteToSatellite(string message)
    {
        if (useTCP && TCPClient.IsConnected())
        {
            TCPClient.WriteLn(message);
        }
        if (useSerial && Serial.checkOpen())
        {
            Serial.Write(message);
        }
    }

}

#if UNITY_EDITOR
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
    }
}
#endif