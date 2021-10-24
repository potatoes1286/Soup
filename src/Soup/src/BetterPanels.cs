using System;
using FistVR;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugin
{
	public class BetterPanels : MonoBehaviour
	{
		[HarmonyPatch(typeof(FVRWristMenu), "SpawnOptionsPanel")]
		[HarmonyPrefix]
		public static bool LockPanelOnSpawn_Patch(FVRWristMenu __instance)
		{
			if (GM.CurrentOptionsPanel == null)
			{
				GameObject currentOptionsPanel = Instantiate(__instance.OptionsPanelPrefab, Vector3.zero, Quaternion.identity);
				GM.CurrentOptionsPanel = currentOptionsPanel;
				currentOptionsPanel.GetComponent<FVRPhysicalObject>().SetIsKinematicLocked(true);
			}
			return true;
		}
	}
}