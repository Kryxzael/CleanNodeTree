using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanNodeTree
{
    internal struct NodeParseData
    {
        internal string Text { get; set; }
        internal int Depth { get; set; }

        internal NodeParseData(string text, int depth)
        {
            Text = text;
            Depth = depth;
        }
    }
}
