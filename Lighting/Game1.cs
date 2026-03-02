using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lighting;
/*
 * intensity and direction od light sources change what surfaces are affected
 * types of basieffects lights - directional, ambient, emissive
 * max intensity (255, 255, 255)
 * 3 directional lights per basiceffects
 * effect.DirectionalLight0.Enabled = true;
 * effect.DirectionalLight0.DiffuseColor = new Vector3(...);
 * effect.DirectionalLight0.Direction = new Vector3(...);
 *
 *
 * ambient - approx many light bounces within a scene
 * one ambinet lighting
 *
 * effect.AmbientLightColor = new Vector3(...);
 *
 * emmisive - like glow stick
 * one emmisive
 * effect.EmmisiveColor
 */
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

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