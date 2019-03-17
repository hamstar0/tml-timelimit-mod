using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;
using Terraria;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;


namespace TimeLimit {
	public delegate void CustomTimerAction();

	
	
	public static partial class TimeLimitAPI {
		public static TimeLimitConfigData GetModSettings() {
			var mymod = TimeLimitMod.Instance;

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert();
			}

			return mymod.Config;
		}

		public static void SaveModSettingsChanges() {
			var mymod = TimeLimitMod.Instance;

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert();
			}

			mymod.ConfigJson.SaveFile();
		}


		////////////////
		
		public static void RunTimesUpAction( string actionName ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( actionName );
			}

			myworld.Logic.RunAction( actionName, false );
		}


		////////////////

		public static void TimerStart( string actionName, int seconds, bool repeats ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( actionName + " for "+seconds+", repeating? "+repeats );
			}

			ActionTimer timer = myworld.Logic.StartTimer( seconds * 60, seconds * 60, actionName, repeats, true );

			if( Main.netMode == 2 ) {
				SendPackets.SendStartTimerCommand( timer, myworld.Logic.Timers.Count, -1 );
			}
		}

		public static void TimerStop( string actionName ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( actionName );
			}

			myworld.Logic.StopTimers( actionName );

			if( Main.netMode == 2 ) {
				SendPackets.SendStopTimersCommand( actionName, -1 );
			}
		}

		public static void TimerPause( string actionName ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( actionName );
			}

			myworld.Logic.PauseTimers( actionName );

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseTimersCommand( actionName, -1 );
			}
		}

		public static void TimerResume( string actionName ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( actionName );
			}

			myworld.Logic.ResumeTimers( actionName );

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeTimersCommand( actionName, - 1 );
			}
		}

		////////////////

		public static void TimerAllStop() {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert();
			}

			myworld.Logic.StopAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendStopAllTimersCommand( -1 );
			}
		}

		public static void TimerAllPause() {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert();
			}

			myworld.Logic.PauseAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseAllTimersCommand( -1 );
			}
		}

		public static void TimerAllResume() {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert();
			}

			myworld.Logic.ResumeAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeAllTimersCommand( -1 );
			}
		}


		////////////////

		public static IList<ActionTimer> GetTimersOf( string actionName ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			IList<ActionTimer> timers = new List<ActionTimer>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( actionName );
			}

			foreach( var timer in myworld.Logic.Timers ) {
				if( timer.Action.Equals(timer) ) {
					timers.Add( timer );
				}
			}
			return timers;
		}


		////////////////

		public static void AddCustomAction( string actionName, CustomTimerAction hook ) {
			var mymod = TimeLimitMod.Instance;
			var logic = mymod.Logic;

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( actionName );
			}

			logic.CustomActions[ actionName ] = hook;
		}
	}
}
