using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class TransFlicker : Scene
{
    private Scene nextScene;

    private GameElement flicker;
    private Texture2D[] flickerFrames;
    private int iflicker;
    private double counter;

    public TransFlicker(Scene next)
    {
        nextScene = next;
    }

    public override void Initialize()
    {
        counter = 0;
        iflicker = 0;

        flickerFrames = new Texture2D[9];
        for (int i = 0; i < flickerFrames.Length; i++)
        {
            flickerFrames[i] = Global.content.Load<Texture2D>("ani_flicker/" + i);
        }
        flicker = new("ani_flicker/0");
        iflicker = 0;
    }

    public override Scene Update()
    {
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter >= Global.aniDelay * .75)
        {
            if (iflicker >= flickerFrames.Length)
            {
                return nextScene;
            }
            counter = 0;
            flicker.SetTexture(flickerFrames[iflicker]);
            iflicker++;
        }
        nextScene.Update();
        return null;
    }

    public override void Draw()
    {
        nextScene.Draw();
        flicker.Draw();
    }
}
