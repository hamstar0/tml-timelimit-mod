using System;
using System.Collections.Generic;
using System.IO;
using ModLibsCore.Libraries.TModLoader.Mods;
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
			return ModBoilerplateLibraries.HandleModCall( typeof( TimeLimitAPI ), args );
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
			throw new InvalidOperationException( "Unrecognized packet" );
		}
	}
}
