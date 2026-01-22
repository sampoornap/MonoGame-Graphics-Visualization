using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UsingCoordinates;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _firework;
    private Vector2 position;
    private Color c;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        position = new Vector2(0.0f, 0.0f);
        c = new Color(1.0f, 1.0f, 0.0f, 0.5f);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    
        // TODO: use this.Content to load your game content here
        _firework = Content.Load<Texture2D>("images/fireworks");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        position.X++;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(_firework, 
            position,
            sourceRectangle:null,
            color:c, 
            rotation: 0.0f, 
            origin: Vector2.Zero,
            scale: 0.3f,
            SpriteEffects.None,
            layerDepth:0.0f);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}