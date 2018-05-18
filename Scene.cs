using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Template;
using static Template.Raytracer;

//Alles in aparte classes taps (aanrader)!
namespace template
{
    public class Scene
    {
        public static Sphere sphere1, sphere2, sphere3;
        public Plane plane;
        public static Light light1;
        public static List<Primitive> prims = new List<Primitive>();

        public Scene()
        {
            //Create all the objects in the scene
            plane = new Plane(new Vector3(0, 1, 0), 3.0f, new Vector3(1.0f ,1.0f ,1.0f), new Vector3(0,-2,0));
            sphere1 = new Sphere(new Vector3(0, 0, 7), 2f, new Vector3(1.0f, 0.0f, 0.0f));
            sphere2 = new Sphere(new Vector3(-5, 0, 7), 2f,new Vector3(0.0f, 1.0f, 0.0f));
            sphere3 = new Sphere(new Vector3(5, 0, 7), 2f, new Vector3(0.0f , 0.0f, 1.0f));
            //Save all the objects in a list
            prims.Add(plane);
            prims.Add(sphere1);
            prims.Add(sphere2);
            prims.Add(sphere3);
            //Create lightsource(s)
            light1 = new Light(new Vector3(0, 5, 3), new Vector3(200, 200, 200), 0.8f);

            
        }
        public static Sphere GetSphere1 => sphere1;
        public static Sphere GetSphere2 => sphere2;
        public static Sphere GetSphere3 => sphere3;

        public static Light GetLight => light1;
    }

    public abstract class Primitive
    {
        public Vector3 Color;
        public abstract Intersection Intersection(ref Ray ray);
    }


    public class Sphere : Primitive
    {
        public Vector3 pos;
        public float rad;

        public Sphere(Vector3 _pos, float _rad, Vector3 _color)
        {
            pos = _pos;
            rad = _rad;
            Color = _color;           
        }

        //The intersection function for the sphere
        public override Intersection Intersection(ref Ray ray) 
        {
            Vector3 CSphere = pos;
            float t = Vector3.Dot(CSphere, ray.D);
            Vector3 q = CSphere - t * ray.D;
            float Psq = Vector3.Dot(q, q);
            if (Psq > this.rad) return null;
            t -= (float)Math.Sqrt(this.rad - Psq);
            if ((t < ray.t) && (t > 0))
                ray.t = t;
            else return null;

            Vector3 I = ray.O + (ray.t * ray.D);
            Vector3 N = (I - this.pos).Normalized();
            return new Intersection(I, N, Color);
        }
    }

    public class Plane : Primitive
    {
        public Vector3 norm;
        public float dist;
        Vector3 planepos;

        public Plane(Vector3 _norm, float _dist, Vector3 _color, Vector3 _planepos)
        {
            norm = _norm;
            dist = _dist;
            Color = _color;
            planepos = _planepos;
        }

        public override Intersection Intersection(ref Ray ray)
        {
            
            float t = -(Vector3.Dot(ray.O, norm) + dist) / (Vector3.Dot(ray.D, norm));
            if (t >= 0)
            {
                ray.t = t;
                Vector3 I = ray.O + (ray.t * ray.D);
                return new Intersection(I, norm, Color);
            }

            else return null;
            
        }
    }
    
    public class Intersection
    {
        public Vector3 I;
        public Vector3 N;
        public Vector3 C;

        public Intersection(Vector3 _I, Vector3 _N, Vector3 _C)
        {
            I = _I;
            N = _N;
            C = _C;
        }

    }
}
