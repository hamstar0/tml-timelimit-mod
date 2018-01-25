using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace TimeLimit.Logic {
	partial class WorldLogic {
		public bool IsLoaded { get; private set; }

		public IList<ActionTimer> Timers { get; private set; }


		////////////////

		public WorldLogic() {
			this.IsLoaded = false;
			this.Timers = new List<ActionTimer>();
		}

		public void Load( IList<int> start_durations, IList<int> durations, IList<string> actions, IList<bool> repeats, IList<bool> running ) {
			this.IsLoaded = true;

			for( int i=0; i<start_durations.Count; i++ ) {
				var timer = new ActionTimer( start_durations[i], durations[i], actions[i], repeats[i], running[i] );
				this.Timers.Add( timer );
			}
		}


		////////////////


		public ActionTimer StartTimer( TimeLimitMod mymod, int start_duration, int duration, string action, bool repeats, bool is_running ) {
			if( !mymod.Logic.IsValidAction(action) ) { throw new Exception( "Invalid action "+action ); }

			var timer = new ActionTimer( duration, duration, action, repeats, is_running );
			this.Timers.Add( timer );

			return timer;
		}

		public void StopAllTimers() {
			this.Timers = new List<ActionTimer>();
		}

		////////////////
		
		public void PauseAllTimers() {
			foreach( var timer in this.Timers ) {
				timer.IsRunning = false;
			}
		}

		public void ResumeAllTimers() {
			foreach( var timer in this.Timers ) {
				timer.IsRunning = true;
			}
		}


		////////////////

		public void Update( TimeLimitMod mymod ) {
			IList<int> expired = new List<int>();

			foreach( ActionTimer timer in this.Timers.ToArray() ) {
				if( timer.Duration == 1 ) {
					this.RunAction( mymod, timer.Action, timer.IsRunning );
				}

				timer.Update();

				if( timer.IsExpired() ) {
					this.Timers.Remove( timer );
				}
			}
		}

		////////////////

		public void DrawTimers( TimeLimitMod mymod, SpriteBatch sb ) {
			if( !Main.playerInventory ) { return; }
			
			int x = mymod.Config.TimerDisplayX >= 0 ? mymod.Config.TimerDisplayX : Main.screenWidth + mymod.Config.TimerDisplayX;
			int y = mymod.Config.TimerDisplayY >= 0 ? mymod.Config.TimerDisplayY : Main.screenHeight + mymod.Config.TimerDisplayY;

			int i = 0;
			foreach( var timer in this.Timers ) {
				string act = "Time until " + ActionTimer.RenderAction( timer.Action );
				Vector2 act_pos = new Vector2( x, y + ( i * 48 ) );
				Vector2 timer_pos = act_pos + new Vector2( 0, 6 );

				sb.DrawString( Main.fontDeathText, act, act_pos, Color.White, 0f, default(Vector2), 0.25f, SpriteEffects.None, 1f );
				sb.DrawString( Main.fontDeathText, ActionTimer.RenderDuration(timer.Duration), timer_pos, Color.Gray );

				i++;
			}
		}
	}
}
