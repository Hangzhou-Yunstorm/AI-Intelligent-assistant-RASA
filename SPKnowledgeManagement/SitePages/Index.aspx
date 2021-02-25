<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="../_catalogs/masterpage/head.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SPKnowledgeManagement.SitePages.Index,SPKnowledgeManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4bc69ad3ebb6cdf7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <link href="~/_Layouts/15/SPKnowledgeManagement/theme/css/Index.css" rel="stylesheet" />
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/Viwepager/Viwepager.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/Viwepager/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContent">
        <div class="row">
            <div class="col-sm-9 col-md-9 col-lg-9 noPadding">
                <!-- 轮播广告 -->
                <div id="banner_tabs" class="flexslider">
                    <ul class="slides">                   
                      
                       
                      
                       
                    </ul>
                    <ul class="flex-direction-nav">
                        <li><a class="flex-prev" href="javascript:;">Previous</a></li>
                        <li><a class="flex-next" href="javascript:;">Next</a></li>
                    </ul>
                </div>
            </div>
            <div class="col-sm-3 col-md-3 col-lg-3 noPadding">
                <div class="divRight">
                    <div class="divUser">
                        <div class="divUser1">
                            <div style="float:left;min-width:55px;height:55px">
                                <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/default_icon.png" class="imgUserIcon" data-id="upload" onclick="JumpUserCenter(this);" />
                                <span class="spanInfoTip" id="spanTipNum" data-id="sharedoc" onclick="JumpUserCenter(this);"></span>
                            </div>
                            <div style="float: left; margin-left: 5px">
                                <span id="spanUserName"></span>
                            </div>
                            <%--<span class="spanUserTodayDownload">今日可下载次数<span>(3)</span></span>--%>
                        </div>
                        <span class="spanUser">
                            <a title="个人中心" class="spanValue" href="UserCenter.aspx" style="cursor: pointer; text-decoration: none; line-height: 60px; position: relative; float: right; position: relative; right: 5%;">个人中心</a></span>
                    </div>
                    <div class="divUserContent">
                        <div class="divIntegral ">
                            <p style="margin: 0; line-height: 30px;">
                                积分
                            </p>
                            <p id="spanIntegral" class="spanValue" style="margin: 0; line-height: 10px;">
                                
                            </p>
                        </div>
                        <div class="divDownload">
                            <a title="公共文档库" class="spanValue" href="DocumentList.aspx?FolderID=<%= publicFolderId %>" style="cursor: pointer; text-decoration: none; line-height: 50px;">公共文档库</a>                          
                        </div>
                        <%--                        <div style="height: 65px;">
                            <span class="spanUserCenter" data-id="upload" onclick="JumpUserCenter(this);">个人中心</span>
                            <ul class="ulUserCenter" style="color: #535EB1">
                                <li data-id="upload" onclick="JumpUserCenter(this);" style="cursor: pointer;"><span>上传记录</span><span id="spanUploadNumber" style="float: right;">(51)</span></li>
                                <li data-id="download" onclick="JumpUserCenter(this);" style="cursor: pointer;"><span>下载记录</span><span id="spanDownloadNumber1" style="float: right;">(51)</span></li>
                                <li data-id="sharedoc" onclick="JumpUserCenter(this);" style="cursor: pointer;"><span>未读分享</span><span id="spanUnReadNumber" style="float: right;">(51)</span></li>
                            </ul>
                        </div>--%>
                    </div>
                    <hr style="border-top: 1px dashed #E6E6E6; margin: 0; width: 100%; position: relative;">

                    <div class="divUserImg">
                        <a onclick="KnowledgeMapCenter()" style="cursor: pointer">
                            <div style="width: 45%; height: 30px; background-color: #408D61; margin-top: 15px; border-radius: 3px; float: left; margin-left: 3%; margin-right: 2%; line-height: 30px; color: #fff">
                                <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/map_icon.png" width="15" height="15" style="margin-top: -2px; margin-right: 5px">知识地图
                            </div>
                        </a>
                        <a onclick="DocUpload()" style="cursor: pointer">
                            <div style="width: 45%; height: 30px; background-color: #4479C5; margin-top: 15px; border-radius: 3px; float: left; margin-left: 2%; margin-right: 3%; line-height: 30px; color: #fff">
                                <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/upload_icon.png" width="15" height="15" style="margin-top: -2px; margin-right: 5px">上传文档
                            </div>
                        </a><%--<a class="imgMap" onclick="KnowledgeMapCenter()">
                            <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/map_icon.png" width="15" height="15" style="margin-top: -2px" />&nbsp;&nbsp;&nbsp;&nbsp;<span class="spanColor">知识地图</span></a>
                        <a class="imgUpload" onclick="DocUpload()">
                            <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/upload_icon.png" width="15" height="15" style="margin-top: -2px" />&nbsp;&nbsp;&nbsp;&nbsp;<span class="spanColor">上传文档</span></a>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-9 col-md-9 col-lg-9" style="padding: 0px;">
                <div class="row">
                    <div class="col-sm-4 col-md-4 col-lg-4 noPadding">
                        <div class="divTitle">
                            <img id="spanPartTitleIcon1" src="" />
                            <span id="spanPartTitle1" class="spanCategoryTitle">技术文档</span>
                        </div>
                        <div>
                            <ul id="ulPartContent1" class="ulSet">
                            </ul>
                            <%--                            <img id="imgMore1" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" />--%>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4 noPadding">
                        <div class="divTitle">
                            <img id="spanPartTitleIcon2" src="" />
                            <span id="spanPartTitle2" class="spanCategoryTitle">企业制度</span>
                        </div>
                        <div>
                            <ul id="ulPartContent2" class="ulSet">
                            </ul>
                            <%--<img id="imgMore2" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" />--%>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4 noPadding">
                        <div class="divTitle">
                            <img id="spanPartTitleIcon3" src="" />
                            <span id="spanPartTitle3" class="spanCategoryTitle">战略规划</span>
                        </div>
                        <div>
                            <ul id="ulPartContent3" class="ulSet">
                            </ul>
                            <%--<img id="imgMore3" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" />--%>
                        </div>
                    </div>
                </div>
                <hr style="border-top: 2px solid #E6E6E6; margin: 10px 5%; width: 90%; /* position: absolute; */">
                <div class="row">
                    <div class="col-sm-4 col-md-4 col-lg-4 noPadding">
                        <div class="divTitle">
                            <img id="spanPartTitleIcon4" src="" />
                            <span id="spanPartTitle4" class="spanCategoryTitle">项目文档</span>
                        </div>
                        <div>
                            <ul id="ulPartContent4" class="ulSet">
                            </ul>
                            <%--<img id="imgMore4" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" />--%>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4 noPadding">
                        <div class="divTitle">
                            <img id="spanPartTitleIcon5" src="" />
                            <span id="spanPartTitle5" class="spanCategoryTitle">图书阅读</span>
                        </div>
                        <div>
                            <ul id="ulPartContent5" class="ulSet">
                            </ul>
                            <%--<img id="imgMore5" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" />--%>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4 noPadding">
                        <div class="divTitle">
                            <img id="spanPartTitleIcon6" src="" />
                            <span id="spanPartTitle6" class="spanCategoryTitle">业内动态</span>
                        </div>
                        <div>
                            <ul id="ulPartContent6" class="ulSet">
                            </ul>
                            <%--<img id="imgMore6" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" />--%>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-3 col-md-3 col-lg-3">
                <div class="divYouLike">
                    <span class="spanCategoryTitle" data-id="youLike" style="float: left;">猜你喜欢</span>
                    <%--<img data-id="youLike" id="imgMoreYouLike" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" style="margin-top: 16px;" />--%>
                </div>
                <div class="divGuessYouLikeContent" style="">
                    <ul id="ulGuessYouLike">
                        <li>
                            <span class="spanGuessYouLikeTitle"></span>
                        </li>
                        <li>
                            <a class="aRankTop3"></a><span class="spanRankTitle"></span><span class="spanRankingIntegral">50</span>
                        </li>
                        <li>
                            <a class="aRankTop3"></a><span class="spanRankTitle"></span><span class="spanRankingIntegral">50</span>
                        </li>
                        <li>
                            <a class="aRankOther"></a><span class="spanRankTitle"></span><span class="spanRankingIntegral">50</span>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="row" style="margin-top: 20px">
            <div class="col-sm-9 col-md-9 col-lg-9 noPadding" style="padding: 0px;">
                <div class="col-sm-6 col-md-6 col-lg-6 noPadding">
                    <div class="divContent1">
                        <div class="divNewUpload">
                            <div class="divNewUpload1">
                                <span class="spanCategoryTitle" data-id="newUpload" style="float: left;">最新上传</span>
                                <%--<img data-id="newUpload" id="imgMoreNewUpload" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" style="margin-top: 16px;" />--%>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-6 noPadding">
                    <div class="divContent2">
                        <div class="divRank">
                            <div class="divRank1">
                                <span class="spanCategoryTitle" data-id="docRanking" style="float: left; cursor: pointer">文档排行榜</span>
                                <%--<img data-id="docRanking" id="imgMoreDocRanking" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/more.png" class="imgMore" style="margin-top: 16px;" />--%>
                            </div>
                        </div>
                        <div class="divRankContent">
                            <ul id="ulDocRanking">
                                <li>
                                    <a class="aRankTop3"></a><span class="spanRankTitle"></span><span class="spanRankingIntegral">50</span>
                                </li>
                                <li>
                                    <a class="aRankTop3"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankTop3"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                                <li>
                                    <a class="aRankOther"></a><span class="spanRankTitle"></span>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-3 col-md-3 col-lg-3 noPadding">
                <div class="divContent2">
                    <div class="divRank">
                        <div class="divRank1">用户排行榜</div>
                    </div>
                    <div class="divRankContent">
                        <ul id="ulUserRanking">
                            <li>
                                <a class="aRankTop3"></a><span class="spanRankTitle"></span><span class="spanRankingIntegral"></span>
                            </li>
                            <li>
                                <a class="aRankTop3"></a><span class="spanRankTitle"></span><span class="spanRankingIntegral"></span>
                            </li>
                            <li>
                                <a class="aRankTop3"></a><span class="spanRankTitle"></span><span class="spanRankingIntegral"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span><span class="spanRankingIntegral"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span>
                            </li>
                            <li>
                                <a class="aRankOther"></a><span class="spanRankTitle"></span>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //六块内容
        var tipNum = <%=tipNum%>;
        var json = <%=json%>;
        var folderName = <%=folderName%>;
        var tmpUpload = '<%=newUpload%>';
        var newUpload = eval(tmpUpload.replace(/<br>/g,';'));
        //var newUpload= <%=newUpload%>;
        var userName = '<%=userName%>';
        var integral = '<%=integral%>';
        var downNumber = <%=downNumber%>;
        var uploadNumber = <%=uploadNumber%>;
        var userRanking = <%=userRanking%>;
        var docRanking = <%=docRanking%>;
        var imgCarousel = <%=imgCarousel%>;
        //var resGuessYouLike = <%=resGuessYouLike%>;
        var tmpGuessLike = '<%=resGuessYouLike%>';
        var resGuessYouLike = eval(tmpGuessLike.replace(/<br>/g,';'));
        //var resGuessYouLike = "[]";
        //页面跳转
        $(".imgMore").click(function(e){
            dataID = $(e.target).attr("data-id");
            
            window.location.href = "DocumentList.aspx?FolderID=" + dataID + "";
        })

        $(".spanCategoryTitle").click(function(e){
            dataID = $(e.target).attr("data-id");
            window.location.href = "DocumentList.aspx?FolderID=" + dataID + "";
        })
        
        $(".spanYouLike").click(function(e){
            dataID = $(e.target).attr("data-id");
            window.location.href = "DocumentDown.aspx?FileID=" + dataID + "";
        })
        //个人中心
        function JumpUserCenter(obj){
            var listName = $(obj).attr("data-id");
            window.location.href = "UserCenter.aspx?ListName=" + listName + "";
        }
        function DocUpload(){
            window.location.href = "DocumentUpload.aspx";
        }
        function KnowledgeMapCenter(){
            window.location.href = "KnowledgeMap.aspx";
        }
    </script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/JS/Index.js"></script>
</asp:Content>
