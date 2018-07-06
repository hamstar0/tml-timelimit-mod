using HamstarHelpers.Components.Config;
using HamstarHelpers.DebugHelpers;
using System;
using Terraria;


namespace TimeLimit {
	public class TimeLimitConfigData : ConfigurationDataBase {
		public static Version ConfigVersion { get { return new Version(1, 1, 6); } }
		public static string ConfigFileName { get { return "TimeLimit Config.json"; } }


		////////////////

		public string VersionSinceUpdate = TimeLimitConfigData.ConfigVersion.ToString();

		public bool DebugModeInfo = false;
		public bool DebugModeNetInfo = false; 

		public string[] Afflictions = { "Potion Sickness", "Darkness", "Bleeding", "Weak", "Broken Armor", "Ichor", "Chaos State", "Stinky", "Creative Shock" };

		public int TimerDisplayX = -384;
		public int TimerDisplayY = -256;



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new TimeLimitConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= TimeLimitConfigData.ConfigVersion ) {
				return false;
			}

			this.VersionSinceUpdate = TimeLimitConfigData.ConfigVersion.ToString();

			return true;
		}


		internal void LoadFromNetwork( TimeLimitMod mymod, string json ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<TimeLimitPlayer>();
			bool success;

			mymod.ConfigJson.DeserializeMe( json, out success );

			if( !success ) {
				LogHelpers.Log( "TimeLimitConfigData.LoadFromNetwork - Failed to load timer data from network." );
			}

			myplayer.FinishModSettingsSync();
		}
	}
}
