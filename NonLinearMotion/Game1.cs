using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NonLinearMotion;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Textures
    private Texture2D _squareTex;
    private Texture2D _ballTex;

    // 1) Oscillation (sin) sprite
    private Vector2 _oscAnchor;
    private Vector2 _oscPos;

    // 2) Circle (sin+cos) ball
    private Vector2 _circleCenter;
    private Vector2 _circlePos;

    // 3) Ease-out (lerp with easing function)
    private Vector2 _easeOutStart, _easeOutEnd, _easeOutPos;
    private float _easeOutElapsed = 0f;
    private float _easeOutDuration = 2.0f; // seconds

    // 4) Ease-in (lerp with distance-based progress)
    private Vector2 _easeInStart, _easeInEnd;
    private Vector2 _easeInRawPos;   // moves at constant step
    private Vector2 _easeInPos;      // eased position (what we draw)
    private float _easeInStep = 3f;  // pixels per Update call

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Make simple textures (no Content pipeline required)
        _squareTex = MakeSolidTexture(30, 30, Color.White);
        _ballTex = MakeSolidTexture(18, 18, Color.White);

        // 1) Oscillation setup
        _oscAnchor = new Vector2(200, 120);
        _oscPos = _oscAnchor;

        // 2) Circle setup
        _circleCenter = new Vector2(200, 300);
        _circlePos = _circleCenter;

        // 3) Ease-out setup
        _easeOutStart = new Vector2(420, 120);
        _easeOutEnd = new Vector2(720, 120);
        _easeOutPos = _easeOutStart;

        // 4) Ease-in setup
        _easeInStart = new Vector2(420, 300);
        _easeInEnd = new Vector2(720, 360);
        _easeInRawPos = _easeInStart;
        _easeInPos = _easeInStart;
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float t = (float)gameTime.TotalGameTime.TotalSeconds;

        // ------------------------------------------------------------
        // 1) Oscillate back & forth using Sin
        // ------------------------------------------------------------
        _oscPos = OscillateX(_oscAnchor, amplitudePx: 120f, frequencyHz: 0.5f, timeSeconds: t);

        // ------------------------------------------------------------
        // 2) Circle around a point using Cos + Sin
        // ------------------------------------------------------------
        _circlePos = Circle(_circleCenter, radiusPx: 140f, frequencyHz: 0.4f, timeSeconds: t);
        // ------------------------------------------------------------
        // 3) Ease-out using Lerp (time-based)
        // ------------------------------------------------------------
        _easeOutElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        float outT01 = _easeOutElapsed / _easeOutDuration;

        _easeOutPos = EaseOutLerp(_easeOutStart, _easeOutEnd, outT01);

        // loop / ping-pong
        if (outT01 >= 1f)
        {
            _easeOutElapsed = 0f;
            (_easeOutStart, _easeOutEnd) = (_easeOutEnd, _easeOutStart);
        }

        // ------------------------------------------------------------
        // 4) Ease-in using Lerp (distance-based progress)
        //    Track total distance and current distance to end.
        // ------------------------------------------------------------
        _easeInRawPos = MoveStep(_easeInRawPos, _easeInEnd, _easeInStep);
        _easeInPos = EaseInLerpByDistance(_easeInStart, _easeInEnd, _easeInRawPos);

        // loop / ping-pong once we reach end
        if (_easeInRawPos == _easeInEnd)
        {
            (_easeInStart, _easeInEnd) = (_easeInEnd, _easeInStart);
            _easeInRawPos = _easeInStart;
            _easeInPos = _easeInStart;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Labels (optional: remove if you don't want text; requires SpriteFont otherwise)
        // We'll just draw objects without text.

        // 1) Oscillation (red)
        _spriteBatch.Draw(_squareTex, _oscPos, Color.Red);
        _spriteBatch.Draw(_ballTex, _circleCenter, null, Color.Black, 0f,
            new Vector2(_ballTex.Width / 2f, _ballTex.Height / 2f), 0.2f, SpriteEffects.None, 0f);

        // 2) Circle (yellow)
        Vector2 ballOrigin = new Vector2(_ballTex.Width / 2f, _ballTex.Height / 2f);
        _spriteBatch.Draw(_ballTex, _circlePos, null, Color.Yellow, 0f, ballOrigin, 1f, SpriteEffects.None, 0f);

        // 3) Ease-out (green)
        _spriteBatch.Draw(_squareTex, _easeOutPos, Color.LimeGreen);

        // 4) Ease-in (magenta)
        _spriteBatch.Draw(_squareTex, _easeInPos, Color.Magenta);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    // ===================== Helpers =====================

    private Texture2D MakeSolidTexture(int w, int h, Color c)
    {
        var tex = new Texture2D(GraphicsDevice, w, h);
        var data = new Color[w * h];
        for (int i = 0; i < data.Length; i++) data[i] = c;
        tex.SetData(data);
        return tex;
    }

    // (1) Oscillate on X axis using sin
    private Vector2 OscillateX(Vector2 center, float amplitudePx, float frequencyHz, float timeSeconds)
    {
        float xOffset = amplitudePx * MathF.Sin(2f * MathF.PI * frequencyHz * timeSeconds);
        return new Vector2(center.X + xOffset, center.Y);
    }

    // (2) Circle using cos/sin
    private Vector2 Circle(Vector2 center, float radiusPx, float frequencyHz, float timeSeconds)
    {
        float angle = 2f * MathF.PI * frequencyHz * timeSeconds;
        float x = center.X + radiusPx * MathF.Cos(angle);
        float y = center.Y + radiusPx * MathF.Sin(angle);
        return new Vector2(x, y);
    }

    // Move toward end by fixed pixels per Update
    private Vector2 MoveStep(Vector2 current, Vector2 end, float step)
    {
        Vector2 delta = end - current;
        float dist = delta.Length();

        if (dist <= step || dist == 0f)
            return end;

        return current + delta / dist * step;
    }

    // (3) Ease-out curve + Lerp
    private Vector2 EaseOutLerp(Vector2 start, Vector2 end, float t01)
    {
        t01 = MathHelper.Clamp(t01, 0f, 1f);
        float eased = 1f - (1f - t01) * (1f - t01); // quadratic ease-out
        return Vector2.Lerp(start, end, eased);
    }

    // (4) Ease-in using distance-based progress + Lerp
    private Vector2 EaseInLerpByDistance(Vector2 start, Vector2 end, Vector2 current)
    {
        float totalDist = Vector2.Distance(start, end);
        if (totalDist <= 1e-6f) return end;

        float currDist = Vector2.Distance(current, end);

        // progress: 0 at start, 1 at end
        float progress = 1f - (currDist / totalDist);
        progress = MathHelper.Clamp(progress, 0f, 1f);

        // quadratic ease-in
        float eased = progress * progress;

        return Vector2.Lerp(start, end, eased);
    }
}