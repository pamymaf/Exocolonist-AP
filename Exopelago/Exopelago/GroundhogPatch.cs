using System.Reflection.Emit;
using System.Collections.Generic;
using HarmonyLib;
using System;
using System.IO;
using Exopelago.Archipelago;

namespace Exopelago;

//[HarmonyPatch(typeof(Groundhogs))]
//public static class GroundhogsPatch
//{
//	[HarmonyPatch("LoadInner")]
//	[HarmonyPatch("GetGroundhogContents")]
//	[HarmonyPatch("SaveThread")]
//	[HarmonyPrefix]
//	static void Prefix()
//	{
//		string documents = FileManager.documentsPath;
//		string path = Path.Combine(documents, ArchipelagoData.GroundhogsFileName);
//		if (!File.Exists(path)) {
//			using (StreamWriter sw = File.AppendText(path)) {
//				sw.WriteLine("{}");
//			}
//		}
//		path = Path.Combine(documents, ArchipelagoData.GroundhogsFileNameBackup);
//		if (!File.Exists(path)) {
//			using (StreamWriter sw = File.AppendText(path)) {
//				sw.WriteLine("{}");
//			}
//		}
//	}
//	[HarmonyPatch("LoadInner")]
//	[HarmonyPatch("GetGroundhogContents")]
//	[HarmonyPatch("SaveThread")]
//	[HarmonyTranspiler]
//	static IEnumerable<CodeInstruction> jsonTranspiler(IEnumerable<CodeInstruction> instructions)
//	{
//		var codeMatcher = new CodeMatcher(instructions);
//		codeMatcher.MatchForward(false,
//			new CodeMatch(OpCodes.Ldstr, "Groundhogs.json", "fileName")
//			)
//			.SetInstructionAndAdvance(
//				new CodeInstruction(OpCodes.Call, typeof(Exopelago.Archipelago.ArchipelagoData).GetMethod("get_GroundhogsFileName"))
//			);
//		return codeMatcher.Instructions();
//	}
//
//	[HarmonyPatch("LoadInner")]
//	[HarmonyTranspiler]
//	static IEnumerable<CodeInstruction> bakTranspiler(IEnumerable<CodeInstruction> instructions)
//	{
//		var codeMatcher = new CodeMatcher(instructions);
//		codeMatcher.MatchForward(false,
//			new CodeMatch(OpCodes.Ldstr, "Groundhogs.bak", "fileName")
//			)
//			.SetInstructionAndAdvance(
//				new CodeInstruction(OpCodes.Call, typeof(Exopelago.Archipelago.ArchipelagoData).GetMethod("get_GroundhogsFileNameBackup"))
//			);
//		return codeMatcher.Instructions();
//	}
//}