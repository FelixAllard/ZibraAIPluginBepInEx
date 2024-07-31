using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace Zibra {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = null!;

        /*
        [DllImport("ZibraSmokeAndFireNative_Win", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SmokeAndFireNative();

        [DllImport("ZibraLiquidNative_Win", CallingConvention = CallingConvention.Cdecl)]
        private static extern void LiquidNative();
        */
        internal static string pluginPath = "";

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo("Starting to unwrap assemblies!");

            // Find the plugin directory using the pattern
            string pluginPath = Paths.PluginPath;
            string[] matchingDirectories = Directory.GetDirectories(pluginPath, "TheWeavers-ZibraFireSmokeAndLiquid*");

            if (matchingDirectories.Length == 0)
            {
                Logger.LogWarning("No directories matching 'TheWeavers-ZibraFireSmokeAndLiquid*' found.");
                return;
            }

            // Assuming we take the first matching directory
            Zibra.Plugin.pluginPath = matchingDirectories[0];
            Logger.LogInfo($"Found matching directory: {Zibra.Plugin.pluginPath}");

            //Inject the natives :)
            InjectNativeFileIntoGame();
            // Load managed dependencies
            LoadDependencies(Zibra.Plugin.pluginPath);

            // Call the native function to ensure it works
            /*try {
                SmokeAndFireNative();
                Logger.LogInfo("Native function SmokeAndFireNative called successfully.");
            } catch (Exception e) {
                Logger.LogError($"Error calling native function SmokeAndFireNative: {e.Message}");
            }

            try {
                LiquidNative();
                Logger.LogInfo("Native function LiquidNative called successfully.");
            } catch (Exception e) {
                Logger.LogError($"Error calling native function LiquidNative: {e.Message}");
            }*/

        }

        private void LoadDependencies(string targetDirectory)
        {
            if (Directory.Exists(targetDirectory))
            {
                foreach (string dll in Directory.GetFiles(targetDirectory, "ZibraAI*.dll"))
                {
                    try
                    {
                        Assembly.LoadFile(dll);
                        Logger.LogInfo($"Loaded {Path.GetFileName(dll)} successfully.");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Failed to load {Path.GetFileName(dll)}: {ex.Message}");
                    }
                }
            }
            else
            {
                Logger.LogWarning("Dependencies folder not found.");
            }
        }

        private void InjectNativeFileIntoGame()
        {
            string targetPath = "Lethal Company_Data/Plugins/x86_64";
            if (!Directory.Exists(Path.Combine(Paths.GameRootPath, targetPath)))
            {
                Directory.CreateDirectory(Path.Combine(Paths.GameRootPath, targetPath));
            }

            File.Copy(Path.Combine(Zibra.Plugin.pluginPath, "ZibraLiquidNative_Win.dll"), Path.Combine(targetPath, "ZibraLiquidNative_Win.dll"), true);
            File.Copy(Path.Combine(Zibra.Plugin.pluginPath, "ZibraSmokeAndFireNative_Win.dll"), Path.Combine(targetPath, "ZibraSmokeAndFireNative_Win.dll"), true);

            Logger.LogInfo($"Successfully injected assemblies to {targetPath}");
        }
    }
}
