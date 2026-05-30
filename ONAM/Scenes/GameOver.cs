using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class GameOver : Scene
{
    private Random r;
    private Canvas canvas;
    private GameElement stat;
    private Texture2D[] staticFrames;
    private double counter2;
    private int istatic;
    private SFXObject blip, snd;

    public override void Initialize()
    {
        r = new();

        canvas = new();
        canvas.Initialize();

        counter2 = 0;
        // static
        istatic = 0;
        staticFrames = new Texture2D[8];
        for (int i = 0; i < staticFrames.Length; i++)
        {
            staticFrames[i] = Global.content.Load<Texture2D>("ani_cam_static/" + i);
        }
        stat = new("ani_cam_static/0");
        stat.opacity = .4f;
        canvas.Add(8, stat);

        TextDisplay t0 = new("Press Any Button to Continue", "consolas");
        t0.MapBoundsToTextSize();
        t0.SetPosition(Global.renderTarget.Width / 2 - t0.GetWidth() / 2 + 2,
            Global.renderTarget.Height / 2 - t0.GetHeight() / 2 + 2);
        canvas.Add(9, t0);

        TextDisplay t = new("GAME", "fnaf-big");
        t.MapBoundsToTextSize();
        t.SetPosition(Global.renderTarget.Width / 2 - t.GetWidth() / 2 - 25,
            -385);
        canvas.Add(7, t);

        TextDisplay t2 = new("OVER", "fnaf-big");
        t2.MapBoundsToTextSize();
        t2.SetPosition(Global.renderTarget.Width / 2 - t2.GetWidth() / 2 + 75,
            225);
        canvas.Add(7, t2);

        blip = new(Global.content.Load<SoundEffect>("sfx/cam_switch"));

        snd = new(Global.content.Load<SoundEffect>("sfx/game_over"));
        AudioManager.AddSFX(snd);
    }

    public override Scene Update()
    {
        if (KeyboardManager.PressedKeys.Count != 0 || 
            (MouseManager.WithinWindow && (MouseManager.LeftButtonReleased || MouseManager.RightButtonReleased)))
        {
            snd.Pause();
            AudioManager.RemoveSFX(snd);
            if (Global.runData.loop > 5)
            {
                TransFlicker t = new(Global.summary, true);
                Global.summary.Initialize();
                t.Initialize();
                return t;
            }
            else
            {
                AudioManager.AddSFX(blip);
                TransFlicker t = new(Global.mainMenu);
                t.Initialize();
                Global.mainMenu.Initialize();
                return t;
            }
        }
        
        counter2 += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter2 >= Global.aniDelay * 1.5)
        {
            stat.opacity = (float)(r.NextDouble() * (.5f - .4f) + .4f);
            stat.SetTexture(staticFrames[istatic]);
            istatic++;
            counter2 = 0;
            if(istatic >= staticFrames.Length)
            {
                istatic = 0;
            }
        }
        
        return null;
    }

    public override void Draw()
    {
        canvas.Draw();
    }
}
