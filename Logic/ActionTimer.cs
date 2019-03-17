using System;


namespace TimeLimit.Logic {
	public class ActionTimer {
		public static string RenderAction( string action ) {
			switch( action ) {
			case "none":
				return "timer expiration";
			case "exit":
				return "forced to exit (to main menu)";
			case "serverclose":
				return "server closes";
			case "kill":
				return "everyone dies";
			case "hardkill":
				return "game over";
			case "afflict":
				return "permanent status changes";
			case "unafflict": 
				return "recovers from permanent status changes";
			default:
				return "'" + action + "'";
			}
		}

		public static string RenderDuration( int duration ) {
			int totalSeconds = duration / 60;
			int totalMinutes = totalSeconds / 60;
			int totalHours = totalMinutes / 60;
			int minutes = totalMinutes - ( totalHours * 60 );
			int seconds = totalSeconds - ( totalMinutes * 60 );

			return totalHours.ToString( "D2" ) + ":" + minutes.ToString( "D2" ) + ":" + seconds.ToString( "D2" );
		}



		////////////////

		public int StartDuration;
		public int Duration;
		public string Action;
		public bool Repeats;
		public bool IsRunning;


		////////////////

		public ActionTimer( int startDuration, int duration, string action, bool repeats, bool isRunning ) {
			this.StartDuration = startDuration;
			this.Duration = duration;
			this.Action = action;
			this.Repeats = repeats;
			this.IsRunning = isRunning;
		}

		////

		public bool IsExpired() {
			return this.Duration == 0 && !this.Repeats;
		}

		////

		internal void Update() {
			if( this.Duration > 0 && this.IsRunning ) {
				this.Duration--;
			} else {
				if( this.Repeats ) {
					this.Duration = this.StartDuration;
				}
			}
		}
	}
}
