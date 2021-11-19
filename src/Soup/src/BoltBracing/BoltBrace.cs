using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class BoltBrace : MonoBehaviour
	{

		#region ClosedBoltWeapon Bracing
		[HarmonyPatch(typeof(ClosedBoltWeapon), "FVRUpdate")]
		[HarmonyPrefix]
		public static bool BoltBrace_ClosedBolt_Patch(ClosedBoltWeapon __instance)
		{
			if (__instance.transform.parent != BoltBrace_PlayerHeadLock.Instance.transform)
			{
				//is not braced- braced
				if    (!__instance.IsHeld
				    && !__instance.IsAltHeld
				    && __instance.Bolt.CurPos == ClosedBolt.BoltPos.Rear
				    && __instance.Bolt.IsHeld
					&& !__instance.IsKinematicLocked)
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
		#endregion

		#region Handgun Bracing
		[HarmonyPatch(typeof(Handgun), "FVRUpdate")]
		[HarmonyPrefix]
		public static bool BoltBrace_Handgun_Patch(Handgun __instance)
		{
			if (__instance.transform.parent != BoltBrace_PlayerHeadLock.Instance.transform)
			{
				//is not braced- braced
				if    (!__instance.IsHeld
				    && !__instance.IsAltHeld
				    && __instance.Slide.CurPos == HandgunSlide.SlidePos.Rear
				    && __instance.Slide.IsHeld
				    && !__instance.IsKinematicLocked)
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
				if (__instance.IsHeld || __instance.IsAltHeld || __instance.Slide.CurPos != HandgunSlide.SlidePos.Rear || !__instance.Slide.IsHeld)
				{
					__instance.transform.parent = null;
					__instance.SetIsKinematicLocked(false);
				}
			}
			return true;
		}
		#endregion
		
		#region BoltActionRifle Bracing
		[HarmonyPatch(typeof(BoltActionRifle), "FVRFixedUpdate")]
		[HarmonyPrefix]
		public static bool BoltBrace_BoltAction_Patch(BoltActionRifle __instance)
		{
			if (__instance.transform.parent != BoltBrace_PlayerHeadLock.Instance.transform)
			{
				//is not braced- braced
				if    (!__instance.IsHeld
				    && !__instance.IsAltHeld
				    && __instance.CurBoltHandleState == BoltActionRifle_Handle.BoltActionHandleState.Rear
				    && __instance.BoltHandle.IsHeld
				    && !__instance.IsKinematicLocked)
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
		#endregion
		
		//prevents the bolt from doing funky stuff (apparently just holding the bolt ejects the clip in it???)
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