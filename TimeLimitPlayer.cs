using HamstarHelpers.DebugHelpers;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit {
	class TimeLimitPlayer : ModPlayer {
		public bool HasSyncedModSettings;
		public bool HasSyncedModData;
		

		////////////////

		public override void Initialize() {
			this.HasSyncedModSettings = false;
			this.HasSyncedModData = false;
		}

		////////////////

		public override void clientClone( ModPlayer client_clone ) {
			var clone = (TimeLimitPlayer)client_clone;
			clone.HasSyncedModSettings = this.HasSyncedModSettings;
			clone.HasSyncedModData = this.HasSyncedModData;
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (TimeLimitMod)this.mod;

			if( player.whoAmI == Main.myPlayer ) {
				if( Main.netMode == 0 ) {   // Not server
					if( !mymod.JsonConfig.LoadFile() ) {
						mymod.JsonConfig.SaveFile();
						ErrorLogger.Log( "Time Limit config " + TimeLimitConfigData.ConfigVersion.ToString() + " created (ModPlayer.OnEnterWorld())." );
					}
				}

				if( Main.netMode == 1 ) {
					RequestPackets.RequestModSettings( mymod );
					RequestPackets.RequestTimers( mymod );
				}
				if( Main.netMode != 1 ) {   // NOT client
					this.HasSyncedModSettings = true;
					this.HasSyncedModData = true;
				}
			}
		}

		////////////////

		public void FinishModSettingsSync() {
			this.HasSyncedModSettings = true;
		}

		public void CheckModDataSync( int total_timer_count ) {
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();
			int current_timer_count = myworld.Logic.Timers.Count;

			this.HasSyncedModData = current_timer_count == total_timer_count;

			if( current_timer_count > total_timer_count ) {
				LogHelpers.Log( "Timers exceeds server's indicated amount." );
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
