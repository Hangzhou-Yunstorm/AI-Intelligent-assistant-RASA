<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="../_catalogs/masterpage/head.Master" AutoEventWireup="true" CodeBehind="UserCenter.aspx.cs" Inherits="SPKnowledgeManagement.SitePages.UserCenter,SPKnowledgeManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4bc69ad3ebb6cdf7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">

    <SharePoint:CssLink runat="server" />
    <SharePoint:ScriptLink Name="clienttemplates.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="clientforms.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="clientpeoplepicker.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="autofill.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="sp.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="sp.runtime.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="sp.core.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <%--<SharePoint:CssLink runat="server" />
    <SharePoint:ScriptLink Language="javascript" Name="core.js" Localizable="false" Defer="true" runat="server" />
    <SharePoint:ScriptLink Language="javascript" Name="callout.js" OnDemand="true" runat="server" Localizable="false" />
    <SharePoint:CssRegistration ID="CssRegistration1" Name="Themable/corev15.css" runat="server">
    </SharePoint:CssRegistration>--%>


    <link href="~/_Layouts/15/SPKnowledgeManagement/theme/css/UserCenter.css" rel="stylesheet" />
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstraptable_custom/bootstrap-table.min.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstraptable_custom/bootstrap-table-all.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstraptable_custom/bootstrap-table-locale-all.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstraptable_custom/locale/bootstrap-table-zh-CN.js"></script>
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/startScore/star-rating.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/startScore/star-rating.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/JS/UserCenter.js"></script>
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/showLoading.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/jquery.showLoading.min.js"></script>

    <style>
        body {
            overflow-y: auto;
        }

        .ms-dlgTitleBtns {
            margin-right: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="divUserCenter">
        <div class="divNav">
            <ul class="ulNav">
                <li data-id="disable"><span class="nav_root unNav_root" style="cursor: default">当前位置</span></li>
                <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                <li data-id="index"><a href="Index.aspx" class="nav_root">首页</a></li>
                <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                <li data-id="disable"><a href="UserCenter.aspx" class="nav_root" style="cursor: default">个人中心</a></li>
            </ul>
        </div>
        <span style="display: block; display: block; margin-top: 10px; font-size: 22px; font-family: 'Microsoft YaHei';">个人中心</span>
        <hr style="margin: 5px 0px 0px 0px;" />
        <div class="row">
            <div class="col-sm-3 col-md-3 col-lg-3" style="margin-top: 15px;">
                <div class="divRightCenter">
                    <div class="divUserCenter1">
                        <div class="divUser1Center">
                            <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/default_icon.png" class="imgUserIconCenter" />
                            <span class="spanUserCenter"><span id="spanUserName"><%=userName%></span>，欢迎您</span>
                        </div>
                    </div>
                    <div class="divUserContent1">
                        <div class="divIntegralCenter">
                            <p style="margin: 0; line-height: 30px;">
                                积分
                            </p>
                            <p id="spanIntegralCenter" class="spanValueCenter" style="margin: 0; line-height: 10px;">
                                <%=integral%>
                            </p>
                           <%-- <span class="spanTitle1Center">积分</span><br />
                            <span id="spanIntegralCenter" class="spanValueCenter"></span>--%>
                        </div>
                        <div class="divDownloadCenter">
                            <span class="spanTitle1Center">已下载次数</span><br />
                            <span id="spanDownloadNumber" class="spanValueCenter"><%=downNumber %></span>
                        </div>
                        <div class="divLineCenter">
                        </div>
                        <div>
                            <img class="imgShape" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/Shape1.png" />
                            <span class="spanMydocument">我的文档</span>
                            <ul class="ulMyDocument">
                                <li id="liUploadList" class="liListButton"><span class="spanListTitle">我的上传</span></li>
                                <li id="liDownList" class="liListButton"><span class="spanListTitle">我的下载</span></li>
                                <li id="liUnReadList" class="liListButton"><span class="spanListTitle">未读分享</span></li>
                                <li id="liReadList" class="liListButton"><span class="spanListTitle">所有分享</span></li>
                                <li id="liExamineList" class="liListButton"><span class="spanListTitle">未审批文档</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="divUserImgCenter">
                        <%--                        <img class="imgUploadCenter" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/upload1.png" onclick="DocUpload();" />--%>
                        <a class="imgUpload" onclick="DocUpload()">
                            <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/upload_icon.png" />&nbsp;&nbsp;&nbsp;&nbsp;<span class="spanColor">上传文档</span></a>
                    </div>
                </div>
            </div>
            <div class="col-sm-9 col-md-9 col-lg-9">
                <table id="userCenterTable" style="width: 100%;" data-locale="zh-CN"></table>
            </div>
        </div>
    </div>
    <!-- 共享人物搜索模态框（Modal） -->
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content" id="shareContent">
                <SharePoint:SharePointForm runat="server">
                    <div class="modal-header">
                        <a style="cursor: pointer" type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</a>
                        <h4 class="modal-title" id="searchModalLabel">文档分享</h4>
                    </div>
                    <div class="modal-body" style="height: 150px;">

                        <SharePoint:AjaxDelta ID="DeltaPlaceHolderAdditionalPageHead" Container="false" runat="server">

                            <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="false" EnablePartialRendering="true" EnableScriptGlobalization="false" EnableScriptLocalization="true" />
                            <%-- <SharePoint:PeopleEditor ID="peopleEditor" AutoPostBack="false" runat="server" AllowEmpty="false" ValidatorEnabled="true" MultiSelect="true" SelectionSet="User,SecGroup" Width="100%" Height="100" />--%>
                            <SharePoint:ClientPeoplePicker ID="peoplePicker" runat="server" Width="100%" Height="120" AllowEmailAddresses="true" AllowMultipleEntities="true" AutoFillEnabled="true" Required="true" PrincipalAccountType="User" ValidateRequestMode="Enabled" Rows="6" />
                        </SharePoint:AjaxDelta>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <%--                        <asp:Button ID="btnConfirm" runat="server" CssClass="btn" OnClick="GetShareUser" Text="分享" />--%>
                        <button id="btnConfirm1" runat="server" class="btn btn-default" onclick="ShowLoading();" onserverclick="GetShareUser">分享</button>
                        <asp:HiddenField ID="docID" Value="0" runat="server" />
                        <asp:HiddenField ID="hidMailUrl" runat="server" />
                    </div>
                </SharePoint:SharePointForm>
                <div runat="server" id="ShowLoadingDiv" style="width: 100%; height: 100%; background-color: rgba(255, 255, 255, 0.65); z-index: 5000;position:absolute;top:0;left:0;display:none">
                    <div class="loading-indicator" style="position: absolute; left: 43%; top: 40%; z-index: 5001"></div>
                </div>

            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>
    <script type="text/javascript">
        var listName = '<%=listName%>';

        var tmpDown = '<%=downList%>';
        var downList = eval(tmpDown.replace(/\n/g, ";"));

        var tmpUpload = '<%=uploadList%>';
        var uploadList = eval(tmpUpload.replace(/\n/g, ";"));

        var tmpUnRead = '<%=unReadList%>';
        var unReadList = eval(tmpUnRead.replace(/\n/g, ";"));

        var tmpRead = '<%=readList%>';
        var readList = eval(tmpRead.replace(/\n/g, ";"));

        var tmpExamine = '<%=examineList%>';
        var examineList = eval(tmpExamine.replace(/\n/g, ";"));


        $(document).ready(function () {
            ratingRefresh();
            switch (listName) {
                case '上传列表':
                    $('#liUploadList').click();
                    break;
                case '下载列表':
                    $('#liDownList').click();
                    break;
                case '未读分享':
                    $('#liUnReadList').click();
                    break;
            }
        });
        function DocUpload(obj) {
            window.location.href = "DocumentUpload.aspx";
        }

        function OpenShare(obj) {
            var docId = $(obj).attr("data-id");
            $("#searchModal").modal("show");
            $("#<%=docID.ClientID%>").val(docId);
        }

        function ShowLoading() {
            var markDiv = "<%= ShowLoadingDiv.ClientID %>";
            $('#' + markDiv).show();
        }
    </script>
</asp:Content>
