using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db
{
    class Criteria
    {
        private Dictionary<string, object> _filters;
        private Dictionary<string, object> _excludes;

        public Criteria filter(Dictionary<string,object> args)
        {
            return this;
        }

        public Criteria exclude()
        {
            return this;
        }

        public List<object> select()
        {
            return new List<object>();
        }

        public void delete()
        {
        }
    }
}
