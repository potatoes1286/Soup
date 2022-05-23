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
			if (!BepInExPlugin.QuickGrabbing_IsEnabled.Value) return;
			if (hand.m_currentHoveredQuickbeltSlot != null) return;
			
			//if it's a bolt, and you hover over it with ur hand held, grab it
			//i sure do hope this is not an expensive call!
			if ((__instance is ClosedBolt					&& BepInExPlugin.QuickGrabbing_GrabBolts.Value) ||
			    (__instance is ClosedBoltHandle				&& BepInExPlugin.QuickGrabbing_GrabBolts.Value) ||
			    (__instance is OpenBoltReceiverBolt			&& BepInExPlugin.QuickGrabbing_GrabBolts.Value) ||
			    (__instance is OpenBoltChargingHandle		&& BepInExPlugin.QuickGrabbing_GrabBolts.Value) ||
			    (__instance is BoltActionRifle_Handle		&& BepInExPlugin.QuickGrabbing_GrabBolts.Value) ||
			    (__instance is TubeFedShotgunBolt			&& BepInExPlugin.QuickGrabbing_GrabBolts.Value) ||
				(__instance is HandgunSlide					&& BepInExPlugin.QuickGrabbing_GrabPistolSlides.Value) ||
			    (__instance is FVRHandGrabPoint				&& BepInExPlugin.QuickGrabbing_GrabRopes.Value) ||
			    (__instance is PinnedGrenade				&& BepInExPlugin.QuickGrabbing_GrabGrenade.Value) ||
			    (__instance is FVRCappedGrenade				&& BepInExPlugin.QuickGrabbing_GrabGrenade.Value) ||
			    (__instance is SosigWeaponPlayerInterface	&& BepInExPlugin.QuickGrabbing_GrabSosigWeapon.Value)	 ||
				(__instance is TubeFedShotgunHandle			&& BepInExPlugin.QuickGrabbing_GrabFores.Value) ||
				(__instance is FVRAlternateGrip				&& BepInExPlugin.QuickGrabbing_GrabFores.Value) ||
				(__instance is FVRFireArmGrip				&& BepInExPlugin.QuickGrabbing_GrabFores.Value) ||
				(__instance is RPG7Foregrip					&& BepInExPlugin.QuickGrabbing_GrabFores.Value) ||
			 //(__instance is AttachableForegrip			&& BepInExPlugin.QuickGrabbing_GrabFores.Value) ||
			   (__instance is FVRFireArmTopCover			&& BepInExPlugin.QuickGrabbing_GrabFores.Value)) {
				//ensure not running to prevent accidental grabbing
				if (BepInExPlugin.QuickGrabbing_DisableWhenRunning.Value && IsArmSwinging(hand) && __instance is not FVRHandGrabPoint) return;
				//ensure other hand is not the same item
				if (__instance == hand.OtherHand.CurrentInteractable) return;
				//ensure bolt action rifle's bolt is not blocked
				if (__instance is BoltActionRifle_Handle && ((__instance as BoltActionRifle_Handle)!).Rifle.CanBoltMove() == false) return;
				if (hand.Input.IsGrabbing && hand.m_state == FVRViveHand.HandState.Empty)
				{
					if (__instance is SosigWeaponPlayerInterface)
						if ((__instance as SosigWeaponPlayerInterface)!.W.Type != SosigWeapon.SosigWeaponType.Grenade) return;
					
					if (BepInExPlugin.QuickGrabbing_RegrabBolt.Value && __instance is BoltActionRifle_Handle) { //doesnt feel good
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