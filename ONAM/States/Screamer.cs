using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ONAM;

public class Screamer : State
{
    private Mikuling mikuling;
    private State prevState;
    private SFXObject scream;
    private double counter;
    private Vector2 basePos;

    public Screamer(Mikuling mikuling, State prevState)
    {
        this.mikuling = mikuling;
        this.prevState = prevState;
        basePos = mikuling.GetPosition();
        scream = new(Global.content.Load<SoundEffect>("sfx/screamer"));
    }

    public override void Initialize()
    {
        AudioManager.PauseSFXAll();
        AudioManager.PauseBGM();
        AudioManager.AddSFX(scream);
        counter = 0;
    }

    public override State Update()
    {
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (scream.PlaybackClosed)
        {
            AudioManager.RemoveSFX(scream);
            AudioManager.ResumeBGM();
            AudioManager.PlaySFXAll();
            Global.night.stateManager.currState = prevState;
        }
        else if (counter >= .05)
        {
            mikuling.SetPosition(basePos + 3 * new Vector2(Global.random.Next(-1, 2), Global.random.Next(-1, 2)));
            counter = 0;
        }
        return null;
    }
}