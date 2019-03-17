using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using System;


namespace TimeLimit {
	public static partial class TimeLimitAPI {
		internal static object Call( string callType, params object[] args ) {
			if( TimeLimitMod.Instance.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimeLimitAPI.Call - "+callType+": "+string.Join(",", args) );
			}

			string secondsStr;
			string repeats_str;
			int seconds;
			bool repeats;
			string actionName;


			switch( callType ) {
			case "GetModSettings":
				return TimeLimitAPI.GetModSettings();

			case "SaveModSettingsChanges":
				TimeLimitAPI.SaveModSettingsChanges();
				return null;

			////

			case "RunTimesUpAction":
				if( args.Length < 1 ) { throw new HamstarException( "Insufficient parameters for API call " + callType ); }

				actionName = args[0] as string;
				if( string.IsNullOrEmpty( actionName ) ) { throw new HamstarException( "Invalid parameter actionName for API call " + callType ); }

				TimeLimitAPI.RunTimesUpAction( actionName );
				return null;

			////

			case "TimerStart":
				if( args.Length < 3 ) { throw new HamstarException( "Insufficient parameters for API call " + callType ); }

				actionName = args[0] as string;
				if( string.IsNullOrEmpty( actionName ) ) { throw new HamstarException( "Invalid parameter actionName for API call " + callType ); }
				secondsStr = args[0] as string;
				if( string.IsNullOrEmpty( secondsStr ) ) { throw new HamstarException( "Invalid parameter seconds for API call " + callType ); }
				repeats_str = args[0] as string;
				if( string.IsNullOrEmpty( repeats_str ) ) { throw new HamstarException( "Invalid parameter repeats for API call " + callType ); }

				if( !int.TryParse( secondsStr, out seconds ) ) {
					throw new HamstarException( args[0] + " is not an integer" );
				}

				if( !bool.TryParse( repeats_str, out repeats ) ) {
					throw new HamstarException( args[2] + " is not boolean" );
				}

				TimeLimitAPI.TimerStart( actionName, seconds, repeats );
				return null;

			case "TimerStop":
				if( args.Length < 1 ) { throw new HamstarException( "Insufficient parameters for API call " + callType ); }

				actionName = args[0] as string;
				if( string.IsNullOrEmpty( actionName ) ) { throw new HamstarException( "Invalid parameter actionName for API call " + callType ); }

				TimeLimitAPI.TimerStop( actionName );
				return null;

			case "TimerPause":
				if( args.Length < 1 ) { throw new HamstarException( "Insufficient parameters for API call " + callType ); }

				actionName = args[0] as string;
				if( string.IsNullOrEmpty( actionName ) ) { throw new HamstarException( "Invalid parameter actionName for API call " + callType ); }

				TimeLimitAPI.TimerPause( actionName );
				return null;

			case "TimerResume":
				if( args.Length < 1 ) { throw new HamstarException( "Insufficient parameters for API call " + callType ); }

				actionName = args[0] as string;
				if( string.IsNullOrEmpty( actionName ) ) { throw new HamstarException( "Invalid parameter actionName for API call " + callType ); }

				TimeLimitAPI.TimerResume( actionName );
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
				actionName = args[0] as string;
				if( string.IsNullOrEmpty( actionName ) ) { throw new HamstarException( "Invalid parameter actionName for API call " + callType ); }

				return TimeLimitAPI.GetTimersOf( actionName );

			////

			case "AddCustomAction":
				if( args.Length < 3 ) { throw new HamstarException( "Insufficient parameters for API call "+callType);  }
				
				actionName = args[0] as string;
				if( string.IsNullOrEmpty( actionName ) ) { throw new HamstarException( "Invalid parameter actionName for API call " + callType ); }

				var hook = args[1] as CustomTimerAction;
				if( hook == null ) { throw new HamstarException( "Invalid parameter hook for API call " + callType ); }

				TimeLimitAPI.AddCustomAction( actionName, hook );
				return null;
			}

			throw new HamstarException( "No such api call " + callType );
		}
	}
}
