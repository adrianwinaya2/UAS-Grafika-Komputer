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
    internal class Land : Asset3d
    {
        List<Asset3d> objectList = new List<Asset3d>();

        Asset3d landFences = new Asset3d();
        Asset3d landField = new Asset3d();
        Asset3d landFloat = new Asset3d();
        Asset3d landGate = new Asset3d();
        Asset3d landGateDivider = new Asset3d();
        Asset3d landGateHeart = new Asset3d();
        Asset3d landLeaves = new Asset3d();
        Asset3d landRing = new Asset3d();
        Asset3d landTrunk = new Asset3d();

        public Land() { }


        public void load_obj(Vector2i Size)
        {
            landFences = ObjVolume.LoadFromFile("../../../../../../obj/landFences.obj");
            objectList.Add(landFences);

            landField = ObjVolume.LoadFromFile("../../../../../../obj/landField.obj");
            objectList.Add(landField);

            landFloat = ObjVolume.LoadFromFile("../../../../../../obj/landFloat.obj");
            objectList.Add(landFloat);

            landGate = ObjVolume.LoadFromFile("../../../../../../obj/landGate.obj");
            objectList.Add(landGate);

            landGateDivider = ObjVolume.LoadFromFile("../../../../../../obj/landGateDivider.obj");
            objectList.Add(landGateDivider);

            landGateHeart = ObjVolume.LoadFromFile("../../../../../../obj/landGateHeart.obj");
            objectList.Add(landGateHeart);

            landLeaves = ObjVolume.LoadFromFile("../../../../../../obj/landLeaves.obj");
            objectList.Add(landLeaves);

            landRing = ObjVolume.LoadFromFile("../../../../../../obj/landRing.obj");
            objectList.Add(landRing);

            landTrunk = ObjVolume.LoadFromFile("../../../../../../obj/landTrunk.obj");
            objectList.Add(landTrunk);


            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].load_normal(Constants.path + "objectnew.vert", Constants.path + "objectnew.frag", Size.X, Size.Y);
            }
        }

        public void render_obj(Camera _camera, double _time, Vector3[] _pointLightPositions)
        {
            Matrix4 temp = Matrix4.Identity;

            landFences.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landFences.setFragVariable(new Vector3(1f, 0.865f, 0.33f), _camera.Position);

            landField.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landField.setFragVariable(new Vector3(0.205f, 0.799f, 0.120f), _camera.Position);

            landFloat.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landFloat.setFragVariable(new Vector3(1f, 0.741f, 0.459f), _camera.Position);

            landGate.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landGate.setFragVariable(new Vector3(1f, 1f, 1f), _camera.Position);

            landGateDivider.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landGateDivider.setFragVariable(new Vector3(0, 0, 0), _camera.Position);

            landGateHeart.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landGateHeart.setFragVariable(new Vector3(1.0f, 0.037f, 0.063f), _camera.Position);

            landLeaves.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landLeaves.setFragVariable(new Vector3(0f, 0.343f, 0.003f), _camera.Position);

            landRing.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landRing.setFragVariable(new Vector3(0.03f, 0.108f, 0.426f), _camera.Position);

            landTrunk.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            landTrunk.setFragVariable(new Vector3(0.141f, 0.029f, 0.09f), _camera.Position);

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
