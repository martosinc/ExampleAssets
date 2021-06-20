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
    public class DemonSoulItem : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Demon Soul");
            Tooltip.SetDefault("cock");
        }
        public override void SetDefaults() {
            item.damage = 50;
            item.knockBack = 6f;
            item.useStyle = 5;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 25;
            item.useTime = 25;
            item.width = 30;
            item.height = 30;
            item.summon = true;
            item.autoReuse = true;
            item.value = 5000;
            item.rare = 2;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<DemonSoulProj>();
        }
    }
}