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
		public void Start()
		{
			EnableClearStabilization = Config.Bind("General Settings", "Clear Stabilization", true, "Hides your offhand if you're stabilizing a gun with it.");
			ClearStabilizationThreshhold = Config.Bind("General Settings", "Clear Stabilization Threshhold", 0.09f, "Distance, in metres, you must be from the hand holding the gun to activate clear stabilization.");
			//EnableAccurateQBslots = Config.Bind("General Settings", "Accurate QB Slots", true, "Removes QB slot delay. Can / does cause slowdown.");
			EnableEasyAttaching = Config.Bind("General Settings", "Easy Attaching", true, "Enables Easy Attaching");
			EnableQuickGrabbing = Config.Bind("General Settings", "Enable Quick Grabbing", true, "Enables Quick Grabbing");
			Harmony.CreateAndPatchAll(typeof(DecockingRevolver));
			Harmony.CreateAndPatchAll(typeof(LaserPointerPatch));
			Harmony.CreateAndPatchAll(typeof(BoltBrace));
			Harmony.CreateAndPatchAll(typeof(BoltBrace_PlayerHeadLock));
			Harmony.CreateAndPatchAll(typeof(ThumbBullet));
			//Harmony.CreateAndPatchAll(typeof(BetterQBslots));
			Harmony.CreateAndPatchAll(typeof(BoltActionRifleDecocking));
			Harmony.CreateAndPatchAll(typeof(ClearStabilization));
			Harmony.CreateAndPatchAll(typeof(BetterPanels));
			Harmony.CreateAndPatchAll(typeof(EasyAttaching));
			Harmony.CreateAndPatchAll(typeof(PalmRacking));
			//Harmony.CreateAndPatchAll(typeof(QuickerClip));
		}
	}
}