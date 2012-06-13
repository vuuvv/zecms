<%@ Page Title="" Language="C#" MasterPageFile="~/joyou.Master" AutoEventWireup="true" CodeBehind="template.aspx.cs" Inherits="joyouweb.allin.template" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
<%= page.title %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="top_links" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
<%= page.content %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
