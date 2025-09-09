using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Northway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Exopelago.Archipelago;

namespace Exopelago;

[BepInPlugin("org.pamymaf.exocolonist.exopelago", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Exocolonist.exe")]
public class Plugin : BaseUnityPlugin
{
  internal static new ManualLogSource Logger;

  private void Awake()
  {
    Logger = base.Logger;
    Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    var harmony = new Harmony("Exopelago");
    harmony.PatchAll();
    Logger.LogInfo("Exopelago patches done.");
  }
}

