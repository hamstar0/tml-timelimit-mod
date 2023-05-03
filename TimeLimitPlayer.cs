using Terraria;
using Terraria.ModLoader;


namespace TimeLimit {
	partial class TimeLimitPlayer : ModPlayer {
		public bool HasSyncedModSettings;
		public bool HasSyncedModData;


		////////////////

		public override void Initialize() {
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
		}

		public override void clientClone( ModPlayer clientClone ) {
			var clone = (TimeLimitPlayer)clientClone;
			clone.HasSyncedModSettings = this.HasSyncedModSettings;
			clone.HasSyncedModData = this.HasSyncedModData;
		}

		// Required, do not remove.
		public override void SendClientChanges( ModPlayer clientPlayer ) {
			
		}


		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			var mymod = (TimeLimitMod)this.Mod;

			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.Player.whoAmI ) {
					this.OnConnectServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.Player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (TimeLimitMod)this.Mod;

			if( Main.netMode == 0 ) {   // NOT client
				this.OnConnectSingle();
			}
			if( Main.netMode == 1 ) {
				this.OnConnectCurrentClient();
			}
		}


		////////////////
		
		public override void PreUpdate() {
			if( this.Player.whoAmI == Main.myPlayer ) {
				if( Main.netMode == 1 ) {	// Client
					var myworld = ModContent.GetInstance<TimeLimitSystem>();
					myworld.Logic.Update();
				}
			}
		}
	}
}
