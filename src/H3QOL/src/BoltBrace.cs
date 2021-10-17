using FistVR;
using HarmonyLib;
using UnityEngine;

namespace Plugin
{
	public class BoltBrace : MonoBehaviour
	{
		[HarmonyPatch(typeof(ClosedBoltWeapon), "FVRUpdate")]
		[HarmonyPrefix]
		public static bool BoltBracePatch(ClosedBoltWeapon __instance)
		{
			if (__instance.transform.parent != BoltBrace_PlayerHeadLock.Instance.transform)
			{
				//is not braced- braced
				if (!__instance.IsHeld && !__instance.IsAltHeld && __instance.Bolt.CurPos == ClosedBolt.BoltPos.Rear && __instance.Bolt.IsHeld)
				{
					__instance.SetIsKinematicLocked(true);
					__instance.transform.parent = BoltBrace_PlayerHeadLock.Instance.transform;
					__instance.gameObject.AddComponent(typeof(BoltBrace_UnKinematicLock));
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
	}
}