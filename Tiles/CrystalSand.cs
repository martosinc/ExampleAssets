using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Examples.Dusts;

namespace Examples.Tiles
{
    public class CrystalSand : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true; // Is the tile solid
            Main.tileMergeDirt[Type] = true; // Will tile merge with dirt?
            Main.tileLighted[Type] = true; // ???
            Main.tileBlockLight[Type] = false; // Emits Light
            Main.tileSpelunker[Type] = false;

            drop = TileID.Dirt; 
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Crystal Sand");
            AddMapEntry(new Color(255, 153, 204), name);
            minPick = 10;
        }
        public override void AnimateIndividualTile (int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {
            if (Main.rand.Next(3500)==0) {
                Dust.NewDust(new Vector2(i*16,j*16),8,8,ModContent.DustType<CrystalSandDust>());
            }
        }
    }
}