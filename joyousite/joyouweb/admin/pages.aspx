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
                                <input type="text" class="input-medium" readonly="readonly" id="page-parent" />
                                <a href="#" id="page-parent-select">select</a>
                                <p class="help-block">&nbsp;</p>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="page-title">Title:</label>
                            <div class="controls">
                                <input type="text" class="input-xxlarge" id="page-title" />
                                <p class="help-block">&nbsp;</p>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="page-content">Content:</label>
                            <div class="controls">
                                <textarea rows="20" cols="40" id="page-content" class="input-xxlarge xheditor"></textarea>
                                <p class="help-block">&nbsp;</p>
                            </div>
                        </div>
                        <div class="form-actions">
                            <input type="hidden" id="page-parent-id" name="page-parent-id" value="-1" />
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

var setting = {
    data: {
        simpleData: {
            enable: true
        }
    },
    edit: {
        enable: true
    },
    check: {
        enable: true,
        nocheckInherit: true
    },
    view: {
        addHoverDom: addHoverDom,
        removeHoverDom: removeHoverDom,
        nameIsHTML: true
    },
    callback: {
        onClick: onClick,
        beforeRemove: beforeRemove,
        onRemove: onRemove
    }
};

var select_setting = {
    view: {
        dblClickExpand: false,
        showLine: true,
        selectedMulti: false
    },
    data: {
        simpleData: {
            enable:true,
            idKey: "id",
            pIdKey: "pId",
            rootPId: ""
        }
    },
    callback: {
        onClick: function(e, tree_id, tree_node) {
            $("#page-parent").val(tree_node.name);
            $("#page-parent-id").val(tree_node.id);
        }
    }
}

function addHoverDom(treeId, treeNode) {
    var sObj = $("#" + treeNode.tId + "_span");
    if (treeNode.editNameFlag || $("#addBtn_"+treeNode.id).length>0) return;
    var addStr = "<span class='button add' id='addBtn_" + treeNode.id
        + "' title='add node' onfocus='this.blur();'></span>";
    sObj.append(addStr);
    var btn = $("#addBtn_"+treeNode.id);
    if (btn) btn.bind("click", function(){
        new_page(treeId, treeNode);
        return false;
    });
};

function new_page(treeId, treeNode) 
{
}

function removeHoverDom(treeId, treeNode) {
    $("#addBtn_"+treeNode.id).unbind().remove();
};

function beforeRemove(tree_id, tree_node) {
    return confirm("Confirm delete page '" + tree_node.name + "' it?");
}

function onRemove(e, tree_id, tree_node) {
    $.ajax({
        url: "/admin/admin.ashx?action=page_delete&id=" + tree_node.id
    });
}

function onClick(e, tree_id, tree_node) {
    $.getJSON("/admin/admin.ashx?action=page_item&id=" + tree_node.id, function(data){
        console.log(data);
    });
}

function onBodyDown(event) {
    if (!(event.target.id == "page-parents" || event.target.id == "page-parent-tree" || $(event.target).parents("#page-parents").length > 0)) {
        hide_menu();
    }
}

function show_menu() {
    var page_parent = $("#page-parent");
    var offset = page_parent.offset();
    $("#page-parents").css({
        left: offset.left + "px",
        top: offset.top + page_parent.outerHeight() + "px"
    }).slideDown("fast");
    $("body").bind("mousedown", onBodyDown);
}

function hide_menu() {
    $("#page-parents").fadeOut("fast");
    $("body").unbind("mousedown", onBodyDown);
}

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
    $("#page-tree").ztree({
        setting: setting,
        nodes: nodes
    }).ztree("expandAll", true);
    var ddtree = $("#page-parents").ddtree({
        setting: {
            data: {
                simpleData: {
                    enable: true
                }
            }
        },
        outputs: [
            {dom: $("#page-parent"), key: "name"},
            {dom: $("#page-parent-id"), key: "id"}
        ],
        host: $("#page-parent"),
        nodes: nodes
    });
    $("#save_add").click(function() {
        save();
        return false;
    });
    $("#page-parent-select").click(function() {
        ddtree.ddtree("show");
        return false;
    });
});

</script>
</asp:Content>
