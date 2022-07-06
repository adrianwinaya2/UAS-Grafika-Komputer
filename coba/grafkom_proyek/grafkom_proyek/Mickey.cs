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
    internal class Mickey : Asset3d
    {
        List<Asset3d> objectList = new List<Asset3d>();

        Asset3d temp = new Asset3d();
        Vector3 black = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 white = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 grey = new Vector3(0.8f, 0.8f, 0.8f);
        Vector3 dark_grey = new Vector3(0.379f, 0.379f, 0.379f);
        Vector3 red = new Vector3(0.639f, 0.023f, 0.101f);
        Vector3 pink = new Vector3(1.0f, 0.184f, 0.173f);
        Vector3 beige = new Vector3(0.984f, 0.8f, 0.737f);
        Vector3 yellow = new Vector3(0.988f, 0.835f, 0.160f);

        public Mickey() { }


        public void load_obj(Vector2i Size)
        {
            // mickey_body
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_body.obj");
            objectList.Add(temp);

            // mickey_buttons1
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_buttons1.obj");
            objectList.Add(temp);

            // mickey_buttons2
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_buttons2.obj");
            objectList.Add(temp);

            // mickey_buttons3
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_buttons3.obj");
            objectList.Add(temp);

            // mickey_eyes1
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_eyes1.obj");
            objectList.Add(temp);

            // mickey_eyes2
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_eyes2.obj");
            objectList.Add(temp);

            // mickey_eyes3
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_eyes3.obj");
            objectList.Add(temp);

            // mickey_face
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_face.obj");
            objectList.Add(temp);

            // mickey_hands1
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_hands1.obj");
            objectList.Add(temp);

            // mickey_hands2
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_hands2.obj");
            objectList.Add(temp);

            // mickey_nose
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_nose.obj");
            objectList.Add(temp);

            // mickey_pants
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_pants.obj");
            objectList.Add(temp);

            // mickey_shoes
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_shoes.obj");
            objectList.Add(temp);

            // mickey_tongue
            temp = ObjVolume.LoadFromFile("../../../../../../obj/mickey_tongue.obj");
            objectList.Add(temp);

            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].load_normal(Constants.path + "objectnew.vert", Constants.path + "objectnew.frag", Size.X, Size.Y);
            }
        }

        public void render_obj(Camera _camera, double _time, Vector3[] _pointLightPositions)
        {
            Matrix4 temp = Matrix4.Identity;

            // body
            objectList[0].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[0].setFragVariable(black, _camera.Position);

            // buttons 1
            objectList[1].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[1].setFragVariable(white, _camera.Position);

            // buttons 2
            objectList[2].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[2].setFragVariable(grey, _camera.Position);

            // buttons 3
            objectList[3].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[3].setFragVariable(dark_grey, _camera.Position);

            // eyes 1
            objectList[4].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[4].setFragVariable(white, _camera.Position);

            // eyes 2
            objectList[5].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[5].setFragVariable(black, _camera.Position);

            // eyes 3
            objectList[6].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[6].setFragVariable(white, _camera.Position);

            // face
            objectList[7].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[7].setFragVariable(beige, _camera.Position);

            // hands 1
            objectList[8].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[8].setFragVariable(white, _camera.Position);

            // hands 2
            objectList[9].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[9].setFragVariable(black, _camera.Position);

            // nose
            objectList[10].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[10].setFragVariable(black, _camera.Position);

            // pants
            objectList[11].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[11].setFragVariable(red, _camera.Position);

            // shoes
            objectList[12].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[12].setFragVariable(yellow, _camera.Position);

            // tongue
            objectList[13].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            objectList[13].setFragVariable(pink, _camera.Position);

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
