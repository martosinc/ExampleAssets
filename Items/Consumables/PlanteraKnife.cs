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
    public class PlanteraKnife : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Plantera Knife");
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
            item.shoot = ModContent.ProjectileType<PlanteraKnifeProjectile>();

            item.noUseGraphic = true;
            item.thrown = true;

        }
    }
    public class PlanteraKnifeProjectile : ModProjectile {		
        NPC lastTarget;
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("Plantera Knife");
		}

		public override void SetDefaults() {
			projectile.width = 22;
			projectile.height = 36;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 2;
            projectile.tileCollide = true;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            lastTarget = target;
        }
        public override void AI() {
			Vector2 move = Vector2.Zero;
			float distance = 400f;
			bool target = false;
			for (int k = 0; k < 200; k++) {
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5) {
					Vector2 newMove = Main.npc[k].Center - projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    bool alreadyTargeted = Main.npc[k] == lastTarget;
					if (distanceTo < distance && !(alreadyTargeted)) {
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}
			if (target) {
				AdjustMagnitude(ref move);
				projectile.velocity = (10 * projectile.velocity + move*1.75f) / 11f;
                AdjustMagnitude(ref projectile.velocity);
            }

            projectile.rotation += 0.3f;
        }

		private void AdjustMagnitude(ref Vector2 vector) {
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 5f) {
				vector *= 10f / magnitude;
			}
		}
    }
}