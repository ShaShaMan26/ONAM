using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class GameWin : Scene
{
    private TextDisplay text;
    private string message;
    private int i;
    private bool fading;

    private SFXObject[] chimes;

    public override void Initialize()
    {
        fading = false;
        chimes = new SFXObject[5];
        for (int i = 0; i < chimes.Length; i++)
        {
            chimes[i] = new(Global.content.Load<SoundEffect>("sfx/win_chime" + i));
        }

        message = "6AM";
        i = 0;

        text = new("6 AM", "fnaf-big");
        text.MapBoundsToTextSize();
        text.SetPosition(Global.renderTarget.Width / 2 - text.GetWidth() / 2 + 24, -78);
    }

    public override void OnStart()
    {
        AudioManager.CloseSFXAll();
        AudioManager.PauseBGM();

        AudioManager.PlaySFX(chimes[i]);
        text.Text = "6 ";
    }

    public override Scene Update()
    {
        Global.night.camView.UpdateAnimations();
        Global.night.office.door_L.Update();
        Global.night.office.door_R.Update();
        
        if ((!Global.userData.firstTime || Global.customNight) && (MouseManager.LeftButtonClicked || KeyboardManager.PressedKeys.Count != 0) && i >= message.Length)
        {
            text.Text = "6 AM";
            AudioManager.PauseSFX(chimes[i]);
            AudioManager.CloseSFXAll();
            i = chimes.Length - 1;
            AudioManager.PlaySFX(chimes[^1]);
            fading = true;
        }

        if (chimes[i].PlaybackClosed && !fading)
        {
            if (i < chimes.Length)
            {
                i++;
                if (i < message.Length) text.Text += message[i];
                if (i == chimes.Length - 1) fading = true;
                AudioManager.PlaySFX(chimes[i]);
            }
        }
        if (fading)
        {
            text.opacity -= (float) (1 / (chimes[^1].duration - 1) * Global.gameTime.ElapsedGameTime.TotalSeconds);
            if (chimes[i].PlaybackClosed)
            {
                if (Global.customNight)
                {
                    TransFlicker t = new(Global.mainMenu);
                    t.Initialize();
                    Global.mainMenu.Initialize();
                    return t;
                }
                else
                {
                    Global.runData.bankedTokens += Global.night.tokens;
                    Global.runData.powerDrained += 100 - (int) (Global.night.currPower / Global.night.totalPower * 100);
                    Global.SaveUserData();

                    if (Global.runData.loop == 5 || Global.runData.loop > Global.modifiers.Length)
                    {
                        TransFlicker t = new(Global.summary, true);
                        Global.summary.Initialize();
                        t.Initialize();
                        return t;
                    }
                    else
                    {
                        TransFlicker t = new(Global.modSelect, true);
                        t.Initialize();
                        Global.modSelect.Initialize();
                        return t;
                    }
                }
            }
        }
        return null;
    }

    public override void Draw()
    {
        if (i < message.Length) Global.night.Draw();
        text.Draw();
    }
}
