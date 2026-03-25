using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ONAM;

public static class Global
{
    private static Random r;

    // program globals
    public static SpriteBatch spriteBatch;
    public static GraphicsDeviceManager graphics;
    public static ContentManager content;
    public static GameTime gameTime;

    // instance globals
    public static Canvas canvas;
    public static StateManager stateManager;

    // canvases
    public static Office office;
    public static CamView camView;
    
    // states
    public static InOffice inOffice;
    public static InCams inCams;
    public static OpenCams openCams;
    public static CloseCams closeCams;
    public static Jumpscare jumpscare;

    // instance variables
    public static bool doorClose_L, doorClose_R;
    public static int camNum;
    public static int[] progress;
    public static int level_m, level_r, level_y;

    public static void Initialize()
    {
        r = new();
        camNum = 3;
        progress = [1, 1, 1];
        level_m = 2;
        level_r = 8;
        level_y = 5;

        inOffice = new InOffice();
        inOffice.Initialize();
        inCams = new InCams();
        inCams.Initialize();
        jumpscare = new();
        jumpscare.Initialize();

        office = new Office();
        office.Initialize();
        camView = new CamView();
        camView.Initialize();

        openCams = new OpenCams();
        openCams.Initialize();
        closeCams = new CloseCams();
        closeCams.Initialize();

        canvas = office;
        canvas.Initialize();
        stateManager = new StateManager(inOffice);
        stateManager.Initialize();
    }

    private static double counter = 0;
    public static void UpdateMikus()
    {
        counter += gameTime.ElapsedGameTime.TotalSeconds;
        if (counter > 5)
        {
            // miku
            if (r.Next(1, 21) <= level_m)
            {
                counter = progress[0];
                if (progress[0] == 2) 
                    progress[0] = r.Next(3, 5);
                else progress[0]++;
                if (progress[0] > 4 || (counter == 3 && progress[0] == 4)) progress[0] = 1;   
                if (camNum == progress[0] || camNum == counter) camView.InterruptCam(camNum);
            }

            // red
            if (r.Next(1 ,21) <= level_r)
            {
                counter = progress[1];
                progress[1]++;
                if (progress[1] > 3)
                {
                    progress[1] = 1;
                }
                if (camNum == progress[1] || camNum == counter) camView.InterruptCam(camNum);
            }

            // yellow
            if (r.Next(1 ,21) <= level_y)
            {
                counter = progress[2];
                progress[2]++;
                if (progress[2] == 3) progress[2] = 4;
                else if (progress[2] > 4) progress[2] = 1;
                if (camNum == progress[2] || camNum == counter) camView.InterruptCam(camNum);
            }

            counter = 0;
        }

        // foreach (int i in progress)
        // {
        //     Console.Write(i + " ");
        // }
        // Console.WriteLine();
    }
}
