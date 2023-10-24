using System;
using Raylib_CsLo;
using System.Numerics;
using Spaceinvaders;
using System.Collections.Generic;
using static SpaceInvaders.inv;

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

        bool playerInvincible = false;
        

        StartScreen startScreen;
        PauseMenu pauseMenu;
        SettingsScreen settingsScreen;
        developer developerMenu;

       
        public enum GameState { Playing, Win, Lose, Pause, Settings, Main, Dev };
        Stack<GameState> gameState = new Stack<GameState> ();
        void init()
        {
            gameState.Clear(); 
            gameState.Push(GameState.Main); 

            
            startScreen = new StartScreen();
            pauseMenu = new PauseMenu();
            settingsScreen = new SettingsScreen();
            developerMenu = new developer();

            player = null;
            bullets = new List<Bullet>();
            enemies = new List<Enemy>();

            Raylib.InitWindow(screenWidth, screenHeight, "Space Invaders");
            Raylib.SetExitKey(KeyboardKey.KEY_BACKSPACE);
            
            Raylib.InitAudioDevice();
            float playerSpeed = 120;
            int playerSize = 40;
            Vector2 playerStart = new Vector2(screenWidth / 2, screenHeight - playerSize * 2);

            player = new Player(playerStart, new Vector2(0, 0), playerSpeed, playerSize);

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
                Raylib.BeginDrawing();
                switch (gameState.Peek())
                {
                    case GameState.Main:

                        startScreen.Draw();
                        if (startScreen.IsStartPressed())
                        {
                            reset(GameState.Playing);
                            gameState.Push(GameState.Playing);

                        }
                    break;
                    
                    case GameState.Playing:

                        drawGame();
                        
                        UpdateEnemies();
                        player.Update(enemies);
                        foreach (Enemy enemy in enemies.ToList())
                        {
                            enemy.Update(player, playerInvincible);
                        }
                        if (player.health <= 0)
                        {
                            gameState.Push(GameState.Lose);
                            
                        }
                        else if (enemies.Count == 0)
                        {
                            gameState.Push(GameState.Win);
                            
                        }
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                        {

                            gameState.Push(GameState.Pause);

                        }
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                        {
                            reset(GameState.Playing);
                        }
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_F1))
                        {
                            gameState.Push(GameState.Dev);
                        }
                        break;
                    
                    case GameState.Settings: 
                        settingsScreen.Draw();
                        if (settingsScreen.Update())
                        {
                            gameState.Pop();
                        }
                        break;
                    case GameState.Win:
                        drawGameOver();
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                        {
                            reset(GameState.Main);
                        }
                        break;
                    case GameState.Lose: 
                        drawGameOver();
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                        {
                            reset(GameState.Main);
                        }
                        break;
                    case GameState.Pause:
                        pauseMenu.Draw();
                        if (pauseMenu.GoToSettings())
                        {
                            gameState.Push(GameState.Settings);
                        }
                        if (pauseMenu.IsBackButtonPressed())
                        {
                            gameState.Pop();
                        }
                        if (pauseMenu.IsStartButtonPressed())
                        {
                            gameState.Push(GameState.Main);
                        }
                 
                        break;
                    case GameState.Dev:
                        developerMenu.Draw();

                        if (developerMenu.IsBackPressed())
                        {
                            playerInvincible = developerMenu.IsInvincibilityToggled();

                            gameState.Pop();
                        }
                        break;
                }
                
                Raylib.EndDrawing();

            }
            

        }
        public void Dev()
        {

        }
        public void reset(GameState Target)
        {
            
            float playerSpeed = 120;
            int playerSize = 40;
            Vector2 playerStart = new Vector2(screenWidth / 2, screenHeight - playerSize * 2);
            player = new Player(playerStart, new Vector2(0, 0), playerSpeed, playerSize);

            
            bullets.Clear();

            
            enemies.Clear();
            for (int i = 0; i < numEnemies; i++)
            {
                int row = i / 5;
                int col = i % 5;

                Vector2 position = new Vector2(col * 100 + 100, row * 100 + 100);
                Enemy enemy = new Enemy(position, new Vector2(1, 0), 50, "images/enemy.png", new List<Player> { player });

                enemies.Add(enemy);
            }

            
            gameState.Clear();
            gameState.Push(Target);
        }

        void drawGameOver()
        {
            if (gameState.Peek() == GameState.Lose)
            {
                Raylib.ClearBackground(Raylib.RED);
                Raylib.DrawText("Game Over!", 300, 400, 50, Raylib.BLACK);
                Raylib.DrawText("You got:" + player.score + " score", 300, 500, 40, Raylib.BLACK);

            }
            else if (gameState.Peek() == GameState.Win)
            {
                Raylib.ClearBackground(Raylib.LIME);
                Raylib.DrawText("You Win!", 300, 400, 50, Raylib.BLACK);
                Raylib.DrawText("You got:" + player.score + " score", 300, 500, 40, Raylib.BLACK);

            }
            Raylib.DrawText("Press ENTER to go main menu", 300, 600, 30, Raylib.BLACK);
            Raylib.DrawText("Press BACKSPACE to quit", 300, 700, 30, Raylib.BLACK);
        }

        void drawGame()
        {
            Raylib.ClearBackground(Raylib.WHITE);
            player.Draw();
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw();
            }
            player.DrawScore();
        }

        void UpdateEnemies()
        {
            bool wallHit = false;
            float enemySpeed = speed;

            
            foreach (Enemy enemy in enemies)
            {
                enemy.position.X += enemySpeed * enemy.transform.direction.X;

                
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