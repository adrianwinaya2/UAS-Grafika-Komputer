using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafkom_proyek
{
    class ObjVolume : Asset3d
    {
        public static Asset3d LoadFromFile(string filename)
        {
            Asset3d obj = new Asset3d();
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(reader.ReadToEnd());
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: {0}", filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading file: {0}", filename);
            }

            return obj;
        }

        public static Asset3d LoadFromString(string obj)
        {
            List<String> lines = new List<string>(obj.Split('\n')); //split file menjadi per baris
            List<Vector3> tempVertices = new List<Vector3>();
            List<Vector3> tempNormals = new List<Vector3>();

            Asset3d asset = new Asset3d();
            asset._vertices = new List<Vector3>();
            asset._normals = new List<Vector3>();

            foreach (String line in lines)
            {
                String[] data = line.Split(' '); 
                if (line.StartsWith("v ")) // Vertex definition
                {
                    Vector3 vec = new Vector3();
                    if (line.Count((char c) => c == ' ') == 3) 
                    {
                        vec.X = float.Parse(data[1], CultureInfo.InvariantCulture.NumberFormat);
                        vec.Y = float.Parse(data[2], CultureInfo.InvariantCulture.NumberFormat);
                        vec.Z = float.Parse(data[3], CultureInfo.InvariantCulture.NumberFormat);
                    }
                    tempVertices.Add(vec);
                }
                else if (line.StartsWith("vn ")) // normal definition
                {
                    Vector3 vec = new Vector3();
                    vec.X = float.Parse(data[1], CultureInfo.InvariantCulture.NumberFormat);
                    vec.Y = float.Parse(data[2], CultureInfo.InvariantCulture.NumberFormat);
                    vec.Z = float.Parse(data[3], CultureInfo.InvariantCulture.NumberFormat);
                    tempNormals.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    String temp = line.Substring(2);
                    String[] vertexparts;
                    String[] fparts = temp.Split(' ');
                    for (int i = 0; i < fparts.Length; i++)
                    {
                        vertexparts = fparts[i].Split('/'); 
                        int indexvert = int.Parse(vertexparts[0]);
                        int indexnorm = int.Parse(vertexparts[2]);
                        asset._vertices.Add(tempVertices[indexvert - 1]);
                        asset._normals.Add(tempNormals[indexnorm - 1]);
                    }
                }
            }
            return asset; 
        }

    }
}
