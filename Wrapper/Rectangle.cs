using CleanNodeTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wrapper
{
    public struct Rectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public HierarchyNode SerializeNode()
        {
            var _ = new HierarchyNode("Rect");
            _.Add("x", X);
            _.Add("y", Y);
            _.Add("w", Width);
            _.Add("h", Height);

            return _;
        }
    }
}
