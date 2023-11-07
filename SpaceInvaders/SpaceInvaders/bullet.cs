using Raylib_CsLo;
using System.Numerics;

namespace Spaceinvaders
{
    internal class Bullet
    {
        public Vector2 position;
        public Vector2 velocity;

        public Bullet(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        public void Update()
        {
            position += velocity;
        }

        public void Draw()
        {
            Raylib.DrawCircle((int)position.X, (int)position.Y, 5, Raylib.RED);
        }
    }
}