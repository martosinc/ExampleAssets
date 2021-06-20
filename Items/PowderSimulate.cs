using System;
using Examples.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Examples.Items
{
    public class PowderSimulate : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Powder Simulate");
            Tooltip.SetDefault("cock");
        }
        public override void SetDefaults() {
            item.damage = 50;
            item.knockBack = 6f;
            item.useStyle = 4;  
            item.useAnimation = 25;
            item.useTime = 25;
            item.width = 30;
            item.height = 30;
            item.magic = true;
            item.autoReuse = true;
            item.value = 5000;
            item.rare = 2;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<TimeKeeperAttack>();
            item.noUseGraphic = false;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            damage = 30;
            knockBack = 0f;
            position = Main.MouseWorld;
            speedX=0f;
            speedY=0f;
            return true;
        }
    }
}