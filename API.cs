using System.Collections.Generic;
using Terraria;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;


namespace TimeLimit {
	public delegate void CustomTimerAction();

	
	
	public static partial class TimeLimitAPI {
		public static TimeLimitConfigData GetModSettings() {
			return TimeLimitMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			TimeLimitMod.Instance.ConfigJson.SaveFile();
		}


		////////////////
		
		public static void RunTimesUpAction( string action_name ) {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();
			myworld.Logic.RunAction( TimeLimitMod.Instance, action_name, false );
		}


		////////////////

		public static void TimerStart( string action_name, int seconds, bool repeats ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			
			ActionTimer timer = myworld.Logic.StartTimer( mymod, seconds * 60, seconds * 60, action_name, repeats, true );

			if( Main.netMode == 2 ) {
				SendPackets.SendStartTimerCommand( TimeLimitMod.Instance, timer, myworld.Logic.Timers.Count, -1 );
			}
		}

		public static void TimerStop( string action_name ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.StopTimers( action_name );

			if( Main.netMode == 2 ) {
				SendPackets.SendStopTimersCommand( mymod, action_name, -1 );
			}
		}

		public static void TimerPause( string action_name ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.PauseTimers( action_name );

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseTimersCommand( mymod, action_name, -1 );
			}
		}

		public static void TimerResume( string action_name ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.ResumeTimers( action_name );

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeTimersCommand( mymod, action_name, - 1 );
			}
		}

		////////////////

		public static void TimerAllStop() {
			if( Main.netMode == 1 ) { return; }

			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.StopAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendStopAllTimersCommand( TimeLimitMod.Instance, -1 );
			}
		}

		public static void TimerAllPause() {
			if( Main.netMode == 1 ) { return; }

			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.PauseAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseAllTimersCommand( TimeLimitMod.Instance, -1 );
			}
		}

		public static void TimerAllResume() {
			if( Main.netMode == 1 ) { return; }

			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.ResumeAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeAllTimersCommand( TimeLimitMod.Instance, -1 );
			}
		}


		////////////////

		public static IList<ActionTimer> GetTimersOf( string action ) {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();
			IList<ActionTimer> timers = new List<ActionTimer>();

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

			logic.CustomActions[ action_name ] = hook;
		}
	}
}
