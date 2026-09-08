using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public class Miku : GameElement
{
    protected static Random r;
    private Texture2D overlay;
    public float shadow = .8f;
    public Miku next;
    public int id, level, progress;
    public double counter, moveDelay, startDelay, health, tempHealth, attackDelay;
    public bool attacking;

    public Miku(string path) : base(path)
    {
        attacking = false;

        level = 0;
        progress = 1;
        moveDelay = 5;
        counter = 0;
        startDelay = 0;
        attackDelay = 0;

        tempHealth = -1;
        switch (path)
        {
            case "miku":
                id = 0;
                break;
            case "red-miku":
                id = 1;
                break;
            case "yellow-miku":
                id = 2;
                break;
            case "green-miku":
                id = 3;
                break;
        }
        overlay = Global.content.Load<Texture2D>("shadow");
        next = null;
    }

    public void Update()
    {
        if (tempHealth == -1) tempHealth = health;

        if (startDelay > 0)
        {
            startDelay -= Global.gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        { 
            counter += Global.gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        if (attacking)
        {
            UpdateAttack();
        }
        else if (counter >= moveDelay)
        {
            if (Global.random.Next(1, 21) <= level)
            {
                MakeMove();
            }
            counter = 0;
        }
    }

    public virtual void MakeMove() { }
    public virtual void UpdateAttack() { }

    public override void Draw()
    {
        base.Draw();

        if (texture != null && visible)
            Global.spriteBatch.Draw(overlay, new Rectangle(pos.ToPoint(), dims.ToPoint()), null, Color.White * shadow, rotation, Vector2.Zero, SpriteEffects.None, 0);
    }
}
