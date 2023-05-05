using System;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using Spaceinvaders;

namespace SpaceInvaders
{
    class inv
    {
        public static int numEnemies = 25;

        const int screenWidth = 1000;
        const int screenHeight = 1000;

        Player player;
        List<Bullet> bullets;
        List<Enemy> enemies;
        private Vector2 position;

        public bool moveRight = true;
        public static bool shouldChangeDirection = false;
        public bool moveDown = false;
        public float speed = 0.02f;

        void init()
        {
            Raylib.InitWindow(screenWidth, screenHeight, "Space Invaders");

            float playerSpeed = 120;
            int playerSize = 40;
            Vector2 playerStart = new Vector2(screenWidth / 2, screenHeight - playerSize * 2);

            player = new Player(playerStart, new Vector2(0, 0), playerSpeed, playerSize);

            bullets = new List<Bullet>();

            enemies = new List<Enemy>();

            for (int i = 0; i < numEnemies; i++)
            {
                int row = i / 5; 
                int col = i % 5; 

                Vector2 position = new Vector2(col * 100 + 100, row * 100 + 100);
                Enemy enemy = new Enemy(position, new Vector2(1, 0), 50, "images/enemy.png", new List<Player> { player });

                enemies.Add(enemy);

            }

            Raylib.SetTargetFPS(2500);
        }

        public void GameLoop()
        {
            init();

            while (!Raylib.WindowShouldClose())
            {
                

                
                

                switch (CheckGameState())
                {
                    case GameState.InProgress:
                        UpdateEnemies();

                        // päivitä
                        player.Update(enemies);
                        foreach (Enemy enemy in enemies.ToList())
                        {
                            enemy.Update(player);
                        }
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Color.WHITE);
                        player.Draw();
                        foreach (Enemy enemy in enemies)
                        {
                            enemy.Draw();
                        }
                        player.DrawScore();

                        Raylib.EndDrawing();
                        break;


                    case GameState.GameOver:
                        // Game over näyttö
                        Raylib.ClearBackground(Color.RED);
                        Raylib.DrawText("You lost!", 400, 500, 50, Color.BLACK);
                        Raylib.DrawText("Press ESC to exit or ENTER to restart game", 200, 600, 30, Color.BLACK);
                        Raylib.EndDrawing();

                        WaitForEscapeKeyPress();
                        break;

                    case GameState.Win:
                        // voitto näyttö
                        Raylib.ClearBackground(Color.GREEN);
                        Raylib.DrawText("You Win!", 400, 500, 50, Color.BLACK);
                        Raylib.DrawText("Press ESC to exit or ENTER to restart game", 200, 600, 30, Color.BLACK);
                        Raylib.EndDrawing();

                        WaitForEscapeKeyPress();
                        
                        break;
                }
            }
        }

        private GameState CheckGameState()
        {
            if (player.health <= 0)
            {
                return GameState.GameOver;
            }

            if (enemies.Count == 0)
            {
                return GameState.Win;
            }

            return GameState.InProgress;
        }

        private void WaitForEscapeKeyPress()
        {
            
            
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                {
                    Raylib.CloseWindow();
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    RestartGame();
                }
        }
        private void RestartGame()
        {
            

            // Reset game state
            player.health = 3;
            player.score = 0;
        }

        private enum GameState
        {
            InProgress,
            GameOver,
            Win
        }

        void UpdateEnemies()
        {
            bool wallHit = false;
            float enemySpeed = speed;

            
            foreach (Enemy enemy in enemies)
            {
                enemy.position.X += enemySpeed * enemy.transform.direction.X;

                // katsoo jos vihut osuu seinään
                if (enemy.position.X - enemy.transform.size / 2 <= 0 || enemy.position.X + enemy.transform.size / 2 >= screenWidth)
                {
                    wallHit = true;
                }
            }

            
            if (wallHit)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.transform.direction.X *= -1.0f;
                    enemy.position.Y += 10;
                }
            }

            // pitääkö vihollisten kääntyä
            if (enemies.Count > 0 && moveRight && enemies.Last().position.X >= screenWidth - 20)
            {
                moveRight = false;
                shouldChangeDirection = true;
                position.Y += 10; 
            }

            else if (enemies.Count > 0 && !moveRight && enemies.First().position.X <= 20)
            {
                moveRight = true;
                shouldChangeDirection = true;
                position.Y += 10; 
            }

            // seinään osuessaan vaihtavat suuntaa
            if (shouldChangeDirection)
            {
                speed *= -1;
                shouldChangeDirection = false;
                moveDown = true;
                foreach (Enemy enemy in enemies)
                {
                    enemy.transform.direction.X *= -1.0f;
                    enemy.position.Y += 10;
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            inv game = new inv();

            game.GameLoop();
        }
    }
}