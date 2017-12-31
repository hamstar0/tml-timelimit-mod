using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerAllPauseCommand : ModCommand {
		public override CommandType Type { get { return CommandType.World; } }
		public override string Command { get { return "timerallpause"; } }
		public override string Usage { get { return "/timerallpause"; } }
		public override string Description { get { return "Pauses all running timers."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();
			myworld.Logic.PauseAllTimers();

			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendPauseTimersCommandFromServer( (TimeLimitMod)this.mod, -1 );
			}
			caller.Reply( "Timers paused." );
		}
	}
}
