using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Devs.Redigit
{
    class YoyoMaelstrom : ModProjectile
    {
        // Field region.

        float projectileSpeed = 10f; // Speed by which the projectile extends/retracts from Redigit.
        float projectileRangeMax = 350; // How far the projectile will extend from Redigit.

        float childProjectileSpeed = 10; // Speed by which the small projectiles fly out.
        int childProjectileDamage = 1; // Damage of the small projectiles.
        int childProjectileDelay = 5; // Delay between each small projectile.
        int childProjectileTimer = 0; // Timer that will enforce the delay.
        int childProjectileAngle = 30; // The angle between each projectile (12 projectiles per full rotation).
        int childProjectileMax = 36; // The maximum amount of projectiles that will be created.
        int childProjectileCounter = 0; // Counter that will keep track of the amount of projectiles created.

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.timeLeft = 900;
            projectile.hostile = true;
            projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoyo Maelstrom");
        }

        public override void AI()
        {
            // ai[0] = Redigit's npc[] entry.
            // ai[1] = Redigit's direction.

            // Find Redigit, or destroys the yoyo if it fails to.

            NPC parent = Main.npc[(int)projectile.ai[0]];
            if (Main.netMode != 1 && (parent == null || parent.type != mod.NPCType("Redigit") || !parent.active))
            {
                projectile.Kill();
            }

            Vector2 projectileToTargetDirection; // Holds the direction from where the projectile is to where its target is.
            float projectileToTargetDistance; // Holds the distance between the projectile and the target.
            Vector2 projectileToSourceDirection; // Holds the direction from where the projectile is to Redigit.
            float projectileToSourceDistance; // Holds the distance between the projectile and Redigit.

            if (childProjectileCounter < childProjectileMax)
            {

                // Get the direction and distance from the projectile to the target.

                projectileToTargetDirection = Main.player[parent.target].Center - projectile.Center;
                projectileToTargetDistance = (float)Math.Sqrt(projectileToTargetDirection.X * projectileToTargetDirection.X + projectileToTargetDirection.Y * projectileToTargetDirection.Y);

                // Get the direction and distance from the projectile to Regidit.

                projectileToSourceDirection = parent.Center - projectile.Center;
                projectileToSourceDistance = (float)Math.Sqrt(projectileToSourceDirection.X * projectileToSourceDirection.X + projectileToSourceDirection.Y * projectileToSourceDirection.Y);

                // Send the projectile towards the target, or back to Redigit if extended beyond its limit.

                projectile.velocity = projectileToTargetDirection * (projectileSpeed / projectileToTargetDistance);

                if (projectileToSourceDistance > projectileRangeMax)
                {
                    projectile.velocity += projectileToSourceDirection * (projectileSpeed / projectileToSourceDistance);
                }

                // Start firing.

                if (Main.netMode != 1 && childProjectileTimer == 0 && childProjectileCounter < childProjectileMax)
                {

                    // Calculates the direction of each individual lesser projectile, and makes them move clockwise or counterclockwise based on Redigit's direction.

                    float horSpeed = (float)Math.Sin(childProjectileCounter * childProjectileAngle * (Math.PI / 180)) * childProjectileSpeed * projectile.ai[1];
                    float vertSpeed = (float)Math.Cos(childProjectileCounter * childProjectileAngle * (Math.PI / 180)) * childProjectileSpeed * projectile.ai[1];
                    // horSpeed += projectile.velocity.X;
                    // vertSpeed += projectile.velocity.Y;

                    // Creates the lesser projectile.

                    Main.PlaySound(2, projectile.position, 1); // Plays a 'throw' sound.
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, horSpeed, vertSpeed, mod.ProjectileType("YoyoMaelstromChild"), childProjectileDamage, 0f, projectile.owner);

                    // Increments the projectile count so it shifts by childProjectileAngle degrees.

                    childProjectileCounter++;

                    // Puts the lesser projectile spawner on cooldown.

                    childProjectileTimer = childProjectileDelay;
                }
                else
                    childProjectileTimer--; // Decreases the cooldown.

            }

            // Let there be light.

            Lighting.AddLight(projectile.position, 1f, 0.5f, 1f);

            // If the maximum amount of projectiles has been reached, Redigit will pull the yoyo back.

            if (childProjectileCounter >= childProjectileMax)
            {

                // Get the direction and distance from the projectile to Redigit.

                projectileToSourceDirection = parent.Center - projectile.Center;
                projectileToSourceDistance = (float)Math.Sqrt(projectileToSourceDirection.X * projectileToSourceDirection.X + projectileToSourceDirection.Y * projectileToSourceDirection.Y);

                // If the projectile is close enough to Redigit, destroy it. Otherwise, move it towards him.

                projectile.velocity = projectileToSourceDirection;

                if (projectileToSourceDistance < 10f)
                    projectile.Kill();
                else
                    projectile.velocity = projectileToSourceDirection * (projectileSpeed / projectileToSourceDistance);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            // This draws the string between Redigit and the projectile. You do NOT want to mess with this if you don't know what you're doing.

            Vector2 linePosition = projectile.Center; // Where to draw the line segment. Starts at the projectile's position.
            Vector2 parentCenter = Main.npc[(int)projectile.ai[0]].Center; // Where Redigit is.
            Vector2 parentLineVector = parentCenter - linePosition; // Vector of the line segment and Redigit.
            float rotation = (float)Math.Atan2((double)parentLineVector.Y, (double)parentLineVector.X) - 1.57f; // Rotation of the line segment.
            Vector2 origin = new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f); // Origin of the line segment.

            bool isValid = true; // What we use to keep track of whether a line segment can or should be drawn.
            if (parentLineVector.X == 0 && parentLineVector.Y == 0) // If the projectile and Redigit have the same center (location), don't draw anything.
                isValid = false;
            if (float.IsNaN(linePosition.X) && float.IsNaN(linePosition.Y)) // If the x and y of linePosition are not numbers (through a division by zero for instance), don't draw anything.
                isValid = false;
            if (float.IsNaN(parentLineVector.X) && float.IsNaN(parentLineVector.Y)) // Same as above.
                isValid = false;

            while (isValid) // Let's draw!
            {
                float distance = (float)Math.Sqrt(parentLineVector.X * parentLineVector.X + parentLineVector.Y * parentLineVector.Y); // Gets the distance between Redigit and the projectile.
                if (distance < Main.fishingLineTexture.Height) // If that distance is smaller that the length of a segment, stop drawing.
                    isValid = false;
                else
                {
                    Vector2 segmentDirection = Vector2.Normalize(parentLineVector); // Get the direction of the segment.
                    linePosition += segmentDirection * Main.fishingLineTexture.Height; // Move the line segment to its next position.
                    parentLineVector = parentCenter - linePosition; // Update the vector to its new value.
                    Main.spriteBatch.Draw(Main.fishingLineTexture, linePosition - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f); // Actually draws the line segment (finally!).
                }
            }

            return true;
        }
    }
}