using System;
using Microsoft.Xna.Framework;

namespace ONAM;

public class ShadowOffice : GameElement
{
    private bool active;
    private double fadeProg, spawnCount, spawnOpp;
    private int level;
    private Random r;

    public ShadowOffice() : base("shadow")
    {
        level = 5;
        spawnOpp = 12;
        spawnCount = 0;
        fadeProg = 0;
        opacity = 0;
        active = false;

        r = new();
    }

    public void Update()
    {
        if (active)
        {
            if (Global.night.stateManager.currState.GetType() == typeof(InOffice))
            {
                if (fadeProg >= 1)
                {
                    Despawn();
                    Global.night.office.jumpscarePNG.SetTexture(texture);
                    Global.night.jumpytime = true;
                }
                else fadeProg += .08 * Global.gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (Global.night.stateManager.currState.GetType() == typeof(InCams))
            {
                fadeProg -= .15 * Global.gameTime.ElapsedGameTime.TotalSeconds;
                if (fadeProg <= 0)
                {
                    Despawn();
                }
            }
            fadeProg = Math.Clamp(fadeProg, 0, 1);
            opacity = .9f * (float) fadeProg;
        }
        else 
        {
            spawnCount += Global.gameTime.ElapsedGameTime.TotalSeconds;
            
            if (spawnCount >= spawnOpp)
            {
                if (Global.night.stateManager.currState.GetType() != typeof(InCams) && level >= r.Next(0, 21)) Spawn();
                spawnCount = 0;
            }
        }
        
    }

    private void Spawn()
    {
        active = true;
    }
    private void Despawn()
    {
        fadeProg = 0;
        opacity = 0;
        active = false;
    }
}
