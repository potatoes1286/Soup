﻿using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class PalmRacking : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRInteractiveObject), "Poke")]
		[HarmonyPostfix]
		public static void PalmRacking_Patch(FVRInteractiveObject __instance, ref FVRViveHand hand)
		{
			if (!BepInExPlugin.EnableQuickGrabbing.Value) return;
			//if it's a bolt, and you hover over it with ur hand held, grab it
			if (__instance is ClosedBolt ||
			    __instance is ClosedBoltHandle ||
			    __instance is OpenBoltReceiverBolt ||
			    __instance is OpenBoltChargingHandle ||
			    __instance is BoltActionRifle_Handle ||
			    __instance is HandgunSlide ||
			    __instance is FVRHandGrabPoint ||
			    __instance is PinnedGrenade ||
			    __instance is FVRCappedGrenade ||
			    __instance is SosigWeaponPlayerInterface)
			{
				if (hand.Input.IsGrabbing && hand.m_state == FVRViveHand.HandState.Empty)
				{
					if (__instance is SosigWeaponPlayerInterface)
						if ((__instance as SosigWeaponPlayerInterface)!.W.Type != SosigWeapon.SosigWeaponType.Grenade) return;
					if (__instance is FVRHandGrabPoint)
						if (hand.OtherHand.m_currentInteractable is FVRHandGrabPoint) return;
					/*if (__instance is BoltActionRifle_Handle) { //doesnt feel good
						var bolt = __instance as BoltActionRifle_Handle;
						bolt.m_wasTPInitiated = true;
					}*/
					__instance.BeginInteraction(hand);
					hand.ForceSetInteractable(__instance);
				}
			}
		}
	}
}