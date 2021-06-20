using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System.IO;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Items.Ammo
{
    public class BrainWormTestItem : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Brain Worm Test");
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
            item.shoot = mod.ProjectileType("BrainWormTest");
            item.shootSpeed = 5.0f;             
            item.ammo = AmmoID.Bullet;
        }
    }
}