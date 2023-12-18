using System;
using System.ComponentModel.Design;
using System.Numerics;
using Raylib_CsLo;

namespace gd
{
    class Menu
    {
        private bool startButtonPressed;
        private bool exitButtonPressed;
        private bool optionsButtonPressed;

        private SettingsScreen settingsScreen;

        enum ScreenState { Start, Settings }
        private ScreenState currentState;
        public Menu()
        {
            Raylib.SetTargetFPS(60);
            startButtonPressed = false;
            exitButtonPressed = false;
            optionsButtonPressed = false;

            settingsScreen = new SettingsScreen();
            currentState = ScreenState.Start;
        }

        public void Draw()
        {
            Raylib.ClearBackground(Raylib.BLACK);

            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();
            if (currentState == ScreenState.Start)
            {
                int titleWidth = Raylib.MeasureText("hyppypeli", 80);
                int titleX = (screenWidth - titleWidth) / 2;
                Raylib.DrawText("hyppypeli", titleX, 150, 80, Raylib.YELLOW);

                int controlsWidth = Raylib.MeasureText("Hyppää SPACEA painamalla", 20);
                int controlsX = (screenWidth - controlsWidth) / 2;
                Raylib.DrawText("Hyppää SPACEA painamalla", controlsX, 325, 20, Raylib.WHITE);

                int startButtonWidth = Raylib.MeasureText("Pelaa", 60) + 40;
                int startButtonHeight = 50;
                int startButtonX = (screenWidth - startButtonWidth) / 2;
                int startButtonY = 450;
                startButtonPressed = RayGui.GuiButton(new Rectangle(startButtonX, startButtonY, startButtonWidth, startButtonHeight), "Pelaa");

                int optionsButtonWidth = Raylib.MeasureText("Asetukset", 60) + 40;
                int optionsButtonHeight = 50;
                int optionsButtonX = (screenWidth - optionsButtonWidth) / 2;
                int optionsButtonY = 550;
                if (optionsButtonPressed = RayGui.GuiButton(new Rectangle(optionsButtonX, optionsButtonY, optionsButtonWidth, optionsButtonHeight), "Asetukset"))
                {
                    currentState = ScreenState.Settings;
                }


                int exitButtonWidth = Raylib.MeasureText("Poistu", 60) + 40;
                int exitButtonHeight = 50;
                int exitButtonX = (screenWidth - exitButtonWidth) / 2;
                int exitButtonY = 650;
                exitButtonPressed = RayGui.GuiButton(new Rectangle(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight), "Poistu");
                if (exitButtonPressed)
                {
                    Environment.Exit(0);
                }
            }
            else if (currentState == ScreenState.Settings)
            {
                settingsScreen.Draw();

                if (settingsScreen.Update())
                {
                    currentState = ScreenState.Start;
                }
            }
        }


        public bool IsStartPressed()
        {
            return startButtonPressed;
        }

        public bool ShouldExit()
        {
            return exitButtonPressed;
        }
    }
}