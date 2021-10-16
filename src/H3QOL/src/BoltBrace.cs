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
			if (__instance.transform.parent != GM.Instance.m_currentPlayerBody.Head.transform)
			{
				if (!__instance.IsHeld && !__instance.IsAltHeld && __instance.Bolt.CurPos == ClosedBolt.BoltPos.Rear)
				{
					__instance.SetIsKinematicLocked(true);
					__instance.transform.parent = GM.Instance.m_currentPlayerBody.Head.transform;
				}
			}
			else
			{
				if (__instance.IsHeld || __instance.IsAltHeld || __instance.Bolt.CurPos != ClosedBolt.BoltPos.Rear)
				{
					__instance.SetIsKinematicLocked(false);
					__instance.transform.parent = null;
				}
			}
			return true;
		}
	}
}