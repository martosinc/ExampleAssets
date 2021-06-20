using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System.IO;
using System;
using Microsoft.Xna.Framework;

namespace Examples.Items.Armor.Helmets
{
    [AutoloadEquip(EquipType.Head)]
    public class ExampleHelmet : ModItem 
    {

        Vector2 PlayerPos;
        Random rand = new Random();

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Example Helmet");
        }

        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 1;
        }

        public virtual void PlaySound() {}

        public override void UpdateEquip(Player player) {
			if (PlayerPos != null) {
				if (PlayerPos != player.position) {
					int randAngle = rand.Next(0, 360);  
                    Vector2 direction = new Vector2((float)Math.Cos(randAngle), (float)Math.Sin(randAngle));
                    direction.Normalize();
                    direction *= 2;
					Dust.NewDust(new Vector2(player.Center.X - 4f, player.Center.Y+22.5f), 5, 5, mod.DustType("ExampleDust"), direction.X, direction.Y);
                    PlayerPos = player.position;
				}
			} else {
				PlayerPos = player.position;
			}
		}
    }
}