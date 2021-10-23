using System;
using FistVR;
using HarmonyLib;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Plugin
{
	public class BoltBrace_PlayerHeadLock : MonoBehaviour
	{
		public static BoltBrace_PlayerHeadLock Instance;
		Transform transform1 = GM.CurrentPlayerBody.Head.transform;

		public void Update()
		{
			var transform2 = transform;
			transform2.position = transform1.position;
			transform2.rotation = transform1.rotation;
			var rot = transform2.localEulerAngles;
			rot.x = 0f;
			rot.z = 0f;
			transform2.localEulerAngles = rot;
		}

		[HarmonyPatch(typeof(FVRPlayerBody), "Start")]
		[HarmonyPrefix]
		public static bool SpawnPHL(FVRPlayerBody __instance)
		{
			var go = new GameObject();
			BoltBrace_PlayerHeadLock.Instance = go.AddComponent<BoltBrace_PlayerHeadLock>();
			return true;
		}
	}
}