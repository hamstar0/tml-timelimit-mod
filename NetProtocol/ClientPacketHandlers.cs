using HamstarHelpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
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
			case TimeLimitProtocolTypes.TimerStart:
				ClientPacketHandlers.ReceiveTimersOnClient( mymod, reader );
				break;
			case TimeLimitProtocolTypes.TimersStop:
				ClientPacketHandlers.ReceiveStopTimersCommandOnClient( mymod, reader );
				break;
			case TimeLimitProtocolTypes.TimersPause:
				ClientPacketHandlers.ReceivePauseTimersCommandOnClient( mymod, reader );
				break;
			case TimeLimitProtocolTypes.TimersResume:
				ClientPacketHandlers.ReceiveResumeTimersCommandOnClient( mymod, reader );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////
		// Client Senders
		////////////////

		public static void SendModSettingsRequestFromClient( TimeLimitMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.RequestModSettings );

			packet.Send();
		}

		public static void SendTimersRequestFromClient( TimeLimitMod mymod ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.RequestTimers );

			packet.Send();
		}




		////////////////
		// Client Receivers
		////////////////

		private static void ReceiveModSettingsOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			mymod.JsonConfig.DeserializeMe( reader.ReadString() );
		}

		private static void ReceiveTimersOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			int start_duration = reader.ReadInt32();
			int duration = reader.ReadInt32();
			string action = reader.ReadString();
			bool repeats = reader.ReadBoolean();
			bool running = reader.ReadBoolean();

			myworld.Logic.StartTimer( start_duration, duration, action, repeats, running );

			Main.NewText( "Timer started to perform action '" + action + "'.", Color.Yellow );
		}

		private static void ReceiveStopTimersCommandOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.StopAllTimers();

			Main.NewText( "Timers stopped.", Color.Yellow );
		}
		
		private static void ReceivePauseTimersCommandOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.PauseAllTimers();

			Main.NewText( "Timers paused.", Color.Yellow );
		}
		
		private static void ReceiveResumeTimersCommandOnClient( TimeLimitMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { throw new Exception( "Not client" ); }

			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.ResumeAllTimers();

			Main.NewText( "Timers resumed.", Color.Yellow );
		}
	}
}
