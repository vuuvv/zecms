<%@ Page Language="C#" MasterPageFile="~/joyou.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="joyouweb.allin.index" %>

<asp:Content ContentPlaceHolderID="title" runat="server">All in JOYOU</asp:Content>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Scripts/allin/remaind.js" type="text/javascript"></script>
    <link href="/Styles/allin/style.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="top_links" runat="server">
    <span class="current-site">
        <b>All in JOYOU</b>
    </span>
    <span class="links-pipe">|</span>
    <a href="#">JOYOU官网</a>
    <span class="links-pipe">|</span>
    <a href="#">JOYOU官博</a>
    <span class="links-pipe">|</span>
    <a href="#">中宇品质秀</a>
    <span class="links-pipe">|</span>
    <a href="#">分享</a>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="home-page">
        <div class="home-main">
            <div class="home-main-overlay"></div>
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
        <div class="home-banner">
            <ul>
                <li>
                    <a href="#" hidefocus="true">
                        <img height="48" width="48" alt="往  届" src="https://img.tenpay.com/v2/res/img/global/index_app/app_credit.png" />
                        <strong>往  届</strong>
                    </a>
                </li>
                <li>
                    <a href="#" hidefocus="true">
                        <img height="48" width="48" alt="活  动" src="https://img.tenpay.com/v2/res/img/global/index_app/app_plane.png" />
                        <strong>活  动</strong>
                    </a>
                </li>
                <li>
                    <a href="#" hidefocus="true">
                        <img height="48" width="48" alt="视  频" src="https://img.tenpay.com/v2/res/img/global/index_app/app_safetymarket.png" />
                        <strong>视  频</strong>
                    </a>
                </li>
                <li>
                    <a href="#" hidefocus="true">
                        <img height="48" width="48" alt="热  图" src="https://img.tenpay.com/v2/res/img/global/index_app/app_mortgage.png" />
                        <strong>热  图</strong>
                    </a>
                </li>
                <li>
                    <a href="#" hidefocus="true">
                        <img height="48" width="48" alt="相约中宇" src="https://img.tenpay.com/v2/res/img/global/index_app/app_jiaofei.png" />
                        <strong>相约中宇</strong>
                    </a>
                </li>
                <div style="clear: both"></div>
            </ul>
        </div>
    </div>
</asp:Content>
