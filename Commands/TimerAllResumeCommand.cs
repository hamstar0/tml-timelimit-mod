﻿using ModLibsCore.Libraries.Debug;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.NetProtocol;

namespace TimeLimit.Commands {
	class TimerAllResumeCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command => "timer-resume-all";
		public override string Usage => "/" + this.Command;
		public override string Description => "Resumes all running timers.";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (TimeLimitMod)this.Mod;
			var myworld = ModContent.GetInstance<TimeLimitSystem>();
			myworld.Logic.ResumeAllTimers();

			if( Main.netMode == 2 ) {
				SendPackets.SendResumeAllTimersCommand( -1 );
			}

			caller.Reply( "Timers resumed." );

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Warn( "Success." );
			}
		}
	}
}
