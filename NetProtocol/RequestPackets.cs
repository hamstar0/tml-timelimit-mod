using HamstarHelpers.Helpers.Debug;
using System.IO;
using Terraria.ModLoader;
using TimeLimit.Logic;


namespace TimeLimit.NetProtocol {
	static class RequestPackets {
		public static bool HandlePacket( TimeLimitProtocolTypes protocol, BinaryReader reader, int sourceWho ) {
			var mymod = TimeLimitMod.Instance;
			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Log( "<< TimeLimit.RequestPackets.HandlePacket - " + protocol.ToString() );
			}

			switch( protocol ) {
			case TimeLimitProtocolTypes.RequestTimers:
				RequestPackets.ReceiveTimersRequest( reader, sourceWho );
				return true;
			}
			return false;
		}

		
		////////////////

		public static void RequestTimers( int toWho = -1, int ignoreWho = -1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.RequestTimers );

			packet.Send( toWho, ignoreWho );
		}

		
		////////////////

		private static void ReceiveTimersRequest( BinaryReader reader, int sourceWho ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = ModContent.GetInstance<TimeLimitWorld>();

			foreach( ActionTimer timer in myworld.Logic.Timers ) {
				if( timer.Duration == 0 && !timer.Repeats ) {
					continue;
				}

				SendPackets.SendStartTimerCommand( timer, myworld.Logic.Timers.Count, sourceWho );
			}
		}
	}
}
