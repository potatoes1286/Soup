using FistVR;
using HarmonyLib;
using UnityEngine;

namespace PotatoesSoup
{
	public class QuickerClip : MonoBehaviour
	{
		public static float clipejectdist = 0.12f;
		/*[HarmonyPatch(typeof(FVRFireArmClip), "Awake")]
		public static bool ClipReduceDist(FVRFireArmClip __instance)
		{
			__instance.EndInteractionDistance = clipejectdist;
			return true;
		}*/
		
		[HarmonyPatch(typeof(FVRInteractiveObject), "UpdateInteraction")]
		[HarmonyPrefix]
		public static bool RemoveClipCorrectly(FVRInteractiveObject __instance, ref FVRViveHand hand)
		{
			if (__instance is FVRFireArmClip)
			{
				var clip = __instance as FVRFireArmClip;
				float dist = Vector3.Distance(clip.transform.position, hand.transform.position);
				Debug.Log(dist);
				if (dist >= clipejectdist)
				{
					Debug.Log("Let me go!");
					hand.ForceSetInteractable(null);
					clip.Release();
					hand.ForceSetInteractable(__instance);
				}
			}
			return true;
		}
	}
}