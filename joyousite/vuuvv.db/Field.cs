using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.db
{
    public abstract class Field
    {
        public abstract object to_object(object value);

        public abstract string to_json(object value);

        public object clean(object value)
        {
            return to_object(value);
        }

    }

    public class StringField : Field
    {
        public int length { get; set; }

        public StringField()
        {
            this.length = 50;
        }

        public StringField(int length)
        {
            this.length = length;
        }

        public override object to_object(object value)
        {
            return value.ToString();
        }

        public override string to_json(object value)
        {
            return string.Format("\"{0}\"", value.ToString());
        }
    }

    public class IntegerField : Field
    {
        public override object to_object(object value)
        {
            Type t = value.GetType();
            if (t == typeof(int))
                return value;
            if (t == typeof(string))
                return int.Parse((string)value);
            return (int)value;
        }

        public override string to_json(object value)
        {
            return value.ToString();
        }
    }

    public class BooleanField : Field
    {
        public override object to_object(object value)
        {
            Type t = value.GetType();
            if (t == typeof(string))
            {
                if ((string)value == "true")
                    return true;
                if ((string)value == "false")
                    return false;
            }
            else if (t == typeof(int))
            {
                if ((int)value == 0)
                    return false;
                if ((int)value == 1)
                    return true;
            }
            return (bool)value;
        }

        public override string to_json(object value)
        {
            return value.ToString();
        }
    }
}
