<%@ Page Title="" Language="C#" EnableViewState="false" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true" CodeBehind="pages.aspx.cs" Inherits="joyouweb.admin.Pages" %>
<%@ Register TagName="pagetree" src="~/controls/pagetree.ascx" TagPrefix="vuuvv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<link href="/Styles/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery.ztree.all-3.2.js" type="text/javascript"></script>
<script src="/Scripts/ztree.js" type="text/javascript"></script>
<script src="/Scripts/xheditor/xheditor-1.1.13-en.min.js" type="text/javascript"></script>
<style type="text/css">
.sidebox 
{
    border: 1px solid #d4d8eb;
    border-radius: 3px 3px 3px 3px;
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.1);
    margin: 0 0 15px;
    padding-bottom: 1px;
}
.sidebox .hd
{
    background: #e5e6f1;
    border-radius: 3px 3px 0 0;
    padding: 4px 7px 5px;
}
.sidebox bd 
{
    background: #f9f9fc;
    border-radius: 0 0 3px 3px;
    font-size: 12px;
    padding: 10px 8px;
}
.sidebox .hd .h2 
{
    color: #30418c;
    font-size: 15px;
    font-weight: bold;
    margin: 0;
}

.content 
{
    border-radius: 4px 4px 4px 4px;
    box-shadow: 0 0 5px #BFBFBF;
    padding: 1em;
    min-height: 400px;
}

.btns 
{
    margin-bottom: 0;
    background-color: #EEEEEE;
    background-repeat: repeat-x;
    border-bottom: 1px solid #E5E5E5;
    width: 100%;
}

.btns li 
{
    padding: 2px 3px;
    cursor: pointer;
}

.btns li:hover
{
    background: #f5f5f5;
}

.ztree 
{
    min-height: 360px;
}
.ztree li span.button.add {margin-left:2px; margin-right: -1px; background-position:-144px 0; vertical-align:top; *vertical-align:middle}

.input-xxlarge 
{
    width: 520px;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="container-fluid">
    <div class="row-fluid">
        <div class="span3">
            <div class="sidebox">
                <div class="hd">
                    <div class="h2">Page tree</div>
                </div>
                <ul class="nav nav-pills btns">
                    <li>
                        <i class="icon-plus"></i>
                    </li>
                    <li>
                        <i class="icon-remove"></i>
                    </li>
                    <li>
                        <i class="icon-refresh"></i>
                    </li>
                </ul>
                <div class="bd">
                    <ul id="page-tree" class="ztree"> </ul>
                </div>
            </div>
        </div>
        <div class="span9">
            <div class="content">
                <form class="form-horizontal" action="admin.ashx" id="page-form">
                    <fieldset>
                        <legend>New Page</legend>
                        <div class="control-group">
                            <label class="control-label" for="page-parent">Parent:</label>
                            <div class="controls">
                                <input type="text" name="parent" class="input-medium" readonly="readonly" id="page-parent" />
                                <a href="#" id="page-parent-select">select</a>
                                <a href="#" id="page-parent-clear">clear</a>
                                <p class="help-block">&nbsp;</p>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="page-title">Title:</label>
                            <div class="controls">
                                <input type="text" class="input-xxlarge" id="page-title" name="title" />
                                <p class="help-block">&nbsp;</p>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="page-content">Content:</label>
                            <div class="controls">
                                <textarea rows="20" cols="40" id="page-content" name="content" class="input-xxlarge xheditor"></textarea>
                                <p class="help-block">&nbsp;</p>
                            </div>
                        </div>
                        <div class="form-actions">
                            <input type="hidden" id="page-parent-id" name="parent_id" value="-1" />
                            <button id="save_add" class="btn btn-primary" type="submit">Save and add another</button>
                            <button id="save_edit" class="btn btn-primary" type="submit">Save and continue editing</button>
                        </div>
                    </fieldset>
                </form>
            </div>
        </div>
    </div>
    <div id="page-parents" class="sidebox" style="display:none;position:absolute">
        <ul id="page-parent-tree" class="ztree" style="background: #f0f6e4; margin-top:0;width: 180px; min-height: 160px;"></ul>
    </div>
</div>
<script type="text/javascript">

function save() {
    var data = {
        parent_id: $("#page-parent-id").val(),
        title: $("#page-title").val(),
        content: $("#page-content").val()
    };
    $.ajax({
        url: "/admin/admin.ashx?action=page_add",
        data: data,
        type: "post",
        success: function(data){
            console.log(data);
        }
    });
}

var nodes = [
<% foreach(models.Page page in pages) { %>
    {id:<%= page.id %>, parent_id:<%= page.parent_id %>, name:"<%= page.title %>"},
<% } %>
]

$(function() {
    $("#page-tree").dbtree({
        nodes: nodes
    });
    $("#page-parents").ddtree({
        outputs: {
            name: $("#page-parent"),
            id: $("#page-parent-id")
        },
        host: $("#page-parent"),
        nodes: nodes
    });
    $("#page-parent-select").click(function() {
        $("#page-parents").ddtree("show");
        return false;
    });
    $("#page-parent-clear").click(function() {
        $("#page-parents").ddtree("clear");
        return false;
    });
    $("#save_add").click(function() {
        save();
        return false;
    });
});

</script>
</asp:Content>
