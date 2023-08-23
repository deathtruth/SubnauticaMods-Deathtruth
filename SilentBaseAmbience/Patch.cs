using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SilentBaseAmbience
{
	[HarmonyPatch(typeof(MapRoomFunctionality), "Start")]
	public static class MapRoomFunctionality_Start_Patch
	{

		public static List<MapRoomFunctionality> mapRoomList = new List<MapRoomFunctionality>();

		[HarmonyPostfix]
		private static void Postfix(MapRoomFunctionality __instance)
		{
			if (Main.Config.muteScannerRoom)
			{
				__instance.ambientSound.Stop();
			}
			mapRoomList.Add(__instance);
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
			Main.Config.OnChangeInsideSounds();
		}
	}

	[HarmonyPatch(typeof(MapRoomFunctionality), "Update")]
	public static class MapRoomFunctionality_Update_Patch
	{

		[HarmonyPostfix]
		private static void Postfix(MapRoomFunctionality __instance)
		{
            if (Main.Config.muteScannerRoom && __instance.ambientSound.playing)
            {
				__instance.ambientSound.Stop();
            }
			else if (!Main.Config.muteScannerRoom && !__instance.ambientSound.playing && (__instance.CheckIsPowered() || __instance.forcePoweredIfNoRelay))
            {
				__instance.ambientSound.Play();
			}
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

		public static bool playerInside;

		[HarmonyPostfix]
		private static void Postfix(Player __instance)
		{
			Main.Config.UpdateIsUnderwater();
		}
	}

	[HarmonyPatch(typeof(Creature), "Start")]
	public static class Creature_Start_Patch
	{
		public static List<Reefback> reefbackList = new List<Reefback>();
		//public static List<SandShark> sandSharkList = new List<SandShark>();
		public static List<Stalker> stalkerList = new List<Stalker>();
		public static List<GasoPod> gasoPodList = new List<GasoPod>();

		[HarmonyPostfix]
		private static void Postfix(Creature __instance)
		{
            if (__instance.GetComponentInParent<Reefback>())
            {
				reefbackList.Add(__instance.GetComponentInParent<Reefback>());
				if (Player_Start_Patch.playerObj != null && Player_Start_Patch.playerObj.IsInBase())
				{
					Main.Config.ChangeReefbackVolume(Main.Config.reefbackVolumeInside);
				}
                else
                {
					Main.Config.ChangeReefbackVolume(Main.Config.reefbackVolumeOutside);
				}
			}
			/*if (__instance.GetComponentInParent<SandShark>())
			{
				sandSharkList.Add(__instance.GetComponentInParent<SandShark>());
				if (Player_Start_Patch.playerObj.IsInBase())
				{
					Main.Config.ChangeSandSharkVolume(Main.Config.sandSharkVolumeInside);
				}
				else
				{
					Main.Config.ChangeSandSharkVolume(Main.Config.sandSharkVolumeOutside);
				}
			}*/
			if (__instance.GetComponentInParent<Stalker>())
			{
				stalkerList.Add(__instance.GetComponentInParent<Stalker>());
				if (Player_Start_Patch.playerObj != null && Player_Start_Patch.playerObj.IsInBase())
				{
					Main.Config.ChangeStalkerVolume(Main.Config.stalkerVolumeInside);
				}
				else
				{
					Main.Config.ChangeStalkerVolume(Main.Config.stalkerVolumeOutside);
				}
			}
			if (__instance.GetComponentInParent<GasoPod>())
			{
				gasoPodList.Add(__instance.GetComponentInParent<GasoPod>());
				Main.Config.OnchangeGasoPodSounds();
			}
		}
	}

	[HarmonyPatch(typeof(GasPod), "Start")]
	public static class GasPod_Start_Patch
	{

		[HarmonyPrefix]
		private static void Prefix(GasPod __instance)
		{
			if ((Main.Config.muteGasoPodSoundsOutside && !Player_Start_Patch.playerObj.IsInBase()) || (Main.Config.muteGasoPodSoundsInside && Player_Start_Patch.playerObj.IsInBase()))
			{
				__instance.releaseSound = null;
				__instance.burstSound = null;
			}
		}
	}
}