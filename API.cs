using System;
using Terraria;


namespace TimeLimit {
	public static class TimeLimitAPI {
		public delegate void TimesUpHook();


		public static TimeLimitConfigData GetModSettings() {
			return TimeLimitMod.Instance.Config;
		}


		public static void OnTimesUp( Player player, string hook_name, TimesUpHook hook ) {
			var myworld = TimeLimitMod.Instance.GetModWorld<TimeLimitWorld>();
			myworld.Logic.TimesUpHooks[ hook_name ] = new Action(hook);
		}
	}
}
