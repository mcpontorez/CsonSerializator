using SerializatorApp.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp
{
    class Size
    {
        public float x, y;
    }
    class SimplePerson
    {
        public int Id;
        public string Name;
        public Size Size;
    }
    class Program
    {
        static void Main(string[] args)
        {
            SimplePerson simplePerson = new SimplePerson
            { 
                Id = 12, 
                Name = "Boris", 
                Size = new Size 
                { 
                    x = 10F, 
                    y = 15F
                } 
            };

            ICsonConverter converter = new MainConverter();
            Console.WriteLine(converter.To(simplePerson).Cson);

            Console.Read();
        }
    }
}
