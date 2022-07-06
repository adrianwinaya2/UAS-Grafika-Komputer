using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grafkom_proyek
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1800, 900),
                Title = "Mickey Mouse Clubhouse"
            };

            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }

            //open clc++ (0,0,0) itu mulai dari kiri atas, karena blm di normalisasi
            //kalo open tk itu udah dinormalisasi, jadi (0,0) di tengah
        }
    }
}
