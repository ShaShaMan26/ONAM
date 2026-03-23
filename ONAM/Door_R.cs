using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Door_R : GameElement
{
    public bool closing, opening, open;
    private Texture2D[] frames;
    private double counter;
    private int i;

    public Door_R() : base("ani_door_r/0")
    {
        open = true;
        counter = 0;
        i = 0;

        frames = new Texture2D[16];
        for (int i = 0; i < frames.Length; i ++)
        {
            frames[i] = Global.content.Load<Texture2D>("ani_door_r/" + i);
        }
    }

    public void Toggle()
    {
        if (!closing && !opening)
        {   
            if (open)
            {
                closing = true;
            }
            else
            {
                opening = true;
            }
        }
    }

    public void Update()
    {
        if (opening)
        {
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter > .017 || i == frames.Length)
            {
                SetTexture(frames[i]);
                i--;
                counter = 0;
                if(i < 0)
                {
                    i = 0;
                    open = true;
                    opening = false;
                }
            }
        }
        else if (closing)
        {
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter > .017 || i == 0)
            {
                SetTexture(frames[i]);
                i++;
                counter = 0;
                if(i >= frames.Length)
                {
                    i = frames.Length - 1;
                    open = false;
                    closing = false;
                }
            }
        }
    }
}
