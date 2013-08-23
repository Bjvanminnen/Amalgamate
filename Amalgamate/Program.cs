using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Amalgamate
{
    class Program
    {
        static void Main(string[] args)
        {
            var tileset = new TileSet(32, 32);
            
            foreach (var arg in args)
            {
                var path = Path.GetFullPath(arg);
                var isFile = File.Exists(path);
                var isDir = Directory.Exists(path);

                if (isFile)
                {
                    tileset.AddFile(path);
                }
                else if (isDir)
                {
                    tileset.AddDirectory(path);
                }
                else
                {
                    Console.WriteLine("\"{0} does not exist\"", arg);
                }
            }

            var output = @"c:\tmp\output.png";
            tileset.Amalgamate(output);
        
        }
    }
}
