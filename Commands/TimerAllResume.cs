using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerAllResumeCommand : ModCommand {
		public override CommandType Type { get { return CommandType.World; } }
		public override string Command { get { return "timerallresume"; } }
		public override string Usage { get { return "/timerallresume"; } }
		public override string Description { get { return "Resumes all running timers."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var myworld = this.mod.GetModWorld<TimeLimitWorld>();
			myworld.Logic.ResumeAllTimers();

			if( Main.netMode == 2 ) {
				ServerPacketHandlers.SendResumeTimersCommandFromServer( (TimeLimitMod)this.mod, -1 );
			}
			caller.Reply( "Timers resumed." );
		}
	}
}
