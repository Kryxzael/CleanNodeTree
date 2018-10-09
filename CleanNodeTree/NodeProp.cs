using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CleanNodeTree
{
    public partial class HierarchyNode : ICloneable
    {
        string _name;

        /// <summary>
        /// Gets or sets the text of this node
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value.Replace(";", "_").Trim();
        }        

        /// <summary>
        /// The children nodes of this node
        /// </summary>
        public HierarchyCollection Children { get; } = new HierarchyCollection();

        /// <summary>
        /// True if this node has a single child
        /// </summary>
        public bool IsDataNode
        {
            get => Children.Count == 1;
        }

        /// <summary>
        /// True if this node has any children
        /// </summary>
        public bool HasChildren
        {
            get => Children.Any();
        }

        /// <summary>
        /// Returns the first child of his node that has a specific name
        /// </summary>
        /// <param name="name">Name of node to get</param>
        /// <returns></returns>
        public HierarchyNode this[string name]
        {
            get => Children[name];
        }

        /// <summary>
        /// Checks if this node contains another node at any level inside it
        /// </summary>
        /// <param name="node">Node to check for</param>
        /// <returns></returns>
        public bool HasChild(HierarchyNode node)
        {
            foreach (HierarchyNode i in Children)
            {
                if (i == node)
                {
                    return true;
                }

                if (i.HasChild(node))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Retrives all the children nodes of this node as a one-dimensional enumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HierarchyNode> GetAllChildren()
        {
            var _ = new List<HierarchyNode>();

            foreach (var i in Children)
            {
                _.Add(i);
                _.AddRange(i.GetAllChildren());
            }

            return _.AsReadOnly();
        }

        /// <summary>
        /// Checks if this node contains another node with the given name at any level inside it
        /// </summary>
        /// <param name="nodeName">Name of node to check for</param>
        /// <returns></returns>
        public bool HasChild(object nodeName)
        {
            return GetNodeWithName(nodeName) != null;
        }

        /// <summary>
        /// Retrives a node from any level inside this node, that has the given name
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public HierarchyNode GetNodeWithName(object nodeName)
        {
            return GetNodeThatMatches(i => i.Name == ObjectToString(nodeName));
        }

        /// <summary>
        /// Retrives a node from any level inside this node, that has the given name
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public IEnumerable<HierarchyNode> GetEveryNodeWithName(object nodeName)
        {
            return GetEveryNodeThatMatches(i => i.Name == ObjectToString(nodeName));
        }

        /// <summary>
        /// Gets the first node (from top to bottom) that matches a predicate
        /// </summary>
        /// <param name="predicate">Predicate to test against</param>
        /// <returns></returns>
        public HierarchyNode GetNodeThatMatches(Predicate<HierarchyNode> predicate)
        {
            foreach (var i in Children)
            {
                if (predicate(i))
                {
                    return i;
                }
            }

            foreach (var i in Children)
            {
                var _ = i.GetNodeThatMatches(predicate);

                if (_ != null)
                {
                    return _;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets every node recursivly that matches a predicate
        /// </summary>
        /// <param name="predicate">Predicate to test against</param>
        /// <returns></returns>
        public IEnumerable<HierarchyNode> GetEveryNodeThatMatches(Predicate<HierarchyNode> predicate)
        {
            var _ = new List<HierarchyNode>();

            foreach (var i in Children)
            {
                if (predicate(i))
                {
                    _.Add(i);
                }

                _.AddRange(i.GetEveryNodeThatMatches(predicate));
            }

            return _.AsReadOnly();
        }

        /// <summary>
        /// Transforms the hierarchy using a standard LINQ select procedure by returning a new node for every node
        /// </summary>
        /// <param name="selector">Selector to use for select procedure</param>
        /// <returns></returns>
        public HierarchyNode Select(Func<HierarchyNode, HierarchyNode> selector)
        {
            var _ = selector(this);

            foreach (var i in Children)
            {
                _.Add(i.Select(selector));
            }

            return _;
        }

        /// <summary>
        /// Transforms the hierarchy using a standard LINQ select procedure by returning a new name for every node
        /// </summary>
        /// <param name="selector">Selector to use for select procedure</param>
        /// <returns></returns>
        public HierarchyNode Select(Func<HierarchyNode, object> selector)
        {
            return Select(i => new HierarchyNode(selector(i)));
        }

        /// <summary>
        /// Filters the tree recursivly removing branches that does not match a predicate
        /// </summary>
        /// <param name="predicate">Predicate used for filter</param>
        /// <returns></returns>
        public HierarchyNode Where(Predicate<HierarchyNode> predicate)
        {
                        //DO NOT replace with this.Clone()
            var _ = new HierarchyNode(Name);

            foreach (var i in Children)
            {
                if (predicate(i))
                {
                    _.Add(i.Where(predicate));
                }
            }

            return _;
        }

        /// <summary>
        /// Gets the parent node of a child that is contained at any level inside this node
        /// </summary>
        /// <param name="child">Node to get parent of</param>
        /// <returns></returns>
        public HierarchyNode GetParentOfChild(HierarchyNode child)
        {
            if (!HasChild(child))
            {
                return null;
            }

            RecursiveSeachForParent(this, child, out HierarchyNode p);
            return p;

        }


        private bool RecursiveSeachForParent(HierarchyNode n, HierarchyNode t, out HierarchyNode parent)
        {
            if (n.Children.Contains(t))
            {
                parent = n;
                return true;
            }
            foreach (HierarchyNode i in n.Children)
            {
                if (RecursiveSeachForParent(i, t, out parent))
                {
                    return true;
                }
            }

            parent = null;
            return false;
        }

        /// <summary>
        /// Sets the data of this node, if it is a datanode
        /// </summary>
        /// <param name="value">Data to set</param>
        public void SetData(object value)
        {
            SetData(new HierarchyNode(value));
        }

        /// <summary>
        /// Sets the data of this node, if it is a datanode
        /// </summary>
        /// <param name="node">Data to set</param>
        public void SetData(HierarchyNode node)
        {
            if (!IsDataNode)
            {
                throw new InvalidOperationException("This node has more than one child and is therefor not considered a data node");
            }

            if (HasChildren)
            {
                Children.Clear();
            }

            Children.Add(node);

        }

        /// <summary>
        /// Gets the single-child data of this node as a string
        /// </summary>
        /// <returns></returns>        
        public string String
        {
            get => ValueNode.Name;
            set => SetData(value);
        }

        /// <summary>
        /// Gets the integer value of the data node of this node
        /// </summary>
        /// <returns></returns>
        public int Int
        {
            get
            {
                if (int.TryParse(String, System.Globalization.NumberStyles.Integer, c, out int i))
                {
                    return i;
                }

                throw new InvalidOperationException("Data node does not contain an integer");
            }
            set => SetData(value);
        }

        /// <summary>
        /// Gets the double value of the data node of this node
        /// </summary>
        /// <returns></returns>
        public double Double
        {
            get
            {
                if (double.TryParse(String, System.Globalization.NumberStyles.Float, c, out double i))
                {
                    return i;
                }

                throw new InvalidOperationException("Data node does not contain an number");
            }
            set => SetData(value);
        }

        /// <summary>
        /// Gets the float value of the data node of this node
        /// </summary>
        /// <returns></returns>
        public double Float
        {
            get
            {
                if (float.TryParse(String, System.Globalization.NumberStyles.Float, c, out float i))
                {
                    return i;
                }

                throw new InvalidOperationException("Data node does not contain an number");
            }
            set => SetData(value);
        }

        /// <summary>
        /// Gets the boolean value of the data node of this node
        /// </summary>
        /// <returns></returns>
        public bool Boolean
        {
            get
            {
                string s = String;

                if (s == "1")
                {
                    return true;
                }
                else if (s == "0")
                {
                    return false;
                }
                throw new InvalidOperationException("Data node does not contain a boolean");
            }
            set => SetData(value);
        }


        /// <summary>
        /// Gets the single-child data node of this node
        /// </summary>
        /// <returns></returns>
        public HierarchyNode ValueNode
        {
            get
            {
                if (!IsDataNode)
                {
                    throw new InvalidOperationException("This node has more than one child and is therefor not considered a data node");
                }
                if (!HasChildren)
                {
                    return null;
                }

                return Children[0];
            }
            set => SetData(value);
        }

        object ICloneable.Clone()
        {
            return Select(i => new HierarchyNode(i.Name));
        }

        /// <summary>
        /// Creates an exact duplicate of this node and its children.
        /// </summary>
        /// <returns></returns>
        public HierarchyNode Clone()
        {
            return (this as ICloneable).Clone() as HierarchyNode;
        }

        /// <summary>
        /// Returns the string representation of this node
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsDataNode && Children.Count != 0)
            {
                return "(" + Name + "):(" + String + ")";
            }

            if (Children.Count == 1)
            {
                return "(" + Name + ") + 1 child";
            }
            else if (Children.Count == 0)
            {
                return "(" + Name + ")";
            }

            return "(" + Name + ") + " + Children.Count + " children";


        }


    }
}
