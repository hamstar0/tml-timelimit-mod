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
	class TimerLogic {
		internal IDictionary<string, Action> TimesUpHooks = new Dictionary<string, Action>();

		public bool IsInitialized { get; private set; }

		public IList<int> TimerStartDurations { get; private set; }
		public IList<int> TimerDurations { get; private set; }
		public IList<string> TimerActions { get; private set; }
		public IList<bool> TimerRepeats { get; private set; }


		////////////////

		public TimerLogic() {
			this.IsInitialized = false;
			this.TimerStartDurations = null;
			this.TimerDurations = null;
			this.TimerActions = null;
			this.TimerRepeats = null;
		}

		public void Initialize( IList<int> timer_start_durations, IList<int> timer_durations, IList<string> timer_actions, IList<bool> timer_repeats ) {
			this.IsInitialized = true;

			this.TimerStartDurations = timer_start_durations;
			this.TimerDurations = timer_durations;
			this.TimerActions = timer_actions;
			this.TimerRepeats = timer_repeats;
		}

		////////////////

		public void Add( int duration, string action, bool repeats ) {
			this.TimerStartDurations.Add( duration );
			this.TimerDurations.Add( duration );
			this.TimerActions.Add( action );
			this.TimerRepeats.Add( repeats );
		}

		public void AbortAll() {
			this.TimerStartDurations = new List<int>();
			this.TimerDurations = new List<int>();
			this.TimerActions = new List<string>();
			this.TimerRepeats = new List<bool>();
		}


		////////////////

		public void Update( TimeLimitMod mymod ) {
			for( int i=0; i<this.TimerDurations.Count; i++ ) {
				int duration = this.TimerDurations[i];

				if( duration == 1 ) {
					this.RunAction( mymod, this.TimerActions[i] );
				}

				if( duration > 0 ) {
					this.TimerDurations[i]--;
				} else {
					if( this.TimerRepeats[i] ) {
						this.TimerDurations[i] = this.TimerStartDurations[i];
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
			for( int i=0; i<this.TimerDurations.Count; i++ ) {
				if( this.TimerDurations[i] == 0 ) { continue; }

				sb.DrawString( Main.fontDeathText, this.RenderTimer(i), new Vector2(x, y + (j*32)), Color.Gray );
				j++;
			}
		}


		public string RenderTimer( int which ) {
			int timer = this.TimerDurations[ which ];
			int total_seconds = timer / 60;
			int total_minutes = total_seconds / 60;
			int total_hours = total_minutes / 60;
			int minutes = total_minutes - (total_hours * 60);
			int seconds = total_seconds - (total_minutes * 60);

			return total_hours.ToString("D2")+":"+minutes.ToString("D2")+":"+seconds.ToString("D2");
		}


		////////////////

		private void RunAction( TimeLimitMod mymod, string action ) {
			switch( action ) {
			case "none":
				break;
			case "exit":
				TmlHelpers.Exit();
				break;
			case "kill":
				Main.LocalPlayer.KillMe( PlayerDeathReason.ByCustomReason("Time's up."), 9999, 0 );
				break;
			case "hardkill":
				PlayerHelpers.KillWithPermadeath( Main.LocalPlayer, "Time's up." );
				break;
			case "afflict":
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
