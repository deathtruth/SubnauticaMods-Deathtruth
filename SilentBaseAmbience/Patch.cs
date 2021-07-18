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
			Main.Config.OnChangeScannerRoom();
		}
	}

	[HarmonyPatch(typeof(Base), "Start")]
	public static class Base_Start_Patch
	{
		public static List<GameObject> InsideSoundsList = new List<GameObject>();

		[HarmonyPostfix]
		private static void Postfix(Base __instance)
		{
			InsideSoundsList.Add(__instance.gameObject.FindChild("insideSounds"));
			Main.Config.OnChangeInsideSounds();
		}
	}

	[HarmonyPatch(typeof(FiltrationMachine), "Start")]
	public static class FiltrationMachine_Start_Patch
	{
		public static List<FiltrationMachine> FiltrationMachineList = new List<FiltrationMachine>();

		[HarmonyPostfix]
		private static void Postfix(FiltrationMachine __instance)
		{
			FiltrationMachineList.Add(__instance);
		}
	}

	[HarmonyPatch(typeof(FiltrationMachine), "UpdateFiltering")]
	public static class FiltrationMachine_UpdateFiltering_Patch
	{

		[HarmonyPostfix]
		private static void Postfix(FiltrationMachine __instance)
		{
			Main.Config.OnChangeWaterFiltrationSounds();
		}
	}

	[HarmonyPatch(typeof(Player), "Start")]
	public static class Player_Start_Patch
	{
		public static Player playerObj;
		public static GameObject backgroundObj;

		[HarmonyPostfix]
		private static void Postfix(Player __instance)
		{
			playerObj = __instance;
			backgroundObj = GameObject.Find("/Player/SpawnPlayerSounds/PlayerSounds(Clone)/waterAmbience/background");
		}
	}

	[HarmonyPatch(typeof(Player), "UpdateIsUnderwater")]
	public static class Player_UpdateIsUnderwater_Patch
	{

		[HarmonyPostfix]
		private static void Postfix(Player __instance)
		{
			Main.Config.OnChangeBackgroundSounds();
		}
	}
}