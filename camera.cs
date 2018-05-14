using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template
{
    public class Camera
    {
        public Vector3 CamPos;
        public Vector3 CamDir;

        public Camera(Vector3 pos, Vector3 dir)
        {
            CamPos = pos;
            CamDir = dir;
        }
    }
}
