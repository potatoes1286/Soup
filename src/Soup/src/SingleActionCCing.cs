using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class SingleActionCCing : MonoBehaviour
	{
		[HarmonyPatch(typeof(SingleActionRevolver), "UpdateInteraction")]
		[HarmonyPrefix]
		public static bool SingleActionRevolver_UpdateInteraction_EnableCCing(SingleActionRevolver __instance, ref FVRViveHand hand)
		{
			//streamlined users BTFO'd
			if (!__instance.IsAltHeld && __instance.m_isStateToggled && !__instance.IsAltHeld && !hand.IsInStreamlinedMode && hand.Input.TouchpadDown &&
				Vector2.Angle(hand.Input.TouchpadAxes, Vector2.up) < 45f)
			{
				__instance.CurChamber = __instance.PrevChamber;
				__instance.PlayAudioEvent(FirearmAudioEventType.FireSelector);
			}
			return true;
		}
	}
}