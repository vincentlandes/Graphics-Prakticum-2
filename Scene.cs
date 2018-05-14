using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    public class Scene
    {
        public static Sphere sphere1, sphere2, sphere3;
        public Plane plane;

        public Scene()
        {
            plane = new Plane(new Vector3(0, 1, 0), -1f);
            sphere1 = new Sphere(new Vector3(0, 0, 7), 1f, 0x00ff00);
            sphere2 = new Sphere(new Vector3(-5, 0, 7), 1f, 0xff0000);
            sphere3 = new Sphere(new Vector3(5, 0, 7), 1f, 0xadd8e6);
        }

        public static Sphere GetSphere1 => sphere1;
        public static Sphere GetSphere2 => sphere2;
        public static Sphere GetSphere3 => sphere3;     
    }

    public class Primitive
    {

    }


    public class Sphere : Primitive
    {
        public Vector3 pos;
        public float rad;
        public int Color;

        public Sphere(Vector3 _pos, float _rad, int _color)
        {
            pos = _pos;
            rad = _rad;
            Color = _color;
            
        }

    }

    public class Plane : Primitive
    {
        public Vector3 norm;
        public float dist;

        public Plane(Vector3 _norm, float _dist)
        {
            norm = _norm;
            dist = _dist;
        }
    }
}
