using FistVR;
using HarmonyLib;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace PotatoesSoup
{
	public class ThumbBullet : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRFireArmChamber), "EjectRound")]
		[HarmonyPostfix]
		public static void ThumbBullet_Chamber_Patch(FVRFireArmChamber __instance, ref FVRFireArmRound __result)
		{
			bool canThumb =
				   __instance.Firearm is BoltActionRifle
				|| __instance.Firearm is ClosedBoltWeapon 
				|| __instance.Firearm is Handgun;

			if (canThumb)
			{
				FVRInteractiveObject io = null;

				if (__instance.Firearm is BoltActionRifle)  io =  (__instance.Firearm as BoltActionRifle).BoltHandle;
				if (__instance.Firearm is ClosedBoltWeapon) io = (__instance.Firearm as ClosedBoltWeapon).Bolt;
				if (__instance.Firearm is Handgun)			io =		  (__instance.Firearm as Handgun).Slide;

				if (io.IsHeld)
				{
					if (__instance.Firearm.Magazine != null && __result != null  && io.m_hand.Input.TouchpadPressed)
					{
						if (!__result.IsSpent)
						{
							if (__instance.Firearm.Magazine.m_numRounds < __instance.Firearm.Magazine.m_capacity)
							{
								__instance.Firearm.Magazine.AddRound(__result, true, true);
								Destroy(__result.gameObject);
							}
						}
					}
				}
			}
		}
	}
}