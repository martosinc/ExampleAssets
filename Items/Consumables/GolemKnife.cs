using System;
using Examples.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Examples.Items.Consumables
{
    public class GolemKnife : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Golem Knife");
            Tooltip.SetDefault("cock");
        }
        public override void SetDefaults() {
            item.damage = 100;
            item.knockBack = 6f;
            item.useStyle = 1;  
            item.useAnimation = 17;
            item.useTime = 17;
            item.width = 22;
            item.height = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = true;
            item.value = 5000;
            item.rare = 2;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<GolemKnifeProjectile>();

            item.noUseGraphic = true;
            item.thrown = true;

        }
    }
    public class GolemKnifeProjectile : ModProjectile {		
        NPC target;
        Vector2 offset;
        bool STICKED = false;
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Plantera Knife");
		}

		public override void SetDefaults() {
			projectile.width = 22;
			projectile.height = 44;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
            projectile.tileCollide = true;
		}
        public override bool OnTileCollide(Vector2 oldVelocity) {
            Explode(1);
            return false;
        }
        public override void OnHitNPC(NPC target2, int damage, float knockback, bool crit) {
            Stick(projectile.position);
            target = target2;

            offset = projectile.position - target.position;

            projectile.hostile = false;
            projectile.friendly = false;
        }
        private const int MAX_TICKS = 45;
        private int TICKS = 0;
        private int TIMER = 0;
        public override void AI() {
            TICKS++;
            if (TICKS == 0) {
                projectile.velocity *= 2;
            }

            if (TICKS >= MAX_TICKS) {
				const float velXmult = 0.98f;
				const float velYmult = 0.35f;
				TICKS = MAX_TICKS;
				projectile.velocity.X *= velXmult;
				projectile.velocity.Y += velYmult*2;
			}
            if (!STICKED) {
                projectile.rotation =
                    projectile.velocity.ToRotation() +
                    MathHelper.ToRadians(90f); 
            }
            
            if (STICKED) {
                TIMER++;
                Stick(target.position+offset);
                if (TIMER == 40) {
                    Explode();
                }
            }
        }
        private void Stick(Vector2 target) {
            projectile.velocity = Vector2.Zero;

            STICKED = true;

            projectile.position = target;
        }
        private void Explode(float mult = 15f) {
            projectile.active = false;

            Vector2 explosionPos = projectile.Center + projectile.velocity * mult;

            Projectile.NewProjectile(explosionPos.X,explosionPos.Y,0f,0f,ModContent.ProjectileType<GolemKnifeExplosion>(),75,0f,projectile.owner);
        }
    }

    public class GolemKnifeExplosion : ModProjectile {
		public override void SetDefaults() {
            Main.projFrames[projectile.type] = 5;

            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.damage = 75;

			projectile.width = 50;
			projectile.height = 50;
            projectile.scale *= 1.5f;
			projectile.penetrate = -1;

            projectile.timeLeft = 25;
            projectile.tileCollide = false;
		}

        public override void AI() {
            // projectile.scale *= 1.05f;
            // projectile.alp

            AnimateProjectile();
        }
        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if(projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 5;
                projectile.frameCounter = 0;
            }
        }
    }
}