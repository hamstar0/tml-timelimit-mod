using System;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit {
	class TimeLimitPlayer : ModPlayer {
		public bool HasEnteredWorld;


		////////////////

		public override void Initialize() {
			this.HasEnteredWorld = false;
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (TimeLimitPlayer)client_clone;
			clone.HasEnteredWorld = this.HasEnteredWorld;
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (TimeLimitMod)this.mod;

			if( player.whoAmI == this.player.whoAmI ) {
				if( Main.netMode != 2 ) {   // Not server
					if( !mymod.JsonConfig.LoadFile() ) {
						mymod.JsonConfig.SaveFile();
						ErrorLogger.Log( "Time Limit config " + TimeLimitConfigData.ConfigVersion.ToString() + " created (ModPlayer.OnEnterWorld())." );
					}
				}

				if( Main.netMode == 1 ) {   // Client
					ClientPacketHandlers.SendModSettingsRequestFromClient( mymod );
					ClientPacketHandlers.SendTimersRequestFromClient( mymod );
				}
			}

			this.HasEnteredWorld = true;
		}

		
		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {
				if( Main.netMode == 1 ) {
					var myworld = this.mod.GetModWorld<TimeLimitWorld>();
					myworld.Logic.Update( (TimeLimitMod)this.mod );
				}
			}
		}
	}
}
