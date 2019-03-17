using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;
using Terraria.ModLoader;
using TimeLimit.Logic;


namespace TimeLimit.NetProtocol {
	static class SendPackets {
		public static bool HandlePacket( TimeLimitProtocolTypes protocol, BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;

			if( mymod.Config.DebugModeNetInfo ) {
				LogHelpers.Log( ">> TimeLimit.SendPackets.HandlePacket - " + protocol.ToString() );
			}

			switch( protocol ) {
			case TimeLimitProtocolTypes.ModSettings:
				SendPackets.ReceiveModSettings( reader );
				return true;
			case TimeLimitProtocolTypes.TimerStart:
				SendPackets.ReceiveTimer( reader );
				return true;
			case TimeLimitProtocolTypes.TimersStop:
				SendPackets.ReceiveStopTimersCommand( reader );
				return true;
			case TimeLimitProtocolTypes.TimersPause:
				SendPackets.ReceivePauseTimersCommand( reader );
				return true;
			case TimeLimitProtocolTypes.TimersResume:
				SendPackets.ReceiveResumeTimersCommand( reader );
				return true;
			case TimeLimitProtocolTypes.TimersAllStop:
				SendPackets.ReceiveStopAllTimersCommand( reader );
				return true;
			case TimeLimitProtocolTypes.TimersAllPause:
				SendPackets.ReceivePauseAllTimersCommand( reader );
				return true;
			case TimeLimitProtocolTypes.TimersAllResume:
				SendPackets.ReceiveResumeAllTimersCommand( reader );
				return true;
			}
			return false;
		}


		////////////////

		public static void SendModSettings( int toWho=-1, int ignoreWho=-1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.ModSettings );
			packet.Write( (string)mymod.ConfigJson.SerializeMe() );

			packet.Send( toWho, ignoreWho );
		}

		public static void SendStartTimerCommand( ActionTimer timer, int totalTimers, int toWho = -1, int ignoreWho = -1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimerStart );
			packet.Write( (int)timer.StartDuration );
			packet.Write( (int)timer.Duration );
			packet.Write( (string)timer.Action );
			packet.Write( (bool)timer.Repeats );
			packet.Write( (bool)timer.IsRunning );
			packet.Write( (int)totalTimers );

			packet.Send( toWho, ignoreWho );
		}

		public static void SendStopTimersCommand( string action, int toWho = -1, int ignoreWho = -1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersStop );
			packet.Write( (string)action );

			packet.Send( toWho, ignoreWho );
		}

		public static void SendPauseTimersCommand( string action, int toWho = -1, int ignoreWho = -1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersPause );
			packet.Write( (string)action );

			packet.Send( toWho, ignoreWho );
		}

		public static void SendResumeTimersCommand( string action, int toWho = -1, int ignoreWho = -1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersResume );
			packet.Write( (string)action );

			packet.Send( toWho, ignoreWho );
		}

		public static void SendStopAllTimersCommand( int toWho=-1, int ignoreWho=-1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersAllStop );

			packet.Send( toWho, ignoreWho );
		}

		public static void SendPauseAllTimersCommand( int toWho=-1, int ignoreWho=-1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersAllPause );

			packet.Send( toWho, ignoreWho );
		}

		public static void SendResumeAllTimersCommand( int toWho=-1, int ignoreWho=-1 ) {
			var mymod = TimeLimitMod.Instance;
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)TimeLimitProtocolTypes.TimersAllResume );

			packet.Send( toWho, ignoreWho );
		}


		////////////////

		private static void ReceiveModSettings( BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;
			string json = reader.ReadString();

			mymod.Config.LoadFromNetwork( json );
		}

		private static void ReceiveTimer( BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();
			int startDuration = reader.ReadInt32();
			int duration = reader.ReadInt32();
			string action = reader.ReadString();
			bool repeats = reader.ReadBoolean();
			bool running = reader.ReadBoolean();
			int totalTimers = reader.ReadInt32();
			
			myworld.Logic.StartTimerFromNetwork( mymod, startDuration, duration, action, repeats, running, totalTimers );
		}

		private static void ReceiveStopTimersCommand( BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			string action = reader.ReadString();

			myworld.Logic.StopTimersFromNetwork( action );
		}

		private static void ReceivePauseTimersCommand( BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			string action = reader.ReadString();

			myworld.Logic.PauseTimersFromNetwork( action );
		}

		private static void ReceiveResumeTimersCommand( BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			string action = reader.ReadString();

			myworld.Logic.ResumeTimersFromNetwork( action );
		}

		private static void ReceiveStopAllTimersCommand( BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.StopAllTimersFromNetwork();
		}

		private static void ReceivePauseAllTimersCommand( BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.PauseAllTimersFromNetwork();
		}

		private static void ReceiveResumeAllTimersCommand( BinaryReader reader ) {
			var mymod = TimeLimitMod.Instance;
			var myworld = mymod.GetModWorld<TimeLimitWorld>();

			myworld.Logic.ResumeAllTimersFromNetwork();
		}
	}
}
