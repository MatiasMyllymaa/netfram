using Raylib_CsLo;

namespace Spaceinvaders
{
    internal class developer
    {
        private bool backButtonPressed;
        public bool invincibilityToggled;

        public developer()
        {
            Raylib.SetTargetFPS(60);
            backButtonPressed = false;
            invincibilityToggled = false;
        }

        public void Draw()
        {
            Raylib.ClearBackground(Raylib.BLACK);

            int screenWidth = Raylib.GetScreenWidth();
            int screenHeight = Raylib.GetScreenHeight();

            // PiirrÃ¤ Developer menu
            int titleWidth = Raylib.MeasureText("Developer Menu", 60);
            int titleX = (screenWidth - titleWidth) / 2;
            Raylib.DrawText("Developer Menu", titleX, 150, 60, Raylib.WHITE);

            // Nappi: Kuolemattomuus ja takaisin
            int toggleButtonWidth = Raylib.MeasureText("Toggle Invincibility", 40) + 20;
            int toggleButtonHeight = 50;
            int toggleButtonX = (screenWidth - toggleButtonWidth) / 2;
            int toggleButtonY = (screenHeight - toggleButtonHeight) / 2;

            if (RayGui.GuiButton(new Rectangle(toggleButtonX, toggleButtonY, toggleButtonWidth, toggleButtonHeight), "Toggle Invincibility"))
            {
                invincibilityToggled = !invincibilityToggled;
            }

            string invincibilityStatus = "Invincibility: " + (invincibilityToggled ? "On" : "Off");
            int statusWidth = Raylib.MeasureText(invincibilityStatus, 20);
            int statusX = (screenWidth - statusWidth) / 2;
            Raylib.DrawText(invincibilityStatus, statusX, toggleButtonY - 40, 20, Raylib.WHITE);

            // Nappi: Takasin peliin
            int backButtonWidth = Raylib.MeasureText("Back", 40) + 20;
            int backButtonHeight = 50;
            int backButtonX = (screenWidth - backButtonWidth) / 2;
            int backButtonY = screenHeight - backButtonHeight - 30;
            backButtonPressed = RayGui.GuiButton(new Rectangle(backButtonX, backButtonY, backButtonWidth, backButtonHeight), "Back");
        }

        public bool IsBackPressed()
        {
            return backButtonPressed;
        }

        public bool IsInvincibilityToggled()
        {
            return invincibilityToggled;
        }

    }
}