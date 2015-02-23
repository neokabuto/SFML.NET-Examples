using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SFMLBreakout
{
    internal class Ball : CircleShape
    {
        /// <summary>
        /// The speed the ball moves at
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// The ball's current velocity
        /// </summary>
        public Vector2f Velocity { get; set; }

        public Ball()
        {
            Radius = 8.0f;
            Speed = 200.0f;
            ResetBall();
        }

        /// <summary>
        /// Move the ball to the starting position and speed
        /// </summary>
        public void ResetBall()
        {
            Position = new Vector2f(Program.Window.Size.X / 2 - Radius, Program.Window.Size.Y - 200);
            Velocity = new Vector2f(0, Speed);
        }

        public void Update(float delta)
        {
            Vector2f nextpos = Position + Velocity * delta;
            Vector2f nextvel = Velocity;

            // Check edges
            if (nextpos.X < 0)
            {
                nextpos.X = 0;
                nextvel.X *= -1;
            }
            else if (nextpos.X > Program.Window.Size.X - Radius)
            {
                nextpos.X = Program.Window.Size.X;
                nextvel.X *= -1;
            }

            if (nextpos.Y < 0)
            {
                nextpos.Y = 0;
                nextvel.Y *= -1;
            }

            // Check collisions
            if (Program.Paddle.IsColliding(this))
            {
                nextvel.Y *= -1.0f;
                Position = new Vector2f(Position.X, Position.Y - (Position.Y + (Radius * 2) - Program.Paddle.Position.Y));

                // Calculate "surface friction" change to X-speed
                nextvel.X += Program.Paddle.Velocity.X / 2;

                if (Math.Abs(nextvel.Y) < Speed / 2)
                {
                    nextvel.Y = Math.Sign(nextvel.Y) * Speed / 2;
                }

                // Normalize to Speed
                float length = (float) Math.Sqrt(nextvel.X * nextvel.X + nextvel.Y * nextvel.Y);
                nextvel *= Speed / length;
            }
            else
            {
                List<Brick> tempBricks = new List<Brick>();
                List<Brick> hitBricks = new List<Brick>();

                foreach (Brick b in Program.Bricks)
                {
                    if (b.IsColliding(this))
                    {
                        // The ball has hit at least one brick this tick
                        hitBricks.Add(b);
                    }
                    else
                    {
                        // The brick will still exist
                        tempBricks.Add(b);
                    }
                }

                Program.Bricks = tempBricks;

                if (hitBricks.Count > 0)
                {
                    bool hitTop = false;
                    bool hitLeft = false;
                    bool hitRight = false;
                    bool hitBottom = false;
                    float centerY = Position.Y + Radius;
                    float centerX = Position.X + Radius;

                    foreach (Brick hitBrick in hitBricks)
                    {
                        // Determine collision results
                        float diagonalslope = hitBrick.Size.Y / hitBrick.Size.X;

                        // y = m (x - x1) + y1 ( with radius to check against center)
                        float diagonal1Position = diagonalslope * (centerX - hitBrick.Position.X) + hitBrick.Position.Y;
                        float diagonal2Position = -1.0f * diagonalslope * (centerX - hitBrick.Position.X) + hitBrick.Position.Y + hitBrick.Size.Y;

                        if (centerY > diagonal1Position && centerY > diagonal2Position) // Above both, top side
                        {
                            hitTop = true;
                        }
                        else if (centerY > diagonal1Position) // Above 1 but not 2, right side
                        {
                            hitRight = true;
                        }
                        else if (centerY > diagonal2Position) // Below 1 but above 2, left side
                        {
                            hitLeft = true;
                        }
                        else // Below both, bottom side
                        {
                            hitBottom = true;
                        }
                    }

                    // Change velocity based on collisions
                    if (hitTop || hitBottom)
                    {
                        nextvel.Y *= -1.0f;
                    }

                    if (hitLeft || hitRight)
                    {
                        nextvel.X *= -1.0f;
                    }
                }
            }

            // Check if the ball has gone offscreen
            if (nextpos.Y > Program.Window.Size.Y)
            {
                ResetBall();
            }
            else // We don't want to override the reset
            {
                nextpos = Position + nextvel * delta;
                Position = nextpos;
                Velocity = nextvel;
            }
        }
    }
}