using HamstarHelpers.Components.Errors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
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


		////////////////

		public ActionTimer StartTimer( int startDuration, int duration, string action, bool repeats, bool isRunning ) {
			var mymod = TimeLimitMod.Instance;

			if( !mymod.Logic.IsValidAction(action) ) {
				throw new ModHelpersException( "Invalid action "+action );
			}

			var timer = new ActionTimer( duration, duration, action, repeats, isRunning );
			this.Timers.Add( timer );

			return timer;
		}

		public void StopTimers( string action ) {
			foreach( ActionTimer timer in this.Timers.ToArray() ) {
				if( timer.Action == action ) {
					this.Timers.Remove( timer );
				}
			}
		}

		public void PauseTimers( string action ) {
			foreach( ActionTimer timer in this.Timers.ToArray() ) {
				if( timer.Action == action ) {
					timer.IsRunning = false;
				}
			}
		}

		public void ResumeTimers( string action ) {
			foreach( ActionTimer timer in this.Timers.ToArray() ) {
				if( timer.Action == action ) {
					timer.IsRunning = true;
				}
			}
		}

		////////////////

		public void StopAllTimers() {
			this.Timers = new List<ActionTimer>();
		}
		
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

		public void Update() {
			IDictionary<ActionTimer, bool> willRun = new Dictionary<ActionTimer, bool>();

			foreach( ActionTimer timer in this.Timers.ToArray() ) {
				// This way, each timer's action runs only when the timer is running:
				bool runs = timer.Duration == 1;

				timer.Update();
				
				if( runs ) {
					willRun[ timer ] = timer.IsExpired();
				}

				if( timer.IsExpired() ) {
					this.Timers.Remove( timer );
				}
			}

			foreach( var kv in willRun ) {
				string actionName = kv.Key.Action;
				bool isExpired = kv.Value;

				this.RunAction( actionName, !isExpired );
			}
		}


		////////////////

		public void DrawTimers( SpriteBatch sb ) {
			if( !Main.playerInventory ) { return; }

			var mymod = TimeLimitMod.Instance;
			int x = mymod.Config.TimerDisplayX >= 0 ? mymod.Config.TimerDisplayX : Main.screenWidth + mymod.Config.TimerDisplayX;
			int y = mymod.Config.TimerDisplayY >= 0 ? mymod.Config.TimerDisplayY : Main.screenHeight + mymod.Config.TimerDisplayY;

			int i = 0;
			foreach( var timer in this.Timers ) {
				string act = "Time until " + ActionTimer.RenderAction( timer.Action );
				Vector2 actPos = new Vector2( x, y + ( i * 48 ) );
				Vector2 timerPos = actPos + new Vector2( 0, 6 );

				sb.DrawString( Main.fontDeathText, act, actPos, Color.White, 0f, default(Vector2), 0.25f, SpriteEffects.None, 1f );
				sb.DrawString( Main.fontDeathText, ActionTimer.RenderDuration(timer.Duration), timerPos, Color.Gray );

				i++;
			}
		}
	}
}
