using Raylib_CsLo;
using System.Numerics;

namespace Spaceinvaders
{
    

    internal class Player
    {
        public static int numEnemies = 5;
        public event EventHandler<Vector2> enemyKilled;
        public int score = 0;
        private float maxSpeed;
        public Vector2 position;
        public float speed;
        public int health;
        public List<Bullet> bullets;

        public Texture texture;
        public Sound shootSound;
        public Player(Vector2 position, Vector2 vector2, float speed, int health)
        {
            this.position = position;
            this.speed = speed;
            this.maxSpeed = speed;
            this.health = health;
            this.bullets = new List<Bullet>();
            this.texture = Raylib.LoadTexture("images/player.png");
            shootSound = Raylib.LoadSound("Sounds/shoot.mp3");
            
        }

        public void DrawScore()
        {
            Raylib.DrawText($"Points: {score}", Raylib.GetScreenWidth() - 150, 20, 20, Raylib.BLACK);
        }

        public void Update(List<Enemy> enemies)
        {
            Vector2 mousePosition = Raylib.GetMousePosition();

            // Calculate the new speed based on the distance to the mouse position
            float desiredSpeed = Math.Clamp(Math.Abs(mousePosition.X - position.X), 0, maxSpeed);

            if (mousePosition.X < position.X && position.X > 20)
            {
                position.X -= desiredSpeed;
            }
            else if (mousePosition.X > position.X && position.X < Raylib.GetScreenWidth() - 20)
            {
                position.X += desiredSpeed;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                bullets.Add(new Bullet(new Vector2(position.X + 50, position.Y - 20), new Vector2(0, -5)));
                Raylib.PlaySound(shootSound);
            }

            
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();

                bool hitEnemy = false;

                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    if (Raylib.CheckCollisionCircles(bullets[i].position, 5, enemies[j].position, 20))
                    {
                        enemyKilled.Invoke(this, enemies[j].position);
                        enemies.RemoveAt(j);
                        bullets.RemoveAt(i);

                        numEnemies--;

                        score += 10;

                        break;
                    }
                }

                
                for (int k = bullets.Count - 1; k >= 0; k--)
                {
                    if (bullets[k].position.Y < 0 || bullets[k].position.Y > Raylib.GetScreenHeight())
                    {
                        bullets.RemoveAt(k);
                    }
                }

            }
        }
        public void Draw()
        {
            Raylib.DrawTextureEx(texture, position, 0f, 0.1f, Raylib.WHITE);

            foreach (Bullet bullet in bullets)
                bullet.Draw();

            Raylib.DrawText("Health: " + health, 10, 10, 20, Raylib.BLACK);
        }
    }
}
