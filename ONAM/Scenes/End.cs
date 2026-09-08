using System;
using Microsoft.Xna.Framework;

namespace ONAM;

public class End : Scene
{
    private Canvas canvas;
    private GameElement miku, topBox, bottomBox, fadeCover;
    private TextDisplay topText, bottomText;

    public override void Initialize()
    {
        // base.Initialize();

        canvas = new();
        canvas.Initialize();

        miku = new("gold-miku");
        canvas.Add(8, miku);
        miku.SetDimensions(750, 749);
        miku.SetPosition(Global.renderTarget.Width / 2 - miku.GetWidth() / 2,
            Global.renderTarget.Height / 2 - miku.GetHeight() / 2);
        miku.opacity = 0;
        miku.visible = false;

        topText = new("I've been turned into a marketable plush", "edward");
        topText.SetPosition(Global.renderTarget.Width / 2 - topText.GetWidth() / 2,
            0);
        canvas.Add(topText);
        bottomText = new("Can't have shit in Detroit", "edward");
        bottomText.SetPosition(Global.renderTarget.Width / 2 - bottomText.GetWidth() / 2,
            Global.renderTarget.Height - bottomText.GetHeight());
        canvas.Add(bottomText);

        topBox = new("miku");
        topBox.SetTexture(Global.multiTexture);
        topBox.color = Color.Black;
        topBox.SetDimensions(topText.GetBounds().Width + 5, topText.GetBounds().Height);
        topBox.SetPosition(topText.GetPosition());
        canvas.Add(7, topBox);
        
        bottomBox = new("miku");
        bottomBox.SetTexture(Global.multiTexture);
        bottomBox.color = Color.Black;
        bottomBox.SetDimensions(bottomText.GetBounds().Size.ToVector2());
        bottomBox.SetPosition(bottomText.GetPosition());
        canvas.Add(7, bottomBox);

        fadeCover = new("office");
        fadeCover.SetTexture(Global.multiTexture);
        fadeCover.color = Color.Black;
        fadeCover.opacity = 0;
        fadeCover.visible = false;
        canvas.Add(9, fadeCover);
    }

    public override void OnStart()
    {
        miku.visible = true;
    }

    public override Scene Update()
    {
        if (!miku.visible)
        {
            
        }
        else if (miku.opacity < 1)
        {
            miku.opacity += (float) Math.Clamp(0.5 * Global.gameTime.ElapsedGameTime.TotalSeconds, 0, 1);
        }
        else if (topBox.GetPosition().X < Global.renderTarget.Width)
        {
            topBox.SetPosition(topBox.GetPosition().X + (float) (450 * Global.gameTime.ElapsedGameTime.TotalSeconds), topBox.GetPosition().Y);
        }
        else if (bottomBox.GetPosition().X < Global.renderTarget.Width)
        {
            bottomBox.SetPosition(bottomBox.GetPosition().X + (float) (450 * Global.gameTime.ElapsedGameTime.TotalSeconds), bottomBox.GetPosition().Y);
        }
        else if (!fadeCover.visible && (MouseManager.LeftButtonClicked || MouseManager.RightButtonClicked || KeyboardManager.PressedKeys.Count != 0))
        {
            fadeCover.visible = true;
        }
        else if (fadeCover.visible && fadeCover.opacity < 1)
        {
            fadeCover.opacity += (float) Math.Clamp(.25 * Global.gameTime.ElapsedGameTime.TotalSeconds, 0, 1);
        }
        else if (fadeCover.opacity >= 1)
        {
            
            TransFlicker t = new(Global.summary, true);
            Global.summary.Initialize();
            t.Initialize();
            return t;
        }

        return null;
    }

    public override void Draw()
    {
        canvas.Draw();
    }
}
