using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CleanNodeTree
{
    /// <summary>
    /// Represents a single node of a NodeTree
    /// </summary>
    public partial class HierarchyNode
    {
        /// <summary>
        /// Creates a new node
        /// </summary>
        /// <param name="name">Text of the node</param>
        public HierarchyNode(object name)
        {
            Name = ObjectToString(name);
        }

        /// <summary>
        /// Creates a new node with one child
        /// </summary>
        /// <param name="name">Text of the node</param>
        /// <param name="innerData">Text of this node's child</param>
        public HierarchyNode(object name, object innerData) : this(name)
        {
            Children.Add(innerData);
        }

        /// <summary>
        /// Creates a new node with children nodes
        /// </summary>
        /// <param name="name">Text of the node</param>
        /// <param name="children">Children of this node</param>
        public HierarchyNode(object name, HierarchyNode[] children) : this(name) 
        //Warning. Do NOT replace HierarchyNode[] with IEnumerable<HierarchyNode>. Unexpected results
        {
            Children.AddRange(children.AsEnumerable());
        }

        /// <summary>
        /// Creates a new node with children nodes
        /// <param name="name">Text of the node</param>
        /// <param name="childrenNames">Names of the children of this node</param>
        /// </summary>
        public HierarchyNode(object name, object[] childrenNames) : this(name)
        {
            Children.AddRange(childrenNames);
        }


        /// <summary>
        /// Adds a node to this node's children
        /// </summary>
        /// <param name="node">Node to add</param>
        public HierarchyNode Add(HierarchyNode node)
        {
            Children.Add(node);
            return node;
        }

        /// <summary>
        /// Creates a node and adds it to this node's children
        /// </summary>
        /// <param name="name">Name of node to create and add</param>
        public HierarchyNode Add(object name)
        {
            var n = new HierarchyNode(name);
            Children.Add(n);
            return n;
        }

        /// <summary>
        /// Adds a range of nodes to the node
        /// </summary>
        /// <param name="nodes">Nodes to add</param>
        /// <returns></returns>
        public IEnumerable<HierarchyNode> AddRange(IEnumerable<HierarchyNode> nodes)
        {
            Children.AddRange(nodes);
            return nodes;
        }

        /// <summary>
        /// Adds a range of nodes (with the given names) to the node
        /// </summary>
        /// <typeparam name="T">Type to remain implicit</typeparam>
        /// <param name="names">Name of nodes</param>
        /// <returns></returns>
        public IEnumerable<HierarchyNode> AddRange<T>(IEnumerable<T> names)
        {
            var n = from i in names select new HierarchyNode(i);
            Children.AddRange(n);
            return n;
        }

        /// <summary>
        /// Creates a datanode and adds it to this node's children
        /// </summary>
        /// <param name="name">Name of node to create and add</param>
        /// <param name="innerData">Name of node to add to the datanode</param>
        public HierarchyNode Add(object name, object innerData)
        {
            var n = new HierarchyNode(name, innerData);
            Children.Add(n);
            return n;
        }

        internal static CultureInfo c = CultureInfo.GetCultureInfo(0x0409);

        static string ObjectToString(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            if (obj is string)
            {
                return (string)obj;
            }
            else if (obj is byte)
            {
                return ((byte)obj).ToString(c);
            }
            else if (obj is short)
            {
                return ((short)obj).ToString(c);
            }
            else if (obj is int)
            {
                return ((int)obj).ToString(c);
            }
            else if (obj is long)
            {
                return ((long)obj).ToString(c);
            }
            else if (obj is sbyte)
            {
                return ((sbyte)obj).ToString(c);
            }
            else if (obj is ushort)
            {
                return ((ushort)obj).ToString(c);
            }
            else if (obj is uint)
            {
                return ((uint)obj).ToString(c);
            }
            else if (obj is ulong)
            {
                return ((ulong)obj).ToString(c);
            }
            else if (obj is float)
            {
                return ((float)obj).ToString(c);
            }
            else if (obj is double)
            {
                return ((double)obj).ToString(c);
            }
            else if (obj is decimal)
            {
                return ((decimal)obj).ToString(c);
            }
            else if (obj is bool)
            {
                if (obj.Equals(true))
                {
                    return "1";
                }
                return "0";
            }

            return obj.ToString();
        }
    }
}
