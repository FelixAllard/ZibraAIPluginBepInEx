using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace Zibra {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        internal static new ManualLogSource Logger = null!;
        public static AssetBundle? ModAssets;

        private void Awake() {
            Logger = base.Logger;
            // If you don't want your mod to use a configuration file, you can remove this line, Configuration.cs, and other references.
        }
    }
}