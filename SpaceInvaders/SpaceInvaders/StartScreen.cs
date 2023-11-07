using System;
using System.ComponentModel.Design;
using System.Numerics;
using Raylib_CsLo;
using SpaceInvaders;

namespace Spaceinvaders
{
    class StartScreen
    {
        private bool startButtonPressed;
        private bool exitButtonPressed;
        private bool optionsButtonPressed;

        private SettingsScreen settingsScreen;

        enum ScreenState { Start, Settings }
        private ScreenState currentState;
        public StartScreen()
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
                // Tekstikenttä: Pelin nimi
                int titleWidth = Raylib.MeasureText("Space Invaders", 80);
                int titleX = (screenWidth - titleWidth) / 2;
                Raylib.DrawText("Space Invaders", titleX, 150, 80, Raylib.YELLOW);

                // Tekstikenttä: Ohjeet
                int controlsWidth = Raylib.MeasureText("Ohjaus: Hiiren kursori = Liiku, SPACE = Ammu", 20);
                int controlsX = (screenWidth - controlsWidth) / 2;
                Raylib.DrawText("Ohjaus: Hiiren kursori = Liiku, SPACE = Ammu", controlsX, 325, 20, Raylib.WHITE);

                // Nappi: ohjelman aloittaminen
                int startButtonWidth = Raylib.MeasureText("Start", 60) + 40;
                int startButtonHeight = 50;
                int startButtonX = (screenWidth - startButtonWidth) / 2;
                int startButtonY = 450;
                startButtonPressed = RayGui.GuiButton(new Rectangle(startButtonX, startButtonY, startButtonWidth, startButtonHeight), "Start");

                // Nappi: Asetus valikkoon
                int optionsButtonWidth = Raylib.MeasureText("Options", 60) + 40;
                int optionsButtonHeight = 50;
                int optionsButtonX = (screenWidth - optionsButtonWidth) / 2;
                int optionsButtonY = 550;
                if(optionsButtonPressed = RayGui.GuiButton(new Rectangle(optionsButtonX, optionsButtonY, optionsButtonWidth, optionsButtonHeight), "Options"))
                {
                    currentState = ScreenState.Settings;
                }
                  
                
                // Nappi: Ohjelman sulkeminen
                int exitButtonWidth = Raylib.MeasureText("Exit", 60) + 40;
                int exitButtonHeight = 50;
                int exitButtonX = (screenWidth - exitButtonWidth) / 2;
                int exitButtonY = 650;
                exitButtonPressed = RayGui.GuiButton(new Rectangle(exitButtonX, exitButtonY, exitButtonWidth, exitButtonHeight), "Exit");
                if (exitButtonPressed)
                {
                    Environment.Exit(0);
                }
            }else if(currentState == ScreenState.Settings)
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