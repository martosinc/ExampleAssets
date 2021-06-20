using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System.IO;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Projectiles
{
    public class Example : ModProjectile {
        bool spawned = false;
        int timer = 0;
        Random rand = new Random();
        float dir = 0.4f;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Example");
        }
        public override void SetDefaults() {
            projectile.width = 8;
            projectile.height = 8;
            projectile.ranged = true;
            projectile.friendly = true;
        }


        public override void AI()
        {
            if (!spawned) {
                Vector2 position = Main.MouseWorld;

                projectile.position = position;
                projectile.velocity = new Vector2(0,0);
                spawned = true;

                var angle = rand.Next(0,360);

                projectile.velocity = new Vector2((float)Math.Sin(angle)*20, (float)Math.Cos(angle)*20);
                if (rand.Next(0,2) == 1) {dir*=-1;}
            }

            if (timer < 5) {
                Vector2 vel = projectile.velocity;
                float angle = (float)Math.Atan2(vel.X, vel.Y);

                projectile.velocity = new Vector2((float)Math.Sin(angle+dir)*10, (float)Math.Cos(angle+dir)*10);

                timer++;
            }
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
				projectile.velocity = (10 * projectile.velocity + move*1.5f) / 11f;
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