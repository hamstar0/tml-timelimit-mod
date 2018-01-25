using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;


namespace TimeLimit {
	public delegate void CustomTimerAction();

	
	
	public static class TimeLimitAPI {
		internal static object Call( string call_type, params object[] args ) {
			string seconds_str;
			string repeats_str;
			int seconds;
			bool repeats;
			string action_name;


			switch( call_type ) {
			case "GetModSettings":
				return TimeLimitAPI.GetModSettings();

			case "SaveModSettingsChanges":
				TimeLimitAPI.SaveModSettingsChanges();
				return null;

			case "RunTimesUpAction":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				TimeLimitAPI.RunTimesUpAction( action_name );
				return null;

			case "TimerStart":
				if( args.Length < 3 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }
				seconds_str = args[0] as string;
				if( string.IsNullOrEmpty( seconds_str ) ) { throw new Exception( "Invalid parameter seconds for API call " + call_type ); }
				repeats_str = args[0] as string;
				if( string.IsNullOrEmpty( repeats_str ) ) { throw new Exception( "Invalid parameter repeats for API call " + call_type ); }

				if( !int.TryParse( seconds_str, out seconds ) ) {
					throw new Exception( args[0] + " is not an integer" );
				}

				if( !bool.TryParse( repeats_str, out repeats ) ) {
					throw new Exception( args[2] + " is not boolean" );
				}

				TimeLimitAPI.TimerStart( action_name, seconds, repeats );
				return null;

			case "TimerAllStop":
				TimeLimitAPI.TimerAllStop();
				return null;

			case "TimerAllPause":
				TimeLimitAPI.TimerAllPause();
				return null;

			case "TimerAllResume":
				TimeLimitAPI.TimerAllResume();
				return null;

			case "GetTimersOf":
				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				return TimeLimitAPI.GetTimersOf( action_name );

			case "AddCustomAction":
				if( args.Length < 3 ) { throw new Exception("Insufficient parameters for API call "+call_type);  }
				
				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				var hook = args[1] as CustomTimerAction;
				if( hook == null ) { throw new Exception( "Invalid parameter hook for API call " + call_type ); }

				TimeLimitAPI.AddCustomAction( action_name, hook );
				return null;
			}

			throw new Exception( "No such api call " + call_type );
		}


		////////////////

		public static TimeLimitConfigData GetModSettings() {
			return TimeLimitMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			TimeLimitMod.Instance.JsonConfig.SaveFile();
		}


		////////////////
		
		public static void RunTimesUpAction( string action_name ) {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();
			myworld.Logic.RunAction( TimeLimitMod.Instance, action_name, false );
		}


		////////////////

		public static void TimerStart( string action_name, int seconds, bool repeats ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			ActionTimer timer = myworld.Logic.StartTimer( mymod, seconds * 60, seconds * 60, action_name, repeats, true );

			if( Main.netMode == 2 ) {
				SendPackets.SendStartTimerCommand( TimeLimitMod.Instance, timer, myworld.Logic.Timers.Count, -1 );
			}
		}

		public static void TimerAllStop() {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.StopAllTimers();
			if( Main.netMode == 2 ) {
				SendPackets.SendStopTimersCommand( TimeLimitMod.Instance, -1 );
			}
		}

		public static void TimerAllPause() {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.PauseAllTimers();
			if( Main.netMode == 2 ) {
				SendPackets.SendPauseTimersCommand( TimeLimitMod.Instance, -1 );
			}
		}

		public static void TimerAllResume() {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.ResumeAllTimers();
			if( Main.netMode == 2 ) {
				SendPackets.SendResumeTimersCommand( TimeLimitMod.Instance, -1 );
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
