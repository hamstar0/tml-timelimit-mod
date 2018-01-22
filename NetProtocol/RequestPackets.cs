using System.IO;
using Terraria.ModLoader;
using TimeLimit.Logic;


namespace TimeLimit.NetProtocol {
	static class RequestPackets {
		public static bool HandlePacket( TimeLimitMod mymod, BinaryReader reader, int source_who ) {
			TimeLimitProtocolTypes protocol = (TimeLimitProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case TimeLimitProtocolTypes.RequestModSettings:
				RequestPackets.ReceiveModSettingsRequest( mymod, reader, source_who );
				return true;
			case TimeLimitProtocolTypes.RequestTimers:
				RequestPackets.ReceiveTimersRequest( mymod, reader, source_who );
				return true;
			}
			return false;
		}

		
		////////////////

		public static void RequestModSettings( TimeLimitMod mymod, int to_who=-1, int ignore_who=-1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.RequestModSettings );

			packet.Send( to_who, ignore_who );
		}

		public static void RequestTimers( TimeLimitMod mymod, int to_who = -1, int ignore_who = -1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.RequestTimers );

			packet.Send( to_who, ignore_who );
		}

		
		////////////////

		private static void ReceiveModSettingsRequest( TimeLimitMod mymod, BinaryReader reader, int source_who ) {
			SendPackets.SendModSettings( mymod, source_who );
		}

		private static void ReceiveTimersRequest( TimeLimitMod mymod, BinaryReader reader, int source_who ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			foreach( ActionTimer timer in myworld.Logic.Timers ) {
				if( timer.Duration == 0 && !timer.Repeats ) {
					continue;
				}

				SendPackets.SendStartTimerCommand( mymod, timer, myworld.Logic.Timers.Count, source_who );
			}
		}
	}
}
