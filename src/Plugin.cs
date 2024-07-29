using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace Zibra {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        internal static new ManualLogSource Logger = null!;

        private void Awake() {
            Logger = base.Logger;
            Logger.LogInfo("Starting to UnWrap Assemblies!");
            LoadDependencies();
            // If you don't want your mod to use a configuration file, you can remove this line, Configuration.cs, and other references.
        }
        private void LoadDependencies()
        {
            string pluginPath = Paths.PluginPath;
            string[] matchingDirectories = Directory.GetDirectories(pluginPath, "TheWeavers-ZibraFireSmokeAndLiquid*");
    
            if (matchingDirectories.Length == 0)
            {
                Logger.LogWarning("No directories matching 'TheWeavers-ZibraFireSmokeAndLiquid*' found.");
                return;
            }
    
            // Assuming we take the first matching directory
            string targetDirectory = matchingDirectories[0];
            Logger.LogInfo($"Found matching directory: {targetDirectory}");

            if (Directory.Exists(targetDirectory))
            {
                foreach (string dll in Directory.GetFiles(targetDirectory, "Zibra*.dll"))
                {
                    try
                    {
                        Assembly.LoadFile(dll);
                        Logger.LogInfo($"Loaded {Path.GetFileName(dll)} successfully.");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Failed to load {Path.GetFileName(dll)}: {ex}");
                    }
                }
            }
            else
            {
                Logger.LogWarning("Dependencies folder not found.");
            }
            
            
        }
    }
}