using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Amalgamate
{
    class TileSet
    {
        private bool _dimensionsRestricted;
        private int _maxWidth;
        private int _maxHeight;

        private List<string> _files;
        private List<string> _rejectedFiles;

        public TileSet()
        {
            _maxWidth = 0;
            _maxHeight = 0;
            _dimensionsRestricted = false;
            _files = new List<string>();
            _rejectedFiles = new List<string>();
        }

        public TileSet(int maxWidth, int maxHeight)
        {
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
            _dimensionsRestricted = true;
            _files = new List<String>();
            _rejectedFiles = new List<string>();
        }

        public bool AddFile(string path)
        {
            if (!File.Exists(path))
            {
                _rejectedFiles.Add(path);
                return false;
            }

            Image img;
            try
            {
                img = Image.FromFile(path);
                
            }
            catch (Exception)
            {
                _rejectedFiles.Add(path);
                return false;
            }

            if (this._dimensionsRestricted)
            {
                if (img.Width > _maxWidth || img.Height > _maxHeight)
                {
                    _rejectedFiles.Add(path);
                    return false;
                }
            }
            else
            {
                _maxWidth = Math.Max(_maxWidth, img.Width);
                _maxHeight = Math.Max(_maxHeight, img.Height);
            }
            _files.Add(path);

            return true;
        }

        public bool AddDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                _rejectedFiles.Add(path);
                return false;
            }

            foreach (var file in Directory.GetFiles(path))
            {
                this.AddFile(file);
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                this.AddDirectory(dir);
            }

            return true;
        }

        public void Amalgamate(string path)
        {
            int width = (int)Math.Ceiling(Math.Sqrt(_files.Count()));
            var height = width;

            var output = new Bitmap(width * _maxWidth, height * _maxHeight);
            Graphics g = Graphics.FromImage(output);

            var i = 0;
            for (var y = 0; y < output.Height; y += _maxHeight)
            {
                for (var x = 0; x < output.Width; x += _maxWidth)    
                {
                    if (i >= _files.Count())
                    {
                        break;
                    }
                    var img = Image.FromFile(_files[i++]);
                    g.DrawImage(img, x, y, _maxWidth, _maxHeight);
                    if (img.Width > 32 || img.Height > 32)
                        Console.WriteLine("Big image");
                }
            }

            output.Save(path);
        }

    }
}
