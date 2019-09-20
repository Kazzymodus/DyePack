using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TCFBattles.NPCs.Devs
{
    class Cenx : ModNPC
    {

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 4000;
            npc.damage = 1;
            npc.defense = 50;
            npc.knockBackResist = 0f;
            npc.width = 18;
            npc.height = 40;
            npc.boss = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.lavaImmune = true;
            music = MusicID.LunarBoss;
            Main.npcFrameCount[npc.type] = 9;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cenx");
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Dust.NewDust(new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 4), npc.width, npc.height, 62);
            return true;
        }

        public override void AI()
        {

            // ai[0] = stages.
            // 0 = flying.
            // 1 = landing.
            // 2 = grounded
            // ai[1] = stage timer.
            //

            // Let there be light.

            Lighting.AddLight(npc.position, 1f, 1f, 1f);

            npc.TargetClosest();

            // Flying stage.

            if (npc.ai[0] == 0)
            {

                float horMoveSpeed = 0.07f;
                float vertMoveSpeed = 0.15f;
                float maxMoveSpeed = 10f;

                // Going up.

                if (npc.Center.Y > Main.player[npc.target].Center.Y - 200)
                {
                    if (npc.velocity.Y > 0)
                        npc.velocity.Y *= 0.8f;

                    npc.velocity.Y -= vertMoveSpeed;

                    if (npc.velocity.Y < -maxMoveSpeed)
                        npc.velocity.Y = -maxMoveSpeed;
                }

                // Going down.

                if (npc.Center.Y < Main.player[npc.target].Center.Y - 200)
                {
                    if (npc.velocity.Y < 0)
                        npc.velocity.Y *= 0.8f;

                    npc.velocity.Y += vertMoveSpeed;

                    if (npc.velocity.Y > maxMoveSpeed)
                        npc.velocity.Y = maxMoveSpeed;
                }

                // Going left.

                if (npc.Center.X > Main.player[npc.target].Center.X - 200)
                {
                    if (npc.velocity.X > 0)
                        npc.velocity.X *= 0.98f;

                    npc.velocity.X -= horMoveSpeed;

                    if (npc.velocity.X < -maxMoveSpeed)
                        npc.velocity.X = -maxMoveSpeed;
                }

                // Going right.

                if (npc.Center.X < Main.player[npc.target].Center.X - 200)
                {
                    if (npc.velocity.X < 0)
                        npc.velocity.X *= 0.98f;

                    npc.velocity.X += horMoveSpeed;

                    if (npc.velocity.X > maxMoveSpeed)
                        npc.velocity.X = maxMoveSpeed;
                }

                // When velocities are too low, set them to zero (prevents jittery movement).

                if (npc.velocity.X > -0.05f && npc.velocity.X < 0.05f)
                    npc.velocity.X = 0;
                if (npc.velocity.Y > -0.05f && npc.velocity.Y < 0.05f)
                    npc.velocity.Y = 0;

                // Timer for the stages.

                if (npc.ai[1] >= 600 && npc.ai[1] <= 900 && npc.ai[1] % 3 == 0)
                {
                    StarStrike();
                }               
                else if (npc.ai[1] >= 1200 && npc.ai[1] <= 1500 && npc.ai[1] % 20 == 0)
                {
                    StarStrike();
                }
                else if (npc.ai[1] == 1800)
                {
                    
                }
                else if (npc.ai[1] == 2400)
                {
                    npc.ai[0] = 1;
                    npc.ai[1] = -1;
                    npc.netUpdate = true;
                }
                else if (npc.ai[1] % 600 != 0 && npc.ai[1] % 600 != 5 && npc.ai[1] % 600 != 10 && (npc.ai[1] % 150 == 0 || npc.ai[1] % 150 == 5 || npc.ai[1] % 150 == 10))
                {
                    float burstSpeed = 20f;

                    Vector2 velocity = Main.player[npc.target].Center - npc.Center;
                    velocity.Normalize();
                    velocity *= burstSpeed;
                    velocity += npc.velocity;
                }
                
                if(Main.rand.Next(npc.life / 10) == 0)
                {
                    StarStrike();
                }

                npc.ai[1]++;
            }

            // Landing stage.

            else if (npc.ai[0] == 1)
            {

                float vertMoveSpeed = 0.8f;
                float vertMaxMoveSpeed = 3f;
                float horMinMoveSpeed = 0.2f;
                npc.noTileCollide = false;

                // Going down.

                if (npc.velocity.Y < 0)
                    npc.velocity.Y *= 0.8f;

                npc.velocity.Y += vertMoveSpeed;

                if (npc.velocity.Y > vertMaxMoveSpeed)
                    npc.velocity.Y = vertMaxMoveSpeed;


                // Brakes.

                npc.velocity.X *= 0.98f;

                if (npc.velocity.X < horMinMoveSpeed && npc.velocity.X > 0.0f)
                    npc.velocity.X = horMinMoveSpeed;
                if (npc.velocity.X > -horMinMoveSpeed && npc.velocity.X < 0.0f)
                    npc.velocity.X = -horMinMoveSpeed;

                if (npc.collideY == true)
                {
                    npc.velocity = Vector2.Zero;
                    npc.ai[0] = 2;
                }

                Vector2 targetDirection = npc.Center - Main.player[npc.target].Center;
                float targetDistance = (float)Math.Sqrt(targetDirection.X * targetDirection.X + targetDirection.Y * targetDirection.Y);

                if (targetDistance > 1000f)
                {
                    npc.ai[0] = 0;
                    npc.noTileCollide = true;
                }
            }

            // Grounded stage.

            else if (npc.ai[0] == 2)
            {
                Vector2 targetDirection = npc.Center - Main.player[npc.target].Center;
                float targetDistance = (float)Math.Sqrt(targetDirection.X * targetDirection.X + targetDirection.Y * targetDirection.Y);

                if (npc.ai[1] % 30 == 0 && npc.ai[1] < 180)
                {
                    StarMagnet(((int)npc.ai[1] / 30));
                }
                if (npc.ai[1] == 300)
                {
                    npc.ai[0] = 0;
                    npc.ai[1] = -1;
                    npc.noTileCollide = true;
                }

                if (targetDistance > 1000f)
                {
                    npc.ai[0] = 0;
                    npc.noTileCollide = true;
                }

                npc.ai[1]++;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            if (npc.frameCounter == 24 && npc.ai[0] == 0)
            {
                Main.PlaySound(2, npc.position, 32);
            }
            npc.frameCounter %= 24;
            if (npc.ai[0] == 0)
            {
                if (npc.velocity.Y < 2.5f)
                    npc.frame.Y = frameHeight * (((int)npc.frameCounter / 8) + 1);
                else
                    npc.frame.Y = frameHeight * (((int)npc.frameCounter / 8) + 5);
            }
            if (npc.ai[0] == 1)
                npc.frame.Y = frameHeight * 7;
            if (npc.ai[0] == 2)
                npc.frame.Y = 0;

        }

        public override void BossHeadSpriteEffects(ref SpriteEffects spriteEffects)
        {
            spriteEffects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        /*
        private void SparkStorm()
        {
            int offset = 100;
            // float xPosition = Main.player[npc.target].position.X + Main.rand.Next(-offset, offset + 1);
            // float yPosition = Main.player[npc.target].position.Y + Main.rand.Next(-offset, offset + 1);
            float xPosition = Main.rand.Next((int)Main.screenPosition.X, (int)Main.screenPosition.X + Main.screenWidth);
            float yPosition = Main.rand.Next((int)Main.screenPosition.Y, (int)Main.screenPosition.Y + Main.screenHeight);
            Main.PlaySound(27, new Vector2(xPosition, yPosition), 0);
            Projectile.NewProjectile(xPosition, yPosition, 0, 0, mod.ProjectileType("SparkStorm"), 1, 0, Main.myPlayer, -1, 0);
        }
        */

        private void StarStrike()
        {
            Main.PlaySound(2, npc.position, 9);
            float xPosition = Main.rand.Next((int)Main.screenPosition.X, (int)Main.screenPosition.X + Main.screenWidth);
            float yPosition = Main.screenPosition.Y - 200;
            Vector2 targetDirection = Main.player[npc.target].position - new Vector2(xPosition, yPosition);
            targetDirection.Normalize();
            float projectileSpeed = 15;
            float xVelocity = projectileSpeed * targetDirection.X;
            float yVelocity = projectileSpeed * targetDirection.Y;
            Projectile.NewProjectile(xPosition, yPosition, xVelocity, yVelocity, mod.ProjectileType("StarStrike"), 1, 1, Main.myPlayer, 0, npc.target);
        }
        
        private void StarBomb()
        {
            /*
            Vector2 targetDirection = Main.player[npc.target].position - new Vector2(xPosition, yPosition);
            targetDirection.Normalize();
            */
        }

        private void StarMagnet(int pass)
        {
            int angle = 72;
            int angleOffset = 36;
            int distance = 1000;
            float projectileSpeed = 20;
            for(int i = 0; i < 5; i++)
            {
                float xPosition = (float)Math.Sin((angle * i + pass * angleOffset) * (Math.PI / 180f)) * distance;
                float yPosition = (float)Math.Cos((angle * i + pass * angleOffset) * (Math.PI / 180f)) * distance;
                Vector2 targetDirection = npc.position - new Vector2(xPosition, yPosition);
                targetDirection.Normalize();
                float xVelocity = targetDirection.X * projectileSpeed;
                float yVelocity = targetDirection.Y * projectileSpeed;
                
                Projectile.NewProjectile(xPosition, yPosition, xVelocity, yVelocity, mod.ProjectileType("StarStrikeChild"), 1, 0, Main.myPlayer);

            }
        }
    }
}

