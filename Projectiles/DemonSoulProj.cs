using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Projectiles   
{
    public class DemonSoulProj : ModProjectile {
        Random rand = new Random();

        bool spawned = false;
        int timer = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("DemonSoulProj");

            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults() {
            projectile.width = 61;
            projectile.height = 40;
            projectile.ranged = true;
            projectile.friendly = true;
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 8)
            {
                projectile.frame++;
                projectile.frame %= 4;
                projectile.frameCounter = 0;
            }
        }

        public override void AI() {
            int limit = 1 * 60;

            if (!spawned) {
                projectile.velocity /= 2;
                spawned = true;
            }
            Vector2 vel = projectile.velocity;

            float angle = (float)Math.Atan2(vel.X, vel.Y);

            projectile.rotation = -((angle * 180f) / 3.14f)/60+1.5f;
            if (projectile.rotation % 6 > 1.5 && projectile.rotation % 6 < 4.5) {
                projectile.spriteDirection = -1;
                projectile.rotation += 3f;
            }

            if (timer == limit) {
                projectile.velocity *= 3;
            }

            timer+=1;

            AnimateProjectile();
        }
    }
}