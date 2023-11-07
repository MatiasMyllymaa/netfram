using Raylib_CsLo;
using System.Numerics;

namespace Spaceinvaders
{
    internal class Particles
    {
        public Vector2 position;
        public Vector2 direction;
        public float speed;
        public float radius;
        public Vector2 velocity;
        public Color color;
        public float lifetime;
        public float maxLifetime;
        public float maxSize;
        public float size;

        public Particles(Vector2 position, Vector2 direction, float speed, float radius, Color color, float lifetime, float maxSize)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.radius = radius;
            this.velocity = direction * speed;
            this.color = color;
            this.lifetime = lifetime;
            this.maxLifetime = lifetime;
            this.maxSize = maxSize;
            this.size = maxSize;
        }
        public void Update()
        {
            // Decrease lifetime
            lifetime -= Raylib.GetFrameTime();

            // Calculate progress based on remaining lifetime
            float progress = lifetime / maxLifetime;

            // Calculate the new size
            size = Math.Max(progress * maxSize, 0.0f); // size is never negative

            position += velocity;

            // Check if the particle has expired
            if (lifetime <= 0)
            {

            }
        }

        public void Draw()
        {
            Raylib.DrawCircle((int)position.X, (int)position.Y, (int)size, color);
        }
    }
}