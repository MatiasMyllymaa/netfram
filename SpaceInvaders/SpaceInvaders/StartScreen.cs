using System.Numerics;
using Raylib_cs;
using Spaceinvaders;

namespace SpaceInvaders
{
    class StartScreen
    {
        public StartScreen()
        {
            Raylib.SetTargetFPS(60);
        }

        public void Draw()
        {
            Raylib.ClearBackground(Color.BLACK);
            Raylib.DrawText("Space Invaders", 65, 350, 100, Color.YELLOW);
            Raylib.DrawText("Press ENTER to start", 225, 550, 40, Color.GRAY);
        }

        public bool Update()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}