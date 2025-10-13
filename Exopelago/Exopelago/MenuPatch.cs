using HarmonyLib;
using Northway.Utils;
using System;
using UnityEngine.UI;
using Exopelago.Archipelago;

namespace Exopelago;

[HarmonyPatch]
class MenuPatches
{
  static string connectButtonText = "Connect";

  [HarmonyPatch(typeof(SettingsMenu), "SetSetting")]
  [HarmonyPrefix]
  public static bool SettingsMenuSetSettingsPrefix(SettingsMenu __instance, string settingName, object value, NWButton buttonToUpdate = null)
  {
    if (ArchipelagoClient.serverData.forceBattles && settingName == "skipCardChallenges") {
      buttonToUpdate.text = "Skip Card Challenges disabled by Archipelago";
      Settings.instance.skipCardChallenges = false;
      return false;
    }
    return true;
  }

  [HarmonyPatch(typeof(SettingsMenu), "CreateButton")]
  [HarmonyPostfix]
  public static void SettingsMenuCreateButtonPostfix(SettingsMenu __instance, Selectable __result, string settingName, Selectable aboveButton)
  {
    if (__instance == null || __result == null || settingName != "hardwareMouseCursor")
    {
        return;
    }
    Selectable currentAboveButton = __result;

    // Code for separator and header button taken from Exoloader
    NWButton separatorButton = CreateSeparator(__instance);
    if (separatorButton != null) {
      ConnectNavigation(currentAboveButton, separatorButton);
      currentAboveButton = separatorButton;
    }
    NWButton headerButton = CreateModHeader(__instance);
    if (headerButton != null) {
      ConnectNavigation(currentAboveButton, headerButton);
      currentAboveButton = headerButton;
    }

    if (!ArchipelagoClient.authenticated) {
     connectButtonText = "Connect";
    } else {
     connectButtonText = "Disconnect";
    }
    NWButton spawnIPPopup = __instance.AddButton("Change Archipelago IP", SetIP, "");
    NWButton spawnPortPopup = __instance.AddButton("Change Port", SetPort, "");
    NWButton spawnSlotPopup = __instance.AddButton("Change Slot", SetSlot, "");
    NWButton spawnPassPopup = __instance.AddButton("Change Password", SetPassword, "");
    NWButton connectToAP = __instance.AddButton($"{connectButtonText}", Connect, "Connect to Archipelago");
    NWButton matchFlags = __instance.AddButton(GetBoolButtonText("matchFlags"), matchFlagsToggle, ExopelagoSettings.settingDescriptions["matchFlags"]);
    
  }

  private static string GetBoolButtonText(string settingName)
  {
   return ExopelagoSettings.settingNames[settingName] + " " + (ExopelagoSettings.settingBools[settingName] ? TextLocalized.Localize("button_on") : TextLocalized.Localize("button_off"));
  }

  private static void SetIP()
  {
    PopupMenu.ShowInput("Archipelago IP", "", ArchipelagoClient.serverData.uri, SetIP2);
  }

  private static void SetIP2(string ip)
  {
    Plugin.Logger.LogInfo($"IP set to {ip}");
    ArchipelagoClient.serverData.uri = ip;
  }

  private static void SetPort()
  {
    PopupMenu.ShowInput("Archipelago Port", "", ArchipelagoClient.serverData.port, SetPort2);
  }

  private static void SetPort2(string port)
  {
    Plugin.Logger.LogInfo($"Port set to {port}");
    ArchipelagoClient.serverData.port = port;
  }

  private static void SetSlot()
  {
    PopupMenu.ShowInput("Archipelago Slot", "", ArchipelagoClient.serverData.slotName, SetSlot2);
  }

  private static void SetSlot2(string slot)
  {
    Plugin.Logger.LogInfo($"Slot set to {slot}");
    ArchipelagoClient.serverData.slotName = slot;
  }

  private static void SetPassword()
  {
    PopupMenu.ShowInput("Archipelago Password", "", ArchipelagoClient.serverData.password, SetPassword2);
  }

  private static void SetPassword2(string pass)
  {
    Plugin.Logger.LogInfo($"Password set to {pass}");
    ArchipelagoClient.serverData.password = pass;
  }

  private static void Connect()
  {
    if (ArchipelagoClient.authenticated) {
      Groundhogs.Save();
      ArchipelagoClient.Disconnect();
      Groundhogs.instance.groundhogs = new StringDictionary();
      Groundhogs.Save();
      connectButtonText = "Connect";
    } else {
      Groundhogs.instance.groundhogs = new StringDictionary();
      Groundhogs.Save();
      bool connected = ArchipelagoClient.ConnectSavedInfo();
      if (connected) {
        Groundhogs.instance.Load();
        connectButtonText = "Disconnect";
      } else {
        connectButtonText = "Invalid details";
      }
    }
    if (ArchipelagoClient.serverData.forceBattles) {
      Settings.instance.skipCardChallenges = false;
    }
    Singleton<SettingsMenu>.instance.UpdateButtons();
  }

  public static void matchFlagsToggle()
  {
    ExopelagoSettings.settingBools["matchFlags"] = !ExopelagoSettings.settingBools["matchFlags"];
    Console.WriteLine($"Match flags {ExopelagoSettings.settingBools["matchFlags"]}");
    Singleton<SettingsMenu>.instance.UpdateButtons();
  }



// ========== Things to make the pretty mod header button and spacer ========== \\
// Taken from Exoloader
  private static NWButton CreateModHeader(SettingsMenu settingsMenu)
  {
    try {
      NWButton headerButton = settingsMenu.AddButton("Exopelago", null);
      headerButton.interactable = false;

      if (headerButton.GetComponent<Text>() != null) {
        var textComponent = headerButton.GetComponent<Text>();
        textComponent.fontStyle = UnityEngine.FontStyle.Bold;
        textComponent.color = new UnityEngine.Color(1f, 0.8f, 0.2f, 1f);
      }

      return headerButton;
    } catch (Exception e) {
      Plugin.Logger.LogError($"CreateModHeader: {e}");
      return null;
    }
  }

  private static NWButton CreateSeparator(SettingsMenu settingsMenu)
  {
    try {
      NWButton separatorButton = settingsMenu.AddButton("", null);
      separatorButton.interactable = false;
      var buttonImage = separatorButton.GetComponent<UnityEngine.UI.Image>();
      if (buttonImage != null) {
        buttonImage.color = new UnityEngine.Color(0,0,0,0);
      }
      var textComponent = separatorButton.GetComponent<Text>();
      if (textComponent != null) {
        textComponent.text = "";
        textComponent.color = new UnityEngine.Color(0,0,0,0);
      }
      var rectTransform = separatorButton.GetComponent<UnityEngine.RectTransform>();
      if (rectTransform != null) {
        rectTransform.sizeDelta = new UnityEngine.Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y * 1.5f);
      }
      return separatorButton;
    } catch (Exception e) {
      Plugin.Logger.LogError($"CreateModHeader: {e}");
      return null;
    }
  }

  private static void ConnectNavigation(Selectable top, Selectable bottom)
  {
    if (!(top == null) || !(bottom == null))
    {
      if (bottom is NWSelectable nWSelectable)
      {
        nWSelectable.selectOverrideOnUp = top;
      }
      else if (bottom is NWButton nWButton)
      {
        nWButton.selectOverrideOnUp = top;
      }
      else if (bottom is NWDropdown nWDropdown)
      {
        nWDropdown.selectOverrideOnUp = top;
      }
      if (top is NWSelectable nWSelectable2)
      {
        nWSelectable2.selectOverrideOnDown = bottom;
      }
      else if (top is NWButton nWButton2)
      {
        nWButton2.selectOverrideOnDown = bottom;
      }
      else if (top is NWDropdown nWDropdown2)
      {
        nWDropdown2.selectOverrideOnDown = bottom;
      }
    }
  }
}