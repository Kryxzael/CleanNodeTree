using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CleanNodeTree
{
    public partial class HierarchyNode
    {
        /// <summary>
        /// Creates a node from a CleanNodeTree file
        /// </summary>
        /// <param name="path">File to read</param>
        /// <returns></returns>
        public static HierarchyNode FromFile(string path)
        {
            return FromString(File.ReadAllText(path));
        }

        /// <summary>
        /// Creates a node from raw ClearNodeTree data
        /// </summary>
        /// <param name="data">Data to convert</param>
        /// <returns></returns>
        public static HierarchyNode FromString(string data)
        {
            //Clears whitespace
            data = data.Replace("\n", "").Replace("\r", "").Replace("\t", "    ");

            //Creates list of entries split by semicolon
            NodeParseData[] entries = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(i => new NodeParseData(i.TrimStart(), GetDepthIndex(i))).ToArray();

            //Failsafe for roots
            if (entries.First().Depth != 0)
            {
                throw new NoRootException();
            }
            else if (entries.Where(i => i.Depth == 0).Count() > 1)
            {
                throw new MultipleRootsException();
            }

            //Creates root node
            return PopultateNode(0, entries);
        }

        static HierarchyNode PopultateNode(int index, NodeParseData[] entries)
        {
            //Creates node to return and stores it's local depth for easy access
            var _ = new HierarchyNode(entries[index].Text);
            int d = entries[index].Depth;

            //Runs through children
            for (int i = index + 1; i < entries.Length; i++)
            {
                //Failsafe for invalid format
                if (i != 0)
                {
                    if (entries[i].Depth - 1 > entries[i - 1].Depth)
                    {
                        throw new InvalidIndentException();
                    }
                }


                //If child is found
                if (entries[i].Depth == d + 1)
                {
                    //Recursive add child
                    _.Children.Add(PopultateNode(i, entries));
                }
                //If sibling is found
                else if (entries[i].Depth <= d)
                {
                    break;
                }
            }

            return _;
        }

        static int GetDepthIndex(string line)
        {
            int a = line.Length - line.TrimStart(' ').Length;

            if (a % 4 != 0)
            {
                throw new InvalidIndentException();
            }

            return a / 4;
        }

        /// <summary>
        /// Serializes this node as raw string data
        /// </summary>
        /// <returns></returns>
        public string ToStringData()
        {
            return WriteMyContents(0);
        }

        string WriteMyContents(int depth)
        {
            string _ = new string(' ', depth * 4) + Name + ";" + Environment.NewLine;

            foreach (HierarchyNode i in Children)
            {
                _ += i.WriteMyContents(depth + 1);
            }

            return _;
        }

        string CreateSafeString(string str)
        {
            return str.Replace("{", "\\{").Replace("}", "\\}").Replace(",", "\\,").Replace("\\", "\\\\");
        }


        /// <summary>
        /// Serializes this node in a file
        /// </summary>
        /// <param name="path"></param>
        public void ToFile(string path)
        {
            File.WriteAllText(path, ToStringData());
        }
    }
}
