using System;
using template;
using static template.Scene;
using OpenTK;
using System.Collections.Generic;

namespace Template
{
    public class Raytracer
    {
        // member variables
        public static List<Primitive> Objects;
        public Surface screen;
        public Surface screenDebug;
        public Camera camera;
        public Scene scene;

        //Angle for the PoV
        public int angle = 100;
        public float recursive = 50.0f;
            

        public struct Ray
        {
            public Vector3 O; // ray origin
            public Vector3 D; // ray direction (normalized)
            public float t; // distance
        };


        // initialize
        public void Init()
        {
            ChangePOV(angle);
        }

        // Render: renders one frame
        public void Render()
        {
            screen.Clear(0);
            Vector3 Color;

            //render screens
            for (int y = 0; y < screen.height; y++)
            {
                for (int x = 0; x < screen.width; x++)
                {
                    Ray ray;
                    ray.t = 30;
                    ray.O = camera.CamPos;
                    ray.D = screen.pos0 + (x * ((screen.pos1 - screen.pos0) / 512.0f)) + (y * ((screen.pos2 - screen.pos0) / 512.0f));
                    //ray normalized direction
                    ray.D = (ray.D - camera.CamPos).Normalized();

                    Intersection nearest = null;
                    Intersection nearestref = null;

                    foreach (Primitive p in prims)
                    {
                        Intersection overr = p.Intersection(ref ray);

                        if (overr != null)
                            nearest = overr;
                    }

                    // if there is an intersection, create a shadow ray
                    if (nearest != null)
                    {
                        if (nearest.isMirror == false)
                            Color = CastShadowRay(nearest);
                        else
                        {
                            Color = CastShadowRay(nearest) * (1 - recursive/100);
                            Ray reflectionRay;
                            reflectionRay.O = nearest.I;
                            reflectionRay.t = 300;
                            reflectionRay.D = ray.D - 2 * nearest.N * (Vector3.Dot(ray.D, nearest.N));

                            foreach (Primitive p in prims)
                            {
                                Intersection overr = p.Intersection(ref reflectionRay);

                                if (overr != null)
                                    nearestref = overr;
                            }

                            Vector3 Color2 = Vector3.Zero;
                            if(nearestref != null)
                            {
                                Color2 = CastShadowRay(nearestref) * (recursive / 100.0f);
                            }
                            else
                                Color2 = Vector3.Zero;

                            Color = Color + Color2;

                            //draw reflectionrays on debug screen.
                            if (x % 20 == 0 && y == screen.height / 2)
                                screenDebug.Line(CordxTrans(reflectionRay.O.X), CordzTrans(reflectionRay.O.Z), CordxTrans(reflectionRay.D.X * ray.t), CordzTrans(reflectionRay.D.Z * ray.t), 0xff00ff);                            
                        }
                    }
                    else
                        Color = Vector3.Zero;
  

                    //plot the correct color on the correct pixel
                    screen.Plot(x, y, Color);

                    //Draw 1 in 10 rays on the debugscreen
                    if (x % 20 == 0 && y == screen.height / 2)
                        screenDebug.Line(CordxTrans(camera.CamPos.X), CordzTrans(camera.CamPos.Z), CordxTrans(ray.D.X * ray.t), CordzTrans(ray.D.Z * ray.t), 0xffff00);              
                }
            }
            

            //Draw Debug screen
            //Draw line between screen and debug screen
            screenDebug.Line(0, 0, 0, 1024, 0xffffff);
            //Draw camera as 2 orange lines
            screenDebug.Line(CordxTrans(camera.CamPos.X) - 5, CordzTrans(camera.CamPos.Y) - 1, CordxTrans(camera.CamPos.X) + 5, CordzTrans(camera.CamPos.Y) - 1, 0xffa500);
            screenDebug.Line(CordxTrans(camera.CamPos.X) - 5, CordzTrans(camera.CamPos.Y) + 1, CordxTrans(camera.CamPos.X) + 5, CordzTrans(camera.CamPos.Y) + 1, 0xffa500);
            //Draw screen as a blue line
            screenDebug.Line(CordxTrans(screen.pos1.X), CordzTrans(screen.pos1.Z), CordxTrans(screen.pos2.X), CordzTrans(screen.pos2.Z), 0x00ffff);
            //Draw spheres
            var sphere1 = GetSphere1;
            screenDebug.DrawSphere(sphere1);
            var sphere2 = GetSphere2;
            screenDebug.DrawSphere(sphere2);
            var sphere3 = GetSphere3;
            screenDebug.DrawSphere(sphere3);
        }


        public Vector3 CastShadowRay(Intersection nearest)
        {
            foreach (Light l in lights)
            {
                Ray shadowRay = new Ray();
                shadowRay.D = (l.pos - nearest.I).Normalized();
                shadowRay.O = nearest.I + shadowRay.D * 0.0001f;
                shadowRay.t = 300;

                Vector3 Color = Vector3.Zero;

                var ndotl = Vector3.Dot(nearest.N, shadowRay.D);
                if (ndotl > 0)
                {
                    bool occluded = false;

                    foreach (Primitive p in prims)
                    {
                        if (p.Intersection(ref shadowRay) != null)
                        {
                            // if there is an object interfearing the light
                            occluded = true;
                            break;
                        }
                    }

                    if (!occluded)
                    {
                        //make the shadow
                        var dist = (l.pos - shadowRay.O).Length;
                        Color = (ndotl / (dist * dist)) * l.color * nearest.C * ndotl * ndotl;
                    }
                    else
                    {
                        Color = new Vector3(0, 0, 0);
                    }
                }
                return Color;
            }
            return Vector3.Zero;
        }

        //change coordinate to pixel location
        public int CordxTrans(float x)
        {
            float xx;
            xx = x * 51.2f + 512f;
            return (int)xx;
        }

        //change coordinate to pixel location
        public int CordzTrans(float y)
        {
            float yy;
            yy = 1024 - (y * 51.2f) - 150;
            return (int)yy;
        }

        //change pixel location to coordinate
        public int InvertxTrans(float x)
        {
            float xx;
            xx = (x - 512) / 51.2f;
            return (int)xx;
        }

        //change pixel location to coordinate
        public int InvertyTrans(float y)
        {
            float yy;
            yy = (y - 1024 + 150) / 51.2f;
            return (int)yy;
        }

        //calculate the screen with the given angle
        public void ChangePOV(double _angle)
        {
            double angle = (_angle * Math.PI) / 180/ 2;
            double disToScreen = 2 / Math.Tan(angle);
            Vector3 ScreenC = camera.CamPos + (float)disToScreen * camera.CamDir;
            screen.pos0 = ScreenC + new Vector3(-2, 2, 0);
            screen.pos1 = ScreenC + new Vector3(2, 2, 0);
            screen.pos2 = ScreenC + new Vector3(-2, -2, 0);
        }

        //change the Color vector to an integer --> aanpassen nog!
        public static int VecToInt(Vector3 c)
        {
            int color;
            int red = (int)(Math.Min(1.0f, c.X) * 255.0f);
            int blue = (int)(Math.Min(1.0f, c.Z) * 255.0f);
            int green = (int)(Math.Min(1.0f, c.Y) * 255.0f);

            color = (blue << 0) | (green << 8) | (red << 16);
            return color;
        }
    }
}
