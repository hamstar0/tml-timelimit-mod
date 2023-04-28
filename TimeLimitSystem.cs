using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TimeLimit.Logic;


namespace TimeLimit {
	class TimeLimitSystem : ModSystem {
		public WorldLogic Logic { get; private set; }


		////////////////

		public override void Load() {
			this.Logic = new WorldLogic();
		}

		public override void LoadWorldData( TagCompound tags ) {
			this.Logic.Load( tags );
		}

		public override void SaveWorldData( TagCompound tags ) {
			this.Logic.Save( tags );
		}


		////////////////

		public override void PreUpdateWorld() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				this.Logic.Update();
			}
		}
	}
}
