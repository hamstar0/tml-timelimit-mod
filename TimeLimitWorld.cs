using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TimeLimit.Logic;


namespace TimeLimit {
	class TimeLimitWorld : ModWorld {
		public TimerLogic Logic { get; private set; }
		public string ID { get; private set; }
		public bool HasCorrectID { get; private set; }


		////////////////

		public override void Initialize() {
			this.Logic = new TimerLogic();
			this.ID = Guid.NewGuid().ToString( "D" );
			this.HasCorrectID = false;  // 'Load()' decides if no pre-existing one is found
		}

		////////////////

		public override void Load( TagCompound tags ) {
			if( tags.ContainsKey( "world_id" ) ) {
				IList<int> timer_start_durations;
				IList<int> timer_durations;
				IList<string> timer_actions;
				IList<bool> timer_repeats;
				IList<bool> timer_runs;

				this.ID = tags.GetString( "world_id" );
				timer_start_durations = tags.GetList<int>( "timer_start_durations" );
				timer_durations = tags.GetList<int>( "timer_durations" );
				timer_actions = tags.GetList<string>( "timer_actions" );
				timer_repeats = tags.GetList<bool>( "timer_repeats" );
				if( tags.ContainsKey( "timer_runs" ) ) {
					timer_runs = tags.GetList<bool>( "timer_runs" );
				} else {
					timer_runs = new List<bool>( timer_durations.Count );
				}

				this.Logic.Load( timer_start_durations, timer_durations, timer_actions, timer_repeats, timer_runs );
			}

			this.HasCorrectID = true;
		}

		public override TagCompound Save() {
			IList<int> start_durations = new List<int>();
			IList<int> durations = new List<int>();
			IList<string> actions = new List<string>();
			IList<bool> repeats = new List<bool>();
			IList<bool> runs = new List<bool>();

			foreach( var timer in this.Logic.Timers ) {
				start_durations.Add( timer.StartDuration );
				durations.Add( timer.Duration );
				actions.Add( timer.Action );
				repeats.Add( timer.Repeats );
				runs.Add( timer.IsRunning );
			}

			var tags = new TagCompound {
				{ "world_id", this.ID },
				{ "timer_start_durations", start_durations },
				{ "timer_durations", durations },
				{ "timer_actions", actions },
				{ "timer_repeats", repeats },
				{ "timer_runs", runs },
			};

			return tags;
		}

		////////////////

		public override void NetReceive( BinaryReader reader ) {
			try {
				string id = reader.ReadString();
				bool is_correct_id = reader.ReadBoolean();

				if( is_correct_id ) { this.ID = id; }
				this.HasCorrectID = is_correct_id;
			} catch( Exception e ) { ErrorLogger.Log( e.ToString() ); }
		}

		public override void NetSend( BinaryWriter writer ) {
			try {
				writer.Write( this.ID );
				writer.Write( this.HasCorrectID );
			} catch(Exception e ) { ErrorLogger.Log( e.ToString() ); }
		}


		////////////////

		public override void PreUpdate() {
			if( Main.netMode != 1 ) {	// Not client
				this.Logic.Update( (TimeLimitMod)this.mod );
			}
		}
	}
}
