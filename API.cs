using System.Collections.Generic;
using ModLibsCore.Libraries.Debug;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;

namespace TimeLimit {
	public delegate void CustomTimerAction();

	
	
	public static partial class TimeLimitAPI {
		public static void RunTimesUpAction( string actionName ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( actionName );
			}

			myworld.Logic.RunAction( actionName, false );
		}


		////////////////

		public static void TimerStart( string actionName, int seconds, bool repeats ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( actionName + " for "+seconds+", repeating? "+repeats );
			}

			ActionTimer timer = myworld.Logic.StartTimer( seconds * 60, seconds * 60, actionName, repeats, true );

			if( Main.netMode == 2 ) {
				SendPackets.SendStartTimerCommand( timer, myworld.Logic.Timers.Count, -1 );
			}
		}

		public static void TimerStop( string actionName ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( actionName );
			}

			myworld.Logic.StopTimers( actionName );

			if( Main.netMode == 2 ) {
				SendPackets.SendStopTimersCommand( actionName, -1 );
			}
		}

		public static void TimerPause( string actionName ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( actionName );
			}

			myworld.Logic.PauseTimers( actionName );

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseTimersCommand( actionName, -1 );
			}
		}

		public static void TimerResume( string actionName ) {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( actionName );
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
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn(string.Empty);
			}

			myworld.Logic.StopAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendStopAllTimersCommand( -1 );
			}
		}

		public static void TimerAllPause() {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( string.Empty );
			}

			myworld.Logic.PauseAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseAllTimersCommand( -1 );
			}
		}

		public static void TimerAllResume() {
			if( Main.netMode == 1 ) { return; }

			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( string.Empty );
			}

			myworld.Logic.ResumeAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeAllTimersCommand( -1 );
			}
		}


		////////////////

		public static IList<ActionTimer> GetTimersOf( string actionName ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();
			IList<ActionTimer> timers = new List<ActionTimer>();

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( actionName );
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
				LogLibraries.Warn( actionName );
			}

			logic.CustomActions[ actionName ] = hook;
		}
	}
}
