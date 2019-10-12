using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace TimeLimit {
	public class TimeLimitConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		public bool DebugModeInfo = false;
		public bool DebugModeNetInfo = false;

		public List<string> Afflictions = new List<string> {
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

		[Range( -2048, 2048 )]
		[DefaultValue( -384 )]
		public int TimerDisplayX = -384;
		[Range( -1024, 1024 )]
		[DefaultValue( 256 )]
		public int TimerDisplayY = -256;



		////////////////

		public override ModConfig Clone() {
			var clone = (TimeLimitConfig)base.Clone();

			clone.Afflictions = new List<string>( this.Afflictions );

			return clone;
		}
	}
}
