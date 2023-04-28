using System;
using System.Collections.Generic;
using ModLibsCore.Libraries.Debug;
using Terraria.ModLoader.IO;

namespace TimeLimit.Logic {
	partial class WorldLogic {
		public void Load( TagCompound tags ) {
			IList<int> timerStartDurations;
			IList<int> timerDurations;
			IList<string> timerActions;
			IList<bool> timerRepeats;
			IList<bool> timerRuns;

			if( tags.ContainsKey( "timer_start_durations" ) ) {
				timerStartDurations = tags.GetList<int>( "timer_start_durations" );
				timerDurations = tags.GetList<int>( "timer_durations" );
				timerActions = tags.GetList<string>( "timer_actions" );
				timerRepeats = tags.GetList<bool>( "timer_repeats" );
				if( tags.ContainsKey( "timer_runs" ) ) {
					timerRuns = tags.GetList<bool>( "timer_runs" );
				} else {
					timerRuns = new List<bool>( timerDurations.Count );
				}

				for( int i = 0; i < timerStartDurations.Count; i++ ) {
					int startDuration = timerStartDurations[i];
					int duration = timerDurations[i];
					string action = timerActions[i];
					bool repeats = timerRepeats[i];
					bool isRunninng = timerRuns[i];
					var timer = new ActionTimer( startDuration, duration, action, repeats, isRunninng );

					this.Timers.Add( timer );
				}
			}

			this.IsLoaded = true;
		}

		public void Save( TagCompound tags ) {
			IList<int> startDurations = new List<int>();
			IList<int> durations = new List<int>();
			IList<string> actions = new List<string>();
			IList<bool> repeats = new List<bool>();
			IList<bool> runs = new List<bool>();

			foreach( var timer in this.Timers ) {
				startDurations.Add( timer.StartDuration );
				durations.Add( timer.Duration );
				actions.Add( timer.Action );
				repeats.Add( timer.Repeats );
				runs.Add( timer.IsRunning );
			}

			try {
				tags.Set( "timer_start_durations", startDurations );
				tags.Set( "timer_durations", durations );
				tags.Set( "timer_actions", actions );
				tags.Set( "timer_repeats", repeats );
				tags.Set( "timer_runs", runs );
			} catch( Exception e ) {
				LogLibraries.Alert( e.ToString() );
			}
		}
	}
}
