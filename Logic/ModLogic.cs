using System;
using System.Collections.Generic;


namespace TimeLimit.Logic {
	class ModLogic {
		internal IDictionary<string, CustomTimerAction> CustomActions = new Dictionary<string, CustomTimerAction>();


		////////////////

		public bool IsValidAction( string action ) {
			switch( action ) {
			case "none":
			case "exit":
			case "kill":
			case "hardkill":
			case "afflict":
				return true;
			default:
				return this.CustomActions.ContainsKey( action );
			}
		}
	}
}
