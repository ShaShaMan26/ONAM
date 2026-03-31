using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Mikuling : GameElement
{
    private Random r;

    private Texture2D overlay;
    private Vector2 basePos, offset;
    private double counter, prevCounter, prevCounter2;
    private int intensity;

    public float vibrationDelay, shadow, haze;
    public bool attacking;

    public Mikuling() : base("mikuling") { }

    public void Initialize()
    {
        r = new();
        basePos = pos;
        overlay = Global.content.Load<Texture2D>("shadow-mikuling");
        Reset();
    }
    
    public void Update()
    {
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;

        if (counter >= 10)
        {
            attacking = true;
            intensity = 3;
        }
        else if (counter >= 5 && intensity < 2)
        {
            intensity = 2;
        }

        if (counter - prevCounter2 >= .5)
        {
            vibrationDelay -= 0.01f;
            prevCounter2 = counter;
        }
        
        if (counter - prevCounter >= vibrationDelay)
        {
            if (intensity < 2) 
                offset = new Vector2(r.Next(-1, 2), 0);
            else if (intensity < 3)
                if (r.Next(0, 2) > 0) offset = new Vector2(r.Next(-1, 2), 0);
                else offset = new Vector2(0, r.Next(-1, 2));
            else if (intensity < 4)
                offset = new Vector2(r.Next(-1, 2), r.Next(-1, 2));

            SetPosition(basePos + (offset * intensity));
            // haze += 0.1f;
            prevCounter = counter;
        }
    }

    public void Reset()
    {
        SetPosition(basePos);
        vibrationDelay = 0.16f;
        intensity = 1;
        haze = 0;
        counter = 0;
        prevCounter = 0;
        prevCounter2 = 0;
        attacking = false;
    }

    public void Draw(Vector2 basePos)
    {
        
        if (texture != null && visible)
        {
            Global.spriteBatch.Draw(texture, new Rectangle((basePos + pos).ToPoint(), dims.ToPoint()), null, color * opacity, rotation, Vector2.Zero, SpriteEffects.None, 0);
            Global.spriteBatch.Draw(overlay, new Rectangle((basePos + pos).ToPoint(), dims.ToPoint()), null, Color.Black * shadow, rotation, Vector2.Zero, SpriteEffects.None, 0);
            Global.spriteBatch.Draw(overlay, new Rectangle((basePos + pos).ToPoint(), dims.ToPoint()), null, Color.Red * haze, rotation, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
