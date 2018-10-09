using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanNodeTree
{
    class InvalidIndentException : Exception
    {
        public InvalidIndentException() : base("A node in the tree had invalid indent")
        {

        }
    }
}
