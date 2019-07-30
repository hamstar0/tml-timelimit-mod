using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TimeLimit.Logic;


namespace TimeLimit {
	class TimeLimitWorld : ModWorld {
		public WorldLogic Logic { get; private set; }


		////////////////

		public override void Initialize() {
			this.Logic = new WorldLogic();
		}

		public override void Load( TagCompound tags ) {
			this.Logic.Load( tags );
		}

		public override TagCompound Save() {
			return this.Logic.Save( );
		}


		////////////////

		public override void PreUpdate() {
			if( Main.netMode != 1 ) {   // Not client
				this.Logic.Update();
			}
		}
	}
}
