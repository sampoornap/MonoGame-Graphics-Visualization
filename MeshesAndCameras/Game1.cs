using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeshesAndCameras;
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
 * specularity - highlights
 */
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Model _teapot;
    private float aspectRatio;
    private Matrix projection;
    private Matrix view;
    private Matrix WorldRotation;
    private float rotationY;
    
    private float lightTime = 0f;
    
    private MouseState _prevMouse;

    private Vector3 cameraTarget = Vector3.Zero;  // the teapot
    private float yaw = 0f;       // rotate around Y axis (left-right)
    private float pitch = 0.3f;   // rotate up/down
    private float radius = 400f;  // distance from object
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        aspectRatio = GraphicsDevice.Viewport.AspectRatio;
        projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10000.0f);
        view = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, 2500), Vector3.Zero, Vector3.Up);

        rotationY = 0.0f;
        _prevMouse = Mouse.GetState();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _teapot = Content.Load<Model>("teapot");
        
        System.Console.WriteLine(_teapot == null ? "TEAPOT NULL" : "TEAPOT LOADED");
    
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        rotationY += gameTime.ElapsedGameTime.Milliseconds * 0.005f;
        WorldRotation = Matrix.CreateRotationY(rotationY);
        
        MouseState ms = Mouse.GetState();
        
        lightTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

// Only rotate while left mouse is held
        if (ms.LeftButton == ButtonState.Pressed && _prevMouse.LeftButton == ButtonState.Pressed)
        {
            int dx = ms.X - _prevMouse.X;
            int dy = ms.Y - _prevMouse.Y;

            float sensitivity = 0.01f;

            yaw   -= dx * sensitivity;
            pitch -= dy * sensitivity;

            // prevent flipping upside-down
            pitch = MathHelper.Clamp(pitch, -1.4f, 1.4f);
        }

// Scroll wheel = zoom
        int scroll = ms.ScrollWheelValue - _prevMouse.ScrollWheelValue;
        radius -= scroll * 0.2f;
        radius = MathHelper.Clamp(radius, 100f, 2000f);

// --- THIS IS THE IMPORTANT PART ---
// Convert yaw + pitch + radius into a camera position

        Vector3 cameraOffset =
            Vector3.Transform(
                new Vector3(0, 0, radius),
                Matrix.CreateRotationX(pitch) *
                Matrix.CreateRotationY(yaw)
            );

        Vector3 cameraPosition = cameraTarget + cameraOffset;

// rebuild view matrix every frame
        view = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);

        _prevMouse = ms;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        DrawMesh(_teapot, new Vector3(0, 0, 0));
        DrawMesh(_teapot, new Vector3(400, 0, 0));
        

        base.Draw(gameTime);
    }
    
    private void DrawMesh(Model m, Vector3 position)
    {
        if (m == null) return;

        Matrix[] transforms = new Matrix[m.Bones.Count];
        m.CopyAbsoluteBoneTransformsTo(transforms);

        foreach (ModelMesh mesh in m.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.LightingEnabled = true;

                if (effect.LightingEnabled)
                {
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0f, 0f);
                    effect.DirectionalLight0.SpecularColor = new Vector3(0f, 1f, 0f);

                    effect.DirectionalLight0.Direction = new Vector3(
                        (float)Math.Cos(lightTime),
                        -1f,
                        (float)Math.Sin(lightTime));
                    
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight1.DiffuseColor = new Vector3(0f, 0f, 1f);
                    effect.DirectionalLight1.SpecularColor = new Vector3(1f, 1f, 1f);

                    effect.DirectionalLight0.Direction = new Vector3(1f, -0.5f, -1f);
                    
                    float halfTime = lightTime * 0.5f;

                    effect.EmissiveColor = new Vector3(
                        (float)Math.Abs(Math.Sin(halfTime)), 0.5f, (float)Math.Abs(Math.Sin(halfTime)));




                }
            
                effect.View = view;
                effect.Projection = projection;

                // effect.EnableDefaultLighting();
                effect.PreferPerPixelLighting = true;
                // effect.DiffuseColor = Vector3.One;
                // effect.AmbientLightColor = new Vector3(0.5f);

                var world =
                    Matrix.CreateScale(200f) *
                    WorldRotation *
                    Matrix.CreateTranslation(position);

                effect.World = world * transforms[mesh.ParentBone.Index];
            }

            mesh.Draw();
        }
    }
}