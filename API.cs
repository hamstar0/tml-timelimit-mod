using System;
using Terraria;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;

namespace TimeLimit {
	public delegate void CustomTimerAction();

	
	
	public static class TimeLimitAPI {
		internal static object Call( string call_type, params object[] args ) {
			switch( call_type ) {
			case "GetModSettings":
				return TimeLimitAPI.GetModSettings();

			case "SaveModSettingsChanges":
				TimeLimitAPI.SaveModSettingsChanges();
				return null;

			case "RunTimesUpAction":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				var action_name1 = args[0] as string;
				if( string.IsNullOrEmpty( action_name1 ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				TimeLimitAPI.RunTimesUpAction( action_name1 );
				return null;

			case "TimerStart":
				if( args.Length < 3 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				var action_name2 = args[0] as string;
				if( string.IsNullOrEmpty( action_name2 ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }
				var seconds_str = args[0] as string;
				if( string.IsNullOrEmpty( action_name2 ) ) { throw new Exception( "Invalid parameter seconds for API call " + call_type ); }
				var repeats_str = args[0] as string;
				if( string.IsNullOrEmpty( action_name2 ) ) { throw new Exception( "Invalid parameter repeats for API call " + call_type ); }

				int seconds;
				if( !int.TryParse( seconds_str, out seconds ) ) {
					throw new Exception( args[0] + " is not an integer" );
				}

				bool repeats;
				if( !bool.TryParse( repeats_str, out repeats ) ) {
					throw new Exception( args[2] + " is not boolean" );
				}

				TimeLimitAPI.TimerStart( action_name2, seconds, repeats );
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

			case "AddCustomAction":
				if( args.Length < 3 ) { throw new Exception("Insufficient parameters for API call "+call_type);  }
				
				var action_name = args[0] as string;
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
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();
			ActionTimer timer = myworld.Logic.StartTimer( seconds * 60, seconds * 60, action_name, repeats, true );

			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendStartTimerCommandFromServer( TimeLimitMod.Instance, -1, timer );
			}
		}

		public static void TimerAllStop() {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.StopAllTimers();
			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendStopTimersCommandFromServer( TimeLimitMod.Instance, -1 );
			}
		}

		public static void TimerAllPause() {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.PauseAllTimers();
			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendPauseTimersCommandFromServer( TimeLimitMod.Instance, -1 );
			}
		}

		public static void TimerAllResume() {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();

			myworld.Logic.ResumeAllTimers();
			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendResumeTimersCommandFromServer( TimeLimitMod.Instance, -1 );
			}
		}


		////////////////

		public static void AddCustomAction( string action_name, CustomTimerAction hook ) {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();
			myworld.Logic.CustomActions[ action_name ] = hook;
		}
	}
}
