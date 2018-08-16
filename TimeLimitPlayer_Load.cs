using HamstarHelpers.Helpers.DebugHelpers;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit {
	partial class TimeLimitPlayer : ModPlayer {
		private void OnConnectSingle() {
			this.HasSyncedModSettings = true;
			this.HasSyncedModData = true;
		}

		private void OnConnectCurrentClient() {
			var mymod = (TimeLimitMod)this.mod;
			RequestPackets.RequestModSettings( mymod );
			RequestPackets.RequestTimers( mymod );
		}

		private void OnConnectServer() {
			this.HasSyncedModSettings = true;
			this.HasSyncedModData = true;
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
	}
}
