using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class ClearStabilization : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRFireArm), "IsTwoHandStabilized")]
		[HarmonyPrefix]
		public static bool ClearStabilization_Patch(FVRFireArm __instance)
		{
			if (!BepInExPlugin.EnableClearStabilization.Value || !GM.Options.QuickbeltOptions.HideControllerGeoWhenObjectHeld) return true;
			
			//i don't actually understand this if statement.
			if (__instance.m_hand != null && __instance.m_hand.OtherHand != null && (__instance.m_hand.OtherHand.CurrentInteractable == null || __instance.m_hand.OtherHand.CurrentInteractable is Flashlight || __instance.m_hand.OtherHand.CurrentInteractable is FVRFireArmMagazine))
			{
				float num = Vector3.Distance(__instance.m_hand.PalmTransform.position, __instance.m_hand.OtherHand.PalmTransform.position);
				if (num < BepInExPlugin.ClearStabilizationThreshhold.Value)
					__instance.m_hand.OtherHand.Display_Controller.SetActive(false);
				else if (num < BepInExPlugin.ClearStabilizationThreshhold.Value + 0.06f)
					ReDisplayViveHand(ref __instance.m_hand.OtherHand);
			}
			return true;
		}
		
		/*[HarmonyPatch(typeof(FVRViveHand), "CurrentInteractable_set")]
		[HarmonyPrefix]
		public static bool ClearStabilization_ResetOtherHand_Patch(FVRViveHand __instance)
		{
			ReDisplayViveHand(ref __instance.OtherHand);
			return true;
		}*/


		public static void ReDisplayViveHand(ref FVRViveHand hand)
		{
			if (hand.m_currentInteractable == null)
			{
				if (!hand.Display_InteractionSphere.activeSelf)
				{
					hand.Display_InteractionSphere.SetActive(true);
				}
				if (!hand.Display_InteractionSphere_Palm.activeSelf)
				{
					hand.Display_InteractionSphere_Palm.SetActive(true);
				}
			}
			else
			{
				if (hand.Display_InteractionSphere.activeSelf)
				{
					hand.Display_InteractionSphere.SetActive(false);
				}
				if (hand.Display_InteractionSphere_Palm.activeSelf)
				{
					hand.Display_InteractionSphere_Palm.SetActive(false);
				}
			}
			if (GM.Options.QuickbeltOptions.HideControllerGeoWhenObjectHeld)
			{
				if (hand.m_currentInteractable == null)
				{
					if (!hand.Display_Controller.activeSelf)
					{
						hand.Display_Controller.SetActive(true);
					}
				}
				else if (hand.Display_Controller.activeSelf)
				{
					hand.Display_Controller.SetActive(false);
				}
			}
			else if (!hand.Display_Controller.activeSelf)
			{
				hand.Display_Controller.SetActive(true);
			}
		}
	}
}