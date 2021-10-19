using FistVR;
using HarmonyLib;
using UnityEngine;
using ItemSpawnerUI = On.FistVR.ItemSpawnerUI;

namespace Plugin
{
	public class ThumbBullet : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRFireArmChamber), "EjectRound")]
		[HarmonyPostfix]
		public static void ThumbBullet_BAR_Patch(FVRFireArmChamber __instance, ref FVRFireArmRound __result)
		{
			bool canThumb = (__instance.Firearm is BoltActionRifle || __instance.Firearm is ClosedBoltWeapon);

			if (canThumb)
			{
				FVRInteractiveObject io = null;

				if (__instance.Firearm is BoltActionRifle) io = (__instance.Firearm as BoltActionRifle).BoltHandle;
				if(__instance.Firearm is ClosedBoltWeapon) io =(__instance.Firearm as ClosedBoltWeapon).Bolt;
				
				if (io.IsHeld)
				{
					if (io.m_hand.Input.TouchpadPressed)
					{
						if (__instance.Firearm.Magazine != null)
						{
							if (__result != null)
							{
								if (!__result.IsSpent)
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
}