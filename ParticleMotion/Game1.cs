using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ParticleMotion;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D particleTexture;

    private Particle[] particles;

    private int particlecount = 200;
    int screenWidth = 800;
    int screenHeight = 600;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = screenWidth;
        _graphics.PreferredBackBufferHeight = screenHeight;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        particles = new Particle[particlecount];

        Random rand = new Random();

        for (int i = 0; i < particlecount; i++)
        {
            Vector2 startPos = new Vector2(screenWidth / 2, screenHeight / 2);

            Vector2 startVel = new Vector2(
                (float)(rand.NextDouble() * 4 - 2),
                (float)(rand.NextDouble() * 4 - 2));

            particles[i] = new Particle(startPos, startVel);
        
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        
        particleTexture = new Texture2D(GraphicsDevice, 4, 4);
        
        Color[] data  = new Color[16];

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Color.White;
        }
        
        particleTexture.SetData(data);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        Vector2 gravity = new Vector2(0, 0.1f);

        foreach (Particle particle in particles)
        {
            particle.ApplyForce(gravity);
            particle.Update(screenWidth, screenHeight);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        
        foreach (Particle p in particles)
        {
            _spriteBatch.Draw(
                particleTexture,
                p.position,
                Color.White
            );
        }
        
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}




public class Particle
{
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;

    public Particle(Vector2 startPosition, Vector2 startVelocity)
    {
        position = startPosition;
        velocity = startVelocity;
        acceleration = Vector2.Zero;
    }

    public void ApplyForce(Vector2 force)
    {
        acceleration += force;
    }

    public void Update(int screenWidth, int screenHeight)
    {
        // physics update
        velocity += acceleration;
        position += velocity;

        // reset acceleration each frame
        acceleration = Vector2.Zero;

        // bounce off walls
        if (position.X <= 0 || position.X >= screenWidth)
        {
            velocity.X *= -1;
        }

        if (position.Y <= 0 || position.Y >= screenHeight)
        {
            velocity.Y *= -1;
        }
    }
}
