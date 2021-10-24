using FistVR;
using HarmonyLib;
using UnityEngine;

namespace Plugin
{
	public class FlushBegone : MonoBehaviour
	{
		[HarmonyPatch(typeof(Debug), "Log")]
		[HarmonyPrefix]
		public static bool FlushBegone_Patch(Debug __instance, object message)
		{
			if (message.ToString() == "Flush") return false;
			return true;
		}
	}
}