using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;


namespace TimeLimit.Commands {
	class TimerStartCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command { get { return "timer-start"; } }
		public override string Usage { get { return "/"+this.Command+" 300 exit false"; } }
		public override string Description { get { return "Starts a new timer."+
			"\n   Parameters: <seconds> <action> <loops>"+
			"\n   Actions types: 'none', 'exit', 'serverclose', 'kill', 'hardkill', 'afflict', '<custom>'"; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (TimeLimitMod)this.mod;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			int seconds;
			bool repeats;
			string action;

			if( args.Length < 2 ) {
				throw new UsageException("Insufficient arguments.");
			}

			if( !int.TryParse( args[0], out seconds ) ) {
				caller.Reply( args[0] + " is not an integer", Color.Red );
				return;
			}

			action = args[1];
			if( !mymod.Logic.IsValidAction(action) ) {
				caller.Reply( args[1] + " is not a valid action", Color.Red );
				return;
			}

			if( !bool.TryParse( args[2], out repeats ) ) {
				caller.Reply( args[2] + " is not boolean", Color.Red );
				return;
			}

			try {
				ActionTimer timer = myworld.Logic.StartTimer( mymod, seconds * 60, seconds * 60, action, repeats, true );

				if( Main.netMode != 0 ) {
					caller.Reply( "Timer started." );
					SendPackets.SendStartTimerCommand( mymod, timer, myworld.Logic.Timers.Count, -1, -1 );
				} else {
					caller.Reply( "Timer started to perform action '" + action + "'" + ( repeats ? " repeatedly." : "." ) );
				}
				
				if( mymod.Config.DebugModeInfo ) {
					LogHelpers.Log( "TimeLimit.TimerStartCommand.Action - Success." );
				}
			} catch( Exception e ) {
				LogHelpers.Log( "TimeLimit.TimerStartCommand.Action - " + e.ToString() );
			}
		}
	}
}
