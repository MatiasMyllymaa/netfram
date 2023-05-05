﻿using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spaceinvaders
{
    internal class Enemy
    {
        public List<Bullet> bullets;
        private float bulletCooldown = 0;
        private float bulletCooldownMax = 12000;

        public Transform transform;

        public Vector2 position;
        public int health;

        public bool isAlive = true;

        private Texture2D texture;

  

        private List<Player> players;
        private float speed;
        private float size;

        public Enemy(Vector2 position, Vector2 direction, int health, string texturePath, List<Player> players)
        {
            this.position = position;
            this.health = health;

            this.bullets = new List<Bullet>();
            this.players = players;

            texture = Raylib.LoadTexture(texturePath);

            this.transform = new Transform(position, direction, speed, size);

            // Set the initial bullet cooldown time to a random value between 5 and 12 seconds
            bulletCooldownMax = Raylib.GetRandomValue(5000, 15000);
        }

        public void Update(Player player)
        {
            // Check if the enemy is dead
            if (health <= 0)
            {
                isAlive = false;
                player.score += 10;
            }

            // Subtract time since last update from the bullet cooldown time
            bulletCooldown -= Raylib.GetFrameTime() * 1000;

            if (bulletCooldown <= 0)
            {
                // Time to shoot!
                bullets.Add(new Bullet(new Vector2(position.X + 50, position.Y + 20), new Vector2(0, 1)));
                bulletCooldown = bulletCooldownMax;
            }

            // Move enemy bullets and check for collisions
            foreach (Bullet bullet in bullets.ToList())
            {
                bullet.Update();

                foreach (Player otherPlayer in players)
                {
                    if (Raylib.CheckCollisionCircles(bullet.position, 5, player.position, 20))
                    {
                        // Reduce player health
                        player.health -= 10;

                        bullets.Remove(bullet);
                        break;
                    }
                }
            }

            // Remove bullets that leave the screen
            bullets.RemoveAll(bullet => bullet.position.Y < 0 || bullet.position.Y > Raylib.GetScreenHeight());
        }
        
        public void Draw()
        {
            Raylib.DrawTextureEx(texture, position, 0f, 0.1f, Color.WHITE);

            foreach (Bullet bullet in bullets)
            bullet.Draw();
        }
    }
}