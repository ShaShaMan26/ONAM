using System;
using Microsoft.Xna.Framework;

namespace ONAM;

public class LoopSplashScreen : Scene
{
    private TextDisplay title;
    private GameElement cover;
    private double counter;
    private bool fading;

    public override void Initialize()
    {
        title = new("Loop " + Global.userData.loop, "fnaf");
        title.MapBoundsToTextSize();
        title.SetPosition(Global.renderTarget.Width / 2 - title.GetWidth() / 2,
            Global.renderTarget.Height / 2 - title.GetHeight() / 2);

        cover = new("office");
        cover.color = Color.Black;
        cover.opacity = 0;

        counter = 0;
        fading = false;
    }

    public override void OnStart()
    {
        
    }

    public override Scene Update()
    {
        if (fading)
        {
            if (cover.opacity > .95)
            {
                LoadNight l = new();
                l.Initialize();
                return l;
            }
            cover.opacity += (float) Math.Clamp(Global.gameTime.ElapsedGameTime.TotalSeconds, 0, 1);
        }
        else
        {
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter > 2 || MouseManager.LeftButtonClicked)
            {
                fading = true;
            }
        }
        return null;
    }

    public override void Draw()
    {
        title.Draw();
        cover.Draw();
    }
}
