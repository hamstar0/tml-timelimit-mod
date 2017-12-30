using System;
using Terraria;
using Terraria.ModLoader;


namespace TimeLimit {
	class TimerBeginCommand : ModCommand {
		public override CommandType Type { get { return CommandType.World; } }
		public override string Command { get { return "timerbegin"; } }
		public override string Usage { get { return "/timerbegin 300 exit false"; } }
		public override string Description { get { return "Begins a new timer. Parameters are: <seconds> for duration, <action>, <repeating>. Actions: 'none', 'kill', 'hardkill', 'afflict', '<custom>'"; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 1 ) { throw new Exception("Invalid command call from client."); }

			int duration;
			bool repeats;
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();

			if( !int.TryParse( args[0], out duration ) ) {
				throw new UsageException( args[0] + " is not an integer" );
			}

			string action = args[1];

			if( !bool.TryParse( args[2], out repeats ) ) {
				throw new UsageException( args[0] + " is not boolean" );
			}

			myworld.Logic.Add( duration * 60, action, repeats );

			caller.Reply( "Timer to perform action '"+action+"' added." );
		}
	}



	class TimerEndCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Server; } }
		public override string Command { get { return "timerend"; } }
		public override string Usage { get { return "/timerend"; } }
		public override string Description { get { return "Aborts all running timers."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();
			myworld.Logic.AbortAll();
		}
	}
}
