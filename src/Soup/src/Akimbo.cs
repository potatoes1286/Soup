using System;
using System.Runtime.CompilerServices;
using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup {
	public class Akimbo {
		[HarmonyPatch(typeof(Handgun), "UpdateInteraction")]
		[HarmonyPrefix]
		public static bool Handgun_UpdateInteraction_AkimboReloading(Handgun __instance, ref FVRViveHand hand) {
			if (!BepInExPlugin.Akimbo_IsEnabled.Value || __instance.Magazine != null)
				return true;
			FVRQuickBeltSlot qbslot = null;
			Transform t;
			
			if (__instance.ReloadTriggerWell == null)
				t = hand.PoseOverride.transform;
			else
				t = __instance.ReloadTriggerWell.transform;
			
			for (int i = 0; i < GM.CurrentPlayerBody.QBSlots_Internal.Count; i++) {
				if (GM.CurrentPlayerBody.QBSlots_Internal[i].IsPointInsideMe(t.position)) {
					qbslot = GM.CurrentPlayerBody.QBSlots_Internal[i];
					break;
				}
			}

			if (qbslot == null || qbslot.CurObject == null || !(qbslot.CurObject is FVRFireArmMagazine))
				return true;

			FVRFireArmMagazine mag = qbslot.CurObject as FVRFireArmMagazine;

			if (mag.MagazineType != __instance.MagazineType)
				return true;

			if (BepInExPlugin.AkimboOneHand_IsEnabled.Value ||
			    (hand.OtherHand.CurrentInteractable != null && hand.OtherHand.CurrentInteractable is Handgun)) {
				FVRFireArmMagazine newMag;
				if (mag.m_isSpawnLock) {
					newMag = UnityEngine.Object
					   .Instantiate<GameObject>(mag.ObjectWrapper.GetGameObject(), mag.Transform.position,
					                            mag.Transform.rotation).GetComponent<FVRFireArmMagazine>();
					for (int i = 0; i < Mathf.Min(mag.LoadedRounds.Length, newMag.LoadedRounds.Length); i++)
					{
						if (mag.LoadedRounds[i] != null && mag.LoadedRounds[i].LR_Mesh != null)
						{
							newMag.LoadedRounds[i].LR_Class = mag.LoadedRounds[i].LR_Class;
							newMag.LoadedRounds[i].LR_Mesh = mag.LoadedRounds[i].LR_Mesh;
							newMag.LoadedRounds[i].LR_Material = mag.LoadedRounds[i].LR_Material;
							newMag.LoadedRounds[i].LR_ObjectWrapper = mag.LoadedRounds[i].LR_ObjectWrapper;
						}
					}
					newMag.m_numRounds = mag.m_numRounds;
					newMag.UpdateBulletDisplay();
				}
				else {
					newMag = mag;
					mag.ClearQuickbeltState();
				}

				newMag.Load(__instance);
			}

			return true;
		}
	}
}