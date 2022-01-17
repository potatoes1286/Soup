using System;
using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class BaseGamePatch : MonoBehaviour
	{
		[HarmonyPatch(typeof(LaserPointer), "FVRUpdate")]
		[HarmonyPostfix]
		public static void LaserPointerPatch_Update_FixScale(LaserPointer __instance)
		{
			//get dist between player head and laser
			float num = Vector3.Distance(ManagerSingleton<GM>.Instance.m_currentPlayerBody.Head.transform.position,
										 __instance.BeamHitPoint.transform.position);
			float num2 = Mathf.Lerp(0.01f, 0.2f, num * 0.01f);
			__instance.BeamHitPoint.transform.localScale = new Vector3(num2, num2, num2);
		}

		[HarmonyPatch(typeof(FVRFireArm), "FireMuzzleSmoke", new Type[] {typeof(int)})]
		[HarmonyPrefix]
		public static bool FVRFireArm_FireMuzzleSmoke_FixOOB(FVRFireArm __instance, ref int i)
		{
			if (__instance.m_muzzleSystems.Count < i) return false;
			return true;
		}

		[HarmonyPatch(typeof(FVRPhysicalObject), "Awake")]
		[HarmonyPrefix]
		public static bool FVRFireArm_Awake_FixScaling(FVRPhysicalObject __instance)
		{
			if (__instance.ObjectWrapper == null) return true;
			if (__instance.ObjectWrapper.ItemID == "MadsenLAR" ||
				__instance.ObjectWrapper.ItemID == "MagazineMadsenLAR30rnd" ||
				__instance.ObjectWrapper.ItemID == "MagazineMadsenLAR20rnd" ||
				__instance.ObjectWrapper.ItemID == "MagazineMadsenLAR10rnd")
				__instance.transform.localScale = new Vector3(0.88f, 0.88f, 0.88f);
			if (__instance.ObjectWrapper.ItemID == "PanzerSchreck54")
				__instance.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
			return true;
		}
	}
}