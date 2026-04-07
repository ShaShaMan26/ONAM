using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Door_L : GameElement
{
    private SFXObject door_move;
    public bool closing, opening, open;
    private Texture2D[] frames;
    private double counter;
    private int i;

    public Door_L() : base("ani_door_l/0")
    {
        door_move = new(Global.content.Load<SoundEffect>("sfx/door_move"));
        door_move.SetPan(-.5f);

        open = true;
        counter = 0;
        i = 0;

        frames = new Texture2D[16];
        for (int i = 0; i < frames.Length; i ++)
        {
            frames[i] = Global.content.Load<Texture2D>("ani_door_l/" + i);
        }
    }

    public void Toggle()
    {
        if (!closing && !opening)
        {   
            if (open)
            {
                closing = true;
                Global.night.office.door_button_l.visible = true;
            }
            else
            {
                opening = true;
                Global.night.office.door_button_l.visible = false;
            }
            AudioManager.AddSFX(door_move);
        }
    }

    public void Update()
    {
        if (opening)
        {
            Global.night.doorClose_L = false;
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter >= Global.aniDelay || i == frames.Length)
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
            if (counter > Global.aniDelay || i == 0)
            {
                SetTexture(frames[i]);
                i++;
                counter = 0;
                if(i >= frames.Length)
                {
                    i = frames.Length - 1;
                    open = false;
                    Global.night.doorClose_L = true;
                    closing = false;
                }
            }
        }
    }
}
