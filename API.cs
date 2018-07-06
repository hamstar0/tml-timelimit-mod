using HamstarHelpers.DebugHelpers;
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
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.TimeLimitConfigData" );
			}

			return mymod.Config;
		}

		public static void SaveModSettingsChanges() {
			var mymod = TimeLimitMod.Instance;

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.SaveModSettingsChanges" );
			}

			mymod.ConfigJson.SaveFile();
		}


		////////////////
		
		public static void RunTimesUpAction( string action_name ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.RunTimesUpAction - "+action_name );
			}

			myworld.Logic.RunAction( mymod, action_name, false );
		}


		////////////////

		public static void TimerStart( string action_name, int seconds, bool repeats ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.TimerStart - " + action_name + " for "+seconds+", repeating? "+repeats );
			}

			ActionTimer timer = myworld.Logic.StartTimer( mymod, seconds * 60, seconds * 60, action_name, repeats, true );

			if( Main.netMode == 2 ) {
				SendPackets.SendStartTimerCommand( TimeLimitMod.Instance, timer, myworld.Logic.Timers.Count, -1 );
			}
		}

		public static void TimerStop( string action_name ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.TimerStop - " + action_name );
			}

			myworld.Logic.StopTimers( action_name );

			if( Main.netMode == 2 ) {
				SendPackets.SendStopTimersCommand( mymod, action_name, -1 );
			}
		}

		public static void TimerPause( string action_name ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.TimerPause - " + action_name );
			}

			myworld.Logic.PauseTimers( action_name );

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseTimersCommand( mymod, action_name, -1 );
			}
		}

		public static void TimerResume( string action_name ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.TimerResume - " + action_name );
			}

			myworld.Logic.ResumeTimers( action_name );

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeTimersCommand( mymod, action_name, - 1 );
			}
		}

		////////////////

		public static void TimerAllStop() {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.TimerAllStop" );
			}

			myworld.Logic.StopAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendStopAllTimersCommand( mymod, -1 );
			}
		}

		public static void TimerAllPause() {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.TimerAllPause" );
			}

			myworld.Logic.PauseAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseAllTimersCommand( mymod, -1 );
			}
		}

		public static void TimerAllResume() {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.TimerAllResume" );
			}

			myworld.Logic.ResumeAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeAllTimersCommand( mymod, -1 );
			}
		}


		////////////////

		public static IList<ActionTimer> GetTimersOf( string action_name ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			IList<ActionTimer> timers = new List<ActionTimer>();

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.GetTimersOf - "+ action_name );
			}

			foreach( var timer in myworld.Logic.Timers ) {
				if( timer.Action.Equals(timer) ) {
					timers.Add( timer );
				}
			}
			return timers;
		}


		////////////////

		public static void AddCustomAction( string action_name, CustomTimerAction hook ) {
			var mymod = TimeLimitMod.Instance;
			var logic = mymod.Logic;

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.AddCustomAction - " + action_name );
			}

			logic.CustomActions[ action_name ] = hook;
		}
	}
}
