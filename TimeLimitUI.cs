using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TimeLimit.Logic;
using TimeLimit.NetProtocol;

namespace TimeLimit; 

partial class TimeLimitUI : ModSystem {
	public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
		int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
		if( idx != -1 ) {
			GameInterfaceDrawMethod drawMethod = delegate {
				var myworld = ModContent.GetInstance<TimeLimitSystem>();

				myworld.Logic.DrawTimers( Main.spriteBatch );

				return true;
			};

			var interfaceLayer = new LegacyGameInterfaceLayer( "TimeLimit: Timers", drawMethod,
				InterfaceScaleType.UI );

			layers.Insert( idx, interfaceLayer );
		}
	}
}
