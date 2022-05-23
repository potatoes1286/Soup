using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace PotatoesSoup
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
	[BepInProcess("h3vr.exe")]
	public class BepInExPlugin : BaseUnityPlugin
	{
		#region Configs
		//general
		
		//other
		public const  string            SETTING_OTHER_NAME = "Other";
		public static ConfigEntry<bool> EasyAttaching_IsEnabled;
		public static ConfigEntry<bool> AccurateQBSlots_IsEnabled;
		private void SetConfig_Other()
		{
			//EnableAccurateQBslots = Config.Bind("General Settings", "Accurate QB Slots", true, "Removes QB slot delay. Can / does cause slowdown.");
			EasyAttaching_IsEnabled = Config.Bind(SETTING_OTHER_NAME, "Enable Easy Attaching", true, "Enables Easy Attaching");
		}

		//quick grabbing
		public const  string            SETTING_QG_NAME = "Quick Grabbing";
		public static ConfigEntry<bool> QuickGrabbing_IsEnabled;
		public static ConfigEntry<bool> QuickGrabbing_DisableWhenRunning;
		public static ConfigEntry<bool> QuickGrabbing_RegrabBolt;
		public static ConfigEntry<bool> QuickGrabbing_GrabFores;
		public static ConfigEntry<bool> QuickGrabbing_GrabPistolSlides;
		private void SetConfig_QuickGrabbing()
		{
			QuickGrabbing_IsEnabled = Config.Bind(SETTING_QG_NAME, "Is Enabled", true, "Enables Quick Grabbing");
			QuickGrabbing_DisableWhenRunning = Config.Bind(SETTING_QG_NAME, "Disable When Running", true, "When true, Quick Grab will be disabled when running in Armswinger to prevent accidental grabs.");
			QuickGrabbing_RegrabBolt = Config.Bind(SETTING_QG_NAME, "Enable Re-grabbing Gun When Quick Grabbing Bolt (Bolt Action Only)", false, "When true, if you close the bolt on a bolt action after quick-grabbing it, you will automatically regrab the gun.");
			QuickGrabbing_GrabFores = Config.Bind(SETTING_QG_NAME, "Enable Grabbing Foregrips", false, "Allows Quick Grabbing to apply to foregrips");
			QuickGrabbing_GrabPistolSlides = Config.Bind(SETTING_QG_NAME, "Enable Grabbing Pistol Slides", true, "Allows Quick Grabbing to apply to pistol slides");
		}
		
		//clear stab
		public const  string             SETTING_CS_NAME = "Clear Stabilization";
		public static ConfigEntry<bool>  ClearStab_IsEnabled;
		public static ConfigEntry<float> ClearStab_Treshhold;
		private void SetConfig_ClearStab()
		{
			ClearStab_IsEnabled = Config.Bind(SETTING_CS_NAME, "Is Enabled", true, "Hides your offhand if you're stabilizing a gun with it.");
			ClearStab_Treshhold = Config.Bind(SETTING_CS_NAME, "Treshhold", 0.09f, "Distance, in metres, you must be from the hand holding the gun to activate clear stabilization.");
		}

		public void SetConfigs()
		{
			SetConfig_Other();
			SetConfig_QuickGrabbing();
			SetConfig_ClearStab();
		}
		#endregion
		
		
		
		public void Start()
		{
			SetConfigs();
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
		internal const string VERSION = "4.0.1"; 
	}
}