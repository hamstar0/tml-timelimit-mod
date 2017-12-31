using HamstarHelpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using TimeLimit.Logic;


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

		public static void SendModSettingsFromServer( TimeLimitMod mymod, int to_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.ModSettings );
			packet.Write( (string)mymod.JsonConfig.SerializeMe() );

			packet.Send( to_who );
		}

		public static void SendStartTimerCommandFromServer( TimeLimitMod mymod, int to_who, ActionTimer timer ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimerStart );
			packet.Write( (int)timer.StartDuration );
			packet.Write( (int)timer.Duration );
			packet.Write( (string)timer.Action );
			packet.Write( (bool)timer.Repeats );
			packet.Write( (bool)timer.IsRunning );

			packet.Send( to_who );
		}

		public static void SendStopTimersCommandFromServer( TimeLimitMod mymod, int to_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersStop );

			packet.Send( to_who );
		}

		public static void SendPauseTimersCommandFromServer( TimeLimitMod mymod, int to_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersPause );

			packet.Send( to_who );
		}

		public static void SendResumeTimersCommandFromServer( TimeLimitMod mymod, int to_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersResume );

			packet.Send( to_who );
		}



		////////////////
		// Server Receivers
		////////////////

		private static void ReceiveModSettingsRequestOnServer( TimeLimitMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			ServerPacketHandlers.SendModSettingsFromServer( mymod, player_who );
		}

		private static void ReceiveTimersRequestOnServer( TimeLimitMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { throw new Exception( "Not server" ); }

			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			foreach( var timer in myworld.Logic.Timers ) {
				if( timer.Duration == 0 && !timer.Repeats ) {
					continue;
				}

				ServerPacketHandlers.SendStartTimerCommandFromServer( mymod, player_who, timer );
			}
		}
	}
}
