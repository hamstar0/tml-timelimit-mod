using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;
using Terraria.ModLoader;


namespace TimeLimit {
	partial class TimeLimitPlayer : ModPlayer {
		public bool HasSyncedModSettings;
		public bool HasSyncedModData;


		////////////////

		public override bool CloneNewInstances { get { return false; } }

		public override void Initialize() {
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
		}

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (TimeLimitPlayer)client_clone;
			clone.HasSyncedModSettings = this.HasSyncedModSettings;
			clone.HasSyncedModData = this.HasSyncedModData;
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			var mymod = (TimeLimitMod)this.mod;

			if( Main.netMode == 2 ) {
				if( to_who == -1 && from_who == this.player.whoAmI ) {
					this.OnServerConnect();
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (TimeLimitMod)this.mod;
			
			if( Main.netMode == 0 ) {   // Not server
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
					ErrorLogger.Log( "Time Limit config " + TimeLimitConfigData.ConfigVersion.ToString() + " created (ModPlayer.OnEnterWorld())." );
				}
			}

			if( Main.netMode == 0 ) {   // NOT client
				this.OnSingleConnect();
			}
			if( Main.netMode == 1 ) {
				this.OnClientConnect();
			}
		}


		////////////////
		
		public override void PreUpdate() {
			if( this.player.whoAmI == Main.myPlayer ) {
				if( Main.netMode == 1 ) {	// Client
					var myworld = this.mod.GetModWorld<TimeLimitWorld>();
					myworld.Logic.Update( (TimeLimitMod)this.mod );
				}
			}
		}
	}
}
