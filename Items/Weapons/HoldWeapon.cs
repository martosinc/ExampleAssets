using System;
using System.IO;
using Examples.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Examples.Items.Weapons
{
    public class HoldWeapon : ModItem {
        bool charging = false;
        bool charged = false;
        int hold = 0;
        int shootType;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hold Weapon");
            Tooltip.SetDefault("cock");
        }
        public override void SetDefaults() {
            item.damage = 50;
            item.knockBack = 6f;
            item.useStyle = 5;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 250;
            item.useTime = 250;
            item.width = 30;
            item.height = 30;
            item.ranged = true;
            item.autoReuse = false;
            item.value = 5000;
            item.rare = 2;
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }

        public override void HoldItem(Player player) {
            if (Main.mouseLeft) {
                charging = true;
                hold++;
                if (hold % 10 == 0) {
                    // Main.NewText((hold).ToString(), 175, 75, 255);
                    var angle = (hold*180/150);
                    Vector2 position = new Vector2(player.Center.X,player.Center.Y) + GetVector(angle)*50;
                    Projectile.NewProjectile(position,Vector2.Zero, ModContent.ProjectileType<HoldWeaponProjectile>(), 50, 0f,player.whoAmI);
                }
                if (hold > 110) {
                    hold = 110;
                    charged = true;
                }

                File.WriteAllText("/home/martos/.local/share/Terraria/ModLoader/Mod Sources/Examples/Items/Weapons/info.txt", hold.ToString());       
            } else if (charging) {
                for (int i = 0;i < Main.projectile.Length;i++) {
                    var proj = Main.projectile[i];
                    if (proj.type == ModContent.ProjectileType<HoldWeaponProjectile>()) {
                        proj.active = false;
                    }
                }
                Shot(player,charged,hold);
                charging = false;
                charged = false;
                hold = 0;
            }
        }

        public void Shot(Player player, bool charged, int damage) {
            var angle = -(float)Math.Atan2(Main.MouseWorld.Y-player.position.Y,Main.MouseWorld.X-player.position.X)+MathHelper.ToRadians(90);
            var pos = player.position;

            Vector2 direction = GetVector(angle);
            if (charged) {
                int numberProjectiles = 4 + Main.rand.Next(2); 
                for (int i = 0; i < numberProjectiles; i++) {
                    Vector2 perturbedSpeed = direction.RotatedByRandom(MathHelper.ToRadians(30));
                    Projectile.NewProjectile(pos.X,pos.Y, perturbedSpeed.X*5,perturbedSpeed.Y*5,ModContent.ProjectileType<HoldBullet>(),damage,0f,player.whoAmI);
                }   
            } else {
                Projectile.NewProjectile(pos.X,pos.Y, direction.X*5,direction.Y*5,ModContent.ProjectileType<HoldBullet>(),damage,0f,player.whoAmI);
            }
            // Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
        }

        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
    }
    public class HoldWeaponProjectile : ModProjectile {
        bool spawned = true;
        Vector2 direction;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hold Weapon");
        }
        public override void SetDefaults() {
            projectile.width = 8;
            projectile.height = 8;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.tileCollide = false;
        }


        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (spawned) {
                direction = new Vector2(projectile.position.X-player.position.X,projectile.position.Y-player.position.Y);
                spawned = false;
            }

            projectile.position = player.position + direction;
        }
    }

    public class HoldBullet : ModProjectile {
        bool spawned = true;
        Vector2 direction;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hold bullet");
        }
        public override void SetDefaults() {
            projectile.width = 8;
            projectile.height = 8;
            projectile.scale=0.125f;
            projectile.ranged = true;
            projectile.friendly = true;
        }
    }
}