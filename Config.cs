using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace TimeLimit {
	public class TimeLimitConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		public bool DebugModeInfo = false;
		public bool DebugModeNetInfo = false;

		public string[] Afflictions = {
			"Potion Sickness",
			"Darkness",
			"Bleeding",
			"Weak",
			"Broken Armor",
			"Ichor",
			"Chaos State",
			"Stinky",
			"Creative Shock"
		};

		[DefaultValue( -384 )]
		public int TimerDisplayX = -384;
		[DefaultValue( 256 )]
		public int TimerDisplayY = -256;
	}
}
