using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLPong
{
    internal class Program
    {
        public static RenderWindow Window;
        public static Ball Ball;
        public static Paddle Player1Paddle;
        public static Paddle Player2Paddle;

        public static uint Player1Score = 0;
        public static uint Player2Score = 0;

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
            Player1Paddle = new PlayerPaddle(new Vector2f(32, Window.Size.Y / 2 - 48));
            Player2Paddle = new AIPaddle(new Vector2f(Window.Size.X - 48, Window.Size.Y / 2 - 48));
            Text Score1 = new Text("0", new Font(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts) + "/arial.ttf"), 24);
            Score1.Position = new Vector2f(100, 20);
            Text Score2 = new Text("0", new Font(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts) + "/arial.ttf"), 24);
            Score2.Position = new Vector2f(Window.Size.X - 100 - Score2.GetLocalBounds().Width, 20);

            // Put the ball in the middle of the screen
            Ball.Position = ((Vector2f) Window.Size * 0.5f) - new Vector2f(Ball.Radius, Ball.Radius);

            // Set up timing
            Clock clock = new Clock();
            float delta = 0.0f;

            // Game loop
            while (Window.IsOpen)
            {
                // Update objects
                delta = clock.Restart().AsSeconds();
                Ball.Update(delta);
                Player1Paddle.Update(delta);
                Player2Paddle.Update(delta);

                Score1.DisplayedString = Player1Score.ToString();
                Score2.DisplayedString = Player2Score.ToString();

                Window.DispatchEvents();

                // Display objects
                Window.Clear(new Color((byte) 50, (byte) 50, (byte) 180));
                Window.Draw(Ball);
                Window.Draw(Player1Paddle);
                Window.Draw(Player2Paddle);
                Window.Draw(Score1);
                Window.Draw(Score2);
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