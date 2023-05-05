using System;
using Raylib_CsLo;

public class Snowman
{
    public static void Main()
    {
        // Set up the window and background color
        Raylib.InitWindow(800, 600, "Snowman");
        Raylib.SetTargetFPS(60);
        Raylib.ClearBackground(Raylib.WHITE);

        // Set up the snowman's body parts
        int snowmanX = 400;
        int snowmanY = 450;
        int snowmanBodyRadius = 50;
        int snowmanHeadRadius = 40;
        int snowmanEyeRadius = 5;

        // Draw the snowman's body
        Raylib.DrawCircle(snowmanX, snowmanY, snowmanBodyRadius, Raylib.WHITE);
        Raylib.DrawCircle(snowmanX, snowmanY - snowmanBodyRadius - snowmanHeadRadius, snowmanHeadRadius, Raylib.WHITE);
        Raylib.DrawCircle(snowmanX - 20, snowmanY - snowmanBodyRadius - snowmanHeadRadius - 10, snowmanEyeRadius, Raylib.BLACK);
        Raylib.DrawCircle(snowmanX + 20, snowmanY - snowmanBodyRadius - snowmanHeadRadius - 10, snowmanEyeRadius, Raylib.BLACK);
        Raylib.DrawCircle(snowmanX, snowmanY - snowmanBodyRadius - 10, snowmanEyeRadius, Raylib.ORANGE);

        // Display the window and wait for it to close
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            // Draw the snowman's body
            Raylib.DrawCircle(snowmanX, snowmanY, snowmanBodyRadius, Raylib.WHITE);
            Raylib.DrawCircle(snowmanX, snowmanY - snowmanBodyRadius - snowmanHeadRadius, snowmanHeadRadius, Raylib.WHITE);
            Raylib.DrawCircle(snowmanX - 20, snowmanY - snowmanBodyRadius - snowmanHeadRadius - 10, snowmanEyeRadius, Raylib.BLACK);
            Raylib.DrawCircle(snowmanX + 20, snowmanY - snowmanBodyRadius - snowmanHeadRadius - 10, snowmanEyeRadius, Raylib.BLACK);
            Raylib.DrawCircle(snowmanX, snowmanY - snowmanBodyRadius - 10, snowmanEyeRadius, Raylib.ORANGE);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
