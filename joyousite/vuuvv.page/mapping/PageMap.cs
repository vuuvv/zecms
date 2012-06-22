using FluentNHibernate.Mapping;

using vuuvv.page.entities;

namespace vuuvv.page.mapping
{
    public class PageMap : ClassMap<Page>
    {
        public PageMap() 
        {
            Id(x => x.id);
            Map(x => x.lft);
            Map(x => x.rgt);
            Map(x => x.level);
            Map(x => x.title);
            Map(x => x.slug);
            Map(x => x.content);
            Map(x => x.is_published);
            Map(x => x.in_navigation);
            References(x => x.parent);
        }
    }
}
