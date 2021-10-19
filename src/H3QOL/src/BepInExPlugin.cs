using System;
using BepInEx;
using BepInEx.Configuration;
using H3VRUtilsConfig.QOLPatches;
using HarmonyLib;
using Plugin;

namespace Plugin
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
	[BepInProcess("h3vr.exe")]
	public class BepInExPlugin : BaseUnityPlugin
	{
		public static ConfigEntry<bool> EnableClearStabilization;
		public void Start()
		{
			Harmony.CreateAndPatchAll(typeof(DecockingRevolver));
			Harmony.CreateAndPatchAll(typeof(LaserPointerPatch));
			Harmony.CreateAndPatchAll(typeof(BoltBrace));
			Harmony.CreateAndPatchAll(typeof(BoltBrace_PlayerHeadLock));
			Harmony.CreateAndPatchAll(typeof(ThumbBullet));
			Harmony.CreateAndPatchAll(typeof(BetterQBslots));
			Harmony.CreateAndPatchAll(typeof(BoltActionRifleDecocking));
			Harmony.CreateAndPatchAll(typeof(ClearStabilization));
			EnableClearStabilization = Config.Bind("General Settings", "Clear Stabilization", true, "Hides your offhand if you're stabilizing a gun with it.");
		}
	}
}