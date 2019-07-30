using HamstarHelpers.Helpers.Debug;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit {
	partial class TimeLimitPlayer : ModPlayer {
		private void OnConnectSingle() {
			this.HasSyncedModSettings = true;
			this.HasSyncedModData = true;
		}

		private void OnConnectCurrentClient() {
			RequestPackets.RequestTimers();
		}

		private void OnConnectServer() {
			this.HasSyncedModSettings = true;
			this.HasSyncedModData = true;
		}


		////////////////

		public void FinishModSettingsSync() {
			this.HasSyncedModSettings = true;
		}

		public void CheckModDataSync( int totalTimerCount ) {
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();
			int currentTimerCount = myworld.Logic.Timers.Count;

			this.HasSyncedModData = currentTimerCount == totalTimerCount;

			if( currentTimerCount > totalTimerCount ) {
				LogHelpers.Log( "Timers exceeds server's indicated amount." );
			}
		}
	}
}
