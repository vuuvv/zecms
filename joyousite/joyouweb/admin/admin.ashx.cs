﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace joyouweb.admin
{
    /// <summary>
    /// Summary description for admin
    /// </summary>
    public class admin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            /*
            string action = context.Request.Params["action"];
            Type t = this.GetType();
            t.GetMethod(action).Invoke(this, new object[] { context });
            */
        }

        /*
        public void page_add(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            models.Page page = new models.Page();
            page.parent_id = Convert.ToInt32(context.Request.Params["parent_id"]);
            page.title = context.Request.Params["title"];
            page.content = context.Request.Params["content"];
            page.save();
            context.Response.Write(string.Format("{0}", page.id));
        }

        public void page_rename(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            int id = int.Parse(context.Request.Params["id"]);
            models.Page.update(id, new Dictionary<string, object>() {
                {"title", context.Request.Params["title"]}
            });
        }

        public void page_update(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            int id = int.Parse(context.Request.Params["id"]);
            models.Page page = models.Page.get(id);
            if (!string.IsNullOrEmpty(context.Request.Params["parent_id"]))
            {
                int parent_id = Convert.ToInt32(context.Request.Params["parent_id"]);
                if (parent_id != page.parent_id)
                {
                    page.move_to(models.Page.get(parent_id));
                }
            }
        }

        public void page_delete(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            models.Page.get(context.Request.Params["id"]).delete();
        }

        public void page_item(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            models.Page page = models.Page.get(context.Request.Params["id"]);
            context.Response.Write(models.Page.to_json(page, new string[]{"id", "parent_id", "title", "content", "parent"}));
        }

        public void page_tree(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            int id = int.Parse(context.Request.Params["id"]);
            List<models.Page> pages;
            if (id == -1)
            {
                pages = models.Page.all;
            }
            else
            {
                pages = models.Page.get(context.Request.Params["id"]).children;
            }
            List<string> json = new List<string>();
            foreach (models.Page p in pages)
            {
                json.Add(models.Page.to_json(p, new string[] { "id", "parent_id", "title", "content" }));
            }
            context.Response.Write(string.Format("[{0}]", string.Join(",", json.ToArray())));
        }
        */

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}