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
    static class Constants
    {
        public const string path = "../../../Shaders/";
    }
    internal class Window : GameWindow
    {
        Camera _camera;
        double _time;
        bool _firstMove = true;
        Vector2 _lastPos;
        Vector3 _objectPos = new Vector3(0, 0, 0);
        float _rotationSpeed = 0.4f;

        Asset3d LightObject = new Asset3d();
        Asset3d[] LightObjects = new Asset3d[4];

        Asset3d bg;

        Clubhouse clubhouse = new Clubhouse();
        Mickey mickey = new Mickey();
        Land land = new Land();
        Balloon balloon = new Balloon();

        readonly Vector3[] _cubePositions =
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(2.0f, 5.0f, -15.0f),
            new Vector3(-1.5f, -2.2f, -2.5f),
            new Vector3(-3.8f, -2.0f, -12.3f),
            new Vector3(2.4f, -0.4f, -3.5f),
            new Vector3(-1.7f, 3.0f, -7.5f),
            new Vector3(1.3f, -2.0f, -2.5f),
            new Vector3(1.5f, 2.0f, -2.5f),
            new Vector3(1.5f, 0.2f, -1.5f),
            new Vector3(-1.3f, 1.0f, -1.5f)
        };
        private readonly Vector3[] _pointLightPositions =
        {
            new Vector3(0.0f, 7.0f, -2.0f),
            new Vector3(2.3f, -3.3f, -4.0f),
            new Vector3(-4.0f, 2.0f, -12.0f),
            new Vector3(0.0f, 0.0f, -3.0f)
        };

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f); 

            GL.Enable(EnableCap.DepthTest); //untuk obj 3d, biar barang yg dibelakang tidak kelihatan
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _camera = new Camera(new Vector3(0, 0, 1), Size.X / Size.Y);
            _camera.Position -= _camera.Front * 3.5f;
            _camera.Position += _camera.Up * 3.5f;


            CursorGrabbed = false;


            GL.ClearColor(0.4f, 0.75f, 1.0f, 1.0f);

            LightObject.load_withnormal(Constants.path + "objectnew.vert", Constants.path + "objectnew.frag", Size.X, Size.Y);

            land.load_obj(Size);
            clubhouse.load_obj(Size);
            mickey.load_obj(Size);
            balloon.load_obj(Size);

            for (int i = 0; i < 4; i++)
            {
                LightObjects[i] = new Asset3d();
                LightObjects[i].load_withnormal(Constants.path + "shader.vert", Constants.path + "shader.frag", Size.X, Size.Y);
            }

            bg = new Asset3d();
            bg.createCuboid(0, 7, 0, 20, false);
            bg.load_normal(Constants.path + "texture.vert", Constants.path + "texture.frag", Size.X, Size.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _time += 7.0 * args.Time;
            Matrix4 temp = Matrix4.Identity;

            land.render_obj(_camera, _time, _pointLightPositions);
            clubhouse.render_obj(_camera, _time, _pointLightPositions);
            mickey.render_obj(_camera, _time, _pointLightPositions);
            balloon.render_obj(_camera, _time, _pointLightPositions);

            LightObject.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());

            for (int i = 0; i < 4; i++)
            {
                LightObjects[i].render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());
            }

            bg.render(0, _time, temp, _camera.GetViewMatrix(), _camera.GetProjectionMatrix());

            SwapBuffers(); //supaya yg udh dikerjain sama graphic card bisa muncul di layar
        }

        //berubah tiap ukuran layarnya berubah
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
            
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov = _camera.Fov - e.OffsetY;
        }

        public Matrix4 generateArbRotationMatrix(Vector3 axis, float angle)
		{
			angle = MathHelper.DegreesToRadians(angle);

			var arbRotationMatrix = new Matrix4(
				(float)Math.Cos(angle) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(angle))	, axis.X * axis.Y * (1 - (float)Math.Cos(angle)) - axis.Z * (float)Math.Sin(angle)	, axis.X * axis.Z * (1 - (float)Math.Cos(angle)) + axis.Y * (float)Math.Sin(angle)	, 0,
				axis.Y * axis.X * (1 - (float)Math.Cos(angle)) + axis.Z * (float)Math.Sin(angle)	, (float)Math.Cos(angle) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(angle)), axis.Y * axis.Z * (1 - (float)Math.Cos(angle)) - axis.X * (float)Math.Sin(angle)	, 0, 
				axis.Z * axis.X * (1 - (float)Math.Cos(angle)) - axis.Y * (float)Math.Sin(angle)	, axis.Z * axis.Y * (1 - (float)Math.Cos(angle)) + axis.X * (float)Math.Sin(angle)	, (float)Math.Cos(angle) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(angle)), 0,
				0, 0, 0, 1
				);

			return arbRotationMatrix;
		}

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;
            var mouse_input = MouseState; //ini untuk input mouse

            float cameraSpeed = 3.0f;


            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * 0.7f * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * 0.7f * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * 0.7f * cameraSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * 0.7f * cameraSpeed * (float)args.Time;
            }

            var mouse = MouseState;
            var sensitivity = 0.2f;
            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }

            if (_camera.Position.X > 9f)
            {
                _camera.Position = new Vector3(9f, _camera.Position.Y, _camera.Position.Z);
            }
            else if (_camera.Position.X < -9f)
            {
                _camera.Position = new Vector3(-9f, _camera.Position.Y, _camera.Position.Z);
            }
            if (_camera.Position.Y > 15f)
            {
                _camera.Position = new Vector3(_camera.Position.X, 15f, _camera.Position.Z);
            }
            else if (_camera.Position.Y < 0f)
            {
                _camera.Position = new Vector3(_camera.Position.X, 0f, _camera.Position.Z);
            }
            if (_camera.Position.Z > 9f)
            {
                _camera.Position = new Vector3(_camera.Position.X, _camera.Position.Y, 9f);
            }
            else if (_camera.Position.Z < -9f)
            {
                _camera.Position = new Vector3(_camera.Position.X, _camera.Position.Y, -9f);
            }

            //if (KeyboardState.IsKeyDown(Keys.K))
            //{
            //    var axis = new Vector3(0, 1, 0);
            //    _camera.Position -= _objectPos;
            //    _camera.Yaw += _rotationSpeed;
            //    _camera.Position = Vector3.Transform(_camera.Position,
            //        generateArbRotationMatrix(axis, _rotationSpeed).ExtractRotation());
            //    _camera.Position += _objectPos;

            //    _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
            //}
            //if (KeyboardState.IsKeyDown(Keys.L))
            //{
            //    var axis = new Vector3(0, 1, 0);
            //    _camera.Position -= _objectPos;
            //    _camera.Yaw -= _rotationSpeed;
            //    _camera.Position = Vector3.Transform(_camera.Position,
            //        generateArbRotationMatrix(axis, -_rotationSpeed).ExtractRotation());
            //    _camera.Position += _objectPos;

            //    _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
            //}
            //if (KeyboardState.IsKeyDown(Keys.I))
            //{
            //    var axis = new Vector3(1, 0, 0);
            //    _camera.Position -= _objectPos;
            //    _camera.Pitch -= _rotationSpeed;
            //    _camera.Position = Vector3.Transform(_camera.Position,
            //        generateArbRotationMatrix(axis, _rotationSpeed).ExtractRotation());
            //    _camera.Position += _objectPos;
            //    _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
            //}
            //if (KeyboardState.IsKeyDown(Keys.M))
            //{
            //    var axis = new Vector3(1, 0, 0);
            //    _camera.Position -= _objectPos;
            //    _camera.Pitch += _rotationSpeed;
            //    _camera.Position = Vector3.Transform(_camera.Position,
            //        generateArbRotationMatrix(axis, -_rotationSpeed).ExtractRotation());
            //    _camera.Position += _objectPos;
            //    _camera._front = -Vector3.Normalize(_camera.Position - _objectPos);
            //}
        }
    }
}
