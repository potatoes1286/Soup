using System.Linq;
using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class ExtractorHit
	{
		[HarmonyPatch(typeof(RevolverEjector), "FVRUpdate")]
		[HarmonyPostfix]
		public static void RevolverEjector_FVRUpdate_HitEjector(RevolverEjector __instance)
		{
			if (__instance.Magnum.IsHeld && !__instance.Magnum.m_isCylinderArmLocked)
			{
				FVRViveHand otherHand = __instance.Magnum.m_hand.OtherHand;
				if (otherHand.CurrentInteractable is Speedloader) if (!(otherHand.CurrentInteractable as Speedloader).Chambers.Any(x => x.IsLoaded)) return;
				float dist = Vector3.Distance(__instance.transform.position, otherHand.Input.Pos);
				float dotdist = Vector3.Dot(-__instance.transform.forward, otherHand.Input.VelLinearWorld.normalized);
				if (dist < 0.035f && dotdist > 0.4f)
				{
					float magnitude = otherHand.Input.VelLinearWorld.magnitude;
					if (magnitude > 1f)
					{
						if (__instance.Ejector != null)
						{
							__instance.Ejector.localPosition = __instance.RearPos;
						}
						__instance.Magnum.EjectChambers();
					}
				}
			}
		}
	}
}