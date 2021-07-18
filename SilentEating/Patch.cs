using HarmonyLib;

namespace SilentEating
{
	[HarmonyPatch(typeof(CraftData), "GetUseEatSound")]
	public static class CraftData_GetUseEatSound_Patch
	{

		[HarmonyPrefix]
		private static bool Prefix(CraftData __instance, ref string __result)
		{
			if (Main.Config.silentEating)
			{
				if (Main.Config.eatingSound != null && Main.Config.eatingSoundChoice != null)
				{
					Main.Config.eatingSound.TryGetValue(Main.Config.eatingSoundChoice,out string soundString);
					__result = soundString;
				}
				else
				{
					__result = "";
				}
				return false;
			}
            else
            {
				return true;
			}
		}
	}
}