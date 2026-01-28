using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HandsOnCreatingTint;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _img;
    private Color[] _imgBuffer;
    private Color[] _originalImgBuffer;

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


        _img = Content.Load<Texture2D>("images/fireworks");

        _graphics.PreferredBackBufferHeight = _img.Height;
        _graphics.PreferredBackBufferWidth = _img.Width;
        
        _graphics.ApplyChanges();

        _imgBuffer = new Color[_img.Width * _img.Height];
        _img.GetData(_imgBuffer);
        
        _originalImgBuffer = new Color[_img.Width * _img.Height];
        _img.GetData(_originalImgBuffer);

        // TODO: use this.Content to load your game content here
    }

    private void ApplyTint(Texture2D texture, Color tint)
    {
        for (int i = 0; i < _img.Width; i++)
        {
            for (int j = 0; j < _img.Height; j++)
            {
                int index = i + j * _img.Width;
                Color c = _originalImgBuffer[index];
                
                _imgBuffer[index] = new Color(
                    (byte)(c.R * tint.R /255), 
                (byte)(c.G * tint.G / 255),
                    (byte)(c.B * tint.B / 255), 
                        (byte)(c.A * tint.A / 255));
            }
        }
        
        texture.SetData(_imgBuffer);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        var keyboard = Keyboard.GetState();
        
        if(keyboard.IsKeyDown(Keys.R))
            ApplyTint(_img, Color.Red);
        
        if(keyboard.IsKeyDown(Keys.G))
            ApplyTint(_img, Color.Green);
        
        if(keyboard.IsKeyDown(Keys.B))
            ApplyTint(_img, Color.Blue);
        
        if(keyboard.IsKeyDown(Keys.W))
            ApplyTint(_img, Color.White);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin();
        _spriteBatch.Draw(_img, Vector2.Zero, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}