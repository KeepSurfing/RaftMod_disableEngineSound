using HarmonyLib;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class DisableEngineSoundMod : Mod
{
    private const string VERSION = "1.0.0";
    private const string MOD_NAME = "DisableEngineSound";
    
    public void Start()
    {
        Debug.Log($"{MOD_NAME}: Initializing version {VERSION}");
        try
        {

            // Create our patches
            var harmony = new Harmony($"com.{MOD_NAME}.DisableEngineSound".ToLower());
            var motorWheelType = typeof(MotorWheel);
            var handleSoundsMethod = motorWheelType.GetMethod("HandleSounds", 
                BindingFlags.Public | BindingFlags.NonPublic | 
                BindingFlags.Instance | BindingFlags.Static);

            if (handleSoundsMethod != null)
            {
                try
                {
                    harmony.Patch(
                        original: handleSoundsMethod,
                        prefix: new HarmonyMethod(typeof(DisableEngineSoundMod)
                            .GetMethod(nameof(SoundPatchPrefix), 
                                        BindingFlags.NonPublic | BindingFlags.Static))
                    );
                    Debug.Log($"{MOD_NAME}: Successfully patched MotorWheel.HandleSounds");
                }
                catch (Exception e)
                {
                    Debug.LogError($"{MOD_NAME}: Failed to patch MotorWheel.HandleSounds - {e.Message}");
                }
            }

            Debug.Log($"{MOD_NAME}: Initialization complete");
        }
        catch (Exception e)
        {
            Debug.LogError($"{MOD_NAME}: Failed to initialize - {e.Message}");
            Debug.LogError(e.StackTrace);
        }
    }

    private static bool SoundPatchPrefix()
    {
        // Return false to prevent the original method from running
        return false;
    }

    public void OnModUnload()
    {
        Debug.Log($"{MOD_NAME}: Mod unloaded");
    }
}