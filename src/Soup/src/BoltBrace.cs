using FistVR;
using HarmonyLib;
using UnityEngine;

namespace Plugin
{
	public class BoltBrace : MonoBehaviour
	{
		[HarmonyPatch(typeof(ClosedBoltWeapon), "FVRUpdate")]
		[HarmonyPrefix]
		public static bool BoltBrace_ClosedBolt_Patch(ClosedBoltWeapon __instance)
		{
			if (__instance.transform.parent != BoltBrace_PlayerHeadLock.Instance.transform)
			{
				//is not braced- braced
				if (!__instance.IsHeld && !__instance.IsAltHeld && __instance.Bolt.CurPos == ClosedBolt.BoltPos.Rear && __instance.Bolt.IsHeld)
				{
					if (__instance.IsShoulderStabilized())
					{
						__instance.SetIsKinematicLocked(true);
						__instance.transform.parent = BoltBrace_PlayerHeadLock.Instance.transform;
						__instance.gameObject.AddComponent(typeof(BoltBrace_UnKinematicLock));
					}
				}
			}
			else
			{
				//is braced- unbrace
				if (__instance.IsHeld || __instance.IsAltHeld || __instance.Bolt.CurPos != ClosedBolt.BoltPos.Rear || !__instance.Bolt.IsHeld)
				{
					__instance.transform.parent = null;
					__instance.SetIsKinematicLocked(false);
				}
			}
			return true;
		}
		
		[HarmonyPatch(typeof(BoltActionRifle), "FVRFixedUpdate")]
		[HarmonyPrefix]
		public static bool BoltBrace_BoltAction_Patch(BoltActionRifle __instance)
		{
			if (__instance.transform.parent != BoltBrace_PlayerHeadLock.Instance.transform)
			{
				//is not braced- braced
				if (!__instance.IsHeld && !__instance.IsAltHeld && __instance.CurBoltHandleState == BoltActionRifle_Handle.BoltActionHandleState.Rear && __instance.BoltHandle.IsHeld)
				{
					if (__instance.IsShoulderStabilized())
					{
						__instance.SetIsKinematicLocked(true);
						__instance.transform.parent = BoltBrace_PlayerHeadLock.Instance.transform;
						__instance.gameObject.AddComponent(typeof(BoltBrace_UnKinematicLock));
					}
				}
			}
			else
			{
				//is braced- unbrace
				if (__instance.IsHeld || __instance.IsAltHeld || __instance.CurBoltHandleState != BoltActionRifle_Handle.BoltActionHandleState.Rear || !__instance.BoltHandle.IsHeld)
				{
					__instance.transform.parent = null;
					__instance.SetIsKinematicLocked(false);
				}
			}
			return true;
		}
		
		[HarmonyPatch(typeof(FVRFireArm), "EjectClip")]
		[HarmonyPrefix]
		public static bool BoltBrace_BoltAction_Patch_Fix(FVRFireArm __instance)
		{
			if (__instance is BoltActionRifle)
			{
				//var bar = __instance as BoltActionRifle;
				if (__instance.transform.parent == BoltBrace_PlayerHeadLock.Instance.transform)
				{
					return false;
				}
			}
			return true;
		}
	}
}