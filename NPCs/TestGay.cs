using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;          
using Terraria.ModLoader;
using Terraria.Localization;
using Examples.Items.Armor.Helmets;
using System;
using System.IO;
using System.Threading.Tasks;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;

namespace Examples.NPCs
{
    public class TestGay : ModNPC  
    {
        bool purified = false;
        bool colored = false;
        Random rand = new Random();
        public override void SetStaticDefaults() 
        {
            DisplayName.AddTranslation(GameCulture.Russian, "зайоц");
        }

        public override void SetDefaults()   
        {
            npc.width = 30;               
            npc.height = 42;              
            npc.damage = 15;             
            npc.defense = 10;             
            npc.lifeMax = 700;            
            npc.HitSound = SoundID.NPCHit1 ;            
            npc.DeathSound = SoundID.NPCDeath2 ;          
            npc.value = 6000f;             
            npc.knockBackResist = 0f;      
            npc.friendly = true;
            Main.npcFrameCount[npc.type] = 4; 
        }
        public override void AI() {  
            if (!purified) {
                var projs = Main.projectile;

                for (int i = 0; i < projs.Length;i++) {
                    var proj = projs[i];

                    if (proj.type == ProjectileID.VilePowder && !purified && proj.active) {
                        var position = npc.position;
                        var target = proj.position;
                        var hb1 = new System.Drawing.Rectangle((int)position.X,(int)position.Y,npc.width,npc.height);
                        var hb2 = new System.Drawing.Rectangle((int)target.X,(int)target.Y,64,64);
                        bool collision = hb1.IntersectsWith(hb2);
                        if (collision) {
                            Main.NewText("i have been purified i ya зайоц", 175, 75, 255);
                            // npc.active = false;
                            // Dust.NewDust(npc.Center,(int)npc.width,(int)npc.height,16,0f,0f,255,new Color(100,100,100,100),20f);
                            // NPC.NewNPC((int)npc.Center.X,(int)npc.Center.Y,NPCID.Bunny);
                            purified = true;
                            npc.friendly = false;
                            npc.color = new Color(100,100,100,100);
                        }
                    }
                    if (proj.type == ProjectileID.PurificationPowder && proj.active) {
                        var position = npc.position;
                        var target = proj.position;
                        var hb1 = new System.Drawing.Rectangle((int)position.X,(int)position.Y,npc.width,npc.height);
                        var hb2 = new System.Drawing.Rectangle((int)target.X,(int)target.Y,64,64);
                        bool collision = hb1.IntersectsWith(hb2);
                        if (collision) {
                            npc.color = new Color(0,64,0,100);
                        }
                    }
                }
                // невраждебный
            } else {
                // враждебный ai
            }
        }
    }
}