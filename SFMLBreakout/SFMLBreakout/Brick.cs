using System;
using SFML.Graphics;
using SFML.System;

namespace SFMLBreakout
{
    internal class Brick : RectangleShape
    {
        public bool IsColliding(Ball b)
        {
            // Find actual center of circle
            Vector2f circlecenter = b.Position + new Vector2f(b.Radius, b.Radius);

            // Get closest point on paddle to Ball
            float closestX = Math.Min(Math.Max(circlecenter.X, Position.X), Position.X + Size.X);
            float closestY = Math.Min(Math.Max(circlecenter.Y, Position.Y), Position.Y + Size.Y);

            // Compare distance to from point to center to radius of the Ball
            float distSquared = ((circlecenter.X - closestX) * (circlecenter.X - closestX)) + ((circlecenter.Y - closestY) * (circlecenter.Y - closestY));
            return distSquared < (b.Radius * b.Radius);
        }
    }

    internal class BrickBuilder
    {
        public void SetupLevel(uint columns, uint rows)
        {
            // Determine Brick size
            uint brickwidth = Program.Window.Size.X / columns;
            uint brickheight = (uint) (Program.Window.Size.Y * 0.5f / rows);

            // Create Bricks
            Random rand = new Random();
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Brick b = new Brick();
                    b.Position = new Vector2f(x * brickwidth + 1, y * brickheight + 1 + 64);
                    b.Size = new Vector2f(brickwidth - 2, brickheight - 2);

                    // Generate color for Brick
                    Color c = new Color((byte) rand.Next(100, 255), (byte) rand.Next(100, 255), (byte) rand.Next(100, 255));
                    b.FillColor = c;
                    Program.Bricks.Add(b);
                }
            }
        }
    }
}