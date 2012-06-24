using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Linq;

using vuuvv.data;
using vuuvv.page.entities;


namespace vuuvv.test
{
    [TestClass]
    public class DBHelperTest
    {
        [TestMethod]
        public void TestSession()
        {
            ISession session = DBHelper.session;
            Assert.IsNotNull(session);
            DBHelper.close_session();
        }

        [TestMethod]
        public void TestRawSql()
        {
            ISession session = DBHelper.session;
            using (IDbCommand cmd = session.Connection.CreateCommand())
            {
                cmd.CommandText = "Select 1";
                Assert.AreEqual(1, Convert.ToInt32(cmd.ExecuteScalar()));
            }
            DBHelper.close_session();
        }
    }

    [TestClass]
    public class PageTest
    {
        [TestInitialize]
        public void setup()
        {
            // 1
            Page p = new Page();
            p.save();

            // 2,3,4
            Page c = new Page();
            c.parent = p;
            c.save();

            p = c;
            c = new Page();
            c.parent = p;
            c.parent.refresh();
            c.save();

            c = new Page();
            c.parent = p;
            c.parent.refresh();
            c.save();

            // 5,6,7,8,9
            p = p.parent;
            c = new Page();
            c.parent = p;
            c.parent.refresh();
            c.save();

            p = c;
            c = new Page();
            c.parent = p;
            c.parent.refresh();
            c.save();

            c = new Page();
            c.parent = p;
            c.parent.refresh();
            c.save();

            p = c;
            c = new Page();
            c.parent = p;
            c.parent.refresh();
            c.save();

            p = p.parent;
            c = new Page();
            c.parent = p;
            c.parent.refresh();
            c.save();
        }

        [TestMethod]
        public void TestInsert()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestSave()
        {
        }
    }
}
