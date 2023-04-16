﻿using System;
using System.Runtime.CompilerServices;
using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup {
	public class Akimbo {
		
		//I would like to state, for the record,
		//while i am a firm believer in DRY
		//it would take me more time to make, and maintain, a version of this that works for every weapon type
		//than it is to literally just copy + paste the same code 50% of the time.
		//In my defence, this is because of how H3VR is set up, not by my own will or want.
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

			if (BepInExPlugin.AkimboOneHand_IsEnabled.Value || hand.OtherHand.CurrentInteractable != null) {
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
		
		[HarmonyPatch(typeof(ClosedBoltWeapon), "UpdateInteraction")]
		[HarmonyPrefix]
		public static bool ClosedBoltWeapon_UpdateInteraction_AkimboReloading(ClosedBoltWeapon __instance, ref FVRViveHand hand) {
			if (!BepInExPlugin.AkimboAllWeapons_IsEnabled.Value) return true; //wheee
			if (!BepInExPlugin.Akimbo_IsEnabled.Value || __instance.Magazine != null)
				return true;
			FVRQuickBeltSlot qbslot = null;
			Transform t = __instance.MagazineMountPos.transform;
			if (t == null) return true; // there is like no circumstances where this should be an issue but i guarantee you it will be
			
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

			if (BepInExPlugin.AkimboOneHand_IsEnabled.Value || hand.OtherHand.CurrentInteractable != null) {
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
		
		[HarmonyPatch(typeof(OpenBoltReceiver), "UpdateInteraction")]
		[HarmonyPrefix]
		public static bool OpenBoltReceiver_UpdateInteraction_AkimboReloading(OpenBoltReceiver __instance, ref FVRViveHand hand) {
			if (!BepInExPlugin.AkimboAllWeapons_IsEnabled.Value) return true; //wheee
			if (!BepInExPlugin.Akimbo_IsEnabled.Value || __instance.Magazine != null)
				return true;
			FVRQuickBeltSlot qbslot = null;
			Transform t = __instance.MagazineMountPos.transform;
			if (t == null) return true; // there is like no circumstances where this should be an issue but i guarantee you it will be
			
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

			if (BepInExPlugin.AkimboOneHand_IsEnabled.Value || hand.OtherHand.CurrentInteractable != null) {
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