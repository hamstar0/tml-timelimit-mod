using HamstarHelpers.Helpers.DebugHelpers;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerAllPauseCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command { get { return "timerallpause"; } }
		public override string Usage { get { return "/timerallpause"; } }
		public override string Description { get { return "Pauses all running timers."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (TimeLimitMod)this.mod;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.PauseAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendPauseAllTimersCommand( mymod, -1 );
			}

			caller.Reply( "Timers paused." );
			
			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "TimeLimit.TimerAllPauseCommand.Action - Success." );
			}
		}
	}
}
