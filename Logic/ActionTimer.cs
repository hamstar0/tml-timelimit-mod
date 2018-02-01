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
			default:
				return "'" + action + "'";
			}
		}

		public static string RenderDuration( int duration ) {
			int total_seconds = duration / 60;
			int total_minutes = total_seconds / 60;
			int total_hours = total_minutes / 60;
			int minutes = total_minutes - ( total_hours * 60 );
			int seconds = total_seconds - ( total_minutes * 60 );

			return total_hours.ToString( "D2" ) + ":" + minutes.ToString( "D2" ) + ":" + seconds.ToString( "D2" );
		}



		////////////////

		public int StartDuration;
		public int Duration;
		public string Action;
		public bool Repeats;
		public bool IsRunning;


		////////////////

		public ActionTimer( int start_duration, int duration, string action, bool repeats, bool is_running ) {
			this.StartDuration = start_duration;
			this.Duration = duration;
			this.Action = action;
			this.Repeats = repeats;
			this.IsRunning = is_running;
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
