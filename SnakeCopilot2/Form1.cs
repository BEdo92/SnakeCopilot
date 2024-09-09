namespace SnakeCopilot2;

partial class Form1 : Form
{
    private readonly List<Point> snake = [];
    private Point food = new();
    private Direction direction = Direction.Right;
    private readonly System.Windows.Forms.Timer gameTimer = new();
    private readonly Random random = new();
    private int score = 0;
    private int speed = 100;

    public Form1()
    {
        InitializeComponent();
        InitializeGame();
    }

    /// <summary>
    /// Sets up the game, including key event handling and starting the timer.
    /// </summary>
    private void InitializeGame()
    {
        this.KeyDown += new KeyEventHandler(OnKeyPress);
        gameTimer.Interval = speed;
        gameTimer.Tick += new EventHandler(UpdateScreen);
        gameTimer.Start();
        StartNewGame();
    }

    /// <summary>
    /// Resets the snake, score, and generates the first food.
    /// </summary>
    private void StartNewGame()
    {
        snake.Clear();
        snake.Add(new Point(10, 10));
        direction = Direction.Right;
        score = 0;
        GenerateFood();
    }

    /// <summary>
    /// Places food at a random location.
    /// </summary>
    private void GenerateFood()
    {
        food = new Point(random.Next(0, panel1.Width / 10), random.Next(0, panel1.Height / 10));
    }

    /// <summary>
    /// Moves the snake and checks for collisions.
    /// </summary>
    private void UpdateScreen(object sender, EventArgs e)
    {
        MoveSnake();
        CheckCollision();
        panel1.Invalidate();
    }

    /// <summary>
    /// Moves the snake in the current direction.
    /// </summary>
    private void MoveSnake()
    {
        Point head = snake[0];
        Point newHead = head;

        switch (direction)
        {
            case Direction.Right: newHead.X += 1; break;
            case Direction.Down: newHead.Y += 1; break;
            case Direction.Left: newHead.X -= 1; break;
            case Direction.Up: newHead.Y -= 1; break;
        }

        snake.Insert(0, newHead);
        if (newHead == food)
        {
            score++;
            GenerateFood();
            if (score % 5 == 0)
            {
                speed -= 10;
                gameTimer.Interval = speed;
            }
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }

    /// <summary>
    /// Checks for collisions with walls or the snake itself.
    /// </summary>
    private void CheckCollision()
    {
        Point head = snake[0];

        if (IsCollidedWithWall(head))
        {
            StartNewGame();
        }

        for (int i = 1; i < snake.Count; i++)
        {
            if (snake[i] == head)
            {
                StartNewGame();
            }
        }
    }

    /// <summary>
    /// Changes the snake's direction based on user input.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnKeyPress(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Right: if (direction != Direction.Left) direction = Direction.Right; break;
            case Keys.Down: if (direction != Direction.Up) direction = Direction.Down; break;
            case Keys.Left: if (direction != Direction.Right) direction = Direction.Left; break;
            case Keys.Up: if (direction != Direction.Down) direction = Direction.Up; break;
        }
    }

    /// <summary>
    /// Draws the snake and food on the panel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void panel1_Paint(object sender, PaintEventArgs e)
    {
        Graphics canvas = e.Graphics;

        for (int i = 0; i < snake.Count; i++)
        {
            Brush snakeColor = i == 0 ? Brushes.Black : Brushes.Green;
            canvas.FillRectangle(snakeColor, new Rectangle(snake[i].X * 10, snake[i].Y * 10, 10, 10));
        }

        canvas.FillRectangle(Brushes.Red, new Rectangle(food.X * 10, food.Y * 10, 10, 10));

        // Draw score
        string scoreText = $"Score: {score}";
        Font scoreFont = new("Comic Sans", 14, FontStyle.Bold, GraphicsUnit.World);
        Brush scoreColor = Brushes.Black;
        canvas.DrawString(scoreText, scoreFont, scoreColor, new PointF(10, 10));
    }

    private bool IsCollidedWithWall(Point head)
    {
        return head.X < 0 || head.Y < 0 || head.X >= panel1.Width / 10 || head.Y >= panel1.Height / 10;
    }
}
