<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="pagetree.ascx.cs" Inherits="joyouweb.controls.pagetree" %>
<div id="tree">

<ul>
<% 
    System.Collections.Generic.Stack<models.Page> s = new System.Collections.Generic.Stack<models.Page>();
    foreach (models.Page page in pages) { 
%>
    <% 
        if (s.Count > 0)
        {
            models.Page top = s.Peek();
            if (top.tree_id != page.tree_id)
            {
                while(s.Count > 0) 
                {
                    s.Pop();
                    Response.Write("</ul></li>");
                }
            }
            else if (top.rgt < page.rgt)
            {
                while((top = s.Peek()).rgt < page.rgt) 
                {
                    s.Pop();
                    Response.Write("</ul></li>");
                }
            }
    %>
    <%
        }
    %>
        <li id="ordering-<%= page.id %>">
            <div class="row1">
                <a href="javascript:;" class="tree-toggle" id="page-<%= page.id %>">
                    <i class="icon-plus open"></i>
                    <i class="icon-minus close"></i>
                </a>
                <a href="javascript:;" class="delete icon-remove"></a>
                <a href="javascirpt:;" class="changelink"><%= page.title %></a>
                <span class="ordering">
                    <i class="icon-chevron-up"></i>
                    <i class="icon-chevron-down"></i>
                </span>
                <select class="addlist" id="addlink-<%= page.id %>">
                    <option value="">Add...</option>
                    <option value="">Rich Text page</option>
                </select>
            </div>
            <br style="clear:both;" />
    <%
        if (page.is_leaf)
        {
    %>
        </li>
    <%
        }
    %>
    <% 
        if (!page.is_leaf) 
        { 
            s.Push(page);
    %>
       <ul>
    <%  } %>
<%  
    }
    while(s.Count > 0) 
    {
        s.Pop();
        Response.Write("</ul></li>");
    }
%>
</ul>

</div>
<link href="/Styles/page_tree.css" rel="stylesheet" type="text/css" />
<link href="/Styles/typography.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/page_tree.js" type="text/javascript""></script>