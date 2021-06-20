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
    public class TimeKeeperStaff : ModItem {
        Vector2 targetCenter;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Time Keeper Staff");
            Tooltip.SetDefault("cock");
        }
        public override void SetDefaults() {
            item.damage = 50;
            item.knockBack = 6f;
            item.useStyle = 1;  
            item.useAnimation = 15;
            item.useTime = 15;
            item.width = 50;
            item.height = 50;
            item.magic = true;
            item.autoReuse = true;
            item.value = 5000;
            item.rare = 2;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<TimeKeeperStaffAttack>();
            item.noUseGraphic = false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float distanceFromTarget = 700f;
            Vector2 targetCenter = Main.MouseWorld;
            bool foundTarget = false;

            for (int i = 0; i < Main.maxNPCs; i++) {
                var npc = Main.npc[i];
                if (npc.CanBeChasedBy()) {
                    float between = Vector2.Distance(npc.Center, Main.MouseWorld);
                    bool closest = Vector2.Distance(Main.MouseWorld, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    if (((closest && inRange) || !foundTarget)) {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }

            damage = 100;
            knockBack = 0f;
            speedX=0f;
            speedY=0f;
            if (foundTarget && distanceFromTarget < 300f) {
                position = targetCenter;
                return true;
            } else {
                return false;
            }
        }
    }

    public class TimeKeeperStaffAttack : ModProjectile {
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
            projectile.friendly = true;
            projectile.tileCollide = false;
        }
        public override bool PreAI() {
            float distanceFromTarget = 100f;
            Vector2 targetCenter = projectile.position;
            bool foundTarget = false;

            for (int i = 0; i < Main.maxNPCs; i++) {
                var npc = Main.npc[i];
                if (npc.CanBeChasedBy()) {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
                    bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    if (((closest && inRange) || !foundTarget)) {
                        distanceFromTarget = between;
                        target = npc.Center;
                        foundTarget = true;
                    }
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