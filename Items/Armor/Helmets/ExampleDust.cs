using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System.IO;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Items.Armor.Helmets
{
    public class ExampleDust : ModDust
    {
        public override void OnSpawn(Dust dust) {
            dust.velocity *= 0.4f;
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale *= 1f;
        }

        public override bool Update(Dust dust) {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.99f;
            float light = 0.35f * dust.scale;
            Lighting.AddLight(dust.position, light, light, light);
            if (dust.scale < 0.3f) {
                dust.active = false;
            }
            return false;
        }
    }
}