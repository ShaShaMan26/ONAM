using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Sign : GameElement
{
    public bool flipping, isRed, showMark;
    private Texture2D[] frames, maru, batsu, markFrames;
    private Texture2D mark;
    private double counter, mCounter, mbCounter;
    private int i, m, wins;
    private SFXObject spin, won, lost, powUp, powDown;

    public Sign() : base("ani_sign/0")
    {
        frames = new Texture2D[3];
        for (int j = 0; j < frames.Length; j++)
        {
            frames[j] = Global.content.Load<Texture2D>("ani_sign/" + j);
        }

        maru = new Texture2D[2];
        for (int j = 0; j < maru.Length; j++)
        {
            maru[j] = Global.content.Load<Texture2D>("ani_maru/" + j);
        }
        batsu = new Texture2D[2];
        for (int j = 0; j < batsu.Length; j++)
        {
            batsu[j] = Global.content.Load<Texture2D>("ani_batsu/" + j);
        }

        spin = new(Global.content.Load<SoundEffect>("sfx/creak"));
        spin.Volume = .5f;
        won = new(Global.content.Load<SoundEffect>("sfx/won"));
        won.Volume = .4f;
        lost = new(Global.content.Load<SoundEffect>("sfx/lost"));
        lost.Volume = .6f;

        powUp = new(Global.content.Load<SoundEffect>("sfx/heal"));
        powDown = new(Global.content.Load<SoundEffect>("sfx/hurt"));

        isRed = true;
        flipping = false;
        showMark = false;
        counter = 0;
        mCounter = 0;
        mbCounter = 0;
        i = 0;
        m = 0;
        wins = 0;
    }

    public void Flip()
    {
        if (!flipping)
        {
            isRed = !isRed;
            flipping = true;
            AudioManager.AddSFX(spin);
            EndMark();
        }
    }

    public void Win()
    {
        markFrames = maru;
        mark = markFrames[m];
        showMark = true;
        AudioManager.AddSFX(won);
    }
    public void Lose()
    {
        markFrames = batsu;
        mark = markFrames[m];
        showMark = true;
        AudioManager.AddSFX(lost);
    }

    public void Update()
    {
        if (flipping)
        {
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter >= Global.aniDelay * 2)
            {
                if (isRed) i--;
                else i++;
                texture = frames[i];
                counter = 0;
                if (i == 0 || i == frames.Length - 1) flipping = false;
            }
        }
        
        if (showMark)
        {
            mCounter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            mbCounter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (mCounter > Global.aniDelay * 4)
            {
                m++;
                if (m > markFrames.Length - 1) m = 0;
                mark = markFrames[m];
                mCounter = 0;
            }
            if (mbCounter >= 1.25)
            {
                EndMark();
            }
        }
    }

    public override void Draw()
    {
        base.Draw();
        if (!flipping && showMark) Global.spriteBatch.Draw(mark, pos, Color.White);
        if (!flipping)
        {
            for (int d = 0; d < wins; d++)
            {
                Global.spriteBatch.Draw(
                    maru[0], 
                    new Rectangle(
                        (int) ((pos.X - 22) + 30 * d), 
                        (int) pos.Y + 68,
                        75,
                        75
                    ), 
                    Color.White * .65f
                );
            }
        }
    }

    public void OnArrive(bool redCall)
    {
        if (showMark) return;

        if (redCall)
        {
            if (isRed) Win();
            else Lose();
        }
        else
        {
            if (isRed) Lose();
            else Win();
        }
    }

    private void EndMark()
    {
        if (showMark)
        {
            if (markFrames == maru)
            {
                wins += 1;
                if (wins >= 3)
                {
                    wins = 0;
                    Global.night.currPower += 200;
                    AudioManager.AddSFX(powUp);
                }
            }
            else
            {
                wins = 0;
                Global.night.currPower -= 180;
                AudioManager.AddSFX(powDown);
            }
        }

        m = 0;
        mark = null;
        mCounter = 0;
        mbCounter = 0;
        showMark = false;
    }
}
