using HamstarHelpers.Helpers.DebugHelpers;
using System;


namespace TimeLimit {
	public static partial class TimeLimitAPI {
		internal static object Call( string call_type, params object[] args ) {
			if( TimeLimitMod.Instance.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.Call - "+call_type+": "+string.Join(",", args) );
			}

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

			////

			case "RunTimesUpAction":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				TimeLimitAPI.RunTimesUpAction( action_name );
				return null;

			////

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

			case "TimerStop":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				TimeLimitAPI.TimerStop( action_name );
				return null;

			case "TimerPause":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				TimeLimitAPI.TimerPause( action_name );
				return null;

			case "TimerResume":
				if( args.Length < 1 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				TimeLimitAPI.TimerResume( action_name );
				return null;

			////

			case "TimerAllStop":
				TimeLimitAPI.TimerAllStop();
				return null;

			case "TimerAllPause":
				TimeLimitAPI.TimerAllPause();
				return null;

			case "TimerAllResume":
				TimeLimitAPI.TimerAllResume();
				return null;

			////

			case "GetTimersOf":
				action_name = args[0] as string;
				if( string.IsNullOrEmpty( action_name ) ) { throw new Exception( "Invalid parameter action_name for API call " + call_type ); }

				return TimeLimitAPI.GetTimersOf( action_name );

			////

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
	}
}
