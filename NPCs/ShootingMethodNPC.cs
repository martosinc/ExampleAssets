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
    public class ShootingMethodNPC : ModNPC  
    {
        bool purified = false;
        bool colored = false;
        Random rand = new Random();
        public override void SetStaticDefaults() 
        {
            DisplayName.SetDefault("Number One Victory Royale");

            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()   
        {
            npc.width = 18;
            npc.height = 40;               
            npc.lifeMax = 700;             
            npc.friendly = true;
            npc.noGravity = true;
        }
        Player player;
        bool shot = false;
        public override void AI() {
            Vector2 target = GetTarget(ref player);

            if (Vector2.Distance(target,npc.position) > 500f && !shot) {
                Vector2 position = predictedPosition(target, npc.position, player.velocity, 20);

                float angle = -(float)Math.Atan2(position.Y-npc.position.Y,position.X-npc.position.X)+MathHelper.ToRadians(90);
                Vector2 direction = GetVector(angle);
                direction.Normalize();
                direction *= 20;

                // var id = Projectile.NewProjectile(position.X,position.Y,0f,0f,ProjectileID.PinkLaser,30,0f);
                // Main.NewText(id.ToString(), 175, 75, 255);
                Projectile.NewProjectile(npc.position.X,npc.position.Y,direction.X,direction.Y,ProjectileID.PinkLaser,30,0f);
                // Main.NewText(id.ToString(), 175, 75, 255);

                shot = !shot;
                Main.NewText(npc.position.ToString() + " " + position.ToString(), 175, 75, 255);
            }
        }

        private Vector2 predictedPosition(Vector2 targetPosition, Vector2 shooterPosition, Vector2 targetVelocity, float projectileSpeed)
        {
            Vector2 displacement = targetPosition - shooterPosition;
            float targetMoveAngle = Angle(-displacement, targetVelocity) * MathHelper.ToRadians(1);
            if (GetMagnitude(targetVelocity) == 0 || GetMagnitude(targetVelocity) > projectileSpeed && Math.Sin(targetMoveAngle) / projectileSpeed > Math.Cos(targetMoveAngle) / GetMagnitude(targetVelocity))
            {
                return targetPosition;
            }
            //also Sine Formula
            float shootAngle = (float)Math.Asin(Math.Sin(targetMoveAngle) * GetMagnitude(targetVelocity) / projectileSpeed);
            return targetPosition + targetVelocity * GetMagnitude(displacement) / (float)Math.Sin(Math.PI - targetMoveAngle - shootAngle) * (float)Math.Sin(shootAngle) / GetMagnitude(targetVelocity);
        }
        private float Angle(Vector2 a, Vector2 b)
        {
            return (float)Math.Atan2(b.Y - a.Y,b.X - a.X);
        }
        private float GetMagnitude(Vector2 position) {
            var x = position.X;
            var y = position.Y;
            return (float)Math.Sqrt(x*x+y*y);
        }
        private Vector2 GetTarget(ref Player closest) {
            bool foundTarget = false;
            float closestDist = 50000f;
            Vector2 targetCenter = new Vector2(0,0);
            Player[] Players = Main.player;
            for (int i = 0;i<Players.Length;i++) {
                Player player = Players[i];
                float between = Vector2.Distance(player.Center, npc.Center);
                if (between < closestDist && player.active) {
                    closestDist = between;
                    closest = player;
                    targetCenter = player.Center;
                    foundTarget = true;
                }
            }
            return targetCenter;
        }
        public Vector2 GetVector(float angle) {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }
    }
}