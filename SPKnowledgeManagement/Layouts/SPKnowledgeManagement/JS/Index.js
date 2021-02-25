/// <reference path="C:\大华知识库\KM\KB\DocumentList.aspx" />
$(function () {
    var width = document.documentElement.clientWidth;
    //$("#hitxtSeach").css("left", (width - 800) / 2);
    //$("#imgSeach").css("left", (width - 800) / 2 - 35 + 800);
    //$("#hspanHot").css("left", (width - 800) / 2 + 30);
    //var left = $(".divPart").width();
    //var height = $(".divLeft").height();
    var margin;
    if (width > 1560) {
        margin = (width - 1560) / 2;
        if (margin < 100) {
            $("#divContent").css("width", "76%");
            $("#divContent").css("margin", "0 12% 0 12%");
        }
        else {
            $('#divContent').css("left", margin);
        }
    }
    else {
        $("#divContent").css("width", "76%");
        $("#divContent").css("margin", "0 12% 0 12%");
    }

    $(".spanArticleTitle").click(function (e) {
        var fileID = $(e.target).attr("data-id");
        window.location.href = "DocumentDown.aspx?FileID=" + fileID + "";
    });
    //分享数
    if (tipNum == "0") {
        $("#spanTipNum").css("display", "none");
        $("#spanUnReadNumber").text("(" + tipNum + ")")
    }
    else {
        $("#spanTipNum").text(tipNum);
        $("#spanUnReadNumber").text("(" + tipNum + ")")
    }

    //图片轮播
    //HTML拼写
    $(".slides").html("");
    var imgCarouselHtmlUl = "";
    for (var i = 0; i < imgCarousel.length; i++) {
        imgCarouselHtmlUl += '<li>' +
                                                '<a title="" target="_blank" id="' + imgCarousel[i]["Id"] + '" onclick="ImgCarouselJump(this);" style="cursor: pointer;position:absolute;width:100%">' +
                                                    '<span class="spanCarousel">' + imgCarousel[i]["Title"] + '</span>' +
                                                    '<img src="' + imgCarousel[i]["ImageUrl"] + '" />' +
                                                '</a>' +
                                         '</li>';
    }
    $(".slides").html(imgCarouselHtmlUl);
    $("#bannerCtrl").html("");
    var imgCarouselHtmlOl = "";
    for (var i = 0; i < imgCarousel.length; i++) {
        imgCarouselHtmlOl += '<li><a>2</a></li>';
    }
    $("#bannerCtrl").html(imgCarouselHtmlOl);
    //插件调用
    var bannerSlider = new Slider($('#banner_tabs'), {
        time: 15000,
        delay: 300,
        event: 'hover',
        auto: true,
        mode: 'fade',
        controller: $('#bannerCtrl'),
        activeControllerCls: 'active'
    });
    $('#banner_tabs .flex-prev').click(function () {
        bannerSlider.prev()
    });
    $('#banner_tabs .flex-next').click(function () {
        bannerSlider.next()
    });

    for (var i = 0; i < json.length; i++) {
        if (json[i].Name != "活动" && json[i].Name != "Forms" && json[i].Name != "公共文档库") {
            $("#spanPartTitle" + (i + 1)).text(json[i].Name);
            $("#spanPartTitle" + (i + 1)).attr("data-id", json[i].Id);
            $("#spanPartTitleIcon" + (i + 1)).attr('src', "~/_Layouts/15/SPKnowledgeManagement/theme/img/categoryIcon/" + json[i].Name + ".png");
            $("#ulPartContent" + (i + 1)).html("");
            $("#imgMore" + (i + 1)).attr("data-id", json[i].Id);
            var html = "";
            for (var j = 0 ; j < json[i].Items.length; j++) {
                html += '<li data-id=\"' + json[i].Items[j].Id + '\" title="' + json[i].Items[j].Title + '"><a id="' + json[i].Items[j].Id + '"><span>' + GetTitle(json[i].Items[j].Title, 15) + '</span></a></li>';//<span class="liTime">' + json[i].Items[j].Created + '</span>
            }
            $("#ulPartContent" + (i + 1)).html(html);
        }
    }


    //页面跳转
    $(".ulSet li").click(function () {
        var fileID = $(this).attr("data-id");
        window.location.href = "DocumentDown.aspx?FileID=" + fileID + "";
    });

    //最新上传
    var htmlNewUpload = "";

    for (var i = 0; i < newUpload.length; i++) {
        ////$("#imgNewUpload1") 
        //$("#spanNewUploadType" + (i + 1)).text(newUpload[i]["KnowledgeType"]);
        //$("#spanNewUploadTitle" + (i + 1)).text(GetTitle(newUpload[i]["DisplayName"], 15));
        //$("#spanNewUploadTitle" + (i + 1)).attr("data-id", newUpload[i]["Id"]);
        //$("#spanNewUploadTitle" + (i + 1)).attr("title", newUpload[i]["DisplayName"]);
        ////$("#aNewUpload1" + (i + 1))
        ////var remark = GetTitle(newUpload[i]["Remark"], 100);
        ////$("#pNewUploadRemark" + (i + 1)).text(remark);
        //$("#spanUploadTime" + (i + 1)).text(newUpload[i]["Created"]);
        //$("#spanUploadClicks" + (i + 1)).text(newUpload[i]["Clicks"]);
        ////$("#spanUploadComments" + (i + 1)).text(newUpload[i]["Comments"]);
        htmlNewUpload += '<div class="divNewUploadContent">' +
    '<div class="div-100-width">' +
        '<img id="imgNewUpload1" class="imgaArticlePic" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/title_bg.png" />' +
        '<span id="spanNewUploadType1" class="spanArticleType">' + newUpload[i]["KnowledgeType"] + '</span>' +
        '<span id="spanNewUploadTitle1" title="' + newUpload[i]["DisplayName"] + '" data-id="' + newUpload[i]["Id"] + '" class="spanArticleTitle" onclick="DocRankingJump(this)">' + GetTitle(newUpload[i]["DisplayName"], 15) + '</span><br />' +
        '<div class="divIcon">' +
            '<img class="imgArticleIcon" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/ic_alarm.png" />' +
            '<span id="spanUploadTime1" class="spanArticle">' + newUpload[i]["Created"] + '</span>' +
            '<img class="imgArticleIcon" src="~/_Layouts/15/SPKnowledgeManagement/theme/img/ic_visibility.png" />' +
            '<span id="spanUploadClicks1" class="spanArticle">' + newUpload[i]["Clicks"] + '</span>' +
        '</div>' +
    '</div>' +
'</div>';
    }
    $(".divNewUpload").after(htmlNewUpload);









    //个人中心 
    $("#spanUserName").text(userName);
    $("#spanIntegral").text(integral);
    $("#spanDownloadNumber").text(downNumber);
    $("#spanDownloadNumber1").text("(" + downNumber + ")");
    $("#spanUploadNumber").text("(" + uploadNumber + ")");

    //猜你喜欢
    //var guessData = eval(resGuessYouLike);
    //for (var i = 0; i < guessData.length; i++) {
    //    $("#spanYouLike" + (i + 1)).text(GetTitle(guessData[i]["Title"], 18));
    //    $("#spanYouLike" + (i + 1)).attr("data-id", guessData[i]["ID"])
    //    $("#spanYouLike" + (i + 1)).attr("title", guessData[i]["Title"]);
    //    var remark = GetTitle(guessData[i]["Remark"].replace('<br/>', '/n'), 75);
    //    $("#pYouLike" + (i + 1)).text(remark);
    //    $("#pYouLike" + (i + 1)).height($("#pYouLike" + (i + 1))[0].scrollHeight);
    //    $("#pYouLike" + (i + 1)).attr("data-id", guessData[i]["ID"])
    //}
    var guessData = eval(resGuessYouLike);
    var guessLikeHtml = "";
    for (var i = 0; i < guessData.length; i++) {
        guessLikeHtml += '<li data-id="' + guessData[i]["ID"] + '" onclick="DocRankingJump(this)" style="cursor: pointer;"><img src="~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/' + guessData[i]["Extension"] + '.png" style="width:20px;"/><span class="spanGuessYouLikeTitle" title="' + guessData[i]["Title"] + '">' + GetTitle(guessData[i]["Title"], 15) + '</span></li>'
    }
    $("#ulGuessYouLike").html(guessLikeHtml);

    //用户排名
    $("#ulUserRanking").html("");
    var userRankingHtml = "";
    for (var i = 0; i < userRanking.length; i++) {
        if (i < 3) {
            userRankingHtml += '<li><a class="aRankTop3">' + (i + 1) + '</a><span class="spanRankTitle">' + GetTitle(userRanking[i]["UserName"], 10) + '</span><span class="spanRankingIntegral">' + userRanking[i]["Integral"] + '分</span></li>';
        }
        else {
            userRankingHtml += '<li><a class="aRankOther">' + (i + 1) + '</a><span class="spanRankTitle">' + GetTitle(userRanking[i]["UserName"], 10) + '</span><span class="spanRankingIntegral">' + userRanking[i]["Integral"] + '分</span></li>';
        }
    }
    $("#ulUserRanking").html(userRankingHtml);
    //文档排名  
    $("#ulDocRanking").html("");
    var docRankingHtml = "";
    var extension = "";
    for (var i = 0; i < docRanking.length; i++) {
        if (i < 3) {
            docRankingHtml += '<li data-id="' + docRanking[i]["ID"] + '" onclick="DocRankingJump(this)" style="cursor: pointer;"><a class="aRankTop3">' + (i + 1) + '</a><img src="~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/' + docRanking[i]["Extension"] + '.png" style="width:20px;"/><span class="spanRankTitle" title="' + docRanking[i]["Title"] + '">' + GetTitle(docRanking[i]["Title"], 15) + '</span><span class="spanRankingIntegral">' + docRanking[i]["Author"] + '</span></li>';
        }
        else {
            docRankingHtml += '<li data-id="' + docRanking[i]["ID"] + '" onclick="DocRankingJump(this)" style="cursor: pointer;"><a class="aRankOther">' + (i + 1) + '</a><img src="~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/' + docRanking[i]["Extension"] + '.png" style="width:20px;"/><span class="spanRankTitle" title="' + docRanking[i]["Title"] + '">' + GetTitle(docRanking[i]["Title"], 15) + '</span><span class="spanRankingIntegral">' + docRanking[i]["Author"] + '</span></li>';
        }
    }
    $("#ulDocRanking").html(docRankingHtml);
});

function DocRankingJump(obj) {
    var fileId = $(obj).attr("data-id");
    window.location.href = "DocumentDown.aspx?FileID=" + fileId + "";
}

//图片轮播跳转
function ImgCarouselJump(obj) {
    var fileId = $(obj).attr("id");
    window.location.href = "DocumentDown.aspx?FileID=" + fileId + "";
}

//切割名称太长
function GetTitle(title, num) {
    var value;
    if (title.length > num) {
        value = title.substr(0, num) + "...";
    }
    else {
        value = title;
    }
    return value;
}
