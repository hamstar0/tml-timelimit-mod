using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace TimeLimit.Logic {
	partial class WorldLogic {
		public void Load( TimeLimitMod mymod, TagCompound tags ) {
			IList<int> timer_start_durations;
			IList<int> timer_durations;
			IList<string> timer_actions;
			IList<bool> timer_repeats;
			IList<bool> timer_runs;

			if( tags.ContainsKey( "timer_start_durations" ) ) {
				timer_start_durations = tags.GetList<int>( "timer_start_durations" );
				timer_durations = tags.GetList<int>( "timer_durations" );
				timer_actions = tags.GetList<string>( "timer_actions" );
				timer_repeats = tags.GetList<bool>( "timer_repeats" );
				if( tags.ContainsKey( "timer_runs" ) ) {
					timer_runs = tags.GetList<bool>( "timer_runs" );
				} else {
					timer_runs = new List<bool>( timer_durations.Count );
				}

				for( int i = 0; i < timer_start_durations.Count; i++ ) {
					int start_duration = timer_start_durations[i];
					int duration = timer_durations[i];
					string action = timer_actions[i];
					bool repeats = timer_repeats[i];
					bool is_runninng = timer_runs[i];
					var timer = new ActionTimer( start_duration, duration, action, repeats, is_runninng );

					this.Timers.Add( timer );
				}
			}

			this.IsLoaded = true;
		}

		public TagCompound Save( TimeLimitMod mymod ) {
			IList<int> start_durations = new List<int>();
			IList<int> durations = new List<int>();
			IList<string> actions = new List<string>();
			IList<bool> repeats = new List<bool>();
			IList<bool> runs = new List<bool>();

			foreach( var timer in this.Timers ) {
				start_durations.Add( timer.StartDuration );
				durations.Add( timer.Duration );
				actions.Add( timer.Action );
				repeats.Add( timer.Repeats );
				runs.Add( timer.IsRunning );
			}

			var tags = new TagCompound();
			try {
				tags.Set( "timer_start_durations", start_durations );
				tags.Set( "timer_durations", durations );
				tags.Set( "timer_actions", actions );
				tags.Set( "timer_repeats", repeats );
				tags.Set( "timer_runs", runs );
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}

			return tags;
		}
	}
}
