using HarmonyLib;
using UnityEngine;
using SMLHelper.V2.Utility;

namespace AdjustableFireExtinguisher
{
	[HarmonyPatch(typeof(FireExtinguisher), "Update")]
	public static class FireExtinguisher_Update_Patch
	{
		private static MyConfig localConfigRef = AdjustableFireExtinguisher.Config;

		[HarmonyPrefix]
		private static bool PreFix(FireExtinguisher __instance)
		{
			if (Input.GetKey(localConfigRef.adjustKeyCode) && Input.mouseScrollDelta.y > 0f && localConfigRef.feForce < 50f)
			{
				localConfigRef.feForce += 1;
				if (__instance.fuel > 0f && !localConfigRef.muteSound)
				{
					AudioUtils.PlaySound("QMods/AdjustableFireExtinguiser/Assets/airLeaking.ogg");
				}
				localConfigRef.Save();
			}
			if (Input.GetKey(localConfigRef.adjustKeyCode) && Input.mouseScrollDelta.y < 0f && localConfigRef.feForce > 1f)
			{
				localConfigRef.feForce -= 1;
				if (__instance.fuel > 0f && !localConfigRef.muteSound)
				{
					AudioUtils.PlaySound("QMods/AdjustableFireExtinguiser/Assets/airLeaking.ogg");
				}
				localConfigRef.Save();
			}

			__instance.expendFuelPerSecond = 0.7f * localConfigRef.feForce;
			__instance.fireDousePerSecond = 4f * localConfigRef.feForce;
			if (AvatarInputHandler.main.IsEnabled() && Player.main.GetRightHandHeld() && __instance.isDrawn)
			{
				__instance.usedThisFrame = true;
			}
			else
			{
				__instance.usedThisFrame = false;
			}
			
			__instance.UpdateTarget();
			if (__instance.usedThisFrame && __instance.fuel > 0f)
			{
				int num = Player.main.isUnderwater.value ? 1 : 0;
				if (num != __instance.lastUnderwaterValue)
				{
					__instance.lastUnderwaterValue = num;
					if (__instance.fmodIndexInWater < 0)
					{
						__instance.fmodIndexInWater = __instance.soundEmitter.GetParameterIndex("in_water");
					}
					__instance.soundEmitter.SetParameterValue(__instance.fmodIndexInWater, (float)num);
				}
				for (int i = 0; i < __instance.fxControl.emitters.Length; i++)
				{
					ParticleSystem.MainModule mainModule = __instance.fxControl.emitters[i].fxPS.main;
					mainModule.startSizeMultiplier = Mathf.Sqrt(localConfigRef.feForce) / 2;
					mainModule.startSpeedMultiplier = Mathf.Sqrt(localConfigRef.feForce);

				}
				if (Player.main.IsUnderwater())
				{
					Player.main.GetComponent<UnderwaterMotor>().rb.AddForce(-MainCamera.camera.transform.forward * localConfigRef.feForce, ForceMode.Impulse);
				}
				else
                {
					Player.main.GetComponent<PlayerMotor>().rb.AddForce(-MainCamera.camera.transform.forward * localConfigRef.feForce, ForceMode.Acceleration);
				}
				float douseAmount = __instance.fireDousePerSecond * Time.deltaTime;
				float expendAmount;
				/*if (!configInstance.unlimitedFuel)
				{*/
				expendAmount = __instance.expendFuelPerSecond * Time.deltaTime;
				expendAmount = (__instance.fuel > 0f && __instance.fuel < expendAmount) ? __instance.fuel : expendAmount;
                /*}
                else
                {
					__instance.fuel = 100f;
				}*/
				__instance.UseExtinguisher(douseAmount, expendAmount);
				float newPitch = Scale(1f, 50f, 1f, 4f, localConfigRef.feForce);
				__instance.soundEmitter.GetEventInstance().setPitch(newPitch);
				__instance.soundEmitter.Play();
			}
			else
			{
				__instance.soundEmitter.Stop();
				if (__instance.fxControl != null)
				{
					__instance.fxControl.Stop(0);
					__instance.fxIsPlaying = false;
				}
			}
			__instance.UpdateImpactFX();
			return false;
		}

		public static float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
		{

			float OldRange = (OldMax - OldMin);
			float NewRange = (NewMax - NewMin);
			float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

			return (NewValue);
		}
	}

	[HarmonyPatch(typeof(FireExtinguisher), "GetFuelValueText")]
	public static class FireExtinguisher_GetFuelValueText_Patch
	{
		private static MyConfig localConfigRef = AdjustableFireExtinguisher.Config;

		[HarmonyPrefix]
		private static bool PreFix(FireExtinguisher __instance, ref string __result)
		{
			int num = Mathf.FloorToInt(__instance.fuel);
			if (__instance.lastFuelStringValue != num || __instance.fuel < 0.01f)
			{
				float arg = __instance.fuel / __instance.maxFuel;
				__instance.cachedFuelString = Language.main.GetFormat<float>("FuelPercent", arg);
				__instance.lastFuelStringValue = num;
			}
			__result = __instance.cachedFuelString + "\nRegulator: "+ localConfigRef.feForce + " bar";
			return false;
		}
	}

	[HarmonyPatch(typeof(QuickSlots), "SlotNext")]
	public static class QuickSlots_SlotNext_Patch
	{
		private static MyConfig localConfigRef = AdjustableFireExtinguisher.Config;

		[HarmonyPrefix]
		private static bool Prefix(QuickSlots __instance)
		{
			if (Input.GetKey(localConfigRef.adjustKeyCode) && __instance.heldItem.item.GetTechType().AsString() == "FireExtinguisher")
			{
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(QuickSlots), "SlotPrevious")]
	public static class QuickSlots_SlotPrevious_Patch
	{
		private static MyConfig localConfigRef = AdjustableFireExtinguisher.Config;

		[HarmonyPrefix]
		private static bool Prefix(QuickSlots __instance)
		{
			if (Input.GetKey(localConfigRef.adjustKeyCode) && __instance.heldItem.item.GetTechType().AsString() == "FireExtinguisher")
			{
				return false;
			}
			return true;
		}
	}
}