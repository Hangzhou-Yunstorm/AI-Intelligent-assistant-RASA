<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewFolderAdminKnowledgePushUserControl.ascx.cs" Inherits="SPKnowledgeManagement.NewFolderAdminKnowledgePush.NewFolderAdminKnowledgePushUserControl" %>

<script src="~/_layouts/15/SPKnowledgeManagement/jquery/jquery.min.js"></script>

<a id="aMailUrl" class="btn" style="display: block; height: 30px; border: 1px solid #0072C6; width: 100px; font-family: 'Microsoft Yahe'; font-size: 16px; line-height: 30px; text-align: center; text-decoration: none;">精华推送</a>
<asp:HiddenField ID="hidMailUrl" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {
        var isSiteAdmin = "<%= IsButtonVisible%>";
        if (isSiteAdmin == "1") {
            var mailtoUrl = $("#<%= hidMailUrl.ClientID%>").val();
            if (mailtoUrl != "")
                $("#aMailUrl").attr("href", $("#<%= hidMailUrl.ClientID%>").val());
            else
                $("#aMailUrl").attr("href", 'javascript: alert("未指定推送内容，请先设置需推送的知识，并刷新此页")');
        }
        else
        {
            $("#aMailUrl").hide();
        }
    });
</script>
