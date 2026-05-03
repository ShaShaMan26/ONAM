using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class CamReloadBar : GameElement
{
    private Texture2D bar, bumpOn, bumpOff;
    private Vector2 barPos, bumpPos;
    private int needleWidth;
    private double needleProg, needleSpeed, counter;
    public bool success, failure, complete;
    private SFXObject succ, fail, done;

    public CamReloadBar() : base("cam_bar")
    {
        succ = new(Global.content.Load<SoundEffect>("sfx/collect"));
        fail = new(Global.content.Load<SoundEffect>("sfx/done"));
        done = new(Global.content.Load<SoundEffect>("sfx/miss"));

        bar = Global.content.Load<Texture2D>("cam_reload-bar");
        barPos = new(dims.X / 2 - bar.Width / 2, 
            dims.Y / 2 - bar.Height / 2);
        bumpOn = Global.content.Load<Texture2D>("cam_reload-bump");
        bumpOff = Global.content.Load<Texture2D>("cam_reload-bump-off");
        bumpPos = new(272, 
            15);

        needleWidth = 6;
        needleSpeed = 160;
    }

    public void Initialize()
    {
        complete = false;
        success = false;
        failure = false;
        needleProg = 18;
        counter = 0;
    }

    public void Update()
    {
        if (success)
        {
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        }
        else needleProg += Global.gameTime.ElapsedGameTime.TotalSeconds * needleSpeed;
        
        if (needleProg + needleWidth >= bar.Width || counter >= .2)
        {
            complete = true;
            AudioManager.AddSFX(done);
        }
    }

    public override void Draw()
    {
        if (visible)
        {
            Global.spriteBatch.Draw(bar, pos + barPos, Color.White);
            Global.spriteBatch.Draw(success ? bumpOn : bumpOff, pos + bumpPos, Color.White * (failure ? .5f : 1));
            Global.spriteBatch.Draw(Global.multiTexture, 
            new Rectangle((int) (pos.X + needleProg), (int) pos.Y + 6, needleWidth, (int) dims.Y - 12), 
            Color.White);
        }
    }

    public void MakeAttempt()
    {
        if (needleProg + needleWidth > bumpPos.X && needleProg < bumpPos.X + bumpOff.Width)
        {
            AudioManager.AddSFX(succ);
            success = true;
        }
        else
        {
            AudioManager.AddSFX(fail);
            failure = true;
        }
    }
}
