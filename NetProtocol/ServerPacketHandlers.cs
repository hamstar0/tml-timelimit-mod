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

		public static void BroadcastTimerStartFromServer( TimeLimitMod mymod, int duration, string action, bool repeats ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimerStart );
			packet.Write( duration );
			packet.Write( action );
			packet.Write( repeats );

			packet.Send();
		}



		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveModSettingsRequestOnServer( TimeLimitMod mymod, BinaryReader reader, int player_who ) {
			ServerPacketHandlers.SendModSettingsFromServer( mymod, Main.player[player_who] );
		}
	}
}
