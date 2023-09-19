using System.Numerics;
using Raylib_CsLo;

namespace Spaceinvaders
{
    internal class PauseMenu
    {
        private bool backButtonPressed;
        private bool startButtonPressed;
        private bool exitButtonPressed;
        private bool optionsButtonPressed;
        private bool goToSettings;

        public PauseMenu()
        {
            Raylib.SetTargetFPS(60);
            backButtonPressed = false;
            optionsButtonPressed = false;
            goToSettings = false;
        }

        public void Draw()
        {
            Raylib.ClearBackground(Raylib.BLACK);

            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();

            // Piirrä pause
            int titleWidth = Raylib.MeasureText("Pause", 60);
            int titleX = (screenWidth - titleWidth) / 2;
            Raylib.DrawText("Pause", titleX, 150, 60, Raylib.WHITE);

            // Nappi: ohjelman aloittaminen
            int startButtonWidth = Raylib.MeasureText("Main Menu", 60) + 40;
            int startButtonHeight = 50;
            int startButtonX = (screenWidth - startButtonWidth) / 2;
            int startButtonY = 450;
            startButtonPressed = RayGui.GuiButton(new Rectangle(startButtonX, startButtonY, startButtonWidth, startButtonHeight), "Main Menu");

            // Nappi: Asetukset valikko
            int settingsButtonWidth = Raylib.MeasureText("Settings", 60) + 40;
            int settingsButtonHeight = 70;
            int settingsButtonX = (screenWidth - settingsButtonWidth) / 2;
            int settingsButtonY = 550;
            optionsButtonPressed = RayGui.GuiButton(new Rectangle(settingsButtonX, settingsButtonY, settingsButtonWidth, settingsButtonHeight), "Settings");

            // Nappi: Takasin peliin
            int backButtonWidth = Raylib.MeasureText("Back", 40) + 20;
            int backButtonHeight = 50;
            int backButtonX = (screenWidth - backButtonWidth) / 2;
            int backButtonY = screenHeight - backButtonHeight - 30;
            backButtonPressed = RayGui.GuiButton(new Rectangle(backButtonX, backButtonY, backButtonWidth, backButtonHeight), "Back");
        }

        public bool GoToSettings()
        {
            return optionsButtonPressed;
        }
        public bool IsStartButtonPressed()
        {
            return startButtonPressed;
        }
        public bool IsBackButtonPressed()
        {
            return backButtonPressed;
        }
    }
}