using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TCFBattles.Projectiles.Devs.Redigit
{
    public class YoyoSling : ModProjectile
    {
        // Field region.

        float orbitSpeed = 7f; // The speed of the projectile while it orbits Redigit.
        float launchSpeed = 30f; // The speed of the projectile after it's launched.
        float orbitRadius = 0f; // The current radius of the orbit trajectory.
        float maxRadius = 250f; // The maximum radius of the orbit trajectory.
        float projectileDegrees = 0f; // The amount of degrees the projectile has travelled.
        int rotations = 3; // The amount of full rotations the projectile will make before it's launched.

        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.timeLeft = 720;
            projectile.hostile = true;
            projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yoyo Sling");
        }

        public override void AI()
        {
            // ai[0] = Redigit's npc[] entry.
            // ai[1] = Redigit's direction.       

            // First the projectile will make a few full rotations.

            if (rotations >= 0)
            {

                // Find Redigit, or destroys the yoyo if it fails to.

                NPC parent = Main.npc[(int)projectile.ai[0]];
                if (Main.netMode != 1 && (parent == null || parent.type != mod.NPCType("Redigit") || !parent.active))
                {
                    projectile.Kill();
                }

                // Convert degrees to radians (no, I don't like it either).

                float angle = projectileDegrees * (float)(Math.PI / 180);

                // Orbitus! (With thanks to Sin Costan)

                projectile.position.X = parent.Center.X - (float)Math.Cos(angle * projectile.ai[1]) * (orbitRadius * projectile.ai[1]) - projectile.width / 2;
                projectile.position.Y = parent.Center.Y - (float)Math.Sin(angle * projectile.ai[1]) * (orbitRadius * projectile.ai[1]) - projectile.height / 2;

                // 0.1
                // 1.0
                // 0.-1
                // -1.0

                // Makes the projectile spiral outwards.

                if (orbitRadius < maxRadius)
                {
                    orbitRadius += 2;
                    if (orbitRadius > maxRadius)
                        orbitRadius = maxRadius;
                }

                // Get the direction and distance from the projectile and the current direction of the projectile based on its current trajectory.
                // These two vectors are normalised so they can be compared to eachother.

                Vector2 targetLocation = Main.player[parent.target].Center;
                Vector2 projectileDirection = new Vector2((float)Math.Sin(angle * projectile.ai[1]), (float)Math.Cos(angle * projectile.ai[1]));
                projectileDirection.Normalize();
                Vector2 targetDirection = targetLocation - projectile.Center;
                targetDirection.Normalize();                

                // If the projectile has reached the correct point in its trajectory, release it.

                if (rotations == 0 && (targetDirection.X - projectileDirection.X > -0.05f && targetDirection.X - projectileDirection.X < 0.05f) && (targetDirection.Y - projectileDirection.Y > -0.05f && targetDirection.Y - projectileDirection.Y < 0.05f))
                {
                    if (projectile.timeLeft > 120)
                        projectile.timeLeft = 120;

                    //projectile.tileCollide = true;
                    projectile.penetrate = 1;

                    projectile.velocity = projectileDirection * launchSpeed;
                    rotations--;

                    Main.PlaySound(2, projectile.position, 1); // Plays a 'swoosh' sound.
                }

                // Move the projectile along its orbit.

                projectileDegrees += orbitSpeed;

                // Modulate the degrees to count the amount of rotations.

                if (projectileDegrees > 360)
                {
                    projectileDegrees %= 360;
                    if (rotations > 0)
                    {
                        rotations--;
                        Main.PlaySound(2, projectile.position, 19); // Plays a 'launch' sound.
                    }
                }
            }

            // Come on baby light my fire.

            Lighting.AddLight(projectile.position, .7f, .2f, .7f);

        }

        public override void Kill(int timeLeft)
        {
            
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
            if (float.IsNaN(linePosition.X) && float.IsNaN(linePosition.Y)) // If the x and y of linePosition are not numbers (through a division by zero), don't draw anything.
                isValid = false;
            if (float.IsNaN(parentLineVector.X) && float.IsNaN(parentLineVector.Y)) // Same as above.
                isValid = false;

            while (isValid) // Let's draw!
            {
                float distance = (float)Math.Sqrt(parentLineVector.X * parentLineVector.X + parentLineVector.Y * parentLineVector.Y); // Gets the distance between Redigit and the projectile.
                if (distance < Main.fishingLineTexture.Height || distance > 300f) // If that distance is smaller that the length of a segment, stop drawing.
                    isValid = false;
                else
                {
                    Main.spriteBatch.Draw(Main.fishingLineTexture, linePosition - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f); // Actually draws the line segment (finally!).
                    Vector2 segmentDirection = Vector2.Normalize(parentLineVector); // Get the direction of the segment.
                    linePosition += segmentDirection * Main.fishingLineTexture.Height; // Move the line segment to its next position.
                    parentLineVector = parentCenter - linePosition; // Update the vector to its new value.
                }
            }

            return true;
        }
    }
}