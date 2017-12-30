using HamstarHelpers.DebugHelpers;
using System.IO;
using Terraria.ModLoader;


namespace TimeLimit.NetProtocol {
	static class ClientPacketHandlers {
		public static void HandlePacket( TimeLimitMod mymod, BinaryReader reader ) {
			TimeLimitProtocolTypes protocol = (TimeLimitProtocolTypes)reader.ReadByte();
			
			switch( protocol ) {
			case TimeLimitProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveModSettingsOnClient( mymod, reader );
				break;
			case TimeLimitProtocolTypes.TimerStart:
				ClientPacketHandlers.ReceiveTimerStartOnClient( mymod, reader );
				break;
			default:
				DebugHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendModSettingsRequestFromClient( TimeLimitMod mymod ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.RequestModSettings );

			packet.Send();
		}
		


		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettingsOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			mymod.JsonConfig.DeserializeMe( reader.ReadString() );
		}

		private static void ReceiveTimerStartOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			int duration = reader.ReadInt32();
			string action = reader.ReadString();
			bool repeats = reader.ReadBoolean();

			myworld.Logic.Add( duration, action, repeats );
		}
	}
}
