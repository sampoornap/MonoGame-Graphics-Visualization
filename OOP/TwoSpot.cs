using System.Net.Mime;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public class TwoSpot: Spot
{
    public float OffsetDistance { get; set; } = 30f;
    
    

    public TwoSpot(Texture2D tex, Vector2 startPos, Vector2 speed) : base(tex, startPos, speed)
    {
    }
    

    public void Display(SpriteBatch spriteBatch)
    {
        Vector2 center = Position;
        
        Vector2 left = new Vector2(center.X - OffsetDistance, center.Y);
        Vector2 right = new Vector2(center.X + OffsetDistance, center.Y);
        
        spriteBatch.Draw(Texture, left, Color.Red);
        spriteBatch.Draw(Texture, right, Color.Green);
    }

}