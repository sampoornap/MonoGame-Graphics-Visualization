using System.Net.Mime;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public class Spot {
    public Vector2 Position
    {
        get;
        set;
    } = Vector2.Zero;
    
    public Vector2 Speed { get; set; } =  Vector2.Zero;

    public Texture2D Texture
    {
        get;
        set;
    }
    

    public Spot(Texture2D tex, Vector2 startPos, Vector2 speed)
    {
        Texture = tex;
        Position = startPos;
        Speed = speed;
    }

    public void Move(GameTime gameTime)
    {
        float dt =  (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        Position += Speed * dt;
    }

    public void Display(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }

}