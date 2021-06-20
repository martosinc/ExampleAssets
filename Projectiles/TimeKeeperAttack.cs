using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace Examples.Projectiles {
    public class TimeKeeperAttack : ModProjectile {
        int timer = 0;
        Vector2 target;
        public float inertia = 10f;
        public float speed = 15f;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Laser Minion");
        }
        public override void SetDefaults() {
            projectile.width = 24;
            projectile.height = 24;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.tileCollide = false;
        }
        public override bool PreAI() {
            float closestDist = 50000f;
            Player[] Players = Main.player;
            for (int i = 0;i<Players.Length;i++) {
                Player player = Players[i];
                float between = Vector2.Distance(player.Center, projectile.Center);
                if (between < closestDist) {
                    closestDist = between;
                    target = player.Center;
                }
            }

            return true;
        }
        public override void AI() {
            if (timer == 0) {
                Dash(GetVector(MathHelper.ToRadians(Main.rand.Next(360))), speed);
            }
            if (timer < 30 && timer > 15) {
                Dash(GetVector(MathHelper.ToRadians(Main.rand.Next(360))), 3);
            } else if (timer==30) {
                projectile.hostile = true;
                var angle = -(float)Math.Atan2(target.Y-projectile.position.Y-15f,target.X-projectile.position.X-25f)+MathHelper.ToRadians(90);
                Dash(GetVector(angle), speed*3);
            } if (timer > 30) {
                projectile.alpha += 5;
                if (projectile.alpha == 255) {
                    projectile.active = false;
                }
            }
            timer++;
            projectile.velocity = (projectile.velocity * (inertia - 1)) / inertia;
        }
		private void Dash(Vector2 direction, float speed)
		{
            projectile.velocity = direction * speed;

			projectile.netUpdate = true;
		}
        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
    }
}