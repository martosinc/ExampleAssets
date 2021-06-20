using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Examples {
    public class ExamplePlayer : ModPlayer
	{
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) {
            bool flag = false;
            if (proj.minion) {

                for (int slot = 3; slot < 8 + player.extraAccessorySlots; slot++)
                {
                    if (player.armor[slot].type == mod.ItemType("ExampleAccessory"))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag) {
                Main.NewText("YAAAH ITS REWIND TIAME", 175, 75, 255);
            }
        }
    }
}


// Main.NewText((closest.position.X).ToString() + " " + (closest.position.Y).ToString(), 175, 75, 255);