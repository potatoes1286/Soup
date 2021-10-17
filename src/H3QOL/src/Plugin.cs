using System;
using BepInEx;
using H3VRUtilsConfig.QOLPatches;
using HarmonyLib;
using Plugin;

namespace H3VRMod
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
	[BepInProcess("h3vr.exe")]
	public class Plugin : BaseUnityPlugin
	{
		public void Start()
		{
			Harmony.CreateAndPatchAll(typeof(DecockingRevolver));
			Harmony.CreateAndPatchAll(typeof(LaserPointerPatch));
			Harmony.CreateAndPatchAll(typeof(BoltBrace));
			Harmony.CreateAndPatchAll(typeof(BoltBrace_PlayerHeadLock));
			Harmony.CreateAndPatchAll(typeof(ThumbBullet));
		}
	}
}