using HamstarHelpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace TimeLimit.NetProtocol {
	static class ServerPacketHandlers {
		public static void HandlePacket( TimeLimitMod mymod, BinaryReader reader, int player_who ) {
			TimeLimitProtocolTypes protocol = (TimeLimitProtocolTypes)reader.ReadByte();
			
			switch( protocol ) {
			case TimeLimitProtocolTypes.RequestModSettings:
				ServerPacketHandlers.ReceiveModSettingsRequestOnServer( mymod, reader, player_who );
				break;
			case TimeLimitProtocolTypes.RequestTimers:
				ServerPacketHandlers.ReceiveTimersRequestOnServer( mymod, reader, player_who );
				break;
			default:
				DebugHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}


		
		////////////////
		// Server Senders
		////////////////

		public static void SendModSettingsFromServer( TimeLimitMod mymod, Player player ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.ModSettings );
			packet.Write( (string)mymod.JsonConfig.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}

		public static void SendTimerStartFromServer( TimeLimitMod mymod, int to_who, int start_duration, int duration, string action, bool repeats ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimerStart );
			packet.Write( start_duration );
			packet.Write( duration );
			packet.Write( action );
			packet.Write( repeats );

			packet.Send( to_who );
		}



		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveModSettingsRequestOnServer( TimeLimitMod mymod, BinaryReader reader, int player_who ) {
			ServerPacketHandlers.SendModSettingsFromServer( mymod, Main.player[player_who] );
		}

		private static void ReceiveTimersRequestOnServer( TimeLimitMod mymod, BinaryReader reader, int player_who ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			for( int i=0; i<myworld.Logic.TimerDurations.Count; i++ ) {
				int start_duration = myworld.Logic.TimerStartDurations[i];
				int duration = myworld.Logic.TimerDurations[i];
				string action = myworld.Logic.TimerActions[i];
				bool repeats = myworld.Logic.TimerRepeats[i];
				if( duration == 0 && !repeats ) {
					continue;
				}

				ServerPacketHandlers.SendTimerStartFromServer( mymod, player_who, start_duration, duration, action, repeats );
			}
		}
	}
}
