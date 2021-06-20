using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Projectiles   
{
    public class BrainWormTest : ModProjectile {
        Random rand = new Random();


        public override void SetStaticDefaults() {
            DisplayName.SetDefault("BrainWormTest");
        }
        public override void SetDefaults() {
            projectile.width = 16;
            projectile.height = 16;
            projectile.ranged = true;
            projectile.friendly = true;
        }

        public override void AI() {
            Vector2 vel = projectile.velocity;

            float angle = (float)Math.Atan2(vel.X, vel.Y);

            if (rand.Next(0,2) == 1) {
                angle += 0.1f;
            } else {angle -= 0.1f;}

            projectile.velocity = new Vector2((float)Math.Sin(angle)*5, (float)Math.Cos(angle)*5);
        }
    }
}