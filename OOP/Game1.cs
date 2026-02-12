using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OOP;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Spot _spot1;
    private Spot _spot2;
    private Texture2D _spotTexture;
    private TwoSpot _doubleSpot;

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

        _spotTexture = new Texture2D(GraphicsDevice, 20, 20);

        Color[] data = new Color[20 * 20];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Color.White;
        }
        
        _spotTexture.SetData(data);
        
        _spot1 = new Spot(_spotTexture, new Vector2(50, 100), new Vector2(120, 0));
        _spot2 = new Spot(_spotTexture, new Vector2(400, 300), new Vector2(-60, 80));
        
        _doubleSpot = new TwoSpot(_spotTexture, new Vector2(50, 100), new Vector2(120, 0));
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _spot1.Move(gameTime);
        _spot2.Move(gameTime);

        _doubleSpot.Move(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin();
        
        _spot1.Display(_spriteBatch);
        _spot2.Display(_spriteBatch);
        _doubleSpot.Display(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}