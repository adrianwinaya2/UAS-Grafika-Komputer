using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafkom_proyek
{
    internal class Asset
    {
        private readonly string path = "../../../Shader/";

        protected List<Vector3> vertices = new List<Vector3>();
        protected List<uint> indices = new List<uint>();

        protected int _vertexBufferObject;
        protected int _vertexArrayObject;
        protected int _elementBufferObject;

        protected Shader _shader;
        protected Vector3 color;

        protected Matrix4 _view;
        protected Matrix4 _projection;
        protected Matrix4 _model;

        public Vector3 _centerPosition;
        public List<Vector3> _euler;
        public List<Asset> Child;

        public Asset(List<Vector3> vertices, List<uint> indices)
        {
            this.vertices = vertices;
            this.indices = indices;
            setdefault(); 
        }

        public Asset()
        {
            vertices = new List<Vector3>();
            setdefault();
        }
        public void setdefault()
        {
            _euler = new List<Vector3>();

            //sumbu X
            _euler.Add(new Vector3(1, 0, 0));
            //sumbu Y
            _euler.Add(new Vector3(0, 1, 0));
            //sumbu Z
            _euler.Add(new Vector3(0, 0, 1));

            _model = Matrix4.Identity;
            _centerPosition = new Vector3(0, 0, 0);
            Child = new List<Asset>();
        }

        public void load(string shadervert, string shaderfrag)
        {
            //buffer
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

            //vao
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            if (indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            _shader = new Shader(shadervert, shaderfrag);
            _shader.Use();

            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            //_projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), size_x / (float)size_y, 0.1f, 100.0f);
            foreach (var item in Child)
            {
                item.load(shadervert, shaderfrag);
            }
        }

        public void render(int line, double time, Matrix4 temp, Matrix4 camera_view, Matrix4 camera_projection)
        {
            _shader.Use();

            GL.BindVertexArray(_vertexArrayObject);

            _model = temp;

            //_shader.SetVector3("objColor", color);
            _shader.SetMatrix4("model", _model);
            _shader.SetMatrix4("view", camera_view);
            _shader.SetMatrix4("projection", camera_projection);
            _shader.SetVector3("objColor", color);

            if (indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
                Console.WriteLine("tes");

            }
            else
            {
                if (line == 0)
                {
                    GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
                }
                else if (line == 1)
                {
                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertices.Count);
                }
                else if (line == 2)
                {

                }
                else if (line == 3)
                {
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Count);
                }
            }

            foreach (var item in Child)
            {
                item.render(line, time, temp, camera_view, camera_projection);
            }

        }

        public void setColor(float red = 0.0f, float green = 0.0f, float blue = 0.0f)
        {
            color = new Vector3(red, green, blue);
        }


        public void createEllipsoid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z)
        {
            int sectorCount = 72;
            int stackCount = 24;

            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * (float)Math.PI / sectorCount;
            float stackStep = (float)Math.PI / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = z;
                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createHemisphere(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z)
        {
            int sectorCount = 288;
            int stackCount = 96;

            _centerPosition.X = _x;
            _centerPosition.Y = _y;
            _centerPosition.Z = _z;
            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * (float)Math.PI / sectorCount;
            float stackStep = (float)Math.PI / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle);

                for (int j = 0; j <= sectorCount / 2; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = z;
                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createEllipticCylinder(float x, float y, float z, float radius, float height)
        {
            float _X = x;
            float _Y = y;
            float _Z = z;
            float _height = height;
            float _radius = radius;
            Vector3 temp_vector = new Vector3();
            float _pi = 3.14159f;

            int count = 500;
            int temp_index = -1;
            float increament = _pi / count;


            for (float u = 0; u <= 2 * _pi + increament; u += increament)
            {
                for (float v = 0; v <= _height + increament; v += increament)
                {
                    temp_index++;
                    temp_vector.X = _X + _radius * (float)Math.Cos(u);
                    temp_vector.Y = _Y + _radius * (float)Math.Sin(u);
                    temp_vector.Z = _Z + _radius * v;
                    vertices.Add(temp_vector);
                    if (u != 0)
                    {
                        if ((temp_index % count) + 1 < count)
                        {
                            indices.Add(Convert.ToUInt32(temp_index));
                            indices.Add(Convert.ToUInt32(temp_index - count));
                            indices.Add(Convert.ToUInt32(temp_index - count + 1));
                        }
                        if (temp_index % count > 0)
                        {
                            indices.Add(Convert.ToUInt32(temp_index));
                            indices.Add(Convert.ToUInt32(temp_index - count));
                            indices.Add(Convert.ToUInt32(temp_index - 1));
                        }
                    }
                }
                if (u == 0)
                {
                    count = vertices.Count;
                }
            }
        }

        public void createTorus(float center_x, float center_y, float center_z, float r1, float r2)
        {
            _centerPosition.X = center_x;
            _centerPosition.Y = center_y;
            _centerPosition.Z = center_z;

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            //int temp_index = -1;

            for (float u = 0; u <= 2 * pi; u += pi / 700)
            {
                for (float v = 0; v <= 2 * pi; v += pi / 700)
                {
                    //temp_index++;
                    temp_vector.X = center_x + (r1 + r2 * (float)Math.Cos(v)) * (float)Math.Cos(u);
                    temp_vector.Y = center_y + (r1 + r2 * (float)Math.Cos(v)) * (float)Math.Sin(u);
                    temp_vector.Z = center_z + r2 * (float)Math.Sin(v);
                    vertices.Add(temp_vector);

                    //indices.Add(Convert.ToUInt32(temp_index));
                }
            }
        }

        public void createBoxVertices(float x, float y, float z, float length, float uk_x, float uk_y, float uk_z)
        {
            _centerPosition.X = x;
            _centerPosition.Y = y;
            _centerPosition.Z = z;

            //kalau x dibesarin jadi persegi panjang horizontal
            //kalau y dibesarin jadi persegi panjang vertical

            Vector3 temp_vector;

            float _x = uk_x;
            float _y = uk_y;
            float _z = uk_z;

            //TITIK 1
            temp_vector.X = x - length / _x;
            temp_vector.Y = y - length / _y;
            temp_vector.Z = z - length / _z;
            vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / _x;
            temp_vector.Y = y - length / _y;
            temp_vector.Z = z - length / _z;
            vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x + length / _x;
            temp_vector.Y = y + length / _y;
            temp_vector.Z = z - length / _z;
            vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x - length / _x;
            temp_vector.Y = y + length / _y;
            temp_vector.Z = z - length / _z;
            vertices.Add(temp_vector);
            //TITIK 5
            temp_vector.X = x - length / _x;
            temp_vector.Y = y - length / _y;
            temp_vector.Z = z + length / _z;
            vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / _x;
            temp_vector.Y = y - length / _y;
            temp_vector.Z = z + length / _z;
            vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x + length / _x;
            temp_vector.Y = y + length / _y;
            temp_vector.Z = z + length / _z;
            vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x - length / _x;
            temp_vector.Y = y + length / _y;
            temp_vector.Z = z + length / _z;
            vertices.Add(temp_vector);

            indices = new List<uint>
            {

                 //front
                0, 7, 3,
                0, 4, 7,
                //back
                1, 2, 6,
                6, 5, 1,
                //left
                0, 2, 1,
                0, 3, 2,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //bottom
                0, 1, 5,
                0, 5, 4

            };

        }

        //public void rotate(float x, float y, float z)
        //{
        //    _model = _model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(y));
        //    _model = _model * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(x));
        //    _model = _model * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(z));
        //    //foreach (var meshobj in child)
        //    //{
        //    //    meshobj.rotate(x, y, z);
        //    //}
        //}

        //public void rotate(Vector3 pivot, Vector3 vector, float angle)
        //{
        //    //pivot > mau di rotate ke titik mana
        //    //vector > mau rotate ke sumbu apa (x, y, z)
        //    //angle > rotate berapa derajat

        //    var real_angle = angle;
        //    angle = MathHelper.DegreesToRadians(angle);

        //    //merotasi
        //    for (int i = 0; i < vertices.Count; i++)
        //    {
        //        vertices[i] = getRotationResult(pivot, vector, angle, vertices[i]);
        //    }
        //}

        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            //pivot -> mau rotate di titik mana
            //vector -> mau rotate di sumbu apa? (x,y,z)
            //angle -> rotatenya berapa derajat?

            angle = MathHelper.DegreesToRadians(angle);

            //mulai ngerotasi
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = getRotationResult(pivot, vector, angle, vertices[i]);
            }
            //rotate the euler direction
            for (int i = 0; i < 3; i++)
            {
                _euler[i] = getRotationResult(pivot, vector, angle, _euler[i], true);

                //NORMALIZE
                //LANGKAH - LANGKAH
                //length = akar(x^2+y^2+z^2)
                float length = (float)Math.Pow(Math.Pow(_euler[i].X, 2.0f) + Math.Pow(_euler[i].Y, 2.0f) + Math.Pow(_euler[i].Z, 2.0f), 0.5f);
                Vector3 temporary = new Vector3(0, 0, 0);
                temporary.X = _euler[i].X / length;
                temporary.Y = _euler[i].Y / length;
                temporary.Z = _euler[i].Z / length;
                _euler[i] = temporary;
            }
            _centerPosition = getRotationResult(pivot, vector, angle, _centerPosition);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes,
                vertices.ToArray(), BufferUsageHint.StaticDraw);
        }

        //public void translate(float x, float y, float z)
        //{
        //    //model *= Matrix4.CreateTranslation(x, y, z);
        //    for(int i = 0; i < vertices.Count; i++)
        //    {
        //        vertices[i] = Matrix4.CreateTranslation(x, y, z);
        //    }
        //}

        //Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        //{
        //    Vector3 temp, newPosition;
        //    if (isEuler)
        //    {
        //        temp = point;
        //    }
        //    else
        //    {
        //        temp = point - pivot;
        //    }

        //    newPosition.X =
        //        (float)temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
        //        (float)temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
        //        (float)temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));
        //    newPosition.Y =
        //        (float)temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
        //        (float)temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
        //        (float)temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));
        //    newPosition.Z =
        //        (float)temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
        //        (float)temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
        //        (float)temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

        //    if (isEuler)
        //    {
        //        temp = newPosition;
        //    }
        //    else
        //    {
        //        temp = newPosition + pivot;
        //    }
        //    return temp;
        //}

        public void resetEuler()
        {
            _euler[0] = new Vector3(1, 0, 0);
            _euler[1] = new Vector3(0, 1, 0);
            _euler[2] = new Vector3(0, 0, 1);
        }

        public Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;
            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));

            newPosition.Y =
                temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle));

            newPosition.Z =
                temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }

        //public void resetEuler()
        //{
        //    _euler[0] = new Vector3(1, 0, 0);
        //    _euler[1] = new Vector3(0, 1, 0);
        //    _euler[2] = new Vector3(0, 0, 1);
        //}
    }
}
