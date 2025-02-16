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
			if (!hand.IsInStreamlinedMode && !__instance.IsAltHeld && __instance.m_isStateToggled
				&& hand.Input.TouchpadDown && Vector2.Angle(hand.Input.TouchpadAxes, Vector2.up) < 45f)
			{
				__instance.CurChamber = __instance.PrevChamber;
				__instance.PlayAudioEvent(FirearmAudioEventType.FireSelector);
			}

			// So this activates the default CW action
			// So we do CCW twice!
			// And don't play the sound bc CW already does it
			// GENIUS!
			if (hand.IsInStreamlinedMode && hand.Input.AXButtonDown && hand.Input.BYButtonPressed
				&& !__instance.IsAltHeld && __instance.m_isStateToggled) {
				__instance.CurChamber = __instance.PrevChamber;
				__instance.CurChamber = __instance.PrevChamber;
			}
			return true;
		}
	}
}