using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    class Mortar : ModProjectile
    {
        private const int BufferSize = 15; // This is now 15 instead of 10, to account for the lagging (see below).
        private const int DustLag = 5; // The amount of frames we're not drawing, as to create the trail 'delay'.
        private const float TrailDensity = 2;
        private Vector2[] positionBuffer = new Vector2[BufferSize];
        private int bufferTail = 0;

        private const int ExplosionRadius = 100; // Not really a radius since it's a box but shut up.

        private float Gravity
        {
            get { return projectile.ai[0]; }
        }

        private bool HasExploded
        {
            get { return projectile.ai[1] == 1; }
            set { projectile.ai[1] = value ? 1 : 0; }
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.minion = true;
            projectile.friendly = true;
            projectile.timeLeft = 300;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            positionBuffer[bufferTail] = projectile.position; // We're now storing the center of the projectile, as part of getting rid of the jitter.
            bufferTail++;
            if (bufferTail >= BufferSize)
            {
                bufferTail = 0;
            }

            int lastDust = bufferTail + BufferSize - DustLag;

            int previousIndex = bufferTail - 1;
            if (previousIndex < 0)
            {
                previousIndex += BufferSize;
            }
            Vector2 oldPosition = positionBuffer[previousIndex];

            for (int i = bufferTail; i < lastDust; i++)
            {
                Vector2 newPosition = positionBuffer[i % BufferSize];
                Vector2 spawnDomain = Vector2.Normalize(oldPosition - newPosition) * 5;

                if (newPosition != Vector2.Zero && oldPosition != Vector2.Zero)
                {
                    for (int j = 0; j < TrailDensity; j++)
                    {
                        float randomX = Main.rand.NextFloat(-spawnDomain.X, spawnDomain.X);
                        float randomY = Main.rand.NextFloat(-spawnDomain.Y, spawnDomain.Y);

                        Dust dust = Dust.NewDustPerfect(newPosition, 75, default(Vector2), projectile.alpha, default(Color), 1f);

                        dust.position.X += randomX;
                        dust.position.Y += randomY;
                        
                        // This here we already had.

                        // dust.alpha = projectile.alpha;
                        // dust.velocity *= 0f;
                        dust.noGravity = true;
                        dust.fadeIn *= 1.8f;
                        dust.scale = 1f;
                    }
                }

                oldPosition = newPosition; // Don't forget this!
            }

            if (projectile.timeLeft <= 3)
            {
                if (!HasExploded)
                {
                    Explode();
                }

                return;
            }
            else
            {
                projectile.velocity.Y += Gravity;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!HasExploded)
            {
                Explode();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!HasExploded)
            {
                Explode();
            }

            return false;
        }

        private void Explode()
        {
            projectile.timeLeft = 3;
            projectile.alpha = 255;
            projectile.velocity = Vector2.Zero;

            for (int i = 0; i < 10; i++)
            {
                float xVelocity = Main.rand.Next(-5, 6);
                float yVelocity = Main.rand.Next(-5, 6);
                int dustId = Dust.NewDust(projectile.position, projectile.width, projectile.height, 46, xVelocity, yVelocity, 0, default(Color), 1.5f);
            }

            Vector2 explosionSize = new Vector2(ExplosionRadius);
            projectile.position -= explosionSize * 0.5f;
            projectile.Size = explosionSize;

            HasExploded = true;
        }
    }
}
