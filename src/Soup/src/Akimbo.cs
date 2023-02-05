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
			for (int i = 0; i < GM.CurrentPlayerBody.QBSlots_Internal.Count; i++) {
				if (GM.CurrentPlayerBody.QBSlots_Internal[i].IsPointInsideMe(__instance.ReloadTriggerWell.transform.position)) {
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
				FVRFireArmMagazine nmag;
				if (mag.m_isSpawnLock) {
					nmag = UnityEngine.Object
					   .Instantiate<GameObject>(mag.ObjectWrapper.GetGameObject(), mag.Transform.position,
					                            mag.Transform.rotation).GetComponent<FVRFireArmMagazine>();
				}
				else {
					nmag = mag;
					mag.ClearQuickbeltState();
				}

				nmag.Load(__instance);
			}

			return true;
		}
	}
}