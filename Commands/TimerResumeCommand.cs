using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerResumeCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command => "timer-resume";
		public override string Usage => "/"+this.Command+" <action name>";
		public override string Description => "Resumes running timers of a given action type.";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (TimeLimitMod)this.mod;
			var myworld = ModContent.GetInstance<TimeLimitWorld>();

			if( args.Length < 1 ) {
				throw new ModHelpersException( "Insufficient arguments." );
			}

			string action = args[0];
			if( !mymod.Logic.IsValidAction( action ) ) {
				caller.Reply( args[0] + " is not a valid action", Color.Red );
				return;
			}

			myworld.Logic.ResumeTimers( action );

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeTimersCommand( action, - 1 );
			}

			caller.Reply( "Timer '"+action+"' resumed." );
			
			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( "Success." );
			}
		}
	}
}
