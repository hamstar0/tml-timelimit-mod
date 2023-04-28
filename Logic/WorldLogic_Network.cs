using Microsoft.Xna.Framework;
using Terraria;


namespace TimeLimit.Logic {
	partial class WorldLogic {
		public void StartTimerFromNetwork( TimeLimitMod mymod, int startDuration, int duration, string action, bool repeats, bool running, int totalTimers ) {
			if( !Main.LocalPlayer.TryGetModPlayer( out TimeLimitPlayer myPlayer ) ) {
				return;
			}

			this.StartTimer( startDuration, duration, action, repeats, running );

			myPlayer.CheckModDataSync( totalTimers );

			Main.NewText( "Timer started to perform action '" + action + "'" + ( repeats ? " repeatedly." : "." ), Color.Yellow );
		}
		
		public void StopTimersFromNetwork( string action ) {
			this.StopTimers( action );

			Main.NewText( "Timer '" + action + "' stopped.", Color.Yellow );

		}
		public void PauseTimersFromNetwork( string action ) {
			this.PauseTimers( action );

			Main.NewText( "Timer '" + action + "' paused.", Color.Yellow );

		}
		public void ResumeTimersFromNetwork( string action ) {
			this.ResumeTimers( action );

			Main.NewText( "Timer '" + action + "' resumed.", Color.Yellow );

		}


		public void StopAllTimersFromNetwork() {
			this.StopAllTimers();

			Main.NewText( "Timers stopped.", Color.Yellow );
		}
		
		public void PauseAllTimersFromNetwork() {
			this.PauseAllTimers();

			Main.NewText( "Timers paused.", Color.Yellow );
		}

		public void ResumeAllTimersFromNetwork() {
			this.ResumeAllTimers();

			Main.NewText( "Timers resumed.", Color.Yellow );
		}
	}
}
