using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Examples.Items.Weapons
{
    public class SolarEdge : ModItem
    {
        int shootAmount = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solar Edge");
            Tooltip.SetDefault("Я хз что писать, sHiT");
        }

        public override void SetDefaults()
        {
            item.damage = 50;
            item.knockBack = 6f;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 25;
            item.useTime = 25;
            item.width = 30;
            item.height = 30;
            item.melee = true;
            item.autoReuse = true;
            item.value = 5000;
            item.rare = 2;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<SolarEdgeBeam>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (shootAmount==3) {
                float numberProjectiles = 3; // This defines how many projectiles to shot
                float rotation = MathHelper.ToRadians(45);
                position += Vector2.Normalize(new Vector2(speedX, speedY)) * 15f; //this defines the distance of the projectiles form the player when the projectile spawns
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // This defines the projectile roatation and speed. .4f == projectile speed
                    Projectile.NewProjectile(position.X - 25f, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                }
                shootAmount = 0;
            }
            shootAmount++;
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            int vampirism = 10; // сколько хп ворует
            target.life -= vampirism;
            player.statLife += vampirism;
            player.HealEffect(vampirism, true); // эффектос
        }

    }
    public class SolarEdgeBeam : ModProjectile
    {
        Random rand = new Random();
        public override void SetDefaults()
        {
            projectile.width = 2; //sprite is 2 pixels wide
            projectile.height = 20; //sprite is 20 pixels tall
            projectile.aiStyle = 0; //projectile moves in a straight line
            projectile.friendly  = true; //player projectile
            projectile.timeLeft = 1200; //lasts for 600 frames/ticks. Terraria runs at 60FPS, so it lasts 10 seconds.
            projectile.melee = true;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            int vampirism = 10; // сколько хп ворует
            target.life -= vampirism;
            player.statLife += vampirism;
            player.HealEffect(vampirism, true); // эффектос
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + 0.85f;
            Lighting.AddLight(projectile.Center, 0.4f, 0.52f, 1.2f);
        }
        public override void Kill(int timeLeft) {
            for (int i = 0; i < 4; i++) {
                int randAngle = rand.Next(0, 360);
                Vector2 direction = new Vector2((float)Math.Cos(randAngle), (float)Math.Sin(randAngle));
                direction.Normalize();
            }
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            // Main.PlaySound(SoundID.Item10, projectile.position); //это звук, если хочешь - подруби
        }
    }
}