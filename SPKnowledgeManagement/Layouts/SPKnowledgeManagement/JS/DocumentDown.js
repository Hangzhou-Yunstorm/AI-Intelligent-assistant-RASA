$(function () {
    $("#comment").text('');
    var width = document.documentElement.clientWidth;
    var height = document.documentElement.clientHeight;
    var offset = $("#divFooter").offset().top;
    if (offset < height && height > 768) {
        $("#divFooter").css('top', '' + height - offset - 86 + 'px')
    }
    var margin;
    if (width > 1560) {
        margin = (width - 1560) / 2;
        if (margin < 100) {
            $('.spanTitle').css('left', "12%");
            $('.divDownContent').css('width', "76%");
            $('.divDownContent').css('margin', "0 12% 0 12%");
        }
        else {
            $('.spanTitle').css('left', margin);
            $('.divDownContent').css('width', "100%");
            $('.divDownContent').css('left', margin);
        }
    }
    else {
        $('.spanTitle').css('left', "12%");
        $('.divDownContent').css('width', "76%");
        $('.divDownContent').css('margin', "0 12% 0 12%");
    }

    $("#docTitle").text(docInfo[0]["Title"]);
    $("#docUploadTime").text(docInfo[0]["UploadTime"]);
    $("#docAuthor").text("上传者：" + docInfo[0]["Author"]);
    $("#docUploadTime").text(docInfo[0]["UploadTime"]);
    $("#docType").text("当前归类：" + docInfo[0]["KnowledgeType"]);
    $("#docClick").text(docInfo[0]["Clicks"] + "人阅读");
    $("#docDownload").text(docInfo[0]["Downloads"] + "人下载");
    $("#input-1").attr("value", docInfo[0]["Grade"]);
    $("#input-1").attr("title", docInfo[0]["Grade"]);
    $("#docRemark").text(docInfo[0]["Remark"].replace(/<br>/g, '\n'));
    $("#docRemark").height($("#docRemark")[0].scrollHeight);
    $("#spanDownIntegral").text(docInfo[0]["DownloadIntegral"] + "分");
    $("#nowIntegral").text(integral + "分");
    $("#downIntegral").text((integral - docInfo[0]["DownloadIntegral"]) + "分");

    if (docDowned == "yes") {
        $("#docDowned").text("您已下载过该文档");
    }
    else {
        $("#docDowned").text("您尚未下载过该文档");
    }
    var downTime = parseInt(integral / docInfo[0]["DownloadIntegral"]);
    if (isNaN(downTime)) {
        $("#docDown").text("下载此文档需要0积分。");
    }
    else {
        $("#docDownTime").text(downTime);
    }
    if (grade != "0") {
        $("#gradeOwn").attr("value", grade);
        $("#gradeOwn").rating('refresh', { disabled: true });
    }
    else {
        $("#gradeOwn").attr("value", 0);
    }
    //加载星级评论插件
    ratingRefresh();
    //加载评论    
    Page(pageNum);
    LoadComment(1, pageNum);

});


function Page(pageNum) {
    var totalPageNum = 0;
    var fileID = requestID;
    $.ajax({
        type: 'post',
        url: 'DocumentDown.aspx/GetCommentNum',
        data: '{ "fileID": "' + fileID + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            totalPageNum = Math.ceil(parseInt(data.d) / pageNum);
            if (totalPageNum != 0) {
                $.jqPaginator('#pagination1', {
                    first: '<li class="first"><a href="javascript:;">首页</a></li>',
                    prev: '<li class="prev"><a href="javascript:;">上一页</a></li>',
                    next: '<li class="next"><a href="javascript:;">下一页</a></li>',
                    last: '<li class="last"><a href="javascript:;">尾页</a></li>',
                    page: '<li class="page"><a href="javascript:;">{{page}}</a></li>',
                    totalPages: totalPageNum,
                    visiblePages: pageNum,
                    currentPage: 1,
                    visiblePages: 5,
                    onPageChange: function (num, type) {
                        $('#p1').text(type + '：' + num);
                        LoadComment(num, pageNum);
                    }
                });
                $("#pagination1").css("display", "block");
                $("#pagination1").css("margin-top", "40px");
            }
        }
    })
}

function SendComment(obj) {
    $(obj).attr("disabled", "disabled");
    var fileID = requestID;
    var comment = $('#comment').val();
    var grade = $("#gradeOwn").val();
    if (isComment == "no") {
        if (parseFloat(grade) == 0) {
            alert('请评分'); $(obj).removeAttr("disabled"); return;
        }
    }
    else {
        if (comment == "") {
            alert('请输入评论内容'); $(obj).removeAttr("disabled"); return;
        }
    }

    if (comment == "")
        comment = "暂无评论";
    //comment = comment.replace(/\n/g, "<br/>");
    $.ajax({
        type: 'post',
        url: 'DocumentDown.aspx/SendComment',
        data: '{ "fileID": "' + fileID + '","comment":"' + comment + '","grade":"' + grade + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d == "success") {
                alert("评论提交成功！");
                if (isComment == 'no') window.location.reload();
                Page(pageNum);
                LoadComment(1, pageNum);
                $("#comment").val("");
                $("#gradeOwn").parents(".star-rating").removeClass("rating-active");
                $("#gradeOwn").rating('refresh', { disabled: true });
                $(obj).removeAttr("disabled");
                isComment = 'yes';
            }
            else if (data.d == "error") {
                alert("评论提交功能出现错误，请与管理员联系！");
                $(obj).removeAttr("disabled");
            }
        },
        error: function (data) {
            alert('网络错误');
        }
    })
}

function LoadComment(pageId, pageNum) {
    var fileID = requestID;
    $.ajax({
        type: 'post',
        url: 'DocumentDown.aspx/LoadComment',
        data: '{ "fileID": "' + fileID + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var html = "";
            if (data.d != "[]") {
                var commentArr = eval(data.d);
                $("#divComment").html("");
                try {
                    for (var i = pageNum * (pageId - 1) ; i < pageNum * pageId; i++) {
                        if (commentArr[i]["ReplyId"] == "0") {
                            html += '<div class="divUserComment">' +
                                                '<div class="divUserCommentContent">' +
                                                     '<div class="div-100-width">' +
                                                         '<div class="col-sm-9 col-md-9 col-lg-9">' +
                                                            '<span class="spanCommentUserName">' + commentArr[i]["UserName"] + '</span>' +
                                                            '<span class="spanDivision">|</span>' +
                                                            '<input id="inputComment1" value="' + commentArr[i]["Grade"] + '" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />' +
                                                         '</div>' +
                                                        '<div class="col-sm-3 col-md-3 col-lg-3">' +
                                                            '<span class="spanFloor">' + commentArr[i]["Floor"] + '楼</span>' +
                                                        '</div>' +
                                                    '</div>' +
                                                    '<div class="div-100-width">' +
                                                        '<div class="col-sm-9 col-md-9 col-lg-9">' +
                                                            '<textarea style="border: none;resize:none;outline:none;width:100%" readonly="readonly" class="spanCommentContent">' + commentArr[i]["Comment"].replace(/<br>/g, '\n') + '</textarea>' +
                                                        '</div>' +
                                                        '<div class="col-sm-3 col-md-3 col-lg-3">' +
                                                            '<div class="row">' +
                                                                '<span class="spanCommentTime">' + commentArr[i]["Created"] + '</span>' +
                                                            '</div>' +
                                                            '<div class="row">' +
                                                                '<a class="aReply" onclick="OpenModal(this)" data-id="' + commentArr[i]["Id"] + '">回复</a>' +
                                                             '</div>' +
                                                        '</div>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</div>' +
                                            '<hr class="hr" />';
                        }
                        else {
                            html += '<div class="divUserComment">' +
                                                '<div class="divUserCommentContent">' +
                                                     '<div class="div-100-width">' +
                                                         '<div class="col-sm-9 col-md-9 col-lg-9">' +
                                                            '<span class="spanCommentUserName">' + commentArr[i]["UserName"] + '</span>' +
                                                            '<span class="spanDivision">|</span>' +
                                                            '<span class="replyFloor">回复第' + commentArr[i]["ReplyFloor"] + '楼</span>' +
                                                         '</div>' +
                                                        '<div class="col-sm-3 col-md-3 col-lg-3">' +
                                                            '<span class="spanFloor">' + commentArr[i]["Floor"] + '楼</span>' +
                                                        '</div>' +
                                                    '</div>' +
                                                    '<div class="div-100-width">' +
                                                        '<div class="col-sm-9 col-md-9 col-lg-9" style="margin-top:4px;">' +
                                                            '<textarea style="border: none;resize:none;outline:none;width:100%"  readonly="readonly" class="spanCommentContent">' + commentArr[i]["Comment"].replace(/<br>/g, '\n') + '</textarea>' +
                                                        '</div>' +
                                                        '<div class="col-sm-3 col-md-3 col-lg-3" style="margin-top:4px;">' +
                                                            '<div class="row">' +
                                                                '<span class="spanCommentTime">' + commentArr[i]["Created"] + '</span>' +
                                                            '</div>' +
                                                            '<div class="row">' +
                                                                '<a class="aReply" onclick="OpenModal(this)" data-id="' + commentArr[i]["Id"] + '">回复</a>' +
                                                             '</div>' +
                                                        '</div>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</div>' +
                                            '<hr class="hr" />';

                        }
                    }
                }
                catch (e) {
                }
                $("#divComment").html(html);

            }
            else {
                html = '<div class="divUserComment">' +
                                '<div class="divUserCommentContent">' +
                                '<h4>暂无评论</h4>' +
                                '</div>' +
                            '</div>';
                $("#divComment").html(html);
                $(".divUserComment").css("height", "120px");
            }

            $(".spanCommentContent").each(function () {
                $(this).height($(this)[0].scrollHeight);
            })
            ratingRefresh();
        }
    })
}

//模态框
function ModalBand() {
    $(".aReply").each(function (e) {
        $(this).click(function () {
            var replyId = $(this).attr("data-id");
            $('#commentModal').modal('show');
            $("#btnConfirm").attr("data-id", replyId);
        })
    })
}


