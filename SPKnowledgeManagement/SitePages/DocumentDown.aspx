<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="../_catalogs/masterpage/head.Master" AutoEventWireup="true" CodeBehind="DocumentDown.aspx.cs" Inherits="SPKnowledgeManagement.SitePages.DocumentDown,SPKnowledgeManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4bc69ad3ebb6cdf7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <SharePoint:CssLink runat="server" />
    <SharePoint:ScriptLink Name="clienttemplates.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="clientforms.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="clientpeoplepicker.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="autofill.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="sp.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="sp.runtime.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink Name="sp.core.js" runat="server" LoadAfterUI="true" Localizable="false" />

    <%--<script src="~/_layouts/15/SPKnowledgeManagement/jquery/jquery.min.js"></script>--%>
    <link href="~/_layouts/15/SPKnowledgeManagement/plugin/bootstrap-3.3.5-dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="~/_layouts/15/SPKnowledgeManagement/plugin/bootstrap-3.3.5-dist/js/bootstrap.min.js"></script>
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/startScore/star-rating.css" rel="stylesheet" />
    <link href="~/_Layouts/15/SPKnowledgeManagement/theme/css/DocumentDown.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/jqPaginator-master/src/js/jqPaginator.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/JS/DocumentDown.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/startScore/star-rating.js"></script>
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/showLoading.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/jquery.showLoading.min.js"></script>

    <style>
          body {
            overflow-y: auto;
        }
       #functionarea .popover {
            width: 600px;
            height: 500px;
            max-width: 600px;
        }

        #functionarea .popover-content {
            height: 460px;
        }
    </style>
    <%--    <div class="divTitle">
        <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/bg1.png" class="imgTitle" />
        <span class="spanTitle">文档下载</span>
    </div>--%>

    <div class="divDownContent">
        <div class="row">
            <div class="divNav">
                <ul class="ulNav">
                    <li data-id="disable"><span class="nav_root unNav_root" style="cursor: default">当前位置</span></li>
                    <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                    <li data-id="index"><a href="Index.aspx" class="nav_root">首页</a></li>
                    <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                    <li data-id="disable"><a href="DocumentDown.aspx" class="nav_root" style="cursor: default">文档下载</a></li>
                </ul>
            </div>
            <div class="divDownContent1">
                <span id="docTitle" class="spanDocumentTitle"></span>
                <span id="docAuthor" class="spanDetail"></span>
                <span class="spanDetail">|</span>
                <span id="docUploadTime" class="spanDetail"></span>
                <span class="spanDetail">|</span>
                <span id="docType" class="spanDetail"></span>
                <span class="spanDetail">|</span>
                <span id="docClick" class="spanDetail"></span>
                <span class="spanDetail">|</span>
                <span id="docDownload" class="spanDetail"></span>
                <span class="spanDetail">|</span>
                <input id="input-1" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />
            </div>
        </div>
        <div class="row" style="margin-top: 0px;">
            <div class="col-sm-12 col-md-12 col-lg-12">
                <textarea style="border: none; resize: none; outline: none; width: 100%" readonly="readonly" id="docRemark">           
                </textarea>
                <div id="functionarea">
                    <a id="aPreview" tabindex="0" class="btn btn-success" role="button" data-toggle="popover" data-trigger="focus">预览</a>
                    <a tabindex="0" class="btn  btn-primary" data-id="<%=requestID %>" onclick="OpenDownPage();" style="margin-left: 10px; margin-top: 20px; height: 30px; line-height: 15px; width: 100px; color: #fff; font-size: 16px; font-family: 'Microsoft YaHei';">下载</a>
                    <a tabindex="0" class="btn  btn-info" data-id="37" onclick="OpenSharePage();" style="margin-left: 10px; margin-top: 20px; height: 30px; line-height: 15px; width: 100px; color: #fff; font-size: 16px; font-family: 'Microsoft YaHei';">分享</a>
                    <span id="docDowned" class="spanDowm">您尚未下载过该文档</span>
                    <span id="docDown" class="spanDowm">根据您当前积分，您还可以下载<span id="docDownTime" class="spanTimes">5</span>次</span>
                </div>
                <div class="divMyComment">
                    <span class="spanMyComment">您的评论</span>
                    <input id="gradeOwn" value="0" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" />
                    <div>
                        <textarea id="comment" maxlength="300" rows="6" class="textComment" style="resize: none" name="textComment" placeholder="请输入您的评论,字数不得超过300">
                        </textarea>
                        <button id="btnComment" class="btn btn-warning" onclick="SendComment(this)" data-id="<%=requestID %>" style="margin-top: 5px; height: 30px; line-height: 14px; width: 100px; color: #fff; font-size: 16px; font-family: 'Microsoft YaHei'; float: right;">
                            提交评论</button>
                    </div>
                </div>
                <span class="spanUserComment">用户评论</span>
                <div id="divComment" class="userComment">
                    <div class="divUserComment">
                        <div class="divUserCommentContent">
                            <h4>暂无评论</h4>
                        </div>
                    </div>
                </div>
                <ul class="pagination" style="display: none" id="pagination1"></ul>
            </div>
        </div>
    </div>
    <input type="hidden" name="__VIEWSTATE" value="" />
    <!-- 模态框（Modal） -->
    <div class="modal fade" id="commentModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h4 class="modal-title" id="commentModalLabel">评论回复
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <span>回复内容</span>
                        </div>
                        <div class="col-sm-10 col-md-10 col-lg-10">
                            <textarea maxlength="300" id="replyContent" rows="6" style="height: 200px; width: 100%; resize: none" placeholder="请输入评论内容，字数不得超过300"></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default"
                        data-dismiss="modal">
                        关闭
                    </button>
                    <button id="btnConfirm" type="button" class="btn btn-primary" onclick="SubmitComment(this)">
                        确认
                    </button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>
    <!-- 模态框（Modal） -->
    <div class="modal fade" id="confirmDown" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="padding-top: 5px; padding-bottom: 20px;">
                    <a type="button" data-dismiss="modal" aria-hidden="true" style="width: 30px; height: 20px; float: right; cursor: pointer; margin-bottom: 5px; text-align: center;">X</a>
                </div>
                <div class="modal-body" id="divContentDown">
                    <div class="row">
                        <h4>您现有积分数为<span id="nowIntegral" class="downIntegral">5</span></h4>
                    </div>
                    <div class="row" style="display: none">
                        <h4>您下载后积分数为<span id="downIntegral" class="downIntegral">5</span></h4>
                    </div>
                    <div class="row">
                        <h4>下载该文件需要<span id="spanDownIntegral" class="downIntegral">5</span>是否需要下载？</h4>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button id="btnConfirmDown" type="button" class="btn btn-primary" onclick="SubmitDown(this)" data-id="<%=requestID %>">确认</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>
    <!-- 共享人物搜索模态框（Modal） -->
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content" id="shareContent">
                <SharePoint:SharePointForm runat="server">
                    <button id="hiddenbtn" style="display: none" runat="server" onserverclick="hiddenbtn_ServerClick"></button>
                    <asp:HiddenField runat="server" ID="hiddenfieldpath" Value="" />
                    <a style="display: none" href="#" id="hiddenlink"></a>
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
                <div runat="server" id="ShowLoadingDiv" style="width: 100%; height: 100%; background-color: rgba(255, 255, 255, 0.65); z-index: 5000; position: absolute; top: 0; left: 0; display: none">
                    <div class="loading-indicator" style="position: absolute; left: 43%; top: 40%; z-index: 5001"></div>
                </div>

            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>

    <script type="text/javascript">
        var tmpStr = '<%=docInfoJson%>';
        var docInfo = eval(tmpStr);
        //var docInfo = <%=docInfoJson%>;
        var integral = <%=integral%>;
        var docDowned ="<%=docDowned%>";
        var requestID = <%=requestID%>;
        var isComment = "<%=isComment%>";
        var grade = "<%=ownGrade%>";
        var pageNum=10;
        var fileName = "<%= fileName%>";
        var btnClientId = "<%=hiddenbtn.ClientID%>";
        var hiddenValueId = "<%= hiddenfieldpath.ClientID%>";

        $(document).ready(function () {
            PInitialPopover();
            ratingRefresh();
            if (docDowned == "yes")
            {
                if (isComment == "yes")
                {
                    $("#gradeOwn").parents(".star-rating").attr("disabled","disabled");
                }
            }
            else
            {
                $("#gradeOwn").rating('refresh',{disabled:true});
                $("#comment").attr("disabled","disabled");
                $("#btnComment").attr("disabled","disabled");
            }
        });


        function funcDocPreview(){
            $('#spPreviewArea')
        }

        function OpenModal(obj) {
            var replyId = $(obj).attr("data-id");
            $('#commentModal').modal('show');
            $("#btnConfirm").attr("data-id",replyId);
        }

        function OpenDownPage(){
            $('#confirmDown').modal('show');
        }

        function OpenSharePage(){
            $('#searchModal').modal('show');
            $('#<%= docID.ClientID%>').val(requestID);
        }

        function ShowLoading() {
            var markDiv = "<%= ShowLoadingDiv.ClientID %>";
            $('#' + markDiv).show();
        }

        function SubmitComment(obj){
            $(obj).attr("disabled", "disabled");
            var replyId = $(obj).attr("data-id");
            if($("#replyContent").val()=="")
            {
                alert('请输入回复内容');return;
            }
            //var replyContent = $("#replyContent").val().replace(/\n/g, "<br/>");
            var replyContent = $("#replyContent").val();

            var fileID = requestID;
            $.ajax({
                type: 'post',
                url: 'DocumentDown.aspx/ReplyComment',
                data: '{ "replyId": "' + replyId + '","replyContent":"' + replyContent + '","fileID":"' + fileID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == "success") {
                        alert("回复成功！");
                        Page(pageNum);
                        LoadComment(1,pageNum);
                        $("#replyContent").val("");
                        $('#commentModal').modal('hide');
                        $(obj).removeAttr("disabled");
                        $("#gradeOwn").rating('refresh',{disabled:true});
                    }
                    else if (data.d == "error") {
                        alert("回复功能出现错误，请与管理员联系！");
                        $(obj).removeAttr("disabled");
                    }
                }
            })
        }

        function SubmitDown(obj)
        {
            var fileId = $(obj).attr("data-id");
            $(obj).attr("disabled","disabled");
            $.ajax({
                type: 'post',
                url: 'DocumentDown.aspx/SubmitDown',
                data: '{ "fileId":"' + fileId + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "notEnoughIntegral" && data.d != "" )
                    {
                        try {
                            //var elemIF = document.createElement("iframe");   
                            //elemIF.src = data.d;
                            //elemIF.style.display = "none";   
                            //document.body.appendChild(elemIF);
                            
                            $("#"+hiddenValueId).val(data.d);
                            $("#"+btnClientId).click();
                            $("#divContentDown").html("");
                            var html = "<a style=\"text-align: center;display: block;cursor:pointer;font-size: 24px;\" onclick=\"DownloadAgain()\">如果未下载，请点击此链接下载</a>"
                            $("#divContentDown").html(html);
                            //$('#confirmDown').modal('hide');
                            $("#gradeOwn").parents(".star-rating").addClass("rating-active");
                            $("#gradeOwn").parents(".star-rating").removeClass("rating-disabled");        
                            $("#comment").removeAttr("disabled");
                            $("#btnComment").removeAttr("disabled");
                            $("#gradeOwn").rating('refresh',{disabled:false});
                        }
                        catch (e) {
                            alert(e.message);
                        }
                    }
                    else if (data.d == "notEnoughIntegral")
                    {
                        alert("您的账户积分不足，无法下载！");
                        $("#btnConfirmDown").removeAttr("disabled");
                        $('#confirmDown').modal('hide');
                    }
                }
            })
        }

        function DownloadAgain()
        {
            $("#"+btnClientId).click();
        }

        //初始化弹出框
        function PInitialPopover(){
            $('#aPreview').popover({
                trigger: 'click', //触发方式
                //template: '', //你自定义的模板
                title: "预览",//设置 弹出框 的标题
                html: true, // 为true的话，data-content里就能放html代码了
                content: '<iframe id="previewArea" style="width:100%;height:90%;" src="<%=previewUrl%>"></iframe><span style="font-size: 16px;position: relative;top: 10px;">文件名：'+fileName+'</span>',
            }
            );


            

            $('#aPreview').on('inserted.bs.popover', function () {
                $(".popover-title").css("width","80%");
                $(".popover-title").css("float","left");
                $('.spanClost').remove();
                $(".popover-title").after('<span class="spanClost" onclick="ClosePopover()">X</span>');

            })

        
        }

        function ClosePopover(){
            $(".spanClost").remove();
            $('#aPreview').click();
            //$('#aPreview').popover('hide');
            //$('.popover-content').html("");

        }
    </script>
</asp:Content>
