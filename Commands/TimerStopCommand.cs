using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerStopCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command { get { return "timerstop"; } }
		public override string Usage { get { return "/timerstop <action name>"; } }
		public override string Description { get { return "Stops running timers of a given action type."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (TimeLimitMod)this.mod;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			if( args.Length < 1 ) {
				throw new UsageException( "Insufficient arguments." );
			}

			string action = args[0];
			if( !mymod.Logic.IsValidAction( action ) ) {
				caller.Reply( args[0] + " is not a valid action", Color.Red );
				return;
			}

			myworld.Logic.StopTimers( action );

			if( Main.netMode == 2 ) {
				SendPackets.SendStopTimersCommand( (TimeLimitMod)this.mod, action, - 1 );
			}
			caller.Reply( "Timer '"+action+"' stopped." );
		}
	}
}
