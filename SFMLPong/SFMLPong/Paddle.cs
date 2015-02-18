using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLPong
{
    /// <summary>
    /// The base class for all paddles.
    /// </summary>
    internal class Paddle : RectangleShape
    {
        public float MoveSpeed { get; set; }

        /// <summary>
        /// Create a new Paddle at a given position
        /// </summary>
        /// <param name="startposition">Position to place Paddle at</param>
        public Paddle(Vector2f startposition)
            : base(new Vector2f(16, 96))
        {
            Position = startposition;
            MoveSpeed = 150.0f;
        }

        /// <summary>
        /// Update the Paddle to its new position
        /// </summary>
        /// <param name="delta">Portion of a second since last frame</param>
        public virtual void Update(float delta)
        {
        }

        /// <summary>
        /// Determine if a point is inside the paddle's bounds
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <returns>If the point is on the inside of the Paddle</returns>
        public bool PointIntersects(Vector2f point)
        {
            if (point.X < Position.X || point.Y < Position.Y)
            {
                return false;
            }

            if (point.X > Position.X + Size.X || point.Y > Position.Y + Size.Y)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determine if a CircleShape is intersecting the paddle
        /// </summary>
        /// <param name="circle">Circle to test</param>
        /// <returns>If the circle intersects the paddle's bounds</returns>
        public bool CircleIntersects(CircleShape circle)
        {
            // Find actual center of circle
            Vector2f circlecenter = circle.Position + new Vector2f(circle.Radius, circle.Radius);

            // Find closest point on Paddle
            float closestX = Math.Min(Math.Max(circlecenter.X, Position.X), Position.X + Size.X);
            float closestY = Math.Min(Math.Max(circlecenter.Y, Position.Y), Position.Y + Size.Y);

            // Find distance from circle center to closest point
            float distSquared = ((circlecenter.X - closestX) * (circlecenter.X - closestX)) + ((circlecenter.Y - closestY) * (circlecenter.Y - closestY));
            return distSquared < (circle.Radius * circle.Radius);
        }
    }

    /// <summary>
    /// A human-controlled Paddle
    /// </summary>
    internal class PlayerPaddle : Paddle
    {
        public PlayerPaddle(Vector2f startposition)
            : base(startposition)
        {
        }

        /// <summary>
        /// Update the PlayerPaddle to a new position
        /// </summary>
        /// <param name="delta">Portion of a second since last frame</param>
        public override void Update(float delta)
        {
            // Move based on keyboard input
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                // Don't move above the top edge of the screen
                if (Position.Y - MoveSpeed * delta > 0)
                {
                    Position = Position - new Vector2f(0, MoveSpeed * delta);
                }
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                // Don't move below the screen
                if (Position.Y + Size.Y + MoveSpeed * delta < Program.Window.Size.Y)
                {
                    Position = Position + new Vector2f(0, MoveSpeed * delta);
                }
            }
        }
    }

    /// <summary>
    /// A computer-controlled Paddle
    /// </summary>
    internal class AIPaddle : Paddle
    {
        public AIPaddle(Vector2f startposition)
            : base(startposition)
        {
        }

        /// <summary>
        /// Update the AIPaddle to its new position
        /// </summary>
        /// <param name="delta">Portion of a second since last frame</param>
        public override void Update(float delta)
        {
            Vector2f center = Position + new Vector2f(Size.X / 2, Size.Y / 2);
            Vector2f ballcenter = Program.Ball.Position - new Vector2f(Program.Ball.Radius, Program.Ball.Radius);
            float diff = Math.Abs(ballcenter.Y - center.Y);

            // Move to match the ball
            if (ballcenter.Y > center.Y + 32)
            {
                // Don't move below the screen
                if (Position.Y + Size.Y + MoveSpeed * delta < Program.Window.Size.Y)
                {
                    Position = Position + new Vector2f(0, Math.Min(diff, MoveSpeed * delta));
                }
            }
            else if (ballcenter.Y < center.Y - 32)
            {
                // Don't move above the screen
                if (Position.Y - MoveSpeed * delta > 0)
                {
                    Position = Position - new Vector2f(0, Math.Min(diff, MoveSpeed * delta));
                }
            }
        }
    }
}