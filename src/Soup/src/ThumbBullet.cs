using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class ThumbBullet : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRFireArmChamber), "EjectRound")]
		[HarmonyPostfix]
		public static void ThumbBullet_Chamber_Patch(FVRFireArmChamber __instance, ref FVRFireArmRound __result)
		{
			bool canThumb =
				   __instance.Firearm is BoltActionRifle
				|| __instance.Firearm is ClosedBoltWeapon 
				|| __instance.Firearm is Handgun;

			if (canThumb)
			{
				FVRInteractiveObject io = null;

				if (__instance.Firearm is BoltActionRifle)  io =  (__instance.Firearm as BoltActionRifle).BoltHandle;
				if (__instance.Firearm is ClosedBoltWeapon) io = (__instance.Firearm as ClosedBoltWeapon).Bolt;
				if (__instance.Firearm is Handgun)			io =		  (__instance.Firearm as Handgun).Slide;
				if (io.IsHeld)
				{
					bool activate = false;
					if (io.m_hand.IsInStreamlinedMode == false)
						activate = io.m_hand.Input.TouchpadPressed;
					else
						activate = io.m_hand.Input.AXButtonPressed;
					if (__instance.Firearm.Magazine != null && __result != null  && activate)
					{
						if (!__result.IsSpent)
						{
							if (__instance.Firearm.Magazine.m_numRounds < __instance.Firearm.Magazine.m_capacity)
							{
								__instance.Firearm.Magazine.AddRound(__result, true, true);
								Destroy(__result.gameObject);
							}
						}
					}
				}
			}
		}
		
		//Look Bro! My third transpile! Copy pasted- again! kinda. its better now
		//i failed the transpile. time for the safe harbor of prefixes
		[HarmonyPatch(typeof(ClosedBolt), nameof(ClosedBolt.UpdateInteraction))]
		[HarmonyPrefix]
		public static bool RemoveAntonsBoltLock(ClosedBolt __instance, ref FVRViveHand hand)
		{
			__instance.IsHeld = true;
			__instance.m_hand = hand;
			if (!__instance.m_hasTriggeredUpSinceBegin && __instance.m_hand.Input.TriggerFloat < 0.15f) { __instance.m_hasTriggeredUpSinceBegin = true; }
			if (__instance.triggerCooldown > 0f) { __instance.triggerCooldown -= Time.deltaTime; }
			
			if (__instance.HasRotatingPart)
			{
				Vector3 normalized = (__instance.transform.position - __instance.m_hand.PalmTransform.position).normalized;
				if (Vector3.Dot(normalized, __instance.transform.right) > 0f) { __instance.RotatingPart.localEulerAngles = __instance.RotatingPartLeftEulers; }
				else { __instance.RotatingPart.localEulerAngles = __instance.RotatingPartRightEulers; }
			}
			return false;
		}
		
	}
}