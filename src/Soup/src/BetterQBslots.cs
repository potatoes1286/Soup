﻿using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class BetterQBslots : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "MoveContentsInstant")]
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "MoveContents")]
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "MoveContentsCheap")]
		[HarmonyPrefix]
		public static bool QBRemoveCode_Patch(FVRQuickBeltSlot __instance, ref Vector3 dir)
		{
			if(BepInExPlugin.AccurateQBSlots_IsEnabled.Value && __instance.m_isKeepingTrackWithHead) return false;
			if (__instance.CurObject != null)
			{
				if (__instance.CurObject.IsHeld)
				{
					return false;
				}
				else
				{
					FVRInteractiveObject bolt = null;
					if (__instance.CurObject is ClosedBoltWeapon) bolt = (__instance.CurObject as ClosedBoltWeapon).Bolt;
					if (__instance.CurObject is BoltActionRifle) bolt = (__instance.CurObject as BoltActionRifle).BoltHandle;
					if (__instance.CurObject is Handgun) bolt = (__instance.CurObject as Handgun).Slide;
					//this checks if the curobj is considered bolt braced
					if(bolt != null && bolt.IsHeld) return false;
				}
				__instance.CurObject.transform.position = __instance.CurObject.transform.position + dir;
				__instance.CurObject.RootRigidbody.velocity = Vector3.zero;
			}
			return false;
		}
		
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "FixedUpdate")]
		[HarmonyPrefix]
		public static bool QBfuPatch(FVRQuickBeltSlot __instance)
		{
			if(BepInExPlugin.AccurateQBSlots_IsEnabled.Value && __instance.m_isKeepingTrackWithHead) return false;
			return true;
		}

		/*[HarmonyPatch(typeof(FVRPhysicalObject), "SetQuickBeltSlot")]
		[HarmonyPostfix]
		public static void QBChildSelf_Patch(FVRPhysicalObject __instance)
		{
			if (BepInExPlugin.EnableAccurateQBslots.Value)
			{
				if (__instance.m_quickbeltSlot != null)
				{
					__instance.transform.SetParent(__instance.m_quickbeltSlot.PoseOverride);
				}
			}
		}
		
		[HarmonyPatch(typeof(FVRPhysicalObject), "SetQuickBeltSlot")]
		[HarmonyPrefix]
		public static bool QBChildSelfPrefix_Patch(FVRPhysicalObject __instance, ref FVRQuickBeltSlot slot)
		{
			if (BepInExPlugin.EnableAccurateQBslots.Value)
			{
				if (__instance.m_quickbeltSlot != null && slot == null) __instance.transform.SetParent(null);
			}
			return true;
		}*/
		
		/*[HarmonyPatch(typeof(FVRPhysicalObject), "SetQuickBeltSlot")]
		[HarmonyPostfix]
		public static void QBUnChildSelf_Patch(FVRPhysicalObject __instance)
		{
			Debug.Log("2");
			if (BepInExPlugin.EnableAccurateQBslots.Value)
				if (__instance.m_quickbeltSlot != null) __instance.SetParentage(__instance.m_quickbeltSlot.PoseOverride);
		}*/
		
		/*[HarmonyPatch(typeof(FVRQuickBeltSlot), "Awake")]
		[HarmonyPostfix]
		public static void QBMoveWithHead_Patch(FVRQuickBeltSlot __instance)
		{
			if (BepInExPlugin.EnableAccurateQBslots.Value)
			{
				if (__instance.IsPlayer &&
				    __instance.m_isKeepingTrackWithHead &&
				    __instance.Shape == FVRQuickBeltSlot.QuickbeltSlotShape.Sphere)
				{
					__instance.transform.parent = GM.CurrentPlayerBody.Head;
				}
			}
		}*/

		[HarmonyPatch(typeof(FVRQuickBeltSlot), "Update")]
		[HarmonyPostfix]
		public static void QBMoveContentsPatch(FVRQuickBeltSlot __instance)
		{
			if (BepInExPlugin.AccurateQBSlots_IsEnabled.Value && __instance.m_isKeepingTrackWithHead)
			{
				if (__instance.CurObject != null)
				{
					if (__instance.CurObject.IsHeld)
					{
						return;
					}

					if (__instance.IsPlayer &&
					    __instance.CurObject.DoesQuickbeltSlotFollowHead &&
					    __instance.Shape == FVRQuickBeltSlot.QuickbeltSlotShape.Sphere)
					{
						if (__instance.CurObject.DoesQuickbeltSlotFollowHead)
						{
							Vector3 forward = Vector3.ProjectOnPlane(GM.CurrentPlayerBody.Head.transform.forward,
								__instance.transform.right);
							__instance.PoseOverride.rotation =
								Quaternion.LookRotation(forward, GM.CurrentPlayerBody.Head.transform.up);
						}
						else
						{
							__instance.PoseOverride.localRotation = Quaternion.identity;
						}
					}
					
					if (__instance.CurObject.IsHeld)
					{
						return;
					}
					else
					{
						FVRInteractiveObject bolt = null;
						if (__instance.CurObject is ClosedBoltWeapon) bolt = (__instance.CurObject as ClosedBoltWeapon).Bolt;
						if (__instance.CurObject is BoltActionRifle) bolt = (__instance.CurObject as BoltActionRifle).BoltHandle;
						if (__instance.CurObject is Handgun) bolt = (__instance.CurObject as Handgun).Slide;
						//this checks if the curobj is considered bolt braced
						if(bolt != null && bolt.IsHeld) return;
					}
					
					__instance.CurObject.transform.position = __instance.PoseOverride.transform.position;
					__instance.CurObject.transform.rotation = __instance.PoseOverride.transform.rotation;
				}
			}
		}
	}
}