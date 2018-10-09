using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanNodeTree
{
    class NoRootException : Exception
    {
        public NoRootException() : base("Node data did not have a root node")
        {
        }
    }
}
