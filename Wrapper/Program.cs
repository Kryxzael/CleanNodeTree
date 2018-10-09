using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanNodeTree;
using System.Windows.Forms;
using System.Diagnostics;

namespace Wrapper
{
    class Program
    {
        public static void Main(string[] args)
        {
            var node = new HierarchyNode("A", Enumerable.Range(0, 100));

            Console.WriteLine(node.Children.AsIntEnumerable());
        }
    }
}
