using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class SettingsScreen
    {
        private bool backButtonPressed;

        public SettingsScreen()
        {
            Raylib.SetTargetFPS(60);
            backButtonPressed = false;
        }

        public void Draw()
        {
            Raylib.ClearBackground(Raylib.BLACK);

            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();

            // Draw the title
            int titleWidth = Raylib.MeasureText("Settings", 60);
            int titleX = (screenWidth - titleWidth) / 2;
            Raylib.DrawText("Settings", titleX, 150, 60, Raylib.WHITE);

            // Draw back button
            int backButtonWidth = Raylib.MeasureText("Back", 40) + 20;
            int backButtonHeight = 50;
            int backButtonX = (screenWidth - backButtonWidth) / 2;
            int backButtonY = screenHeight - backButtonHeight - 30;
            backButtonPressed = RayGui.GuiButton(new Rectangle(backButtonX, backButtonY, backButtonWidth, backButtonHeight), "Back");
        }

        public bool Update()
        {
            return backButtonPressed;
        }
    }
}
