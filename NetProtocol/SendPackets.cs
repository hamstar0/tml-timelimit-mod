using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;
using Terraria.ModLoader;
using TimeLimit.Logic;


namespace TimeLimit.NetProtocol {
	static class SendPackets {
		public static bool HandlePacket( TimeLimitMod mymod, TimeLimitProtocolTypes protocol, BinaryReader reader ) {
			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Log( ">> TimeLimit.SendPackets.HandlePacket - " + protocol.ToString() );
			}

			switch( protocol ) {
			case TimeLimitProtocolTypes.ModSettings:
				SendPackets.ReceiveModSettings( mymod, reader );
				return true;
			case TimeLimitProtocolTypes.TimerStart:
				SendPackets.ReceiveTimer( mymod, reader );
				return true;
			case TimeLimitProtocolTypes.TimersStop:
				SendPackets.ReceiveStopTimersCommand( mymod, reader );
				return true;
			case TimeLimitProtocolTypes.TimersPause:
				SendPackets.ReceivePauseTimersCommand( mymod, reader );
				return true;
			case TimeLimitProtocolTypes.TimersResume:
				SendPackets.ReceiveResumeTimersCommand( mymod, reader );
				return true;
			case TimeLimitProtocolTypes.TimersAllStop:
				SendPackets.ReceiveStopAllTimersCommand( mymod, reader );
				return true;
			case TimeLimitProtocolTypes.TimersAllPause:
				SendPackets.ReceivePauseAllTimersCommand( mymod, reader );
				return true;
			case TimeLimitProtocolTypes.TimersAllResume:
				SendPackets.ReceiveResumeAllTimersCommand( mymod, reader );
				return true;
			}
			return false;
		}


		////////////////

		public static void SendModSettings( TimeLimitMod mymod, int to_who=-1, int ignore_who=-1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.ModSettings );
			packet.Write( (string)mymod.ConfigJson.SerializeMe() );

			packet.Send( to_who, ignore_who );
		}

		public static void SendStartTimerCommand( TimeLimitMod mymod, ActionTimer timer, int total_timers, int to_who = -1, int ignore_who = -1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimerStart );
			packet.Write( (int)timer.StartDuration );
			packet.Write( (int)timer.Duration );
			packet.Write( (string)timer.Action );
			packet.Write( (bool)timer.Repeats );
			packet.Write( (bool)timer.IsRunning );
			packet.Write( (int)total_timers );

			packet.Send( to_who, ignore_who );
		}

		public static void SendStopTimersCommand( TimeLimitMod mymod, string action, int to_who = -1, int ignore_who = -1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersStop );
			packet.Write( (string)action );

			packet.Send( to_who, ignore_who );
		}

		public static void SendPauseTimersCommand( TimeLimitMod mymod, string action, int to_who = -1, int ignore_who = -1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersPause );
			packet.Write( (string)action );

			packet.Send( to_who, ignore_who );
		}

		public static void SendResumeTimersCommand( TimeLimitMod mymod, string action, int to_who = -1, int ignore_who = -1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersResume );
			packet.Write( (string)action );

			packet.Send( to_who, ignore_who );
		}

		public static void SendStopAllTimersCommand( TimeLimitMod mymod, int to_who=-1, int ignore_who=-1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersAllStop );

			packet.Send( to_who, ignore_who );
		}

		public static void SendPauseAllTimersCommand( TimeLimitMod mymod, int to_who=-1, int ignore_who=-1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersAllPause );

			packet.Send( to_who, ignore_who );
		}

		public static void SendResumeAllTimersCommand( TimeLimitMod mymod, int to_who=-1, int ignore_who=-1 ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersAllResume );

			packet.Send( to_who, ignore_who );
		}


		////////////////

		private static void ReceiveModSettings( TimeLimitMod mymod, BinaryReader reader ) {
			string json = reader.ReadString();

			mymod.Config.LoadFromNetwork( mymod, json );
		}

		private static void ReceiveTimer( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			int start_duration = reader.ReadInt32();
			int duration = reader.ReadInt32();
			string action = reader.ReadString();
			bool repeats = reader.ReadBoolean();
			bool running = reader.ReadBoolean();
			int total_timers = reader.ReadInt32();
			
			myworld.Logic.StartTimerFromNetwork( mymod, start_duration, duration, action, repeats, running, total_timers );
		}

		private static void ReceiveStopTimersCommand( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			string action = reader.ReadString();

			myworld.Logic.StopTimersFromNetwork( action );
		}

		private static void ReceivePauseTimersCommand( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			string action = reader.ReadString();

			myworld.Logic.PauseTimersFromNetwork( action );
		}

		private static void ReceiveResumeTimersCommand( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			string action = reader.ReadString();

			myworld.Logic.ResumeTimersFromNetwork( action );
		}

		private static void ReceiveStopAllTimersCommand( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.StopAllTimersFromNetwork();
		}

		private static void ReceivePauseAllTimersCommand( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.PauseAllTimersFromNetwork();
		}

		private static void ReceiveResumeAllTimersCommand( TimeLimitMod mymod, BinaryReader reader ) {
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.ResumeAllTimersFromNetwork();
		}
	}
}
