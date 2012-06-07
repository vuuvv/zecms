<%@ Page Title="" Language="C#" EnableViewState="false" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true" CodeBehind="pages.aspx.cs" Inherits="joyouweb.admin.pages" %>
<%@ Register TagName="pagetree" src="~/controls/pagetree.ascx" TagPrefix="vuuvv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<vuuvv:pagetree ID="page_tree" runat="server" />

</asp:Content>
