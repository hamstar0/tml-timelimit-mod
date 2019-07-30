using HamstarHelpers.Helpers.Debug;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerAllStopCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command => "timer-stop-all";
		public override string Usage => "/" +this.Command;
		public override string Description => "Stops all running timers.";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (TimeLimitMod)this.mod;
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();
			myworld.Logic.StopAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendStopAllTimersCommand( -1 );
			}

			caller.Reply( "Timers stopped." );

			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Alert( "Success." );
			}
		}
	}
}
