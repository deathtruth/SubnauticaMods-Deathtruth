using HarmonyLib;

namespace SilentWalking
{
	[HarmonyPatch(typeof(FootstepSounds), "Start")]
	public static class FootstepSounds_Start_Patch
	{

		public static FMODAsset metalSound;
		public static FMODAsset landSound;
		public static FMODAsset precursorInteriorSound;
		public static FootstepSounds thisFootstepSounds;

		[HarmonyPrefix]
		private static bool Prefix(FootstepSounds __instance)
		{
			thisFootstepSounds = __instance;
			metalSound = __instance.metalSound;
			landSound = __instance.landSound;
			precursorInteriorSound = __instance.precursorInteriorSound;
			Main.Config.OnChangeMute();
			return true;
		}
	}
}