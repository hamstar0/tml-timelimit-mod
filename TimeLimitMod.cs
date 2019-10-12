using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.TModLoader.Mods;
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

		public TimeLimitConfig Config => ModContent.GetInstance<TimeLimitConfig>();

		public ModLogic Logic = new ModLogic();


		////////////////

		public TimeLimitMod() {
			TimeLimitMod.Instance = this;
		}


		public override void Load() {
		}

		public override void Unload() {
			TimeLimitMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( TimeLimitAPI ), args );
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
			throw new ModHelpersException( "Unrecognized packet" );
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx != -1 ) {
				GameInterfaceDrawMethod drawMethod = delegate {
					var myworld = ModContent.GetInstance<TimeLimitWorld>();

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
