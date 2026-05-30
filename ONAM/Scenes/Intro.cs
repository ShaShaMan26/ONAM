namespace ONAM;

public class Intro : Scene
{
    public GameElement warning;
    private bool seen;
    private double counter;

    public override void Initialize()
    {
        warning = new("disclamer");
        warning.SetPosition(
            Global.renderTarget.Width / 2 - warning.GetWidth() / 2,
            Global.renderTarget.Height / 2 - warning.GetHeight() / 2 - 20
        );
        warning.opacity = 0;

        seen = false;
        counter = -1;
    }

    public override Scene Update()
    {
        if (KeyboardManager.ReleasedKeys.Count > 0
            || MouseManager.LeftButtonReleased
            || MouseManager.RightButtonReleased)
        {
            seen = true;
        }

        if (seen && warning.opacity > 0)
        {
            warning.opacity -= (float) (1.25 * Global.gameTime.ElapsedGameTime.TotalSeconds);
            if (warning.opacity < 0) warning.opacity = 0;
        }
        else if (seen)
        {
            Global.Initialize2();
            return Global.mainMenu;
        }
        else if (counter > -1)
        {
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter >= 2.5) seen = true;
        }
        else
        {
            warning.opacity += (float) Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (warning.opacity >= 1) 
            {
                warning.opacity = 1;
                counter = 0;
            }
        }
        return null;
    }

    public override void Draw()
    {
        warning.Draw();
    }
}
