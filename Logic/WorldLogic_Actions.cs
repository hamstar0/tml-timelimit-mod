using System;
using Microsoft.Xna.Framework;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.TModLoader;
using ModLibsGeneral.Libraries.Buffs;
using ModLibsGeneral.Libraries.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeLimit.Logic {
	partial class WorldLogic {
		internal void RunAction( string action, bool isLoop ) {
			var mymod = TimeLimitMod.Instance;

			if( mymod.Config.DebugModeInfo ) {
				LogLibraries.Info( "TimeLimit.WorldLogic.RunAction - " + action + " (loops? " + isLoop + ")" );
			}

			static void NewText( string text, Color color) {
				if( Main.dedServ ) {
					Console.WriteLine( text );
				} else {
					Main.NewText( text, color );
				}
			}

			switch( action ) {
			case "none":
				if( !isLoop ) {
					NewText( "Time's up.", Color.Red );
				} else {
					NewText( "Timer restarting.", Color.Yellow );
				}
				break;
			case "exit":
				NewText( "Time's up. Bye!", Color.Red );
				if( Main.netMode != 2 ) {
					TmlLibraries.ExitToMenu();
				}
				break;
			case "serverclose":
				if( Main.netMode == 2 ) {
					TmlLibraries.ExitToDesktop();
				} else {
					NewText( "Time's up. Bye, world!", Color.Red );
					TmlLibraries.ExitToMenu();
				}
				break;
			case "kill":
				if( Main.netMode != 2 ) {   // Not server
					for( int i = 0; i < Main.player.Length; i++ ) {
						Player player = Main.player[i];
						if( player == null || !player.active || player.dead ) { continue; }

						player.KillMe( PlayerDeathReason.ByCustomReason("Time's up."), 9999, 0 );
					}
				}
				break;
			case "hardkill":
				if( Main.netMode != 2 ) {   // Not server
					for( int i = 0; i < Main.player.Length; i++ ) {
						Player player = Main.player[i];
						if( player == null || !player.active || player.dead ) { continue; }

						PlayerLibraries.KillWithPermadeath( player, "Time's up. Game over." );
					}
				}
				break;
			case "afflict":
				var afflictions = string.Join( ",", mymod.Config.Afflictions );

				if( mymod.Config.DebugModeInfo ) {
					LogLibraries.Info( " TimeLimit.WorldLogic.RunAction - " + action + ": " + afflictions );
				}

				if( !isLoop ) {
					NewText( "Time's up. You now have the following: " + afflictions, Color.Red );
				} else {
					NewText( "Timer restarting. You now have the following: " + afflictions, Color.Yellow );
				}

				if( Main.netMode != NetmodeID.MultiplayerClient ) {
					this.ApplyAffliction();
				}
				break;
			case "unafflict":
				if( !isLoop ) {
					NewText( "Time's up. All afflictions removed.", Color.Red );
				} else {
					NewText( "Timer restarting. All afflictions removed.", Color.Yellow );
				}
				if( Main.netMode != NetmodeID.MultiplayerClient ) {
					this.RemoveAffliction();
				}
				break;
			default:
				var myworld = ModContent.GetInstance<TimeLimitSystem>();
				
				if( !mymod.Logic.CustomActions.ContainsKey(action) ) {
					LogLibraries.Info( "No such time's up event by name " + action );
					break;
				}
				mymod.Logic.CustomActions[ action ]();
				break;
			}
		}


		////////////////

		public void ApplyAffliction() {
			var mymod = TimeLimitMod.Instance;

			foreach( string affliction in mymod.Config.Afflictions ) {
				if( !BuffAttributesLibraries.DisplayNamesToIds.ContainsKey(affliction) ) {
					LogLibraries.Info( "No such afflication ((de)buff) by name " + affliction );
					continue;
				}

				for( int i=0; i<Main.player.Length; i++ ) {
					Player player = Main.player[i];
					if( player == null || !player.active ) { continue; }
					
					int buffId = BuffAttributesLibraries.DisplayNamesToIds[affliction];

					BuffPermanenceLibraries.AddPermanentBuff( player, buffId );
				}
			}
		}
		
		public void RemoveAffliction() {
			var mymod = TimeLimitMod.Instance;

			foreach( string affliction in mymod.Config.Afflictions ) {
				if( !BuffAttributesLibraries.DisplayNamesToIds.ContainsKey( affliction ) ) {
					LogLibraries.Info( "No such afflication ((de)buff) by name " + affliction );
					continue;
				}

				for( int i = 0; i < Main.player.Length; i++ ) {
					Player player = Main.player[i];
					if( player == null || !player.active ) { continue; }

					int buffId = BuffAttributesLibraries.DisplayNamesToIds[affliction];

					BuffPermanenceLibraries.RemovePermanentBuff( player, buffId );
				}
			}
		}
	}
}
