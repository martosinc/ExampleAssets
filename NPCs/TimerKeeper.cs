using Microsoft.Xna.Framework;
using Examples.Projectiles;
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
  public class TimeKeeper : ModNPC  
  {
    int frameHeight = 102;
    int stage = 1;
    int step = 1;
    int steps = 0;
    int dashes = 0;
    float rot = 1f;
    float rotation = 0;
    int timer = 0;
    bool spawned = true;
    bool spawnedMinions = true;
    bool rotDash = false;
    Vector2 targetCenter;
    Vector2 direction;
    Vector2 move;
    float offset;
    Player closest;
    float angle;
    public override void SetStaticDefaults() 
    {
        DisplayName.SetDefault("Time Keeper");
        Main.npcFrameCount[npc.type] = 2;
    }

    public override void SetDefaults()   
    {
        npc.width = 100;               
        npc.height = frameHeight;              
        npc.damage = 15;             
        npc.defense = 10;             
        npc.lifeMax = 700;            
        npc.HitSound = SoundID.NPCHit1 ;            
        npc.DeathSound = SoundID.NPCDeath2 ;          
        npc.value = 6000f;             
        npc.knockBackResist = 0f;      
        npc.boss = true;
        npc.frameCounter = 0;
        npc.noTileCollide = true;
        npc.noGravity = true;
    }
    public override void AI() {
        float speed = 15f;
        float inertia = 5f;
        switch (stage) {
            case 1:
                switch (step) {
                    case 1:
                        npc.rotation += 0.1f;
                        rotation += 0.15f;
                        if (rotation > 2*3.14f) {
                            rotation = 0f;
                            npc.rotation = 0f;
                            step += 1;
                        }
                        break;
                    case 2:
                        bool foundTarget = false;
                        float closestDist = 50000f;
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
                        if (closestDist==50000f) {
                            npc.active = false;
                            return;
                        }
                        if (foundTarget) {
                            step += 1;
                        }
                        break;
                    case 3:
                        // Main.NewText((closest.position.X).ToString() + " " + (closest.position.Y).ToString(), 175, 75, 255);
                        angle = -(float)Math.Atan2(targetCenter.Y-npc.position.Y,targetCenter.X-npc.position.X)+MathHelper.ToRadians(90);
                        direction = GetVector(angle);
                        direction.Normalize();
                        direction *= speed;
                        npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
                        if (Vector2.Distance(targetCenter, npc.position) < 50f) {
                            step += 1;
                            // npc.velocity = direction;
                        }
                        break;
                    case 4:
                        targetCenter = closest.Center;
                        angle = -(float)Math.Atan2(targetCenter.Y-npc.position.Y,targetCenter.X-npc.position.X)+MathHelper.ToRadians(90);

                        direction = GetVector(angle);
                        direction.Normalize();
                        direction *= speed;

                        npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
                        if (Vector2.Distance(targetCenter, npc.Center) < 300f) {
                            step += 1;
                            targetCenter += direction*22.5f;
                        }
                        break;
                    case 5:
                        angle = -(float)Math.Atan2(targetCenter.Y-npc.position.Y,targetCenter.X-npc.position.X)+MathHelper.ToRadians(90);

                        direction = GetVector(angle);
                        direction.Normalize();
                        direction *= speed*2;

                        npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;

                        if (Vector2.Distance(targetCenter, npc.position) < 25f) {
                            step += 1;
                            // npc.velocity /= 5;
                        }
                        break;
                    case 6:
                        offset = 600f;
                        targetCenter = closest.position;
                        if (Vector2.Distance(new Vector2(targetCenter.X + 50f, targetCenter.Y), npc.position) > Vector2.Distance(new Vector2(targetCenter.X-50f, targetCenter.Y), npc.position)) {
                            offset *= -1;
                        }
                        angle = -(float)Math.Atan2(targetCenter.Y-npc.position.Y,targetCenter.X-npc.position.X+offset-50f)+MathHelper.ToRadians(90);

                        direction = GetVector(angle);
                        direction.Normalize();
                        direction *= speed;

                        npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;

                        if (Vector2.Distance(targetCenter, npc.position) < 700f) {
                            dashes += 1;
                            if (dashes == 4) {
                                step = 1;
                                stage = 2;
                                dashes = 0;
                                steps = 0;
                            } else {
                                step = 2;
                            }
                        }
                        break;    
                    // case 7:    
                    //     targetCenter = closest.position;       
                }
                break;
            case 2:
                if (spawned && npc.life < npc.lifeMax / 2) {
                    spawnedMinions = false;
                    spawned = false;
                }
                if (!spawnedMinions && npc.life < npc.lifeMax / 2) {
                    for (int i = 0;i < 4;i++) {
                        var minion = NPC.NewNPC((int)npc.Center.X,(int)npc.Center.Y,mod.NPCType("TimeKeeperMinion"));
                        float angle = MathHelper.ToRadians(i*90);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))*15  ;
                        Main.npc[minion].velocity = direction;
                    }
                    spawnedMinions = true;
                } else {
                    if (steps % 30 == 0 && Main.rand.Next(2)==0) {
                        targetCenter = closest.position;
                        npc.velocity/=1.5f;
                        move = closest.velocity;
                        targetCenter += move*(15f*Vector2.Distance(targetCenter,npc.position)/750);

                        angle = -(float)Math.Atan2(targetCenter.Y-npc.position.Y-15f,targetCenter.X-npc.position.X-25f)+MathHelper.ToRadians(90);
                        direction = GetVector(angle);
                        direction.Normalize();
                        direction *= 100;

                        Projectile.NewProjectile(npc.Center.X,npc.Center.Y,direction.X,direction.Y, ProjectileID.PinkLaser,50,5f);
                        if (steps == 210) {
                            stage=1;
                            npc.velocity = Vector2.Zero;
                        }
                        steps++;
                    } else if (Main.expertMode) {
                        Projectile.NewProjectile(targetCenter.X,targetCenter.Y,0f,0f,ModContent.ProjectileType<TimeKeeperAttack>(),30,0f);
                        Projectile.NewProjectile(targetCenter.X,targetCenter.Y,0f,0f,ModContent.ProjectileType<TimeKeeperAttack>(),30,0f);
                        steps=30;
                        stage=1;
                        npc.velocity = Vector2.Zero;
                    }
                }
                break;
        }
        AnimateProjectile();
    }
    public Vector2 GetVector(float angle) {
        return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
    }

    public void AnimateProjectile()
        {
            npc.frameCounter++;
            if(npc.frameCounter >= 8)
            {
                npc.frame.Y+=frameHeight;
                npc.frame.Y %= 204;
                npc.frameCounter = 0;
            }
        }
  }
  public class TimeKeeperMinion : ModNPC {
    
    int frameHeight = 16;
    int counter = 0;
    float angle;
    bool slowingDown = false;
    Vector2 direction;
    Player closest;
    Vector2 targetCenter;
    bool spawned;
    float offset;
    bool fadeIn = true;
    int count = 0;

    public override void SetStaticDefaults() 
    {
        DisplayName.SetDefault("Ass Keeper");
    }

    public override void SetDefaults()   
    {
        npc.width = frameHeight;               
        npc.height = frameHeight; 
        npc.damage = 15;             
        npc.defense = 10;             
        npc.lifeMax = 700;            
        npc.HitSound = SoundID.NPCHit1 ;            
        npc.DeathSound = SoundID.NPCDeath2 ;          
        npc.value = 6000f;             
        npc.knockBackResist = 0f;     
        npc.frameCounter = 0;
        npc.noTileCollide = true;
        npc.noGravity = true;
    }
    public override void AI() {
        if (count < 30) {
            count++;
        } else {
        if (spawned) {
            offset = 20*3.14f/180;
        }
        float speed = 5;
        bool foundTarget = false;
        float closestDist = 10000f;
        Player[] Players = Main.player;
        for (int i = 0;i<Players.Length;i++) {
            Player player = Players[i];
            float between = Vector2.Distance(player.Center, npc.Center);
            if (between < closestDist) {
                closestDist = between;
                closest = player;
                targetCenter = player.Center;
                foundTarget = true;
            }
        }
        if (foundTarget) {
            if (count % 180==0){
                angle = -(float)Math.Atan2(targetCenter.Y-npc.position.Y-15f,targetCenter.X-npc.position.X-25f)+MathHelper.ToRadians(90);
                direction = GetVector(angle);
                direction.Normalize();
                direction *= 40;
                if (Main.rand.Next(4)==0) {
                    Projectile.NewProjectile(npc.Center.X,npc.Center.Y,direction.X,direction.Y, ProjectileID.PinkLaser,50,5f);
                }
            }

            angle = -(float)Math.Atan2(targetCenter.Y-npc.position.Y,targetCenter.X-npc.position.X)+MathHelper.ToRadians(90);

            direction = GetVector(angle);
            direction.Normalize();
            if (counter == 120 && !slowingDown ) {
                if (Main.rand.Next(0,3) == 1) {
                    slowingDown = true;
                }

                counter = 0;
            } if (slowingDown && counter == 60) {
                slowingDown = false;        
            }
            if (slowingDown) {
                speed /= 2;
            }
            direction *= speed;

            if (fadeIn) {
                direction *= -1;
                direction += RotateRadians(direction,90*3.14f/180);
            } else {
                direction += RotateRadians(direction*-1,90*3.14f/180);
            }
            if (Vector2.Distance(targetCenter,npc.position) > 400f) {
                fadeIn = false;
            } if (!fadeIn && Vector2.Distance(targetCenter,npc.position) < 250f) {
                fadeIn = true;
            }
        }
        npc.velocity = direction;
        counter++;
        count++;
        }
    }

    public Vector2 RotateRadians(Vector2 v, double radians)
    {
        var ca = (float)Math.Cos(radians);
        var sa = (float)Math.Sin(radians);
        return new Vector2(ca*v.X - sa*v.Y, sa*v.X + ca*v.Y);
    }
    public Vector2 GetVector(float angle) {
        return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
    }
    public void AnimateProjectile()
        {
            npc.frameCounter++;
            if(npc.frameCounter >= 8)
            {
                npc.frame.Y+=frameHeight;
                npc.frame.Y %= 204;
                npc.frameCounter = 0;
            }
        }
  }
}
