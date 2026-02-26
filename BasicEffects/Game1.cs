using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BasicEffects;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    //Matrices for 3D perspective
    private Matrix worldMatrix, viewMatrix, projectionMatrix;

    // Vertex data for rendering
    private VertexPositionColor[] triangleVertices;

    // A Vertex format structure that contains position, normal data, and one set of texture coordinates
    private BasicEffect basicEffect;
    
    // Wire polygons (LineStrip)
    private VertexPositionColor[] _poly1;
    private VertexPositionColor[] _poly2;

  // Filled triangles (TriangleList)
    private VertexPositionColor[] _triangles;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        // Setup the matrices to look forward
        worldMatrix = Matrix.Identity;
        viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 50), Vector3.Zero, Vector3.Up);

        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            GraphicsDevice.Viewport.AspectRatio,
            1.0f, 300.0f);
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        
        basicEffect = new BasicEffect(_graphics.GraphicsDevice);

        basicEffect.World = worldMatrix;
        basicEffect.View = viewMatrix;
        basicEffect.Projection = projectionMatrix;


        // primitive color
        basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
        basicEffect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
        basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
        basicEffect.SpecularPower = 5.0f;
        basicEffect.Alpha = 1.0f;
        // The following MUST be enabled if you want to color your vertices
        basicEffect.VertexColorEnabled = true;

        // Use the built in 3 lighting mode provided with BasicEffect            
        // basicEffect.EnableDefaultLighting();
        basicEffect.LightingEnabled = false;
        
        // -------- Polygon 1: square outline (LineStrip) --------
// NOTE: last vertex repeats first to close the shape
        _poly1 = new VertexPositionColor[]
        {
            new(new Vector3(-15f,  10f, 0f), Color.Red),
            new(new Vector3(-5f,   10f, 0f), Color.Yellow),
            new(new Vector3(-5f,    0f, 0f), Color.Green),
            new(new Vector3(-15f,   0f, 0f), Color.Cyan),
            new(new Vector3(-15f,  10f, 0f), Color.Red), // close
        };

// -------- Polygon 2: pentagon outline (LineStrip) --------
        _poly2 = new VertexPositionColor[]
        {
            new(new Vector3(  8f,  10f, 0f), Color.Magenta),
            new(new Vector3( 15f,   6f, 0f), Color.Orange),
            new(new Vector3( 12f,  -2f, 0f), Color.Lime),
            new(new Vector3(  4f,  -2f, 0f), Color.Blue),
            new(new Vector3(  1f,   6f, 0f), Color.White),
            new(new Vector3(  8f,  10f, 0f), Color.Magenta), // close
        };

// -------- Two triangles (TriangleList) --------
// 6 vertices = 2 triangles
        _triangles = new VertexPositionColor[]
        {
            // Triangle 1 (near origin)
            new(new Vector3(-6f, -10f, 0f), Color.Red),
            new(new Vector3( 0f,  -2f, 0f), Color.Yellow),
            new(new Vector3( 6f, -10f, 0f), Color.Green),

            // Triangle 2 (shifted right & slightly back in Z)
            new(new Vector3(10f, -10f, -5f), Color.Cyan),
            new(new Vector3(16f,  -2f, -5f), Color.Magenta),
            new(new Vector3(22f, -10f, -5f), Color.White),
        };

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

        // Optional but helpful: see both sides regardless of winding
        GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };

        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();

            // ---- LineStrip polygon 1 ----
            GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.LineStrip,
                _poly1,
                0,
                _poly1.Length - 1 // primitives = vertices - 1
            );

            // ---- LineStrip polygon 2 ----
            GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.LineStrip,
                _poly2,
                0,
                _poly2.Length - 1
            );

            // ---- TriangleList (2 triangles) ----
            GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.TriangleList,
                _triangles,
                0,
                2 // 2 triangles
            );
        }

        base.Draw(gameTime);
    }
}