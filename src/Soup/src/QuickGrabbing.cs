using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class QuickGrabbing : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRInteractiveObject), "Poke")]
		[HarmonyPostfix]
		public static void QuickGrabbingPatch(FVRInteractiveObject __instance, ref FVRViveHand hand)
		{
			if (!BepInExPlugin.QuickGrabbing_IsEnabled.Value) return;
			if (hand.m_currentHoveredQuickbeltSlot != null) return;
			
			//if it's a bolt, and you hover over it with ur hand held, grab it
			//i sure do hope this is not an expensive call!
			if ((__instance is ClosedBolt					&& BepInExPlugin.QuickGrabbing_GrabBolts.Value)			||
			    (__instance is ClosedBoltHandle				&& BepInExPlugin.QuickGrabbing_GrabBolts.Value)			||
			    (__instance is OpenBoltReceiverBolt			&& BepInExPlugin.QuickGrabbing_GrabBolts.Value)			||
			    (__instance is OpenBoltChargingHandle		&& BepInExPlugin.QuickGrabbing_GrabBolts.Value)			||
			    (__instance is BoltActionRifle_Handle		&& BepInExPlugin.QuickGrabbing_GrabBolts.Value)			||
			    (__instance is TubeFedShotgunBolt			&& BepInExPlugin.QuickGrabbing_GrabBolts.Value)			||
			    (__instance is FVRFireArmMagazine			&& BepInExPlugin.QuickGrabbing_GrabMags.Value)			||
			    (__instance is Speedloader					&& BepInExPlugin.QuickGrabbing_GrabMags.Value)			||
			    (__instance is FVRFireArmClipInterface		&& BepInExPlugin.QuickGrabbing_FastClip.Value)			||
				(__instance is HandgunSlide					&& BepInExPlugin.QuickGrabbing_GrabPistolSlides.Value)	||
			    (__instance is FVRHandGrabPoint				&& BepInExPlugin.QuickGrabbing_GrabRopes.Value)			||
			    (__instance is PinnedGrenade				&& BepInExPlugin.QuickGrabbing_GrabGrenade.Value)		||
			    (__instance is FVRCappedGrenade				&& BepInExPlugin.QuickGrabbing_GrabGrenade.Value)		||
			    (__instance is SosigWeaponPlayerInterface	&& BepInExPlugin.QuickGrabbing_GrabSosigWeapon.Value)	||
				(__instance is TubeFedShotgunHandle			&& BepInExPlugin.QuickGrabbing_GrabFores.Value)			||
				(__instance is FVRAlternateGrip				&& BepInExPlugin.QuickGrabbing_GrabFores.Value)			||
				(__instance is FVRFireArmGrip				&& BepInExPlugin.QuickGrabbing_GrabFores.Value)			||
				(__instance is RPG7Foregrip					&& BepInExPlugin.QuickGrabbing_GrabFores.Value)			||
				(__instance is AttachableForegrip			&& BepInExPlugin.QuickGrabbing_GrabFores.Value)			||
			    (__instance is FVRFireArmTopCover			&& BepInExPlugin.QuickGrabbing_GrabFores.Value)			||
			    (__instance is FVRFireArmRound				&& BepInExPlugin.QuickGrabbing_GrabBullets.Value)) {
					QuickGrab(__instance, hand);
			}
		}

		public static void QuickGrab(FVRInteractiveObject __instance, FVRViveHand hand) {
			//when you grab fvraltgrip it redirects the hand to PrimaryObject
			//So fvraltgrip is never actually held
			//and quickgrab thinks that you can grab it when you shouldnt be able to
			//this fixes that, by checking the primary object's isheld instead of fvraltgrip's
			if (__instance is FVRAlternateGrip grip)
				if (grip.PrimaryObject.IsAltHeld)
					return;
			if(__instance is FVRFireArmMagazine mag)
				if (mag.State == FVRFireArmMagazine.MagazineState.Locked)
					return;
			
			//ensure not running to prevent accidental grabbing
			if (BepInExPlugin.QuickGrabbing_DisableWhenRunning.Value && IsArmSwinging(hand) && __instance is not FVRHandGrabPoint) return;
			
			//ensure other hand is not the same item
			if (__instance == hand.OtherHand.CurrentInteractable) return;
			
			//ensure bolt action rifle's bolt is not blocked
			if (__instance is BoltActionRifle_Handle && ((__instance as BoltActionRifle_Handle)!).Rifle.CanBoltMove() == false) return;
			
			//if clip inserted and mag is not full, do not quickgrab bolt
			if (__instance is BoltActionRifle_Handle) {
				var handle = __instance as BoltActionRifle_Handle;
				if (handle.Rifle.Clip != null && handle.Rifle.Magazine.m_capacity != handle.Rifle.Magazine.m_numRounds)
					return;
			} 

			if (hand.Input.IsGrabbing && hand.m_state == FVRViveHand.HandState.Empty)
			{
				if (__instance is SosigWeaponPlayerInterface)
					if ((__instance as SosigWeaponPlayerInterface)!.W.Type != SosigWeapon.SosigWeaponType.Grenade) return;
					
				if (BepInExPlugin.QuickGrabbing_RegrabBolt.Value && __instance is BoltActionRifle_Handle) { //doesnt feel good
					var bolt = __instance as BoltActionRifle_Handle;
					bolt.m_wasTPInitiated = true;
				}
					
				hand.Buzz(hand.Buzzer.Buzz_BeginInteraction);
				hand.ForceSetInteractable(__instance);
				__instance.BeginInteraction(hand);
			}
		}

		[HarmonyPatch(typeof(PinnedGrenade), "ReleaseLever")]
		[HarmonyPatch(typeof(FVRCappedGrenade), "CapRemoved")]
		[HarmonyPatch(typeof(SosigWeapon), "FuseGrenade")]
		[HarmonyPostfix]
		public static void GrenadeExtendRadius(FVRPhysicalObject __instance) {
			if (!BepInExPlugin.QuickGrabbing_IsEnabled.Value)
				return;
			SphereCollider trigger = __instance.gameObject.AddComponent<SphereCollider>();
			trigger.isTrigger = true;
			trigger.radius = BepInExPlugin.QuickGrabbing_GrabGrenadeRange.Value;
		}
		
		[HarmonyPatch(typeof(FVRFireArmClipInterface), "UpdateInteraction")]
		[HarmonyPrefix]
		public static bool LetGoOfClip(FVRFireArmClipInterface __instance) {
			if (!BepInExPlugin.QuickGrabbing_IsEnabled.Value || !BepInExPlugin.QuickGrabbing_FastClip.Value)
				return true;
			if (__instance.Clip.FireArm != null &&
			    __instance.Clip.FireArm.Magazine != null &&
			    __instance.Clip.FireArm.Magazine.IsFull()) {
				__instance.m_hand.ForceSetInteractable(null);
				return false;
			}
			return true;
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