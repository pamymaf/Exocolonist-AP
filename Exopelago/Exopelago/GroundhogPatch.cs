using System.Reflection.Emit;
using System.Collections.Generic;
using HarmonyLib;

namespace Exopelago;

[HarmonyPatch(typeof(Groundhogs))]
public static class GroundhogsPatch
{
	[HarmonyPatch("LoadInner")]
	[HarmonyPatch("GetGroundhogContents")]
	[HarmonyPatch("SaveThread")]
	[HarmonyTranspiler]
	static IEnumerable<CodeInstruction> jsonTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		var codeMatcher = new CodeMatcher(instructions);
		codeMatcher.MatchForward(false,
			new CodeMatch(OpCodes.Ldstr, "Groundhogs.json", "fileName")
			)
			.SetInstructionAndAdvance(
				new CodeInstruction(OpCodes.Call, typeof(Exopelago.Archipelago.ArchipelagoData).GetMethod("get_GroundhogsFileName"))
			);
		return codeMatcher.Instructions();
	}
}