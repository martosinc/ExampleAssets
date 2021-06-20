using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System.IO;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Items.Ammo
{
    public class ExampleBulletItem : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Example Bullet");
        }
        public override void SetDefaults() {
            item.damage = 10;
            item.ranged = true;
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.consumable = true;   
            item.knockBack = 2.0f;
            item.value = Item.sellPrice(0, 0, 0, 80);
            item.rare = 5;
            item.shoot = mod.ProjectileType("ExampleBullet");
            item.shootSpeed = 5.0f;             
            item.ammo = AmmoID.Bullet;
        }
    }
}