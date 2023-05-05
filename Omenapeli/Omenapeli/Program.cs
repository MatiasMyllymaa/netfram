using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;


namespace AppleGame
{
    class Player
    {
        public List<Vector2> segments;
        public float speed;
        public float width;
        public float height;
        public Vector2 direction;
        public int score;

        public Player(Vector2 position, float speed, float width, float height)
        {
            segments = new List<Vector2> { position };
            this.speed = speed;
            this.width = width;
            this.height = height;
            direction = new Vector2(0, 0);
            score = 0;
        }
        public void Move(int screenWidth, int screenHeight)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyPressed(KeyboardKey.KEY_D))
                direction = new Vector2(speed, 0);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT) || Raylib.IsKeyPressed(KeyboardKey.KEY_A))
                direction = new Vector2(-speed, 0);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP) || Raylib.IsKeyPressed(KeyboardKey.KEY_W))
                direction = new Vector2(0, -speed);
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) || Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                direction = new Vector2(0, speed);

            Vector2 newPosition = segments[0] + direction;
            if (newPosition.X < 0 || newPosition.X + width > screenWidth)
                direction.X = 0;
            if (newPosition.Y < 0 || newPosition.Y + height > screenHeight)
                direction.Y = 0;

            // move the head
            segments[0] += direction;

            // move the rest of the body
            for (int i = segments.Count - 1; i > 0; i--)
                segments[i] = segments[i - 1];
        }

        public void Grow()
        {
            // add a new segment at the end of the body, in the opposite direction of movement
            Vector2 newSegment = segments[segments.Count - 1] - direction;
            segments.Add(newSegment);

            score += 10;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int screenWidth = 1000;
            const int screenHeight = 650;

            Raylib.InitWindow(screenWidth, screenHeight, "omenapeli");

            Random rand = new Random();
            Vector2 applePosition = new Vector2(rand.Next(0, screenWidth), rand.Next(0, screenHeight));
            const float appleSize = 25;

            Player player = new Player(new Vector2(screenWidth / 2, screenHeight / 2), 0.1f, 20f, 20f);

            while (!Raylib.WindowShouldClose())
            {
                player.Move(screenWidth, screenHeight);

                Rectangle appleRec = new Rectangle(applePosition.X, applePosition.Y, appleSize, appleSize);

                Rectangle playerRec = new Rectangle(player.segments[0].X, player.segments[0].Y, player.width, player.height);
                if (Raylib.CheckCollisionRecs(playerRec, appleRec))
                {
                    applePosition.X = rand.Next(0, screenWidth - 20);
                    applePosition.Y = rand.Next(0, screenHeight - 20);

                    player.Grow();
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.GREEN);

                Raylib.DrawRectangle((int)applePosition.X, (int)applePosition.Y, (int)appleSize, (int)appleSize, Color.RED);
                foreach (Vector2 segment in player.segments)
                    Raylib.DrawRectangle((int)segment.X, (int)segment.Y, (int)player.width, (int)player.height, Color.BLACK);
                Raylib.DrawText($"Score: {player.score}", 10, 10, 20, Color.BLACK);
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}