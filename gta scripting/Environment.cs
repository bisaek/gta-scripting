using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaScript
{
    class Environment
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public void define(string name, object value)
        {
            values.Add(name, value);
        }

        public object get(Token name)
        {
            if (values.ContainsKey(name.lexeme))
            {
                return values[name.lexeme];
            }

            throw new Exception("syntax error");
        }
    }
}
