using FistVR;
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
			//if it's a bolt, and you hover over it with ur hand held, grab it
			if (__instance is ClosedBolt ||
			    __instance is ClosedBoltHandle ||
			    __instance is OpenBoltReceiverBolt ||
			    __instance is OpenBoltChargingHandle ||
			    __instance is BoltActionRifle_Handle ||
			    __instance is HandgunSlide)
			{
				if (hand.Input.GripPressed && hand.CurrentInteractable == null)
				{
					hand.ForceSetInteractable(__instance);
				}
			}
		}
	}
}