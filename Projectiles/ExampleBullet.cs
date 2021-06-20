using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System.IO;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Projectiles
{
    // [AutoloadEquip(EquipType.Head)]
    public class ExampleBullet : ModProjectile {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Example Helmest");
        }
        public override void SetDefaults() {
            projectile.width = 16;
            projectile.height = 16;
            projectile.ranged = true;
            projectile.friendly = true;
        }

        public override void AI()
        {
			Vector2 move = Vector2.Zero;
			float distance = 400f;
			bool target = false;
			for (int k = 0; k < 200; k++) {
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5) {
					Vector2 newMove = Main.npc[k].Center - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance) {
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}
			if (target) {
				AdjustMagnitude(ref move);
				projectile.velocity = (10 * projectile.velocity + move) / 11f;
                AdjustMagnitude(ref projectile.velocity);
            }
        }

		private void AdjustMagnitude(ref Vector2 vector) {
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f) {
				vector *= 10f / magnitude;
			}
		}
    }
}