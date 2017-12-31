using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace TimeLimit.NetProtocol {
	static class ClientPacketHandlers {
		public static void HandlePacket( TimeLimitMod mymod, BinaryReader reader ) {
			TimeLimitProtocolTypes protocol = (TimeLimitProtocolTypes)reader.ReadByte();
			
			switch( protocol ) {
			case TimeLimitProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveModSettingsOnClient( mymod, reader );
				break;
			case TimeLimitProtocolTypes.TimerSend:
				ClientPacketHandlers.ReceiveTimersOnClient( mymod, reader );
				break;
			case TimeLimitProtocolTypes.EndTimers:
				ClientPacketHandlers.ReceiveEndTimersCommandOnClient( mymod, reader );
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

		public static void SendTimersRequestFromClient( TimeLimitMod mymod ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.RequestTimers );

			packet.Send();
		}
		



		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettingsOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			mymod.JsonConfig.DeserializeMe( reader.ReadString() );
		}

		private static void ReceiveTimersOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			int start_duration = reader.ReadInt32();
			int duration = reader.ReadInt32();
			string action = reader.ReadString();
			bool repeats = reader.ReadBoolean();

			myworld.Logic.StartTimer( start_duration, duration, action, repeats );

			Main.NewText( "Timer to perform action '" + action + "' added.", Color.Yellow );
		}

		private static void ReceiveEndTimersCommandOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.EndAllTimers();

			Main.NewText( "Timers ended.", Color.Yellow );
		}
	}
}
