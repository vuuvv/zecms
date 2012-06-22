using NHibernate;

using vuuvv.data;

namespace vuuvv.page.entities
{
    public class Page
    {
        public virtual int id { get; set; }
        public virtual Page parent { get; set; }
        public virtual int lft { get; set; }
        public virtual int rgt { get; set; }
        public virtual int level { get; set; }
        public virtual string title { get; set; }
        public virtual string slug { get; set; }
        public virtual string content { get; set; }
        public virtual bool is_published { get; set; }
        public virtual int in_navigation { get; set; }
    }

    public class PageManager
    {
        public Page model { get; set; }

        private ISession _session;
        public ISession session
        {
            get
            {
                if (_session == null)
                {
                    _session = NHibernateSessionManager.Instance.GetSession();
                }
                return _session;
            }
        }

        public void save()
        {
            session.Save(model);
        }
    }
}
