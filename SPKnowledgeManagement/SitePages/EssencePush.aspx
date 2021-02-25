<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="../_catalogs/masterpage/MasterPages/head.Master" AutoEventWireup="true" CodeBehind="EssencePush.aspx.cs" Inherits="SPKnowledgeManagement.SitePages.EssencePush,SPKnowledgeManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4bc69ad3ebb6cdf7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="~/_Layouts/15/SPKnowledgeManagement/theme/css/UserCenter.css" rel="stylesheet" />
    <div class="row">
        <div>
            <button class="btn btnDown btn-success" data-id="<%=essenceId %>" onclick="PushEssence(this)" >预览</button>
        </div>
    </div>
    <script>
        function PushEssence(obj) {
            var essenceID = $(obj).attr("data-id");
            $.ajax({
                type: 'post',
                url: 'EssencePush.aspx/OpenOutlook',
                data: '{ "pushId": "' + essenceID + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                }
            })
        }
    </script>
</asp:Content>
