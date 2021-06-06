using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SilentBaseAmbience
{
	[HarmonyPatch(typeof(MapRoomFunctionality), "Start")]
	public static class MapRoomFunctionality_Start_Patch
	{

		public static List<MapRoomFunctionality> MapRoomFunctionalityList = new List<MapRoomFunctionality>();

		[HarmonyPostfix]
		private static void Postfix(MapRoomFunctionality __instance)
		{
			MapRoomFunctionalityList.Add(__instance);
			Main.Config.OnChangeSound();
		}
	}

	[HarmonyPatch(typeof(Base), "Start")]
	public static class Base_Start_Patch
	{
		public static List<GameObject> insideSoundsList = new List<GameObject>();

		[HarmonyPostfix]
		private static void Postfix(Base __instance)
		{
			insideSoundsList.Add(__instance.gameObject.FindChild("insideSounds"));
			Main.Config.OnChangeSound();
		}
	}
}