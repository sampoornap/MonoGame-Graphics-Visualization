using System;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Interpolation;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _tex1, _tex2;

// sprite 1: step-move
    private Vector2 _spritePos1;
    private Vector2 _start1, _target1;

// sprite 2: lerp-animate
    private Vector2 _spritePos2;
    private Vector2 _animStart2, _animEnd2;
    private int _animFrame2 = 0;

// matrices (example transforms)
    private Matrix _mat1;
    private Matrix _mat2;

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

    private Vector2 MoveStep(Vector2 current, Vector2 end, float step)
    {
        Vector2 delta = end - current;
        float dist = delta.Length();

        if (dist <= step || dist == 0f)
        {
            return end;
        }
        
        return current + delta / dist * step;
    }

    private Vector2 AnimateLerp(Vector2 start, Vector2 end, float durationSeconds, ref int frameCounter)
    {
        int totalFrames = Math.Max(1, (int)(durationSeconds * 60f));
        float t = MathHelper.Clamp((float)frameCounter / totalFrames, 0f, 1f);
        
        Vector2 pos = Vector2.Lerp(start, end, t);

        if (frameCounter < totalFrames)
            frameCounter++;
        
        return pos;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _tex1 = new Texture2D(GraphicsDevice, 30, 30);
        _tex2 = new Texture2D(GraphicsDevice, 20, 60);

        var d1 = new Color[_tex1.Width * _tex1.Height];
        for (int i = 0; i < d1.Length; i++) d1[i] = Color.White;
        _tex1.SetData(d1);

        var d2 = new Color[_tex2.Width * _tex2.Height];
        for (int i = 0; i < d2.Length; i++) d2[i] = Color.White;
        _tex2.SetData(d2);

// set start/end
        _start1 = new Vector2(50, 80);
        _target1 = new Vector2(600, 80);
        _spritePos1 = _start1;

        _animStart2 = new Vector2(100, 300);
        _animEnd2   = new Vector2(600, 420);
        _spritePos2 = _animStart2;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        // _spritePos1 = MoveStep(_spritePos1, _target1, 3f);
        // _spritePos2 = AnimateLerp(_animStart2, _animEnd2, 2.0f, ref _animFrame2); // 2 sec animation
        // 1) move by fixed step
        _spritePos1 = MoveStep(_spritePos1, _target1, 4f);

// ping-pong for sprite 1
        if (_spritePos1 == _target1)
            (_start1, _target1) = (_target1, _start1);

// 2) animate by lerp over time
        _spritePos2 = AnimateLerp(_animStart2, _animEnd2, 2.0f, ref _animFrame2);

// ping-pong for sprite 2
        if (_spritePos2 == _animEnd2)
        {
            _animFrame2 = 0;
            (_animStart2, _animEnd2) = (_animEnd2, _animStart2);
        }

// 3) matrices for each sprite batch
// Example: camera-like translation or scale
        _mat1 = Matrix.CreateTranslation(new Vector3(0, 0, 0));              // identity-ish
        _mat2 = Matrix.CreateScale(1.0f) * Matrix.CreateTranslation(0, 0, 0); // can change later
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

// Draw sprite 1 with matrix 1
        _spriteBatch.Begin(transformMatrix: _mat1);
        _spriteBatch.Draw(_tex1, _spritePos1, Color.Red);
        _spriteBatch.End();

// Draw sprite 2 with matrix 2 (different transform)
        _spriteBatch.Begin(transformMatrix: _mat2);
        _spriteBatch.Draw(_tex2, _spritePos2, Color.Yellow);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}