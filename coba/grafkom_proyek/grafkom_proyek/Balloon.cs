using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace grafkom_proyek
{
    internal class Balloon : Asset3d
    {
        List<Asset3d> objectList = new List<Asset3d>();

        Asset3d glove = new Asset3d();
        Asset3d stripe_rope = new Asset3d();
        Asset3d handle = new Asset3d();
        Asset3d basket = new Asset3d();

        float degr = 3.2f;

        public Balloon() { }


        public void load_obj(Vector2i Size)
        {
            glove = ObjVolume.LoadFromFile("../../../../../../obj/balloon_glove.obj");
            objectList.Add(glove);

            stripe_rope = ObjVolume.LoadFromFile("../../../../../../obj/balloon_rope.obj");
            objectList.Add(stripe_rope);

            handle = ObjVolume.LoadFromFile("../../../../../../obj/balloon_basket1.obj");
            objectList.Add(handle);

            basket = ObjVolume.LoadFromFile("../../../../../../obj/balloon_basket2.obj");
            objectList.Add(basket);


            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].load_normal(Constants.path + "objectnew.vert", Constants.path + "objectnew.frag", Size.X, Size.Y);
            }
        }

        public void render_obj(Camera _camera, double _time, Vector3[] _pointLightPositions)
        {
            Matrix4 temp = Matrix4.Identity;
            temp = rotateObject(temp);

            glove.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            glove.setFragVariable(new Vector3(1.0f, 1.0f, 1.0f), _camera.Position);

            stripe_rope.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            stripe_rope.setFragVariable(new Vector3(0, 0, 0), _camera.Position);

            handle.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            handle.setFragVariable(new Vector3(0.988f, 0.835f, 0.160f), _camera.Position);

            basket.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            basket.setFragVariable(new Vector3(0.615f, 0.203f, 0.203f), _camera.Position);


            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].setDirectionalLight(new Vector3(-0.2f, -1.0f, -0.5f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));
                objectList[i].setSpotLight(new Vector3(0.0f, 7.0f, 5.0f), new Vector3(0, -0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f,
                    MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                objectList[i].setPointlight(_pointLightPositions, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
            }

        }

        protected Matrix4 rotateObject(Matrix4 temp)
        {
            temp *= Matrix4.CreateTranslation(-1f, 0.75f, 0);
            degr += MathHelper.DegreesToRadians(0.2f);
            temp *= Matrix4.CreateRotationY(degr);
            return temp;
        }
    }
}
