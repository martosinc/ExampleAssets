using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace Examples.Dusts
{
	public class CrystalSandDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity.X = 0f;
            dust.velocity.Y = 3f;
			dust.noGravity = true;
			dust.noLight = true;
			dust.scale *= 1.5f;
		}

		public override bool Update(Dust dust) {
            dust.alpha += 1;
			dust.position -= dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.scale *= 0.99f;
			float light = 0.35f * dust.scale;
			Lighting.AddLight(dust.position, light, light, light);
			if (dust.scale < 0.9f) {
				dust.active = false;
            }
            return false;
        }
    }
}