using HamstarHelpers.Utilities.Config;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TimeLimit.NetProtocol;

namespace TimeLimit {
	class TimeLimitMod : Mod {
		public static TimeLimitMod Instance { get; private set; }
		
		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-timelimit-mod"; } }

		public static string ConfigFileRelativePath {
			get { return JsonConfig<TimeLimitConfigData>.RelativePath + Path.DirectorySeparatorChar + TimeLimitConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( TimeLimitMod.Instance != null ) {
				if( !TimeLimitMod.Instance.JsonConfig.LoadFile() ) {
					TimeLimitMod.Instance.JsonConfig.SaveFile();
				}
			}
		}



		////////////////

		internal JsonConfig<TimeLimitConfigData> JsonConfig;
		public TimeLimitConfigData Config { get { return JsonConfig.Data; } }


		////////////////

		public TimeLimitMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.JsonConfig = new JsonConfig<TimeLimitConfigData>( TimeLimitConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new TimeLimitConfigData() );
		}


		public override void Load() {
			TimeLimitMod.Instance = this;

			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_vers = new Version( 1, 2, 4 );
			if( hamhelpmod.Version < min_vers ) {
				throw new Exception( "Hamstar Helpers must be version " + min_vers.ToString() + " or greater." );
			}

			this.LoadConfigs();
		}

		private void LoadConfigs() {
			if( !this.JsonConfig.LoadFile() ) {
				this.JsonConfig.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Time Limit updated to " + TimeLimitConfigData.ConfigVersion.ToString() );
				this.JsonConfig.SaveFile();
			}
		}

		public override void Unload() {
			TimeLimitMod.Instance = null;
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {   // Client
				ClientPacketHandlers.HandlePacket( this, reader );
			} else if( Main.netMode == 2 ) {    // Server
				ServerPacketHandlers.HandlePacket( this, reader, player_who );
			}
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx != -1 ) {
				GameInterfaceDrawMethod draw_method = delegate {
					var myworld = this.GetModWorld<TimeLimitWorld>();
					if( !myworld.Logic.IsInitialized ) { return true; }

					myworld.Logic.DrawTimers( this, Main.spriteBatch );

					return true;
				};

				var interface_layer = new LegacyGameInterfaceLayer( "TimeLimit: Timers", draw_method,
					InterfaceScaleType.UI );

				layers.Insert( idx, interface_layer );
			}
		}
	}
}
