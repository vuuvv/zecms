<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="filebrowser.aspx.cs" Inherits="joyouweb.admin.filebrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>test</title>

    <script src="/Scripts/elfinder/js/jquery-1.4.1.min.js" type="text/javascript" charset="utf-8"></script>

    <script src="/Scripts/elfinder/js/jquery-ui-1.7.2.custom.min.js" type="text/javascript" charset="utf-8"></script>

    <link rel="stylesheet" href="/Scripts/elfinder/js/ui-themes/base/ui.all.css" type="text/css" media="screen"
        charset="utf-8"/ >

    <script src="/Scripts/elfinder/js/elfinder.full.js" type="text/javascript" charset="utf-8"></script>

    <link rel="stylesheet" href="/Scripts/elfinder/css/elfinder.css" type="text/css" media="screen" charset="utf-8" />

    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $('#finder').elfinder({
                url: 'connector.ashx'
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="finder">
    </div>
    </form>
</body>

</html>
