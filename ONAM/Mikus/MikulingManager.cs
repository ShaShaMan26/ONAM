using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class MikulingManager : GameElement
{
    private Random r;
    private double counter;
    private Texture2D jumpscare;
    private SFXObject caught, call;

    public Mikuling[] mikulings;
    private List<Mikuling> activeMikulings;

    public double startDelay;
    public int level, numTillDeath;

    public MikulingManager() : base("office")
    {
        visible = false;
    }

    public void Initialize()
    {
        r = new();
        counter = 0;
        jumpscare = Global.content.Load<Texture2D>("mikuling-js");

        caught = new(Global.content.Load<SoundEffect>("sfx/thud1"));
        call = new(Global.content.Load<SoundEffect>("sfx/mikudayo"));

        // startDelay = 8;
        // level = 10;
        // numTillDeath = 5;
        // // level = 8;

        mikulings = new Mikuling[16];
        activeMikulings = [];

        for (int i = 0; i < mikulings.Length; i++)
        {
            mikulings[i] = new();
            if (i == 0)
            {
                mikulings[i].SetDimensions(100, 74);
                mikulings[i].SetPosition(570, 411);
                mikulings[i].shadow = .15f;
            }
            else
            {
                if (i == 5)
                {
                    mikulings[i].SetDimensions(96, 71);
                    mikulings[i].SetPosition(mikulings[0].GetPosition()
                        + new Vector2(-35, -47));
                    mikulings[i].shadow = .17f;
                }
                else if (i == 11)
                {
                    mikulings[i].SetDimensions(92, 68);
                    mikulings[i].SetPosition(mikulings[0].GetPosition()
                        + new Vector2(17, -90));
                    mikulings[i].shadow = .19f;
                }
                else
                {
                    mikulings[i].SetDimensions(mikulings[i - 1].GetBounds().Size.ToVector2());
                    mikulings[i].SetPosition(
                        mikulings[i - 1].GetPosition() + new Vector2(mikulings[i].GetWidth() - 10, 0)
                        );
                    mikulings[i].shadow = mikulings[i - 1].shadow;
                }
            }

            mikulings[i].Initialize();
        }
    }

    private void CheckInterations()
    {
        if (Global.night.stateManager.currState == Global.night.inOffice 
            && MouseManager.LeftButtonClicked)
        {   
            foreach (Mikuling m in activeMikulings)
            {
                if (new Rectangle
                    ((m.GetPosition() + GetPosition()).ToPoint(), m.GetBounds().Size)
                    .Contains(MouseManager.Location))
                {
                    m.Reset();
                    activeMikulings.Remove(m);
                    AudioManager.AddSFX(caught);
                    break;
                }
            }
        }
    }

    public void Update()
    {
        foreach(Mikuling m in activeMikulings)
        {
            m.Update();
        }
        CheckInterations();

        if (activeMikulings.Count >= numTillDeath && activeMikulings.All(m => m.attacking))
        {
            Global.night.jumpytime = true;
            Global.night.office.jumpscarePNG.SetTexture(jumpscare);
            activeMikulings.Clear();
        }
        else if (activeMikulings.Count > 0)
        {
            if (call.PlaybackClosed) AudioManager.AddSFX(call);
        }

        if (startDelay > 0)
        {
            startDelay -= Global.gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
            if (counter >= 2.5)
            {
                if (activeMikulings.Count < numTillDeath && r.Next(1, 21) <= level)
                {
                    Mikuling m;
                    do
                    {
                        m = mikulings[r.Next(0, mikulings.Length)];
                    } 
                    while(activeMikulings.Contains(m));
                    activeMikulings.Add(m);
                    activeMikulings = [.. activeMikulings.OrderBy(m => m.shadow)];
                }

                counter = 0;
            }
        }

        switch (activeMikulings.Count(m => m.attacking))
        {
            case 0:
                call.Volume = 0;
                break;
            case 1:
                call.Volume = 0.05f;
                break;
            case 2:
                call.Volume = 0.1f;
                break;
            case 3:
                call.Volume = 0.2f;
                break;
            case 4:
                call.Volume = 0.8f;
                break;
        }
    }

    public override void Draw()
    {
        for(int i = mikulings.Length - 1; i > -1; i--)
        {
            mikulings[i].Draw(GetPosition());
        }
    }
}
