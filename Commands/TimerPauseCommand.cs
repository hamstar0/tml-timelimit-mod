using Microsoft.Xna.Framework;
using ModLibsCore.Libraries.Debug;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;

namespace TimeLimit.Commands {
	class TimerPauseCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command => "timer-pause";
		public override string Usage => "/"+this.Command+" <action name>";
		public override string Description => "Pauses running timers of a given action type.";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (TimeLimitMod)this.Mod;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();

			if( args.Length < 1 ) {
				throw new UsageException( "Insufficient arguments." );
			}

			string action = args[0];
			if( !mymod.Logic.IsValidAction( action ) ) {
				caller.Reply( args[0] + " is not a valid action", Color.Red );
				return;
			}

			myworld.Logic.PauseTimers( action );

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseTimersCommand( action, - 1 );
			}

			caller.Reply( "Timer '" + action + "' paused." );

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( "Success." );
			}
		}
	}
}
