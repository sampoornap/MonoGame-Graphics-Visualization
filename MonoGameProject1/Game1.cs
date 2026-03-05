//Anika Rajkumar, anr3844
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace MonoGameProject1;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    List<Particle> particles = new List<Particle>();
    Random rand = new Random();
    int spawnPerFrame = 2;   
    int maxParticles = 300;  
    Texture2D particleTexture;
    float accel = 0.03f;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        int size = 10; // particle size
        particleTexture = new Texture2D(GraphicsDevice, size, size);

        Color[] data = new Color[size * size];

        for (int i = 0; i < data.Length; i++)
            data[i] = Color.White;   // white square

        particleTexture.SetData(data);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float ex = Window.ClientBounds.Width / 2f;
        float ey = -20f;

        for (int i = 0; i < spawnPerFrame; i++)
        {
            if (particles.Count < maxParticles)
            {
                float vx = (float)(rand.NextDouble() * 6 - 3);   
                float vy = (float)(-(rand.NextDouble() * 6 + 6)); 

                particles.Add(new Particle(ex, ey, vx, vy, particleTexture));
            }
        }

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].applyForces(0, accel); 

            particles[i].bounce(Window.ClientBounds.Width, Window.ClientBounds.Height);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].display(_spriteBatch);
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}

public class Particle
{
    public Vector3 position;
    public Vector3 velocity;
    public Texture2D sprite;

    public Particle(float x, float y, float vx, float vy, Texture2D tex)
    {
        position = new Vector3(x, y, 0);
        velocity = new Vector3(vx, vy, 0);
        sprite = tex;
    }

    public void applyForces(float fx, float fy) // from slides
    {
        velocity.X += fx;
        velocity.Y += fy;

        position.X += velocity.X;
        position.Y += velocity.Y;
    }
    public void bounce(int width, int height)
    {
        // Right wall
        if (position.X > width - sprite.Width)
        {
            position.X = width - sprite.Width;
            velocity.X = -velocity.X;
        }
        // Left wall
        else if (position.X < 0)
        {
            position.X = 0;
            velocity.X = -velocity.X;
        }

        // Bottom wall
        if (position.Y > height - sprite.Height)
        {
            position.Y = height - sprite.Height;
            velocity.Y = -velocity.Y;
        }
        // Top wall 
        else if (position.Y < 0)
        {
            position.Y = 0;
            velocity.Y = -velocity.Y;
        }
    }

    public void display(SpriteBatch sb)
    {
        sb.Draw(
            sprite,
            new Vector2(position.X, position.Y),
            null,
            Color.White,
            0f,
            Vector2.Zero,
            0.2f,             
            SpriteEffects.None,
            0f
        );    }
}