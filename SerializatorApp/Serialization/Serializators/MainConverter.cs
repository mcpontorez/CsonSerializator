using SerializatorApp.Serializators.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SerializatorApp.Serialization.Serializators
{
    public class MainConverter : ICsonConverterBase
    {
        private readonly ICsonConverter _converter = new CsonItemConverter();

        public CsonData To(object source)
        {
            CsonData csonData = _converter.To(new CsData(source, new HashSet<Type>(), 0));
            string cson = $"{GetUsingText(csonData.Types)}{csonData.Cson};";


            return new CsonData(csonData.Types, cson);
        }

        private string GetUsingText(IEnumerable<Type> types)
        {
            //HashSet<string> namespaces = new HashSet<string>(types.Select(t => t.Namespace));
            //string result = string.Empty;
            //foreach (var item in namespaces)
            //    result += $"using {item}; {Environment.NewLine}";
            //return result;
            return string.Empty;
        }
    }
}
