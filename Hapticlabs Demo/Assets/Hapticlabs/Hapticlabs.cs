using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Hapticlabs : MonoBehaviour
{
    public bool logDebugInfos = true;
    private static bool h_debug;

    void Update(){
        if(h_debug != logDebugInfos){
            h_debug = logDebugInfos;
        }
    }

    // Example: StartTrack("trackName");    --> trackName needs to be loaded on the Satellite from Hapticlabs Studio!
    public static void StartTrack(string trackName, bool queue = false, bool looping = false){
        string message = (!queue ? "a(\"s()disableLoop()\")b(\"s()disableLoop()\");" : "") + "startTrack(\"" + trackName + "\")" + (looping ? "a(\"enableLoop()\")b(\"enableLoop()\")" : "") + ";";
        if(h_debug){Debug.Log(message);}
        Serial.Write(message);
    }

    // Example: Hapticlabs.Vibrate("B", 0.5, 120, 200000);
    public static void Vibrate(string track, float intensity, int frequency, int duration, bool queue = false, bool looping = false){
        string message = (!queue ? (track.Contains("A") ? "a(\"s()\")" : "") + (track.Contains("B") ? "b(\"s()\")" : "") + ";" : "");
        string command = "(\"v(" + intensity + " " + frequency + " " + duration + ")" + (looping ? "enableLoop()" : "disableLoop()") + "\")";
        message += (track.Contains("A") ? "a" + command : "") + (track.Contains("B") ? "b" + command : "") + ";";
        if(h_debug){Debug.Log(message);}
        Serial.Write(message);
    }

    // Example: Hapticlabs.Pulse("B", 0.5, 200000);
    public static void Pulse(string track, float intensity, int duration, bool queue = false, bool looping = false){
        string message = (!queue ? (track.Contains("A") ? "a(\"s()\")" : "") + (track.Contains("B") ? "b(\"s()\")" : "") + ";" : "");
        string command = "(\"lp(" + intensity + " 50000 " + duration + ")" + (looping ? "enableLoop()" : "disableLoop()") + "\")";
        message += (track.Contains("A") ? "a" + command : "") + (track.Contains("B") ? "b" + command : "") + ";";
        if(h_debug){Debug.Log(message);}
        Serial.Write(message);
    }

}

// Adding the test button to the inspector GUI
[CustomEditor(typeof(Hapticlabs))]
public class HapticlabsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var hapticlabs = target as Hapticlabs;
        if (GUILayout.Button("Test connection with satellite"))
        {
            Serial.Write("a(\"v(1 120 100000)\")b(\"v(1 120 100000)\");");
            Debug.Log("Test message: a(\"v(1 120 100000)\")b(\"v(1 120 100000)\");");
        }
    }
}
