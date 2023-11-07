using Raylib_CsLo;
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

        List<Particles> particles;
        Random randomGenerator;

        public bool moveRight = true;
        public static bool shouldChangeDirection = false;
        public bool moveDown = false;
        public float speed = 0.02f;

        bool playerInvincible = false;
        

        StartScreen startScreen;
        PauseMenu pauseMenu;
        SettingsScreen settingsScreen;
        developer developerMenu;
        const int gamefps = 60;
       
        public enum GameState { Playing, Win, Lose, Pause, Settings, Main, Dev };
        Stack<GameState> gameState = new Stack<GameState> ();
        void init()
        {

            float deltaTime = 1.0f / gamefps;

            gameState.Clear(); 
            gameState.Push(GameState.Main); 

            
            startScreen = new StartScreen();
            pauseMenu = new PauseMenu();
            settingsScreen = new SettingsScreen();
            developerMenu = new developer();

            particles = new List<Particles>();
            randomGenerator = new Random();

            player = null;
            bullets = new List<Bullet>();
            enemies = new List<Enemy>();

            Raylib.InitWindow(screenWidth, screenHeight, "Space Invaders");
            Raylib.SetExitKey(KeyboardKey.KEY_BACKSPACE);
            
            Raylib.InitAudioDevice();
            float playerSpeed = 120 * deltaTime;
            int playerSize = 40;
            speed = 120 * deltaTime;
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
                Raylib.SetTargetFPS(gamefps);
            
            
        }
        public void OnEnemyKilled(object sender, Vector2 position)
        {
            List<Color> colors = new List<Color>();
            colors.Add(Raylib.GRAY);
            colors.Add(Raylib.YELLOW);
            colors.Add(Raylib.ORANGE);

            for (int i = 0; i < 3; i++)
            {
                Vector2 direction = new Vector2(
                randomGenerator.NextSingle() * 2 - 1,
                randomGenerator.NextSingle() * 2 - 1);
                direction = Vector2.Normalize(direction);

                particles.Add(new Particles(position, direction, 1f, 5, colors[randomGenerator.Next(colors.Count)], 1.0f, 5.0f));
            }
        }
        public void PlayerSmoke()
        {
            if (player.health > 0)
            {
                // Calculate the position for the player's smoke particles
                Vector2 smokePosition = player.position + new Vector2(50, 45); // Adjust the offsets as needed

                // Create a new smoke particle and add it to the list
                Vector2 smokeDirection = new Vector2(0, 1); // Smoke particles go downwards
                float smokeSpeed = 0.7f;
                float smokeRadius = 10;
                Color smokeColor = Raylib.GRAY; // You can change the color if needed
                float smokeLifetime = 1.0f; // Adjust the lifetime as desired
                float smokeMaxSize = 10.0f; // Adjust the maximum size as desired
                particles.Add(new Particles(smokePosition, smokeDirection, smokeSpeed, smokeRadius, smokeColor, smokeLifetime, smokeMaxSize));

                // Update and remove expired smoke particles
                for (int i = particles.Count - 1; i >= 0; i--)
                {
                    particles[i].Update();
                    if (particles[i].lifetime <= 0)
                    {
                        particles.RemoveAt(i);
                    }
                }

                // Draw the smoke particles
                foreach (Particles smokeParticle in particles)
                {
                    smokeParticle.Draw();
                }
            }
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
                        PlayerSmoke();
                        
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
#if DEBUG
                            gameState.Push(GameState.Dev);
#endif
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
       
        public void reset(GameState Target)
        {
            
            float playerSpeed = 120;
            int playerSize = 40;
            Vector2 playerStart = new Vector2(screenWidth / 2, screenHeight - playerSize * 2);
            player = new Player(playerStart, new Vector2(0, 0), playerSpeed, playerSize);
            player.enemyKilled += OnEnemyKilled;


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
            foreach (Particles particle in particles)
            {
                particle.Draw();
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