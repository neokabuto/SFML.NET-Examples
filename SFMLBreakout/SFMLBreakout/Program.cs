using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLBreakout
{
    internal class Program
    {
        public static List<Brick> Bricks = new List<Brick>();
        public static Paddle Paddle;
        public static Ball Ball;
        public static RenderWindow Window;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            // Create window
            Window = new RenderWindow(new VideoMode(800, 600), "Hello SFML.Net", Styles.None);
            Window.SetVerticalSyncEnabled(true);
            Window.SetActive();

            // Setup event handlers
            Window.Closed += new EventHandler(OnClosed);
            Window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

            // Create the game elements
            Ball = new Ball();
            Paddle = new Paddle();
            BrickBuilder builder = new BrickBuilder();
            builder.SetupLevel(9, 10);

            // Set up timing
            Clock clock = new Clock();
            float delta = 0.0f;

            // Game loop
            while (Window.IsOpen)
            {
                // Update objects
                delta = clock.Restart().AsSeconds();
                Ball.Update(delta);
                Paddle.Update(delta);

                // Rebuild level if needed
                if (Bricks.Count == 0)
                {
                    builder.SetupLevel(9, 10);
                    Ball.ResetBall();
                }

                Window.DispatchEvents();

                // Display objects
                Window.Clear(new Color((byte) 50, (byte) 50, (byte) 180));
                Window.Draw(Ball);
                Window.Draw(Paddle);

                foreach (Brick b in Bricks)
                {
                    Window.Draw(b);
                }

                Window.Display();
            }
        }

        /// <summary>
        /// Function called when the window is closed
        /// </summary>
        private static void OnClosed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow) sender;
            window.Close();
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow) sender;
            if (e.Code == Keyboard.Key.Escape)
                window.Close();
        }
    }
}