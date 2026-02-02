using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HandsOnUnderstandingConvolutions;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _img;
    private Color[] originalBuffer;
    private Color[] newBuffer;
    

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

        // TODO: use this.Content to load your game content here
        
        _img = Content.Load<Texture2D>("images/fireworks");
        originalBuffer = new Color[_img.Width * _img.Height];
        _img.GetData(originalBuffer);
        
        newBuffer = new Color[_img.Width * _img.Height];
        _img.GetData(newBuffer);
        
        _graphics.PreferredBackBufferWidth = _img.Width;
        _graphics.PreferredBackBufferHeight = _img.Height;
        _graphics.ApplyChanges();
        
        float[][] myKernel = new float[3][];

        myKernel[0] = [0.0f, -1.0f, 0.0f];
        myKernel[1] = [-1.0f, 5.0f, -1.0f];
        myKernel[2] = [0.0f, -1.0f, 0.0f];
        
        ApplyConvolution(myKernel, 3);

    }

    private void ApplyConvolution(float[][] kernel, int dim)
    {
        for (int x = 1; x < _img.Width-1; x++)
        {
            for (int y = 1; y < _img.Height - 1; y++)
            {
                Vector3 finalColor = new Vector3(); // holds RGB as float
                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        int index = (x + i - 1) + _img.Width * (y + j - 1);
                        finalColor += originalBuffer[index].ToVector3() * kernel[i][j]; // RGB in range (0, 1) * kernel value
                    }
                }

                finalColor.X = float.Abs(finalColor.X); //preserves magnitude
                finalColor.Y = float.Abs(finalColor.Y);
                finalColor.Z = float.Abs(finalColor.Z);

                finalColor = Vector3.Clamp(finalColor, Vector3.Zero, new Vector3(1, 1, 1));
                // ensure it is between 0 and 1
                newBuffer[x + y * _img.Width] = new Color(finalColor);
            }
        }
        
        _img.SetData(newBuffer);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        

        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(_img, new Rectangle(0, 0, _img.Width, _img.Height), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}