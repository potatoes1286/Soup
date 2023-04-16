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
		//Real talk now
		//i have zero fucking clue what this was supposed to do.
		//i definitely remember i added it for *some* reason, and that reason now eludes me.
		//unfortunately, its now causing problems, so i've removed it from being patched.
		//TL;DR this patch is not loaded because i made it and then forgot why i made it.
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