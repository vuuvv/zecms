<%@ Page Language="C#" MasterPageFile="~/joyou.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="joyouweb.allin.index" %>

<asp:Content ContentPlaceHolderID="title" runat="server">All in JOYOU</asp:Content>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Scripts/allin/remaind.js" type="text/javascript"></script>
    <link href="/Styles/allin/style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="top_links" runat="server">
    <span class="current-site">
        <b>English</b>
    </span>
    <span class="links-pipe">|</span>
    <a href="#">German</a>
    <span class="links-pipe">|</span>
    <a href="#">Chinese</a>
    <span class="links-pipe">|</span>
    <a href="#">Sitemap</a>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="home-page">
        <div class="home-main">
            <a class="abs link home-video" href="video.aspx"></a>
            <a class="abs link home-date" href="date.aspx"></a>
            <a class="abs link home-history" href="history.aspx"></a>
            <a class="abs link home-hot" href="hot.aspx"></a>
            <a class="abs link home-activity" href="activity.aspx"></a>
            <div class="abs remaind">
                <div class="abs" id="remaind-day-0">&nbsp;</div>
                <div class="abs" id="remaind-day-1">&nbsp;</div>
                <div class="abs" id="remaind-hour-0">&nbsp;</div>
                <div class="abs" id="remaind-hour-1">&nbsp;</div>
                <div class="abs" id="remaind-minite-0">&nbsp;</div>
                <div class="abs" id="remaind-minite-1">&nbsp;</div>
            </div>
            <script type="text/javascript">
                $(".remaind").remaind({
                    deadline: new Date(2012, 6, 28, 4, 12, 0)
                });
            </script>
        </div>
    </div>
</asp:Content>
