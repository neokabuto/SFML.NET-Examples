using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLBreakout
{
    internal class Paddle : RectangleShape
    {
        public float MoveSpeed { get; set; }

        public Vector2f Velocity { get; set; }

        public Paddle()
        {
            MoveSpeed = 250;
            Size = new Vector2f(100, 16);
            Position = new Vector2f(Program.Window.Size.X / 2 - Size.X / 2, Program.Window.Size.Y - 64);
        }

        public void Update(float delta)
        {
            // Move based on keyboard input
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                Velocity = new Vector2f(-MoveSpeed, 0);
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                Velocity = new Vector2f(MoveSpeed, 0);
            }
            else
            {
                if (Math.Abs(Velocity.X) < 0.1f)
                {
                    Velocity = new Vector2f();
                }
                else
                {
                    Velocity = new Vector2f(Velocity.X * 0.8f, 0);
                }
            }

            // Don't move off the screen
            if (Position.X + Velocity.X * delta < 0)
            {
                Velocity = new Vector2f();
            }
            else if (Position.X + Size.X + Velocity.X * delta > Program.Window.Size.X)
            {
                Velocity = new Vector2f();
            }

            Position = Position + Velocity * delta;
        }

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
}