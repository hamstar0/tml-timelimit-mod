using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace TimeLimit {
	partial class TimeLimitMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-timelimit-mod";

		public static string ConfigFileRelativePath {
			get { return JsonConfig.ConfigSubfolder + Path.DirectorySeparatorChar + TimeLimitConfigData.ConfigFileName; }
		}

		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reload configs outside of single player." );
			}
			if( TimeLimitMod.Instance != null ) {
				if( !TimeLimitMod.Instance.ConfigJson.LoadFile() ) {
					TimeLimitMod.Instance.ConfigJson.SaveFile();
				}
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new HamstarException( "Cannot reset to default configs outside of single player." );
			}

			var newConfig = new TimeLimitConfigData();
			//newConfig.SetDefaults();

			TimeLimitMod.Instance.ConfigJson.SetData( newConfig );
			TimeLimitMod.Instance.ConfigJson.SaveFile();
		}
	}
}
