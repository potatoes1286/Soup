﻿using FistVR;
using HarmonyLib;
using UnityEngine;

namespace Plugin
{
	public class BetterQBslots : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "MoveContentsInstant")]
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "MoveContents")]
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "MoveContentsCheap")]
		[HarmonyPrefix]
		public static bool QBRemoveCode_Patch(FVRQuickBeltSlot __instance)
		{
			return false;
		}
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "Update")]
		[HarmonyPostfix]
		public static void QBMoveContentsPatch(FVRQuickBeltSlot __instance)
		{
			if (__instance.CurObject != null)
			{
				if (__instance.CurObject.IsHeld)
				{
					return;
				}
				__instance.CurObject.transform.position = __instance.PoseOverride.transform.position;

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
			}
		}
		
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "FixedUpdate")]
		[HarmonyPrefix]
		public static bool QBfuPatch(FVRQuickBeltSlot __instance)
		{
			return false;
		}
	}
}