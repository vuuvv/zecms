using System;
using NHibernate;

namespace vuuvv.data
{
    public class Model
    {
        public virtual int id { get; set; }

        private ISession _session;
        public virtual ISession session
        {
            get
            {
                if (_session == null || !_session.IsOpen)
                {
                    _session = DBHelper.session;
                }
                return _session;
            }
        }

        public virtual void save()
        {
            session.Save(this);
        }

        public virtual void delete()
        {
            session.Delete(this);
        }

        public virtual void refresh()
        {
            session.Refresh(this);
        }

        public virtual void flush()
        {
            session.Flush();
        }
    }
}
