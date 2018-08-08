using HamstarHelpers.Components.Config;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace TimeLimit {
	partial class TimeLimitMod : Mod {
		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-timelimit-mod"; } }

		public static string ConfigFileRelativePath {
			get { return JsonConfig.ConfigSubfolder + Path.DirectorySeparatorChar + TimeLimitConfigData.ConfigFileName; }
		}

		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( TimeLimitMod.Instance != null ) {
				if( !TimeLimitMod.Instance.ConfigJson.LoadFile() ) {
					TimeLimitMod.Instance.ConfigJson.SaveFile();
				}
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var new_config = new TimeLimitConfigData();
			//new_config.SetDefaults();

			TimeLimitMod.Instance.ConfigJson.SetData( new_config );
			TimeLimitMod.Instance.ConfigJson.SaveFile();
		}
	}
}
