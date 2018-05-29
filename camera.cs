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
