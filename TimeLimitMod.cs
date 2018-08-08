using HamstarHelpers.Components.Config;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;


namespace TimeLimit {
	partial class TimeLimitMod : Mod {
		public static TimeLimitMod Instance { get; private set; }



		////////////////

		internal JsonConfig<TimeLimitConfigData> ConfigJson;
		public TimeLimitConfigData Config { get { return ConfigJson.Data; } }

		public ModLogic Logic = new ModLogic();


		////////////////

		public TimeLimitMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.ConfigJson = new JsonConfig<TimeLimitConfigData>( TimeLimitConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new TimeLimitConfigData() );
		}


		public override void Load() {
			TimeLimitMod.Instance = this;
			
			this.LoadConfigs();
		}

		private void LoadConfigs() {
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Time Limit updated to " + TimeLimitConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			TimeLimitMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string call_type = args[0] as string;
			if( args == null ) { throw new Exception( "Invalid call type." ); }

			var new_args = new object[args.Length - 1];
			Array.Copy( args, 1, new_args, 0, args.Length - 1 );

			return TimeLimitAPI.Call( call_type, new_args );
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			TimeLimitProtocolTypes protocol = (TimeLimitProtocolTypes)reader.ReadByte();

			if( RequestPackets.HandlePacket( this, protocol, reader, player_who ) ) {
				return;
			}
			if( SendPackets.HandlePacket( this, protocol, reader ) ) {
				return;
			}
			throw new Exception( "Unrecognized packet" );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx != -1 ) {
				GameInterfaceDrawMethod draw_method = delegate {
					var myworld = this.GetModWorld<TimeLimitWorld>();

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
