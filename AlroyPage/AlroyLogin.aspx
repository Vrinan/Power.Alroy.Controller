<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlroyLogin.aspx.cs" Inherits="AlroyLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
    <script src="/Scripts/plugins/jQuery/jquery-2.2.4.min.js" type="text/javascript"></script>
    <script src="/Scripts/layer/layer.js" type="text/javascript"></script>
    <script type="text/javascript">
        var errMsg = "<%= errmsg %>";
        if(errMsg != ""){
            layer.alert(errMsg, {
                skin: 'layui-layer-molv' //样式类名
                ,closeBtn: 0
                ,anim: 4 //动画类型
            }, function(){
                window.location.href = "http://" + window.location.host;
            });
        }
    </script>
</body>
</html>
