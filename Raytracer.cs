using System;
using System.IO;
using template;
using static template.Scene;
using OpenTK;
namespace Template
{
    class Raytracer
    {
        // member variables
        public Surface screen;
        public Surface screenDebug;
        public Camera camera;
        public Scene scene;

        struct Ray
        {
            public Vector3 O; // ray origin
            public Vector3 D; // ray direction
            public float t; // distance
        };


        // initialize
        public void Init() 
	    {
            
	    }
	    // tick: renders one frame
	    public void Render()
	    {
            //Draw ray tracer
		    screen.Clear( 0 );
            for (int y = 0; y < screen.height; y++)
            {
                for (int x = 0; x < screen.width; x++)
                {
                    Ray ray = new Ray();
                    ray.O = camera.CamPos;
                    ray.t = 15;
                    ray.D = (screen.pos1 - camera.CamPos).Normalized() * ray.t;
                    
                    screenDebug.Line(CordxTrans(ray.O.X), CordyTrans(ray.O.Z), CordxTrans(ray.D.X), CordyTrans(ray.D.Y),0xffff00);

                    

                    //wil een ray schieten van camera naar dit punt in het scherm
                }
            }

            
            //Draw Debug screen
            screenDebug.Line(0, 0, 0, 1024, 0xffffff);
            //camera
            screenDebug.Line(CordxTrans(camera.CamPos.X) - 5, CordyTrans(camera.CamPos.Y) - 1, CordxTrans(camera.CamPos.X) + 5, CordyTrans(camera.CamPos.Y) - 1, 0xffa500);
            screenDebug.Line(CordxTrans(camera.CamPos.X) - 5, CordyTrans(camera.CamPos.Y) + 1, CordxTrans(camera.CamPos.X) + 5, CordyTrans(camera.CamPos.Y) + 1, 0xffa500);
            //screen
            screenDebug.Line(CordxTrans(screen.pos1.X), CordyTrans(screen.pos1.Z), CordxTrans(screen.pos2.X), CordyTrans(screen.pos2.Z), 0x00ffff);
            //bollen
            var sphere1 = GetSphere1;
            screenDebug.DrawSphere(sphere1);
            var sphere2 = GetSphere2;
            screenDebug.DrawSphere(sphere2);
            var sphere3 = GetSphere3;
            screenDebug.DrawSphere(sphere3);



        }

        public int CordxTrans(float x)
        {
            float xx;
            xx = x * 51.2f + 512f;
            return (int)xx;
        }

        public int CordyTrans(float y)
        {
            float yy;
            yy = 1024 - (y * 51.2f) - 150;
            return (int)yy;
        }
    }
} // namespace Template