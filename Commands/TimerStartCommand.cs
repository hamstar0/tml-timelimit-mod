using Terraria;
using Terraria.ModLoader;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerStartCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command { get { return "timerstart"; } }
		public override string Usage { get { return "/timerstart 300 exit false"; } }
		public override string Description { get { return "Starts a new timer."+
			"\n   Parameters: <seconds> <action> <loops>"+
			"\n   Actions types: 'none', 'kill', 'hardkill', 'afflict', '<custom>'"; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();
			int seconds;
			bool repeats;
			string action;

			if( !int.TryParse( args[0], out seconds ) ) {
				throw new UsageException( args[0] + " is not an integer" );
			}

			action = args[1];
			if( !myworld.Logic.IsValidAction(action) ) {
				throw new UsageException( args[1] + " is not a valid action" );
			}

			if( !bool.TryParse( args[2], out repeats ) ) {
				throw new UsageException( args[2] + " is not boolean" );
			}
			
			ActionTimer timer = myworld.Logic.StartTimer( seconds * 60, seconds * 60, action, repeats, true );

			if( Main.netMode != 0 ) {
				SendPackets.SendStartTimerCommand( (TimeLimitMod)this.mod, timer, myworld.Logic.Timers.Count, -1 );
			} else {
				caller.Reply( "Timer started to perform action '" + action + "'" + ( repeats ? " repeatedly." : "." ) );
			}
		}
	}
}
