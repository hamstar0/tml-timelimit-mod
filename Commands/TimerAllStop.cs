using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerAllStopCommand : ModCommand {
		public override CommandType Type { get { return CommandType.World; } }
		public override string Command { get { return "timerallstop"; } }
		public override string Usage { get { return "/timerallstop"; } }
		public override string Description { get { return "Stops all running timers."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();
			myworld.Logic.StopAllTimers();

			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendStopTimersCommandFromServer( (TimeLimitMod)this.mod, -1 );
			}
			caller.Reply( "Timers stopped." );
		}
	}
}
