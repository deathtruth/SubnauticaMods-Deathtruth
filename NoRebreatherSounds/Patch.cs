using HarmonyLib;

namespace NoRebreatherSounds
{
	[HarmonyPatch(typeof(BreathingSound), "Start")]
	public class BreathingSound_Start_Patch
	{
		[HarmonyPrefix]
		private static bool Prefix(BreathingSound __instance)
		{
			return false;
		}
	}
}