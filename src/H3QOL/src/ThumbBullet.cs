using FistVR;
using HarmonyLib;
using UnityEngine;

namespace Plugin
{
	public class ThumbBullet : MonoBehaviour
	{
		[HarmonyPatch(typeof(ClosedBolt), "BoltEvent_EjectRound")]
		[HarmonyPrefix]
		public static bool ThumbBulletPatch(ClosedBolt __instance)
		{
			if (__instance.IsHeld)
			{
				if (__instance.m_hand.Input.TouchpadPressed)
				{
					if (__instance.Weapon.Magazine != null)
					{
						var round = __instance.Weapon.Chamber.EjectRound(Vector3.zero, Vector3.zero, Vector3.zero,
							false);
						if(round != null) __instance.Weapon.Magazine.AddRound(round, true, true);
						return false;
					}
				}
			}
			return true;
		}
		
		[HarmonyPatch(typeof(FVRFireArmChamber), "EjectRound")]
		[HarmonyPostfix]
		public static void ThumbBullet_BAR_Patch(FVRFireArmChamber __instance, ref FVRFireArmRound __result)
		{
			if (__instance.Firearm is BoltActionRifle)
			{
				var bar = __instance.Firearm as BoltActionRifle;
				if (bar.BoltHandle.IsHeld)
				{
					if (bar.BoltHandle.m_hand.Input.TouchpadPressed)
					{
						if (bar.Magazine != null)
						{
							if (__result != null)
							{
								bar.Magazine.AddRound(__result, true, true);
								Destroy(__result);
							}
						}
					}
				}
			}
		}
	}
}