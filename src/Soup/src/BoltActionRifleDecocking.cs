using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class BoltActionRifleDecocking : MonoBehaviour
	{
		[HarmonyPatch(typeof(BoltActionRifle), "UpdateInteraction")]
		[HarmonyPrefix]
		public static bool BoltActionRifle_Cocking_Patch(BoltActionRifle __instance, ref FVRViveHand hand)
		{
			if (__instance.RequiresHammerUncockedToToggleFireSelector &&
			    __instance.FireSelector_Modes[__instance.m_fireSelectorMode].ModeType ==
			    BoltActionRifle.FireSelectorModeType.Safe)
			{
				return true;
			}
			if (!__instance.IsHammerCocked)
			{
				if (Vector2.Angle(__instance.m_hand.Input.TouchpadAxes, Vector2.up) <= 45f && __instance.m_hand.Input.TouchpadDown && __instance.m_hand.Input.TouchpadAxes.magnitude > 0.2f)
				{
					__instance.CockHammer();
					__instance.PlayAudioEvent(FirearmAudioEventType.HandleForward);
				}
			}
			return true;
		}
		
		[HarmonyPatch(typeof(BoltActionRifle), "Fire")]
		[HarmonyPrefix]
		public static bool BoltActionRifle_Decocking_Patch(BoltActionRifle __instance)
		{
			if (__instance.IsHeld)
			{
				if (Vector2.Angle(__instance.m_hand.Input.TouchpadAxes, Vector2.up) <= 45f && __instance.m_hand.Input.TouchpadPressed && __instance.m_hand.Input.TouchpadAxes.magnitude > 0.2f)
				{
					return false;
				}
			}
			return true;
		}
	}
}