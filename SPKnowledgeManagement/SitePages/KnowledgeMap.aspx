<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="../_catalogs/masterpage/head.Master" AutoEventWireup="true" CodeBehind="KnowledgeMap.aspx.cs" Inherits="SPKnowledgeManagement.SitePages.KnowledgeMap,SPKnowledgeManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4bc69ad3ebb6cdf7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <link href="~/_layouts/15/SPKnowledgeManagement/plugin/jsMind/style/jsmind.css" rel="stylesheet" />

    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/showLoading.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/jquery.showLoading.min.js"></script>
    <link href="~/_layouts/15/SPKnowledgeManagement/theme/css/KnowledgeMap.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/JS/KnowledgeMap.js"></script>

    <style type="text/css">
        li {
            margin-top: 2px;
            margin-bottom: 2px;
        }

        button {
            width: 140px;
        }

        select {
            width: 140px;
        }

        #layout {
            width: 1230px;
        }

        #jsmind_nav {
            width: 210px;
            height: 600px;
            border: solid 1px #ccc;
            overflow: auto;
            float: left;
        }

        .file_input {
            width: 100px;
        }

        button.sub {
            width: 100px;
        }

        #jsmind_container {
            float: left;
            width: 100%;
            height: 200px;
            border: none;
            background: #fff;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div class="divTitle">
        <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/bg1.png" class="imgTitle" />
        <span class="spanTitle">文档上传</span>
    </div>--%>
    <div class="divMapContent" style="height: 100%; position: relative; max-width: 1560px;">
        <div class="divNav" style="width: 76%; margin: 0 12% 0 12%">
            <ul class="ulNav">
                <li data-id="disable"><span class="nav_root unNav_root" style="cursor: default">当前位置</span></li>
                <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                <li data-id="index"><a href="Index.aspx" class="nav_root">首页</a></li>
                <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                <li data-id="disable"><a class="nav_root" style="cursor: default">知识地图</a></li>
            </ul>
        </div>
        <div>
            <%--<input type="button" style="display: none" id="hiddenClick" onclick="BuildMap()" />--%>
            <div id="ajaxLoading"></div>
            <%--            <div id="KMap" style="width:100%;height:600px;"></div>             --%>
            <div id="jsmind_container"></div>

        </div>
    </div>
    <script src="~/_layouts/15/SPKnowledgeManagement/plugin/jsMind/js/jsmind.js"></script>
    <script type="text/javascript">

        function adaptScreen() {
            var width = document.documentElement.clientWidth;
            var margin;
            if (width > 1560) {
                margin = (width - 1560) / 2;
                if (margin < 100) {
                    $('.divMapContent').css('width', "100%");
                    $('.divMapContent').css('margin', "0");
                    $('.spanTitle').css('left', "0%");
                }
                else {
                    $('.spanTitle').css('left', margin);
                    $('.divMapContent').css('width', "100%");
                    $('.divMapContent').css('left', margin);
                }
            }
            else {
                $('.divMapContent').css('width', "100%");
                $('.divMapContent').css('margin', "0");
                $('.spanTitle').css('left', "0%");
            }
        }
    </script>
</asp:Content>
