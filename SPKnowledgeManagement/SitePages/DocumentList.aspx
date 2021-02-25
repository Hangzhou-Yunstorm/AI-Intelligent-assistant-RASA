<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages"  %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="../_catalogs/masterpage/head.Master" AutoEventWireup="true" CodeBehind="DocumentList.aspx.cs" Inherits="SPKnowledgeManagement.SitePages.DocumentList,SPKnowledgeManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4bc69ad3ebb6cdf7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
        <link href="~/_Layouts/15/SPKnowledgeManagement/theme/css/DocumentList.css" rel="stylesheet" />
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstraptable_custom/bootstrap-table.min.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstraptable_custom/bootstrap-table-all.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstraptable_custom/bootstrap-table-locale-all.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstraptable_custom/locale/bootstrap-table-zh-CN.js"></script>
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/startScore/star-rating.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/startScore/star-rating.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/JS/DocumentList.js"></script>
     <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/showLoading.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/jquery.showLoading.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<%--    <div class="divTitle">
        <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/bg1.png" class="imgTitle" />
        <span class="spanTitle"><%=listName %>列表</span>
    </div>--%>
    <div class="divListContent">
        <div class="row">
            <div class="divNav">
                <ul class="ulNav">
                    <li data-id="disable"><span class="nav_root unNav_root" style="cursor:default">当前位置</span></li>
                    <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                    <li data-id="index"><a href="Index.aspx" class="nav_root">首页</a></li>
                    <li data-id="disable" id="noUse"><span style="color: #C7C7C7;">/</span></li>
                    <li data-id="<%=requestID %>"><a  class="nav_root" id="aListName" ><%=listName %>列表</a></li>
                </ul>
            </div>
            <div class="divTable" style="width: 100%; height: 100%; margin-top: 10px;">
                <div class="divDocumentListTitle" style="width: 100%;">
                    <span class="spanDocumentListTitle"><%=listName %>列表</span>
                    <hr style="border: 1px dashed #BCBCBC;margin-top:5px;margin-bottom:0px;" />
                </div>
                <table id="documentListTable" style="width: 100%;" data-locale="zh-CN"></table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var requestID = '<%=requestID%>';
        var documentList1 = '<%=documentList %>';
        var parentName = '<%=parentName %>';
        var parentId = '<%=parentId %>';

        $(document).ready(function () {
            ratingRefresh();
        });
    </script>
</asp:Content>
