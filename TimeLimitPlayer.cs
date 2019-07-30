using Terraria;
using Terraria.ModLoader;


namespace TimeLimit {
	partial class TimeLimitPlayer : ModPlayer {
		public bool HasSyncedModSettings;
		public bool HasSyncedModData;


		////////////////

		public override bool CloneNewInstances => false;

		public override void Initialize() {
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
		}

		public override void clientClone( ModPlayer clientClone ) {
			var clone = (TimeLimitPlayer)clientClone;
			clone.HasSyncedModSettings = this.HasSyncedModSettings;
			clone.HasSyncedModData = this.HasSyncedModData;
		}


		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			var mymod = (TimeLimitMod)this.mod;

			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					this.OnConnectServer();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (TimeLimitMod)this.mod;

			if( Main.netMode == 0 ) {   // NOT client
				this.OnConnectSingle();
			}
			if( Main.netMode == 1 ) {
				this.OnConnectCurrentClient();
			}
		}


		////////////////
		
		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {
				if( Main.netMode == 1 ) {	// Client
					var myworld = this.mod.GetModWorld<TimeLimitWorld>();
					myworld.Logic.Update();
				}
			}
		}
	}
}
