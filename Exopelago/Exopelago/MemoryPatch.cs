using HarmonyLib;
using System;

namespace Exopelago;

[HarmonyPatch(typeof(Princess))]
class Princess_AddMemoryPatch
{
  [HarmonyPatch("AddMemory")]
  [HarmonyPrefix]
  public static bool Prefix(string id, object value = null)
  {
    try {
      return Helpers.ProcessMemory(id);
    } catch (Exception e) {
      // Magic try/catch block
      // The code works as intended with this here but never prints an error
      // Thanks Sae for the idea
      Plugin.Logger.LogError($"AddMemory ID: {id} error: {e}");
      return true;
    } 
  }
}