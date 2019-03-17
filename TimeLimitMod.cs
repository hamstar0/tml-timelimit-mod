using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
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
			if( args.Length == 0 ) { throw new HamstarException( "Undefined call type." ); }

			string callType = args[0] as string;
			if( args == null ) { throw new HamstarException( "Invalid call type." ); }

			var newArgs = new object[args.Length - 1];
			Array.Copy( args, 1, newArgs, 0, args.Length - 1 );

			return TimeLimitAPI.Call( callType, newArgs );
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int playerWho ) {
			TimeLimitProtocolTypes protocol = (TimeLimitProtocolTypes)reader.ReadByte();

			if( RequestPackets.HandlePacket( protocol, reader, playerWho ) ) {
				return;
			}
			if( SendPackets.HandlePacket( protocol, reader ) ) {
				return;
			}
			throw new HamstarException( "Unrecognized packet" );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx != -1 ) {
				GameInterfaceDrawMethod drawMethod = delegate {
					var myworld = this.GetModWorld<TimeLimitWorld>();

					myworld.Logic.DrawTimers( Main.spriteBatch );

					return true;
				};

				var interfaceLayer = new LegacyGameInterfaceLayer( "TimeLimit: Timers", drawMethod,
					InterfaceScaleType.UI );

				layers.Insert( idx, interfaceLayer );
			}
		}
	}
}
