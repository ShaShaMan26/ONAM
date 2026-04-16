using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class GameWin : Scene
{
    private TextDisplay text;
    private string message;
    private int i;

    private SFXObject[] chimes;

    public override void Initialize()
    {
        chimes = new SFXObject[4];
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
        // if (Global.userData.completion < Global.difficultyManager.id)
        // {
        //     Global.userData.completion = Global.difficultyManager.id;
        //     Global.SaveUserData();
        // }
        Global.userData.loop++; // need to make happen in night splash screen but here for now
        Global.SaveUserData();

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
        
        if (chimes[i].PlaybackClosed)
        {
            if (i < message.Length)
            {
                i++;
                if (i < message.Length) text.Text += message[i];
                AudioManager.PlaySFX(chimes[i]);
            }
            else
            {
                TransFlicker t = new(Global.modSelect);
                t.Initialize();
                Global.modSelect.Initialize();
                return t;
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
