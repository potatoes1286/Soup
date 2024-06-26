﻿using System;
using FistVR;
using H3VRUtils;
using H3VRUtils.UniqueCode;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class BaseGamePatch : MonoBehaviour
	{
		[HarmonyPatch(typeof(LaserPointer), "FVRUpdate")]
		[HarmonyPostfix]
		public static void LaserPointerPatch_Update_FixScale(LaserPointer __instance)
		{
			//get dist between player head and laser
			float num = Vector3.Distance(ManagerSingleton<GM>.Instance.m_currentPlayerBody.Head.transform.position,
										 __instance.BeamHitPoint.transform.position);
			float num2 = Mathf.Lerp(0.01f, 0.2f, num * 0.01f);
			__instance.BeamHitPoint.transform.localScale = new Vector3(num2, num2, num2);
		}

		[HarmonyPatch(typeof(FVRFireArm), "FireMuzzleSmoke", new Type[] {typeof(int)})]
		[HarmonyPrefix]
		public static bool FVRFireArm_FireMuzzleSmoke_FixOOB(FVRFireArm __instance, ref int i)
		{
			if (__instance.m_muzzleSystems.Count < i) return false;
			return true;
		}

		[HarmonyPatch(typeof(FVRPhysicalObject), "Awake")]
		[HarmonyPrefix]
		public static bool FVRFireArm_Awake_FixScaling(FVRPhysicalObject __instance)
		{
			if (__instance.ObjectWrapper == null) return true;
			if (__instance.ObjectWrapper.ItemID == "MadsenLAR" ||
				__instance.ObjectWrapper.ItemID == "MagazineMadsenLAR30rnd" ||
				__instance.ObjectWrapper.ItemID == "MagazineMadsenLAR20rnd" ||
				__instance.ObjectWrapper.ItemID == "MagazineMadsenLAR10rnd")
				__instance.transform.localScale = new Vector3(0.88f, 0.88f, 0.88f);
			if (__instance.ObjectWrapper.ItemID == "PanzerSchreck54")
				__instance.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
			return true;
		}
		
		[HarmonyPatch(typeof(FVRPhysicalObject), "Awake")]
		[HarmonyPrefix]
		public static bool FVRFireArm_Awake_AddMagDump(FVRPhysicalObject __instance)
		{
			if (__instance.ObjectWrapper == null) return true;
			if (__instance.ObjectWrapper.ItemID == "M1912" || __instance.ObjectWrapper.ItemID == "M1912P16")
			{
				var comp = __instance.gameObject.AddComponent<DumpInternalMag>();
				comp.presstoejectbutton = H3VRUtilsMagRelease.TouchpadDirType.Left;
				comp.handgun = __instance as Handgun;
			}

			return true;
		}
		
		// AUG patch
		// Makes dual-stage less sensitive so that you can actually single fire.
		[HarmonyPatch(typeof(ClosedBoltWeapon), "Awake")]
		[HarmonyPrefix]
		public static bool ClosedBoltWeapon_Awake_LessSensitiveDualStage(ClosedBoltWeapon __instance) {
			if (!BepInExPlugin.DualStageAlt_IsEnabled.Value)
				return true;
			if (__instance.ObjectWrapper == null || !__instance.UsesDualStageFullAuto) return true;
			__instance.TriggerDualStageThreshold = 1.1f;

			return true;
		}
		
		[HarmonyPatch(typeof(ClosedBoltWeapon), "UpdateInputAndAnimate")]
		[HarmonyPostfix]
		public static void ClosedBoltWeapon_UpdateInputAndAnimate_DualStageFire(ClosedBoltWeapon __instance, ref FVRViveHand hand) {
			if (!__instance.UsesDualStageFullAuto || !BepInExPlugin.DualStageAlt_IsEnabled.Value)
				return;
			ClosedBoltWeapon.FireSelectorModeType modeType = __instance.FireSelector_Modes[__instance.m_fireSelectorMode].ModeType;
			if (modeType != ClosedBoltWeapon.FireSelectorModeType.Safe && __instance.m_hasTriggeredUpSinceBegin) {
				if (__instance.Bolt.CurPos == ClosedBolt.BoltPos.Forward && modeType == ClosedBoltWeapon.FireSelectorModeType.FullAuto && hand.Input.TriggerPressed)
				{
					__instance.DropHammer();
					__instance.m_hasTriggerReset = false;
					if (__instance.m_CamBurst > 0)
					{
						__instance.m_CamBurst--;
					}
				}
			}
		}
		
		// Fix Latching for Break Action Weapons
		[HarmonyPatch(typeof(BreakActionWeapon), "FVRFixedUpdate")]
		[HarmonyPrefix]
		public static bool BreakActionWeapon_FVRFixedUpdate_PrefixFixLatching(BreakActionWeapon __instance) {
			__instance.m_latchRot *= 2;
			return true;
		}
		
		[HarmonyPatch(typeof(BreakActionWeapon), "FVRFixedUpdate")]
		[HarmonyPostfix]
		public static void BreakActionWeapon_FVRFixedUpdate_PostfixFixLatching(BreakActionWeapon __instance) {
			__instance.m_latchRot *= 0.5f;
		}


		// Bouncy bullets!
		[HarmonyPatch(typeof(FVRFireArmRound), "Awake")]
		[HarmonyPatch(typeof(FVRFireArmRound), "Fire")]
		[HarmonyPostfix]
		public static void FVRFireArmRound_Awake_Bounce(FVRFireArmRound __instance) {
			if (__instance.m_isSpent && BepInExPlugin.BulletBounce_IsEnabled.Value)
				foreach (Collider component in __instance.GetComponentsInChildren<Collider>())
					if (!component.isTrigger)
						component.material = BepInExPlugin.Bounce;
		}

		[HarmonyPatch(typeof(FVRFireArmChamber), "EjectRound", new Type[] {typeof(Vector3), typeof(Vector3), typeof(Vector3), typeof(bool)})]
		[HarmonyPatch(typeof(FVRFireArmChamber), "EjectRound", new Type[] {typeof(Vector3), typeof(Vector3), typeof(Vector3),typeof(Vector3),typeof(Quaternion), typeof(bool)})]
		[HarmonyPrefix]
		public static bool BoltActionRifle_AdaptiveEjectSpeed(FVRFireArmChamber __instance, ref Vector3 EjectionVelocity, ref Vector3 EjectionAngularVelocity) {
			if (!BepInExPlugin.AdaptiveEjectSpeed_IsEnabled.Value)
				return true;
			float modifier = 1;

			if (__instance.Firearm is BoltActionRifle) {
				var wep = (__instance.Firearm as BoltActionRifle);
				if (wep.BoltHandle.m_hand != null) {
					float speed = wep.BoltHandle.m_hand.Input.VelLinearWorld.magnitude;
					modifier = speed / 2;
				}
			} else if (__instance.Firearm is ClosedBoltWeapon) {
				var wep = (__instance.Firearm as ClosedBoltWeapon)!;
				if (!wep.HasHandle && wep.Bolt.m_hand != null) {
					float speed = wep.Bolt.m_hand.Input.VelLinearWorld.magnitude;
					modifier = speed / 3;
				} else if (wep.HasHandle && wep.Handle.m_hand != null) {
					float speed = wep.Handle.m_hand.Input.VelLinearWorld.magnitude;
					modifier = speed / 3;
				}
			} else if (__instance.Firearm is Handgun) {
				var wep = (__instance.Firearm as Handgun)!;
				if (wep.Slide.m_hand != null) {
					float speed = wep.Slide.m_hand.Input.VelLinearWorld.magnitude;
					modifier = speed / 2;
				}
			}else if (__instance.Firearm is OpenBoltReceiver) {
				var wep = (__instance.Firearm as OpenBoltReceiver)!;
				if (wep.Bolt.m_hand != null) {
					float speed = wep.Bolt.m_hand.Input.VelLinearWorld.magnitude;
					modifier = speed / 3;
				}
			} else if (__instance.Firearm is TubeFedShotgun) {
				var wep = (__instance.Firearm as TubeFedShotgun)!;
				if (wep.Bolt.m_hand != null) {
					float speed = wep.Bolt.m_hand.Input.VelLinearWorld.magnitude;
					modifier = speed / 3;
				} else if(wep.HasHandle && wep.Handle.m_hand != null) {
					float speed = wep.Handle.m_hand.Input.VelLinearWorld.magnitude;
						modifier = speed / 3;
				}
			}

			modifier = Math.Max(Math.Min(Math.Abs(modifier), 1f), 0.2f);
			EjectionVelocity *= modifier;
			EjectionAngularVelocity *= modifier;
			
			return true;
		}
		
		[HarmonyPatch(typeof(BoltActionRifle), "Awake")]
		[HarmonyPostfix]
		public static void BoltActionRifle_Awake_CorrectLoading(BoltActionRifle __instance) {
			if (!BepInExPlugin.CorrectLoading_IsEnabled.Value)
				return;
			if (__instance.Magazine != null)
				__instance.Chamber.IsManuallyChamberable = false;
		}
	}
}