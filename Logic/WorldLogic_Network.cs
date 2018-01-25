using Microsoft.Xna.Framework;
using Terraria;


namespace TimeLimit.Logic {
	partial class WorldLogic {
		public void StartTimerFromNetwork( TimeLimitMod mymod, int start_duration, int duration, string action, bool repeats, bool running, int total_timers ) {
			var myplayer = Main.LocalPlayer.GetModPlayer<TimeLimitPlayer>();

			this.StartTimer( mymod, start_duration, duration, action, repeats, running );

			myplayer.CheckModDataSync( total_timers );

			Main.NewText( "Timer started to perform action '" + action + "'" + ( repeats ? " repeatedly." : "." ), Color.Yellow );
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
