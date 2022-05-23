using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class BetterStabilization : MonoBehaviour
	{
		//TODO: CHANGE THIS TO A TRANSPILER SOMETIME
		[HarmonyPatch(typeof(FVRFireArm), "IsTwoHandStabilized")]
		[HarmonyPrefix]
		public static bool FVRFireArm_IsTwoHandStabilized_Patch(FVRFireArm __instance, ref bool __result)
		{
			if (!BepInExPlugin.BetterStabilization_IsEnabled.Value)
				return true;
			__result = false;
			if (__instance.m_hand != null && __instance.m_hand.OtherHand != null)
			{
				float dist = Vector3.Distance(__instance.m_hand.PalmTransform.position, __instance.m_hand.OtherHand.PalmTransform.position);
				if (dist < 0.15f)
					__result = true;
			}
			return false;
		}
	}
}