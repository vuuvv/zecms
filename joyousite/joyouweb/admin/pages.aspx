<%@ Page Title="" Language="C#" EnableViewState="false" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true" CodeBehind="pages.aspx.cs" Inherits="joyouweb.admin.Pages" %>
<%@ Register TagName="pagetree" src="~/controls/pagetree.ascx" TagPrefix="vuuvv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<link href="/Styles/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery.ztree.all-3.2.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<ul id="page_tree" class="ztree"> </ul>
<script type="text/javascript">

var setting = {
    data: {
        simpleData: {
            enable: true
        }
    },
    view: {
        nameIsHTML: true
    }
};
var nodes = [
<% foreach(models.Page page in pages) { %>
    {id:<%= page.id %>, pId:<%= page.parent_id %>, name:"<h1><%= page.title %></h1>"},
<% } %>
]

$(function() {
    $.fn.zTree.init($("#page_tree"), setting, nodes);
});

</script>
</asp:Content>
