using System;
using Examples.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Examples.Items.Weapons
{
    public class ShitYoyo : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shit Yoyo");
            Tooltip.SetDefault("Smells like shit...");
        }
        public override void SetDefaults() {
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 30;
			item.height = 26;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 16f;
			item.knockBack = 4f;
			item.damage = 60;
			item.rare = ItemRarityID.White;

			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;

			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 56);
			item.shoot = ModContent.ProjectileType<ShitYoyoProjectile>();

        }
    }
	public class ShitYoyoProjectile : ModProjectile {
		public override void SetStaticDefaults() {
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 325f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 17f;
		}

		public override void SetDefaults() {
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
		}
		private int TIMER = 0;
		public override void AI() {
			TIMER++;
			if (TIMER==12) {
				var position = projectile.Center + GetVector(MathHelper.ToRadians(Main.rand.Next(360)))*60f;
				var a = position;
				var b = projectile.Center;
				var angle = -(float)Math.Atan2(b.Y-a.Y,b.X-a.X)+MathHelper.ToRadians(90);

				var direction = GetVector(angle);
				direction.Normalize();
				direction *= 15;

				var speedX = direction.X;
				var speedY = direction.Y;

				Projectile.NewProjectile(position.X, position.Y,speedX,speedY, ModContent.ProjectileType<ShitYoyoBeam>(), 45, 7.5f, Main.player[projectile.owner].whoAmI);

				TIMER = 0;
			}
		}
        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
	}
	public class ShitYoyoBeam : ModProjectile {
        public override void SetDefaults() {
            projectile.timeLeft = 50;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.width = 12;
            projectile.height = 12;
            projectile.tileCollide = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
        }
        private int TIMER = 0;
        private int TIMER_MAX = 15;
        // private int alphaChange = 10;
        private int alphaChange = 255/15;
        private float inertia = 2f;
		private bool spawned = false;
		private Vector2 startVelocity;
        public override void AI() {
            TIMER++;

			Lighting.AddLight(projectile.Center, 10f, 10f, 10f);

            if (!spawned) {
                projectile.rotation =
                    projectile.velocity.ToRotation() + 
                    MathHelper.ToRadians(45f); 

                startVelocity = projectile.velocity*2f;
                projectile.velocity = Vector2.Zero;

                spawned = true;
            }
			projectile.rotation += 0.2f;
            if (TIMER < TIMER_MAX) {
                if (projectile.alpha > 0) {projectile.alpha -= alphaChange;}
            } else if (TIMER == TIMER_MAX) {
                projectile.velocity = startVelocity;
            } else {
                projectile.velocity = (projectile.velocity * (inertia - 1)) / inertia;
                projectile.alpha += alphaChange * 2;
            }

            if (TIMER == 50) {
                projectile.active = false;
            }
        }
    }
}