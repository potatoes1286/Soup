using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class EasyAttaching : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRPhysicalObject), "FVRFixedUpdate")]
		[HarmonyPostfix]
		public static void EasyAttach_Patch(FVRPhysicalObject __instance)
		{
			//If the player is holding an object and an attachment,
			//if the attachment is then near a mount of the object held in the other hand, (tolerance: 15cm)
			//then disable all colliders. otherwise, enable them.
			if (!BepInExPlugin.EasyAttaching_IsEnabled.Value) return;
			if (__instance is FVRFireArmAttachment)
			{
				var item = __instance as FVRFireArmAttachment;
				if (__instance.IsHeld)
				{
					if (__instance.m_hand.OtherHand.CurrentInteractable != null)
					{
						FVRPhysicalObject fvrfireArm =
							__instance.m_hand.OtherHand.CurrentInteractable as FVRPhysicalObject;
						if (fvrfireArm == null) return;
						bool nearAttachmentPoint = false;
						foreach (var mount in fvrfireArm.AttachmentMounts)
						{
							float num = Vector3.Distance(mount.transform.position,
								item.Sensor.transform.position);
							if (num <= 0.15f) nearAttachmentPoint = true;
						}
						
						if (nearAttachmentPoint)
						{
							foreach (Collider collider2 in __instance.m_colliders)
							{
								if (collider2 != null && !collider2.isTrigger && collider2.gameObject.layer == LayerMask.NameToLayer("Default"))
								{
									collider2.gameObject.layer = LayerMask.NameToLayer("NoCol");
								}
							}
						}
						else
						{
							foreach (Collider collider2 in __instance.m_colliders)
							{
								if (collider2 != null && !collider2.isTrigger && collider2.gameObject.layer == LayerMask.NameToLayer("NoCol"))
								{
									collider2.gameObject.layer = LayerMask.NameToLayer("Default");
								}
							}
						}
					}
				}
			}
		}
	}
}