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
			if (!BepInExPlugin.EnableQuickGrabbing.Value) return;
			if (hand.m_currentHoveredQuickbeltSlot != null) return;
			
			//if it's a bolt, and you hover over it with ur hand held, grab it
			if (__instance is ClosedBolt ||
			    __instance is ClosedBoltHandle ||
			    __instance is OpenBoltReceiverBolt ||
			    __instance is OpenBoltChargingHandle ||
			    __instance is BoltActionRifle_Handle ||
			    __instance is HandgunSlide ||
			    __instance is TubeFedShotgunBolt ||
			    __instance is TubeFedShotgunHandle ||
			    __instance is FVRHandGrabPoint ||
			    __instance is PinnedGrenade ||
			    __instance is FVRCappedGrenade ||
			    __instance is SosigWeaponPlayerInterface ||
			    __instance is FVRAlternateGrip ||
			    __instance is FVRFireArmGrip ||
				__instance is FVRFireArmTopCover ||
				__instance is TubeFedShotgunBolt) {
				//ensure not running to prevent accidental grabbing
				if (BepInExPlugin.DisableQuickGrabbingWhenRunning.Value && IsArmSwinging(hand) && __instance is not FVRHandGrabPoint) return;
				//ensure other hand is not the same item
				if (__instance == hand.OtherHand.CurrentInteractable) return;
				
				if (hand.Input.IsGrabbing && hand.m_state == FVRViveHand.HandState.Empty)
				{
					if (__instance is SosigWeaponPlayerInterface)
						if ((__instance as SosigWeaponPlayerInterface)!.W.Type != SosigWeapon.SosigWeaponType.Grenade) return;
					
					if (BepInExPlugin.EnableInstaRegrabBoltActionOnQuickGrab.Value && __instance is BoltActionRifle_Handle) { //doesnt feel good
						var bolt = __instance as BoltActionRifle_Handle;
						bolt.m_wasTPInitiated = true;
					}
					__instance.BeginInteraction(hand);
					hand.ForceSetInteractable(__instance);
				}
			}
		}

		public static bool IsArmSwinging(FVRViveHand hand)
		{
			if (hand.MovementManager.Mode == FVRMovementManager.MovementMode.Armswinger)
			{
				if (hand.IsInStreamlinedMode) {
					if (hand.CMode == ControlMode.Index || hand.CMode == ControlMode.WMR)
						return hand.Input.Secondary2AxisNorthPressed;
					else return hand.Input.TouchpadNorthPressed;
				}
				else return hand.Input.BYButtonPressed;
			}
			return false;
		}
	}
}