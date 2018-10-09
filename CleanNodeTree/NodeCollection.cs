using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CleanNodeTree
{

    public partial class HierarchyNode
    {

        /// <summary>
        /// Holds a collection of hierarchy nodes
        /// </summary>
        public class HierarchyCollection : IList<HierarchyNode>
        {
            List<HierarchyNode> _intern = new List<HierarchyNode>();

            internal HierarchyCollection() { }

            /// <summary>
            /// Gets or sets a child node at an index in this node
            /// </summary>
            /// <param name="index">Index of node to get or set</param>
            /// <returns></returns>
            public HierarchyNode this[int index]
            {
                get => _intern[index];
                set => _intern[index] = value;
            }

            /// <summary>
            /// Gets the child node by it's text
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public HierarchyNode this[string name]
            {
                get
                {
                    if (name.Contains(";"))
                    {
                        string current = name.Split(';').First();
                        string next = string.Join(";", name.Split(';').Skip(1).ToArray());

                        return this[current]?.Children[next];
                    }

                    if (this.Where(i => i.Name == name).Count() > 1)
                    {
                        throw new AmbiguousMatchException("More than one node was found with that name");
                    }

                    return _intern.Where(i => i.Name == name).FirstOrDefault();
                }
            }

            /// <summary>
            /// Gets the number of children this node has
            /// </summary>
            public int Count
            {
                get => _intern.Count;            
            }

            /// <summary>
            /// Gets the total amount of children nodes this node has
            /// </summary>
            public int TotalChildrenCount
            {
                get => this.Select(i => i.Children.TotalChildrenCount).Sum() + Count;
            }

            /// <summary>
            /// This property is always set to false and is part of the IList implementation
            /// </summary>
            public bool IsReadOnly
            {
                get => false;
            }

            /// <summary>
            /// Adds a node to this node's children
            /// </summary>
            /// <param name="item">Child to add</param>
            public void Add(HierarchyNode item)
            {
                _intern.Add(item);
            }

            /// <summary>
            /// Adds a node to this node's children
            /// </summary>
            /// <param name="item">Child to add</param>
            public void Add(object item)
            {
                _intern.Add(new HierarchyNode(item));
            }


            /// <summary>
            /// Adds a range of nodes to this node
            /// </summary>
            /// <param name="items">Nodes to add</param>
            public void AddRange(IEnumerable<HierarchyNode> items)
            {
                _intern.AddRange(items);
            }

            /// <summary>
            /// Adds a range of nodes to this node
            /// </summary>
            /// <param name="items">Nodes to add</param>
            public void AddRange<T>(IEnumerable<T> items)
            {
                if (typeof(T).Equals(typeof(HierarchyNode)))
                {
                    AddRange(items.Cast<HierarchyNode>());
                }

                _intern.AddRange(from i in items select new HierarchyNode(i));
            }

            /// <summary>
            /// Removes all children from this node
            /// </summary>
            public void Clear()
            {
                _intern.Clear();
            }

            /// <summary>
            /// Returns true if this node has a specific child
            /// </summary>
            /// <param name="item">Child to check</param>
            /// <returns></returns>
            public bool Contains(HierarchyNode item)
            {
                return _intern.Contains(item);
            }

            /// <summary>
            /// Copies all children to a Node array
            /// </summary>
            /// <param name="array">Array to copy to</param>
            /// <param name="arrayIndex">Index to start writing to in the target array</param>
            public void CopyTo(HierarchyNode[] array, int arrayIndex)
            {
                _intern.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// Gets the IEnumerator of this collection
            /// </summary>
            /// <returns></returns>
            public IEnumerator<HierarchyNode> GetEnumerator()
            {
                return _intern.GetEnumerator();
            }

            /// <summary>
            /// Gets the index of a child of this node
            /// </summary>
            /// <param name="item">Item to get the index of</param>
            /// <returns></returns>
            public int IndexOf(HierarchyNode item)
            {
                return _intern.IndexOf(item);
            }

            /// <summary>
            /// Inserts a child node somewhere in between the other children
            /// </summary>
            /// <param name="index">Index to insert at</param>
            /// <param name="item">Child to insert</param>
            public void Insert(int index, HierarchyNode item)
            {
                _intern.Insert(index, item);
            }

            /// <summary>
            /// Inserts a child node somewhere in between the other children
            /// </summary>
            /// <param name="index">Index to insert at</param>
            /// <param name="item">Child to insert</param>
            public void Insert(int index, string item)
            {
                _intern.Insert(index, new HierarchyNode(item));
            }

            /// <summary>
            /// Inserts a range of nodes somewhere in between the other children
            /// </summary>
            /// <param name="startIndex">Index to start insertion at</param>
            /// <param name="items">Items to add</param>
            public void InsertRange(int startIndex, IEnumerable<HierarchyNode> items)
            {
                _intern.InsertRange(startIndex, items);
            }

            /// <summary>
            /// Inserts a range of nodes somewhere in between the other children
            /// </summary>
            /// <param name="startIndex">Index to start insertion at</param>
            /// <param name="items">Items to add</param>
            public void InsertRange(int startIndex, IEnumerable<string> items)
            {
                _intern.InsertRange(startIndex, from i in items select new HierarchyNode(i));
            }



            /// <summary>
            /// Removes a child from this node
            /// </summary>
            /// <param name="item">Child to remove</param>
            /// <returns></returns>
            public bool Remove(HierarchyNode item)
            {
                return _intern.Remove(item);
            }

            /// <summary>
            /// Removes the child at a specific postion from this node
            /// </summary>
            /// <param name="index">Index of child to remove</param>
            public void RemoveAt(int index)
            {
                _intern.RemoveAt(index);
            }

            /// <summary>
            /// Moves the specified child to a new index
            /// </summary>
            /// <param name="item">Child to move</param>
            /// <param name="newIndex">New index to give child</param>
            public void Move(HierarchyNode item, int newIndex)
            {
                if (!Remove(item))
                {
                    throw new ArgumentException("Child was not found in collection");
                }

                Insert(newIndex, item);

            }

            /// <summary>
            /// Returns the name of every child node of this NodeCollection
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> AsStringEnumerable()
            {
                return this.Select(i => i.Name);
            }


            /// <summary>
            /// Returns the name of every child node of this NodeCollection cast as integers
            /// </summary>
            /// <returns></returns>
            public IEnumerable<int> AsIntEnumerable()
            {
                foreach (HierarchyNode i in this)
                {
                    if (int.TryParse(i.Name, System.Globalization.NumberStyles.Integer, HierarchyNode.c, out int n))
                    {
                        yield return n;
                        continue;
                    }

                    throw new InvalidOperationException("One or more children nodes are not integer");
                }
            }

            /// <summary>
            /// Returns the name of every child node of this NodeCollection cast as floats
            /// </summary>
            /// <returns></returns>
            public IEnumerable<float> AsFloatEnumerable()
            {
                foreach (HierarchyNode i in this)
                {
                    if (float.TryParse(i.Name, System.Globalization.NumberStyles.Integer, HierarchyNode.c, out float n))
                    {
                        yield return n;
                        continue;
                    }

                    throw new InvalidOperationException("One or more children nodes are not floats");
                }
            }

            /// <summary>
            /// Returns the name of every child node of this NodeCollection cast as doubles
            /// </summary>
            /// <returns></returns>
            public IEnumerable<double> AsDoubleEnumerable()
            {
                foreach (HierarchyNode i in this)
                {
                    if (double.TryParse(i.Name, System.Globalization.NumberStyles.Integer, HierarchyNode.c, out double n))
                    {
                        yield return n;
                        continue;
                    }

                    throw new InvalidOperationException("One or more children nodes are not doubles");
                }
            }

            /// <summary>
            /// Returns the name of every child node of this NodeCollection cast as booleans
            /// </summary>
            /// <returns></returns>
            public IEnumerable<bool> AsBooleanEnumerable()
            {
                foreach (HierarchyNode i in this)
                {
                    if (i.Name == "1")
                    {
                        yield return true;
                    }
                    else if (i.Name == "0")
                    {
                        yield return false;
                    }
                    else
                    {
                        throw new InvalidOperationException("One or more children nodes are not boolean");
                    }
                }
            }

            /// <summary>
            /// Gets the IEnumerator of this collection
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _intern.GetEnumerator();
            }
        }
    }
}
