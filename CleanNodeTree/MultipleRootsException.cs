using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanNodeTree
{
    class MultipleRootsException : Exception
    {
        public MultipleRootsException() : base("Node data had multiple root nodes")
        {

        }
    }
}
