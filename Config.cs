using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using System;
using Terraria;


namespace TimeLimit {
	public class TimeLimitConfigData : ConfigurationDataBase {
		public static string ConfigFileName => "TimeLimit Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool DebugModeInfo = false;
		public bool DebugModeNetInfo = false; 

		public string[] Afflictions = {};

		public int TimerDisplayX = -384;
		public int TimerDisplayY = -256;



		////////////////

		private void SetDefaults() {
			this.Afflictions = new string[] { "Potion Sickness", "Darkness", "Bleeding", "Weak", "Broken Armor", "Ichor", "Chaos State", "Stinky", "Creative Shock" };
		}


		////////////////

		public bool CanUpdateVersion() {
			if( this.VersionSinceUpdate == "" ) {
				return true;
			}

			var versSince = new Version( this.VersionSinceUpdate );
			bool canUpdate = versSince < TimeLimitMod.Instance.Version;
			
			return canUpdate;
		}

		public void UpdateToLatestVersion() {
			var mymod = TimeLimitMod.Instance;

			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = mymod.Version.ToString();
		}


		////////////////

		internal void LoadFromNetwork( string json ) {
			var mymod = TimeLimitMod.Instance;
			var myplayer = (TimeLimitPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "TimeLimitPlayer" );
			bool success;

			mymod.ConfigJson.DeserializeMe( json, out success );

			if( !success ) {
				LogHelpers.Warn( "Failed to load timer data from network." );
			}

			myplayer.FinishModSettingsSync();
		}
	}
}
