using System;
using SFML.Graphics;
using SFML.System;

namespace SFMLPong
{
    /// <summary>
    /// A circle that moves around the screen and bounces
    /// </summary>
    internal class Ball : CircleShape
    {
        /// <summary>
        /// Maximum speed of ball
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Current 2D velocity of ball
        /// </summary>
        public Vector2f Velocity { get; set; }

        public Ball()
            : base(16.0f)
        {
            Speed = 150.0f;
            Velocity = new Vector2f(-Speed, 0);
        }

        /// <summary>
        /// Update the position of the ball based on its velocity
        /// </summary>
        /// <param name="deltatime">Portion of a second that has passed since last update</param>
        public void Update(float deltatime)
        {
            Vector2f nextpos = Position + Velocity * deltatime;
            Vector2f nextvel = Velocity;

            // Check boundary collisions
            if (nextpos.X < 0)
            {
                Program.Player2Score++;
                Position = new Vector2f(Program.Window.Size.X / 2 - Radius, Program.Window.Size.Y / 2 - Radius);
                nextvel.X = nextvel.X * -1;
                nextvel.Y = 0;
                Speed = 150.0f;
            }
            else if (nextpos.X > Program.Window.Size.X - Radius * 2)
            {
                Program.Player1Score++;
                Position = new Vector2f(Program.Window.Size.X / 2 - Radius, Program.Window.Size.Y / 2 - Radius);
                nextvel.X = nextvel.X * -1;
                nextvel.Y = 0;
                Speed = 150.0f;
            }

            if (nextpos.Y < 0)
            {
                Position = new Vector2f(Position.X, 0);
                nextvel.Y = nextvel.Y * -1;
            }
            else if (nextpos.Y > Program.Window.Size.Y - Radius * 2)
            {
                Position = new Vector2f(Position.X, Program.Window.Size.Y - Radius * 2);
                nextvel.Y = nextvel.Y * -1;
            }

            // Check paddle collisions
            if (Program.Player1Paddle.CircleIntersects(this))
            {
                // Boost speed
                if (Speed < 300)
                {
                    Speed *= 1.1f;
                }

                float yDiff = nextpos.Y - Program.Player1Paddle.Position.Y + Radius + (Program.Player1Paddle.Size.Y / 2.0f);
                float angle = ((float) Math.PI / 4.0f) * yDiff / (Program.Player1Paddle.Size.Y / 2.0f);

                nextvel.X = Speed * (float) Math.Sin(angle);
                nextvel.Y = -1.0f * Speed * (float) Math.Cos(angle);
            }
            else if (Program.Player2Paddle.CircleIntersects(this))
            {
                // Boost speed
                if (Speed < 300)
                {
                    Speed *= 1.1f;
                }

                float yDiff = nextpos.Y - Program.Player2Paddle.Position.Y + Radius + (Program.Player2Paddle.Size.Y / 2.0f);
                float angle = ((float) Math.PI / 4.0f) * yDiff / (Program.Player2Paddle.Size.Y / 2.0f);

                nextvel.X = -1.0f * Speed * (float) Math.Sin(angle);
                nextvel.Y = -1.0f * Speed * (float) Math.Cos(angle);
            }

            // Normalize velocity
            float length = (float) Math.Sqrt(nextvel.X * nextvel.X + nextvel.Y * nextvel.Y);
            nextvel = nextvel * (Speed / length);

            nextpos = Position + nextvel * deltatime;

            Velocity = nextvel;
            Position = nextpos;
        }
    }
}