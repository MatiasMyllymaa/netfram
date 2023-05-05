using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spaceinvaders
{
    internal class Bullet
    {
        public Vector2 position;
        public Vector2 velocity;

        public Bullet(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            this.velocity = velocity / 20;
        }

        public void Update()
        {
            position += velocity;
        }

        public void Draw()
        {
            Raylib.DrawCircle((int)position.X, (int)position.Y, 5, Color.RED);
        }
    }
}