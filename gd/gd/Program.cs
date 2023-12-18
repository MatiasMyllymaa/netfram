using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_CsLo;

namespace gd
{
    internal class Program
    {
        private enum GameState
        {
            MainMenu,
            Playing,
            GameOver,
            Pause
        }

        private const int screenWidth = 1800;
        private const int screenHeight = 940;
        private const int playerSize = 50;
        private const int spikeWidth = 30;
        private const int spikeHeight = 50;
        private const float playerScale = 0.17f;
        private static GameState gameState = GameState.MainMenu;

        private static void Main(string[] args)
        {
            Raylib.InitWindow(screenWidth, screenHeight, "Gd");
            Raylib.SetExitKey(KeyboardKey.KEY_BACKSPACE);
            Menu mainMenu = new Menu();
            PauseMenu pauseMenu = new PauseMenu();

            
            Texture playerTexture = Raylib.LoadTexture("Images/Player.png");


            float playerX = screenWidth / 4;
            float playerY = screenHeight / 2;

            float gravity = 1000.0f;
            float jumpForce = -600.0f;
            float playerVelocity = 5;

            List<Spike> spikes = new List<Spike>();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.WHITE);

                switch (gameState)
                {
                    case GameState.MainMenu:
                        mainMenu.Draw();

                        if (mainMenu.IsStartPressed())
                        {
                            gameState = GameState.Playing;
                            playerX = screenWidth / 4;
                            playerY = screenHeight / 2;
                            spikes.Clear();
                        }

                        if (mainMenu.ShouldExit())
                        {
                            Raylib.CloseWindow();
                        }
                        break;

                    case GameState.Playing:
                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                        {
                            gameState = GameState.Pause;
                        }

                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && playerY >= screenHeight - playerSize)
                        {
                            playerVelocity = jumpForce;
                        }

                        playerVelocity += gravity * Raylib.GetFrameTime();
                        playerY += playerVelocity * Raylib.GetFrameTime();

                        if (playerY >= screenHeight - playerSize)
                        {
                            playerY = screenHeight - playerSize;
                            playerVelocity = 7;
                        }

                        if (CheckCollisionWithSpikes(playerX, playerY, playerSize, spikes))
                        {
                            gameState = GameState.GameOver;
                        }

                        UpdateSpikes(spikes);

                        // Draw player texture
                        Raylib.DrawTextureEx(playerTexture, new Vector2(playerX, playerY), 0, playerScale, Raylib.WHITE);

                        foreach (var spike in spikes)
                        {
                            Raylib.DrawTriangle(
                                new Vector2(spike.X, spike.Y),
                                new Vector2(spike.X + spikeWidth, spike.Y),
                                new Vector2(spike.X + spikeWidth / 2, spike.Y - spikeHeight),
                                Raylib.RED 
                            );
                        }
                        break;

                    case GameState.GameOver:
                        Raylib.DrawText("Game Over", screenWidth / 2 - 100, screenHeight / 2 - 20, 40, Raylib.RED);
                        Raylib.DrawText("Press Enter to return to Main Menu", screenWidth / 2 - 220, screenHeight / 2 + 20, 20, Raylib.DARKGRAY);

                        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                        {
                            gameState = GameState.MainMenu;
                        }
                        break;

                    case GameState.Pause:
                        pauseMenu.Draw();

                        if (pauseMenu.IsStartButtonPressed())
                        {
                            gameState = GameState.MainMenu;
                        }
                        else if (pauseMenu.IsBackButtonPressed())
                        {
                            gameState = GameState.Playing;
                        }
                        break;
                }

                Raylib.EndDrawing();
            }

            Raylib.UnloadTexture(playerTexture);

            Raylib.CloseWindow();
        }

        private static bool CheckCollisionWithSpikes(float playerX, float playerY, int playerSize, List<Spike> spikes)
        {
            Rectangle playerRect = new Rectangle(playerX, playerY, playerSize, playerSize);

            foreach (var spike in spikes)
            {
                Rectangle spikeRect = new Rectangle(spike.X, spike.Y - spikeHeight, spikeWidth, spikeHeight);

                if (Raylib.CheckCollisionRecs(playerRect, spikeRect))
                {
                    return true;
                }
            }

            return false;
        }

        private static void UpdateSpikes(List<Spike> spikes)
        {
            if (Raylib.GetRandomValue(0, 100) < 2)
            {
                spikes.Add(new Spike(screenWidth, screenHeight));
            }

            foreach (var spike in spikes)
            {
                spike.X -= 200.0f * Raylib.GetFrameTime();
            }

            spikes.RemoveAll(spike => spike.X + spikeWidth < 0);
        }
    }

    internal class Spike
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Spike(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
