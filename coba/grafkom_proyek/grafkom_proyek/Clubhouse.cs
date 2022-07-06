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
    internal class Clubhouse : Asset3d
    {
        List<Asset3d> objectList = new List<Asset3d>();

        Asset3d clubhouse_head = new Asset3d();
        Asset3d clubhouse_body = new Asset3d();
        Asset3d clubhouse_window = new Asset3d();
        Asset3d clubhouse_feet = new Asset3d();

        public Clubhouse() { }


        public void load_obj(Vector2i Size)
        {
            clubhouse_head = ObjVolume.LoadFromFile("../../../../../../obj/clubhouse_head.obj");
            objectList.Add(clubhouse_head);

            clubhouse_body = ObjVolume.LoadFromFile("../../../../../../obj/clubhouse_body.obj");
            objectList.Add(clubhouse_body);

            clubhouse_window = ObjVolume.LoadFromFile("../../../../../../obj/clubhouse_windows.obj");
            objectList.Add(clubhouse_window);

            clubhouse_feet = ObjVolume.LoadFromFile("../../../../../../obj/clubhouse_feet.obj");
            objectList.Add(clubhouse_feet);

            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].load_normal(Constants.path + "objectnew.vert", Constants.path + "objectnew.frag", Size.X, Size.Y);
            }
        }

        public void render_obj(Camera _camera, double _time, Vector3[] _pointLightPositions)
        {
            Matrix4 temp = Matrix4.Identity;

            clubhouse_head.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            clubhouse_head.setFragVariable(new Vector3(0.05f, 0.05f, 0.05f), _camera.Position);

            clubhouse_body.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            clubhouse_body.setFragVariable(new Vector3(0.7f, 0.0f, 0.0f), _camera.Position);

            clubhouse_window.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            clubhouse_window.setFragVariable(new Vector3(0.019f, 0.623f, 1.0f), _camera.Position);

            clubhouse_feet.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            clubhouse_feet.setFragVariable(new Vector3(0.992f, 1.0f, 0.019f), _camera.Position);

            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].setDirectionalLight(new Vector3(-0.2f, -1.0f, -0.5f), new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));
                objectList[i].setSpotLight(new Vector3(0.0f, 5.0f, 5.0f), new Vector3(0, -0.5f, 0), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f,
                    MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                objectList[i].setPointlight(_pointLightPositions, new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0.8f, 0.8f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.09f, 0.032f);
            }
        }
    }
}
