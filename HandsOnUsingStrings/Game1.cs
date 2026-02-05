using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HandsOnUsingStrings;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    String _sampleString;
    private String outDir;
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
        
       
        // Console.WriteLine(outputPath);
        using (var stream = TitleContainer.OpenStream("Content/text/textsample.txt"))
        using (var reader = new StreamReader(stream))
        {
            _sampleString = reader.ReadToEnd();
            Console.WriteLine(_sampleString);

            String outputPath = "/Users/sampoornaporia/MonoGame-GraphicsViz/HandsOnUsingStrings/Content/text/textoutput.txt";
            File.WriteAllText(outputPath, _sampleString);
            String extra = Environment.NewLine + "Our additional line" + Environment.NewLine;
            File.AppendAllText(outputPath, extra);
        }
        
        
       
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

        base.Draw(gameTime);
    }
}