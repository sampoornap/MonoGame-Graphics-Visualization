using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Typography;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private string _text = "";
    private int _cursorIndex = 0;
    
    private KeyboardState _lastKeyboardState;
    
    private Vector2 _origin = new Vector2(10, 10);
    private int _maxPixelWidth;
    

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
        
        _font = Content.Load<SpriteFont>("Fonts/NewFont");
        // s = "Hello World";
        //
        // Vector2 textSize = _font.MeasureString(s); //contains width and height of the rendered text
        // Console.WriteLine(textSize);
        
        _maxPixelWidth = _graphics.PreferredBackBufferWidth-20;

    }

    private bool IsNewKeyPress(Keys k, KeyboardState kb)
    {
        return (kb.IsKeyDown(k) && _lastKeyboardState.IsKeyUp(k));
    }

    private char? KeyToChar(Keys key, bool shift)
    {
        if (key >= Keys.A && key <= Keys.Z)
        {
            char c = (char)('a' + (key -  Keys.A));
            return shift ? char.ToUpperInvariant(c) : c;
        }
        
        if (key >= Keys.D0 && key <= Keys.D9)
        {
            string normal = "0123456789";
            string shifted = ")!@#$%^&*(";
            int i = key - Keys.D0;
            return shift ? shifted[i] : normal[i];
        }
        return null;
    }

    private void InsertChar(char c)
    {
        _text = _text.Insert(_cursorIndex, c.ToString());
        _cursorIndex++;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        var kb = Keyboard.GetState();
        
        bool shift = kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift);
        

        // Special keys
        if (IsNewKeyPress(Keys.Enter, kb))
        {
            InsertChar('\n'); // carriage return / newline
        }
        if (IsNewKeyPress(Keys.Back, kb))
        {
            if (_cursorIndex > 0)
            {
                _text = _text.Remove(_cursorIndex - 1, 1);
                _cursorIndex--;
            }
        }
        if (IsNewKeyPress(Keys.Delete, kb))
        {
            if (_cursorIndex < _text.Length)
                _text = _text.Remove(_cursorIndex, 1);
        }

        // Cursor movement (basic)
        if (IsNewKeyPress(Keys.Left, kb))
            _cursorIndex = Math.Max(0, _cursorIndex - 1);
        if (IsNewKeyPress(Keys.Right, kb))
            _cursorIndex = Math.Min(_text.Length, _cursorIndex + 1);

        // Character keys: loop through newly pressed keys
        foreach (var key in kb.GetPressedKeys())
        {
            if (!IsNewKeyPress(key, kb)) continue;

            // skip keys we already handled
            if (key is Keys.Enter or Keys.Back or Keys.Delete or Keys.Left or Keys.Right
                or Keys.LeftShift or Keys.RightShift or Keys.LeftControl or Keys.RightControl
                or Keys.LeftAlt or Keys.RightAlt or Keys.Tab)
                continue;

            var ch = KeyToChar(key, shift);
            if (ch.HasValue)
                InsertChar(ch.Value);
        }

        _lastKeyboardState = kb;
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, _text, Vector2.Zero, Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}