using FistVR;
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
		public static bool QBMoveContentsPatch(FVRQuickBeltSlot __instance)
		{
			if (__instance.CurObject != null)
			{
				if (__instance.CurObject.IsHeld)
				{
					return false;
				}
				__instance.CurObject.RootRigidbody.position = __instance.PoseOverride.transform.position;
				__instance.CurObject.RootRigidbody.velocity = Vector3.zero;
			}
			return false;
		}
		
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "FixedUpdate")]
		[HarmonyPrefix]
		public static bool QBfuPatch(FVRQuickBeltSlot __instance)
		{
			return false;
		}
		
		[HarmonyPatch(typeof(FVRQuickBeltSlot), "Update")]
		[HarmonyPrefix]
		public static bool QBuPatch(FVRQuickBeltSlot __instance)
		{
			if (__instance.IsPlayer && __instance.CurObject != null && __instance.CurObject.DoesQuickbeltSlotFollowHead && __instance.Shape == FVRQuickBeltSlot.QuickbeltSlotShape.Sphere)
			{
				if (__instance.CurObject.DoesQuickbeltSlotFollowHead)
				{
					Vector3 forward = Vector3.ProjectOnPlane(GM.CurrentPlayerBody.Head.transform.forward, __instance.transform.right);
					__instance.PoseOverride.rotation = Quaternion.LookRotation(forward, GM.CurrentPlayerBody.Head.transform.up);
				}
				else
				{
					__instance.PoseOverride.localRotation = Quaternion.identity;
				}
			}
			return true;
		}
	}
}