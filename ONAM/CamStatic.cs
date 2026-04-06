using System;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class CamStatic : GameElement
{
    private Random r;
    private Texture2D[] frames;
    private double counter;
    private int i;

    private float min, max;

    public CamStatic(float min, float max) : base("ani_cam_static/0")
    {
        this.min = min;
        this.max = max;
    }

    public void Initialize()
    {
        r = new();

        counter = 0;
        i = 0;
        frames = new Texture2D[8];
        for (int i = 0; i < frames.Length; i++)
        {
            frames[i] = Global.content.Load<Texture2D>("ani_cam_static/" + i);
        }
        opacity = min;
    }

    public void Update()
    {
        counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        if (counter >= .04)
        {
            opacity = (float)(r.NextDouble() * (max - min) + min);
            SetTexture(frames[i]);
            i++;
            counter = 0;
            if(i >= frames.Length)
            {
                i = 0;
            }
        }
    }
} 
