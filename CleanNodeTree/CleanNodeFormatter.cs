using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CleanNodeTree
{
    /// <summary>
    /// Formatter that support serialization of a serializable object to a HierarchyNode
    /// </summary>
    public class CleanNodeFormatter : IFormatter
    {
        public ISurrogateSelector SurrogateSelector { get; set; }
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }

        public CleanNodeFormatter()
        {
            Context = new StreamingContext(StreamingContextStates.All);
        }

        public object Deserialize(Stream serializationStream)
        {
            //Open node from stream
            HierarchyNode node = HierarchyNode.FromStream(serializationStream);

            //Get the type of object from root name
            Type type;
            try
            {
                type = Type.GetType(node.Name);
            }
            catch (Exception)
            {
                
            }
            


        }

        public void Serialize(Stream serializationStream, object graph)
        {
            throw new NotImplementedException();
        }
    }
}
