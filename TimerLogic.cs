using HamstarHelpers.BuffHelpers;
using HamstarHelpers.PlayerHelpers;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace TimeLimit {
	struct Timer {
		public static string RenderAction( string action ) {
			switch( action ) {
			case "none":
				return "timer expiration";
			case "exit":
				return "forced to exit (to main menu)";
			case "kill":
				return "everyone dies";
			case "hardkill":
				return "game over";
			case "afflict":
				return "permanent status changes";
			default:
				return "custom action '" + action + "'";
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


		////////////////

		public Timer( int start_duration, int duration, string action, bool repeats ) {
			this.StartDuration = start_duration;
			this.Duration = duration;
			this.Action = action;
			this.Repeats = repeats;
		}
	}



	class TimerLogic {
		internal IDictionary<string, Action> TimesUpHooks = new Dictionary<string, Action>();

		public bool IsInitialized { get; private set; }

		public IList<Timer> Timers { get; private set; }


		////////////////

		public TimerLogic() {
			this.IsInitialized = false;
			this.EndAllTimers();
		}

		public void Initialize( IList<int> timer_start_durations, IList<int> timer_durations, IList<string> timer_actions, IList<bool> timer_repeats ) {
			this.IsInitialized = true;

			for( int i=0; i<timer_start_durations.Count; i++ ) {
				this.Timers.Add( new Timer( timer_start_durations[i], timer_durations[i], timer_actions[i], timer_repeats[i] ) );
			}
		}

		////////////////

		public Timer StartTimer( int start_duration, int duration, string action, bool repeats ) {
			var timer = new Timer( duration, duration, action, repeats );
			this.Timers.Add( timer );

			return timer;
		}

		public void EndAllTimers() {
			this.Timers = new List<Timer>();
		}


		////////////////

		public void Update( TimeLimitMod mymod ) {
			for( int i=0; i<this.Timers.Count; i++ ) {
				Timer timer = this.Timers[i];

				if( timer.Duration == 1 ) {
					this.RunAction( mymod, timer.Action );
				}

				if( timer.Duration > 0 ) {
					timer.Duration--;
				} else {
					if( timer.Repeats ) {
						timer.Duration = timer.StartDuration;
					}
				}
			}
		}

		////////////////

		public void DrawTimers( TimeLimitMod mymod, SpriteBatch sb ) {
			if( !Main.playerInventory ) { return; }
			
			int x = mymod.Config.TimerDisplayX >= 0 ? mymod.Config.TimerDisplayX : Main.screenWidth + mymod.Config.TimerDisplayX;
			int y = mymod.Config.TimerDisplayY >= 0 ? mymod.Config.TimerDisplayY : Main.screenHeight + mymod.Config.TimerDisplayY;

			int j = 0;
			for( int i=0; i<this.Timers.Count; i++ ) {
				Timer timer = this.Timers[i];

				if( timer.Duration == 0 ) { continue; }

				string act = "Time until " + Timer.RenderAction( timer.Action );
				Vector2 act_pos = new Vector2( x, y + ( j * 46 ) );
				Vector2 timer_pos = act_pos + new Vector2( 0, 6 );

				sb.DrawString( Main.fontDeathText, act, act_pos, Color.White, 0f, default(Vector2), 0.25f, SpriteEffects.None, 1f );
				sb.DrawString( Main.fontDeathText, Timer.RenderDuration(timer.Duration), timer_pos, Color.Gray );
				j++;
			}
		}


		////////////////

		private void RunAction( TimeLimitMod mymod, string action ) {
			switch( action ) {
			case "none":
				Main.NewText( "Time's up.", Color.Red );
				break;
			case "exit":
				Main.NewText( "Time's up. Bye!", Color.Red );
				TmlHelpers.ExitToMenu();
				break;
			case "kill":
				Main.LocalPlayer.KillMe( PlayerDeathReason.ByCustomReason("Time's up."), 9999, 0 );
				break;
			case "hardkill":
				PlayerHelpers.KillWithPermadeath( Main.LocalPlayer, "Time's up. Game over." );
				break;
			case "afflict":
				var afflictions = string.Join( ",", mymod.Config.Afflictions );

				Main.NewText( "Time's up. You now have the following: "+afflictions, Color.Red );
				this.ApplyAffliction( mymod );
				break;
			default:
				var myworld = mymod.GetModWorld<TimeLimitWorld>();
				
				if( !myworld.Logic.TimesUpHooks.ContainsKey(action) ) {
					ErrorLogger.Log( "No such time's up event by name " + action );
					break;
				}
				myworld.Logic.TimesUpHooks[action]();
				break;
			}
		}


		public void ApplyAffliction( TimeLimitMod mymod ) {
			foreach( string affliction in mymod.Config.Afflictions ) {
				if( !BuffHelpers.BuffIdsByName.ContainsKey(affliction) ) {
					ErrorLogger.Log( "No such afflication ((de)buff) by name " + affliction );
					continue;
				}

				int buff_id = BuffHelpers.BuffIdsByName[ affliction ];
				BuffHelpers.AddPermaBuff( Main.LocalPlayer, buff_id );
			}
		}
	}
}
