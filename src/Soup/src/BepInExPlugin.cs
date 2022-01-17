using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace PotatoesSoup
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
	[BepInProcess("h3vr.exe")]
	public class BepInExPlugin : BaseUnityPlugin
	{
		public static ConfigEntry<bool> EnableClearStabilization;
		public static ConfigEntry<float> ClearStabilizationThreshhold;
		public static ConfigEntry<bool> EnableAccurateQBslots;
		public static ConfigEntry<bool> EnableEasyAttaching;
		public static ConfigEntry<bool> EnableQuickGrabbing;
		public static ConfigEntry<bool> DisableQuickGrabbingWhenRunning;
		public static ConfigEntry<bool> EnableInstaRegrabBoltActionOnQuickGrab;
		public void Start()
		{
			EnableClearStabilization = Config.Bind("General Settings", "Clear Stabilization", true, "Hides your offhand if you're stabilizing a gun with it.");
			ClearStabilizationThreshhold = Config.Bind("General Settings", "Clear Stabilization Threshhold", 0.09f, "Distance, in metres, you must be from the hand holding the gun to activate clear stabilization.");
			//EnableAccurateQBslots = Config.Bind("General Settings", "Accurate QB Slots", true, "Removes QB slot delay. Can / does cause slowdown.");
			EnableEasyAttaching = Config.Bind("General Settings", "Easy Attaching", true, "Enables Easy Attaching");
			EnableQuickGrabbing = Config.Bind("Quick Grabbing", "Enable Quick Grabbing", true, "Enables Quick Grabbing");
			DisableQuickGrabbingWhenRunning = Config.Bind("Quick Grabbing", "Disable Quick Grabbing On Running", true, "When true, Quick Grab will be disabled when running in Armswinger to prevent accidental grabs.");
			EnableInstaRegrabBoltActionOnQuickGrab = Config.Bind("Quick Grabbing", "Enable Regrab Bolt Action When Quickgrabbing Bolt", false, "When true, if you close the bolt on a bolt action after quick-grabbing it, you will automatically regrab the gun. Doesn't feel very nice, though.");
			Harmony.CreateAndPatchAll(typeof(DecockingRevolver));
			Harmony.CreateAndPatchAll(typeof(BaseGamePatch));
			Harmony.CreateAndPatchAll(typeof(BoltBrace));
			Harmony.CreateAndPatchAll(typeof(BoltBrace_PlayerHeadLock));
			Harmony.CreateAndPatchAll(typeof(ThumbBullet));
			//Harmony.CreateAndPatchAll(typeof(BetterQBslots));
			Harmony.CreateAndPatchAll(typeof(BoltActionRifleDecocking));
			Harmony.CreateAndPatchAll(typeof(ClearStabilization));
			Harmony.CreateAndPatchAll(typeof(BetterPanels));
			Harmony.CreateAndPatchAll(typeof(EasyAttaching));
			Harmony.CreateAndPatchAll(typeof(ExtractorHit));
			Harmony.CreateAndPatchAll(typeof(PalmRacking));
			Harmony.CreateAndPatchAll(typeof(QuickerClip));
			Harmony.CreateAndPatchAll(typeof(SingleActionCCing));
			//Harmony.CreateAndPatchAll(typeof(BetterAkimbo));
		}
	}
	
	internal static class PluginInfo
	{
		internal const string NAME = "Potatoes' Soup";
		internal const string GUID = "dll.potatoes1286.soup";
		internal const string VERSION = "3.1.0";
	}
}