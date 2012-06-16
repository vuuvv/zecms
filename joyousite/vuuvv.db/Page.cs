using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.db
{
    public class Page : TreeModel
    {
        public new static string table = "pages";
        public new static Dictionary<string, Field> fields = new Dictionary<string, Field>()
        {
            {"slug", new StringField()},
            {"title", new StringField()},
            {"content", new StringField()},
            {"is_published", new BooleanField()},
            {"in_navigation", new IntegerField()},
        };

        static Page()
        {
            foreach (var item in TreeModel.fields)
            {
                fields[item.Key] = item.Value;
            }
        }
    }
}
