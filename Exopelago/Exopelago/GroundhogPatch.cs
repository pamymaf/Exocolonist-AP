using System;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Exopelago.Archipelago;

using System.ComponentModel;

namespace Exopelago;


[Serializable]
public class ExopelagoGroundhogs
{
	private static ExopelagoGroundhogs _instance;
	private const int currentVersion = 1;
	public static string slotName = ArchipelagoClient.serverData.slotName;
	public static string seed = ArchipelagoClient.serverData.seed;
	public static string filename = $"{ArchipelagoClient.serverData.slotName}-{ArchipelagoClient.serverData.seed}.json";
	public static string filenameBackup = $"{ArchipelagoClient.serverData.slotName}-{ArchipelagoClient.serverData.seed}.bak";
	public static string filenameOld = $"{ArchipelagoClient.serverData.slotName}-{ArchipelagoClient.serverData.seed}_old.json";
	public static bool isLoaded;
	public int saveFileVersion = 1;
	public StringDictionary groundhogs = new StringDictionary();
	public string groundhogsSerialized;
	public Dictionary<string, List<string>> seenChoices = new Dictionary<string, List<string>>();
	public string seenChoicesSerialized;
	public List<string> seenBackgrounds = new List<string>();
	public List<string> seenCards = new List<string>();
	public List<CheevoID> cheevos = new List<CheevoID>();
	public static ExopelagoGroundhogs instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ExopelagoGroundhogs();
			}
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public void Load()
	{
		if (EditorUtils.isEditor)
		{
			LoadInner();
			return;
		}
		try
		{
			LoadInner();
		}
		catch (Exception ex)
		{
			Debug.LogError("ExopelagoGroundhogs.Load error during FromJsonOverwrite: " + ex);
			isLoaded = true;
			Save();
		}
	}

	public void LoadInner()
	{
		bool flag = false;
		string text = FileManager.LoadFileString(filename, FileManager.documentsPath, warnFileMissing: false);
		if (text.IsNullOrEmptyOrWhitespace())
		{
			text = FileManager.LoadFileString(filenameBackup, FileManager.documentsPath, warnFileMissing: false);
			flag = true;
			if (text.IsNullOrEmptyOrWhitespace())
			{
				Debug.Log($"{filename} not found, no backup, must be a fresh install");
				isLoaded = true;
				Save();
				return;
			}
			Debug.LogError($"{filename} was empty, loaded from backup {filenameBackup}");
		}
		try
		{
			JsonUtility.FromJsonOverwrite(text, this);
			Groundhogs.instance.groundhogs = this.groundhogs;
		}
		catch (Exception ex)
		{
			if (flag)
			{
				Debug.LogError("ExopelagoGroundhogs file is corrupt! Resetting to defaults. " + ex.Message);
				isLoaded = true;
				Save();
				return;
			}
			text = FileManager.LoadFileString(filenameBackup, FileManager.documentsPath, warnFileMissing: false);
			flag = true;
			if (text.IsNullOrEmptyOrWhitespace())
			{
				Debug.LogError("ExopelagoGroundhogs file is corrupt, no backup. Resetting to defaults. " + ex.Message);
				isLoaded = true;
				Save();
				return;
			}
			try
			{
				JsonUtility.FromJsonOverwrite(text, this);
			}
			catch (Exception ex2)
			{
				Debug.LogError("ExopelagoGroundhogs file and backup are corrupt! Resetting to defaults. " + ex2.Message);
				isLoaded = true;
				Save();
				return;
			}
		}
		if (saveFileVersion != 1)
		{
			Debug.Log($"{filename} version changed from " + saveFileVersion + " to " + 1);
			saveFileVersion = 1;
		}
		groundhogs.DeserializeDictionary(groundhogsSerialized);
		seenChoices.DeserializeDictionary(seenChoicesSerialized);
		isLoaded = true;
		if (flag)
		{
			Save();
		}
	}

	public static void Save(bool threaded = true)
	{
		try
		{
			if (!isLoaded)
			{
				Debug.LogError("ExopelagoGroundhogs.Save but hasn't been loaded yet, not saving");
				return;
			}
			ExopelagoGroundhogs clone = instance.DeepClone(serializeDictionaries: true);
			string path = FileManager.documentsPath;
			if (threaded)
			{
				ThreadWorker.ExecuteInThread(delegate
				{
					clone.SaveThread(path);
				}, ThreadProcessID.saveFile);
			}
			else
			{
				clone.SaveThread(path);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Settings.Save failed, " + ex);
		}
	}

	private void SaveThread(string path)
	{
		FileManager.SaveFile(JsonUtility.ToJson(this, prettyPrint: true), filename, path);
	}

	public ExopelagoGroundhogs DeepClone(bool serializeDictionaries = false)
	{
		ExopelagoGroundhogs groundhogs = (ExopelagoGroundhogs)MemberwiseClone();
		if (serializeDictionaries)
		{
			groundhogs.groundhogsSerialized = this.groundhogs.SerializeDictionary();
			groundhogs.groundhogs = null;
			groundhogs.seenChoicesSerialized = seenChoices.SerializeDictionary();
			groundhogs.seenChoices = null;
		}
		else
		{
			groundhogs.groundhogs = this.groundhogs.CloneStringDictionary();
			groundhogs.groundhogsSerialized = null;
			groundhogs.seenChoices = seenChoices.CloneDictionary();
			groundhogs.seenChoicesSerialized = null;
		}
		return groundhogs;
	}

	public static string GetGroundhogContents()
	{
		return FileManager.LoadFileString(filename, FileManager.documentsPath, warnFileMissing: false);
	}
}



[HarmonyPatch]
class GroundhogsPatches
{
	[HarmonyPatch(typeof(Groundhogs), "Load")]
	[HarmonyPrefix]
	public static bool LoadGroundhogsPrefix(Groundhogs __instance)
	{
		if (ArchipelagoClient.authenticated && ArchipelagoClient.serverData.seed != "" && ArchipelagoClient.serverData.seed != null) {
			ExopelagoGroundhogs.instance.LoadInner();
			__instance.groundhogs = ExopelagoGroundhogs.instance.groundhogs;
			return false;
		}
		return true;
	}
	
	[HarmonyPatch(typeof(Groundhogs), "Save")]
	[HarmonyPrefix]
	public static bool SaveGroundhogsPrefix(Groundhogs __instance)
	{
		if (ArchipelagoClient.authenticated && ArchipelagoClient.serverData.seed != "" && ArchipelagoClient.serverData.seed != null) {
			ExopelagoGroundhogs.instance.groundhogs = Princess.groundhogs;
			ExopelagoGroundhogs.Save();
			return false;
		}
		return true;
	}
}