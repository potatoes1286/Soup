using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
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