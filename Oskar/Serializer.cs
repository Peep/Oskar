using System;
using System.Runtime.Serialization;

namespace Oskar
{
    // Use typeof(T) to get a type object
    // Use reflection on it to get it's fields and properties
    // From the properties you can get accessors and setters
    // Which you can use to read and write values to the fields
    [Serializable]
    public class Serializer<T> : ISerializable
    {
        public readonly T Inner;

        private readonly Type _type;
        private int index; 
        

        public Serializer(T t)
        {
            Inner = t;
            _type = Inner.GetType();
        }

        // Sort the property list from the reflected type in alphabetical order
        // then..
        // Use a for loop to iterate through each property
        // and .ToString() the indexer and assign it to GetValue()

        // The fields and properties need to be looped through independently
        // which requires two separate for loops. You'll need to make sure 
        // that the for loop indexers don't collide in the info dictionary. 
        // Use some outer variable to start the second for loop 
        // where the first one ends + 1

        protected Serializer(SerializationInfo info, StreamingContext context)
        {
            var properties = _type.GetProperties();
            Array.Sort(properties);

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof (string))
                {
                    var s = property.GetValue(this, null) as string;
                    info.AddValue(index.ToString(), s);
                }

                index++;
            }

            // http://msdn.microsoft.com/en-us/library/ty01x675%28v=vs.110%29.aspx
        }

        // Iterate through the property list in the reflected type once again
        // using the same for loop method used above. The indexers should line up.

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
