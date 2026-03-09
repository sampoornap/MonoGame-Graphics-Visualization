using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameOfLife;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D pixel;

    int cols = 80;
    int rows = 60;
    int cellSize = 10;

    int[,] current;
    int[,] next;

    bool running = false;
    KeyboardState prevKey;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        current = new int[cols, rows];
        next = new int[cols, rows];

        Random rand = new Random();

        // random initial board
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                current[x, y] = rand.Next(2); // 0 or 1
            }
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // simple white texture for drawing cells
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState key = Keyboard.GetState();

        if (key.IsKeyDown(Keys.Escape))
            Exit();

        // SPACE = toggle simulation
        if (key.IsKeyDown(Keys.Space) && prevKey.IsKeyUp(Keys.Space))
            running = !running;

        // N = single step
        if (key.IsKeyDown(Keys.N) && prevKey.IsKeyUp(Keys.N))
            StepSimulation();

        // C = clear board
        if (key.IsKeyDown(Keys.C) && prevKey.IsKeyUp(Keys.C))
        {
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    current[x, y] = 0;
        }

        // mouse click toggles cell
        MouseState mouse = Mouse.GetState();
        if (mouse.LeftButton == ButtonState.Pressed)
        {
            int gridX = mouse.X / cellSize;
            int gridY = mouse.Y / cellSize;

            if (gridX >= 0 && gridX < cols && gridY >= 0 && gridY < rows)
                current[gridX, gridY] = 1;
        }

        if (mouse.RightButton == ButtonState.Pressed)
        {
            int gridX = mouse.X / cellSize;
            int gridY = mouse.Y / cellSize;

            if (gridX >= 0 && gridX < cols && gridY >= 0 && gridY < rows)
                current[gridX, gridY] = 0;
        }

        if (running)
            StepSimulation();

        prevKey = key;

        base.Update(gameTime);
    }

    void StepSimulation()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                int neighbors = CountNeighbors(x, y);

                if (current[x, y] == 1)
                {
                    if (neighbors < 2 || neighbors > 3)
                        next[x, y] = 0;
                    else
                        next[x, y] = 1;
                }
                else
                {
                    if (neighbors == 3)
                        next[x, y] = 1;
                    else
                        next[x, y] = 0;
                }
            }
        }

        // swap arrays
        var temp = current;
        current = next;
        next = temp;
    }

    int CountNeighbors(int x, int y)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < cols && ny >= 0 && ny < rows)
                    count += current[nx, ny];
            }
        }

        return count;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (current[x, y] == 1)
                {
                    _spriteBatch.Draw(
                        pixel,
                        new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize),
                        Color.White);
                }
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}