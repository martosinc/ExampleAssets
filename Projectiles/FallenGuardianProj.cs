using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Projectiles   
{
    public class FallenGuardianProj : ModProjectile {
        bool spawned = true;
        float angle = 0;
        
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("FallenGuardianProj");
        }
        public override void SetDefaults() {
            projectile.width = 16;
            projectile.height = 16;
            projectile.ranged = true;
            projectile.friendly = true;
        }


        public override void AI() {
            Player player = Main.player[projectile.owner];
            if(spawned) {
                projectile.velocity = Vector2.Zero;

                spawned = false;
            }    

            Vector2 PlayerPos = player.Center;

            projectile.position = new Vector2(PlayerPos.X, PlayerPos.Y) + GetVector(angle)*50;

            angle+=0.1f;
        }   

        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
    }

    public class FallenGuardianProjItem : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("FallenGuardianProj");
        }
        public override void SetDefaults() {
            item.damage = 50;
            item.ranged = true;
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.consumable = true;   
            item.knockBack = 2.0f;
            item.value = Item.sellPrice(0, 0, 0, 80);
            item.rare = 5;
            item.shoot = mod.ProjectileType("FallenGuardianProj");
            item.shootSpeed = 5.0f;             
            item.ammo = AmmoID.Bullet;
        }
    }
}