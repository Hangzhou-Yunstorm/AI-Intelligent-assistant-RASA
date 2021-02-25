$(function () {
    var width = document.documentElement.clientWidth;
    var margin;
    if (width > 1560) {
        margin = (width - 1560) / 2;
        if (margin < 100) {
            $('.divListContent').css('width', "76%");
            $('.divListContent').css('margin', "0 12% 0 12%");
            $('.spanTitle').css('left', "12%");
        }
        else {
            $('.spanTitle').css('left', margin);
            $('.divListContent').css('width', "100%");
            $('.divListContent').css('left', margin);
        }
    }
    else {
        $('.divListContent').css('width', "76%");
        $('.divListContent').css('margin', "0 12% 0 12%");
        $('.spanTitle').css('left', "12%");
    }
    if (requestID == "youLike" || requestID == "newUpload" || requestID == "docRanking") {
        switch (requestID) {
            case "newUpload":
                $(".spanTitle").text("最新上传列表");
                $(".spanDocumentListTitle").text("最新上传列表");
                $("#aListName").text("最新上传列表");
                break;
            case "youLike":
                $(".spanTitle").text("猜你喜欢列表");
                $(".spanDocumentListTitle").text("猜你喜欢列表");
                $("#aListName").text("猜你喜欢列表");
                break;
            case "docRanking":
                $(".spanTitle").text("文档排行列表");
                $(".spanDocumentListTitle").text("文档排行列表");
                $("#aListName").text("文档排行列表");
                break;
        }
    }
    if (parentName != "" && parentId != "") {
        $(".spanDocumentListTitle").text(parentName + "列表");
        var html = "<li data-id=\"" + parentId + "\" ><a class=\"nav_root\" >" + parentName + "列表</a></li>" +
        "<li><span style=\"color: #535EB1;\">/</span></li>"
        $("#noUse").after(html);
    }

    $(".ulNav li").click(function () {
        if ($(this).attr("data-id") != "index" && $(this).attr("data-id") != "disable") {
            var li = $(this).index();
            var fileID = $(this).attr("data-id");
            var titleName = $(this).text();
            $(".spanDocumentListTitle").text(titleName);
            var liNum = $(".ulNav").children("li");
            for (var i = li + 1; i < liNum.length; i++) {
                try {
                    if (navigator.appName == navigatorName) {
                        liNum[i].removeNode(true);
                    }
                    else
                        liNum[i].remove();
                }
                catch (e) {
                    liNum[i].removeNode(true);
                }
            }
            ClickTableRow(fileID);
        }
    });

    var getData = eval(documentList1);
    var data = ChangeData(getData);
    CreatTable(data);
});

//function ClickLi(e) {
//    var li = $(this).index();
//    var liNum = $(".ulNav").children("li");
//}
var navigatorName = "Microsoft Internet Explorer";

function ClickTableRow(fileID) {
    $(".divTable").showLoading();
    $.ajax({
        type: 'post',
        url: 'DocumentList.aspx/ClickTableRow',
        data: '{ "fileID": "' + fileID + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var tableData = ChangeData(eval(data.d));
            CreatTable(tableData);
            $(".divTable").hideLoading();
        }
    })
}

//数据转换
function ChangeData(docList) {
    var docTableList;
    if (docList.length > 0) {
        docTableList = "[";
        for (var i = 0; i < docList.length; i++) {
            if (docList[i]["extension"] == "folder") {
                docTableList += "{" +
                                 "id:'" + docList[i]["Id"] + "'," +
                                 "typeRecord:'" + docList[i]["extension"] + "'," +
                                 "type:'<img src=\"~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/" + docList[i]["extension"] + ".png\" />'," +
                                 "documentNameDisplay:'" + docList[i]["Title"] + "'," +
                                 "documentName:'<a style=\"cursor: pointer;\">" + docList[i]["Title"] + "</a>'," +
                                 "author:'" + docList[i]["Author"] + "'," +
                                 "uploadTime:'" + docList[i]["UploadTime"] + "'," +
                                 "documentType:'" + docList[i]["KnowledgeType"] + "'," +
                                 "clicks:'" + docList[i]["Clicks"] + "'," +
                                 "downloads:'" + docList[i]["Downloads"] + "'," +
                                 "downloadIntegral:'" + docList[i]["DownloadIntegral"] + "'," +
                                 "comments:'" + docList[i]["Comments"].replace(/<br>/g, ";") + "'," +
                                 "grade:''" +
                                 "},";
            }
            else {
                docTableList += "{" +
                                 "id:'" + docList[i]["Id"] + "'," +
                                 "typeRecord:'" + docList[i]["extension"] + "'," +
                                 "type:'<img src=\"~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/" + docList[i]["extension"] + ".png\" />'," +
                                 "documentNameDisplay:'" + docList[i]["Title"] + "'," +
                                 "documentName:'<a style=\"cursor: pointer;\">" + docList[i]["Title"] + "</a>'," +
                                 "author:'" + docList[i]["Author"] + "'," +
                                 "uploadTime:'" + docList[i]["UploadTime"] + "'," +
                                 "documentType:'" + docList[i]["KnowledgeType"] + "'," +
                                 "clicks:'" + docList[i]["Clicks"] + "'," +
                                 "downloads:'" + docList[i]["Downloads"] + "'," +
                                 "downloadIntegral:'" + docList[i]["DownloadIntegral"] + "'," +
                                 "comments:'" + docList[i]["Comments"] + "'," +
                                 "grade:'<input id=\"input-3\" value=\"" + docList[i]["Grade"] + "\"  type=\"number\" class=\"rating\" min=\"0\" max=\"5\" step=\"0.5\" data-size=\"ms\" disabled=\"disabled\" />'" +
                                 "},";
            }

        }
        if (docTableList.charAt(docTableList.length - 1) == ",") {
            docTableList = docTableList.substring(0, docTableList.length - 1);
        }
        docTableList += "]";

    }
    else {
        docTableList = "[]";
    }
    return docTableList;
}

//创建表格
function CreatTable(data) {
    var tableData = eval(data);
    ////$('#documentListTable').bootstrapTable('destroy');
    if ($('#documentListTable').html() == "") {
        $('#documentListTable').bootstrapTable({
            //locale: 'zh-CN',
            method: 'get',
            cache: false,
            striped: true,
            pagination: true,
            search: true,
            //showPaginationSwitch: true,
            sortable: true,
            sortOrder: 'desc',
            sortName: 'grade',
            //height: 360,
            //height: 750,
            pageNumber: 1,
            pageSize: 15,
            pageList: [15, 25, 50, 100],
            minimumCountColumns: 2,
            onAll: function (name, args) {
                //if (name == 'post-header.bs.table')
                ratingRefresh();
            },
            onClickCell: function (field, value, row, $element) {
                if (field == "documentName") {
                    if (row.typeRecord == "folder") {
                        //$('#documentListTable').bootstrapTable('destroy');
                        ClickTableRow(row.id);
                        $(".spanDocumentListTitle").text(row.documentNameDisplay + "列表");
                        var html = "<li><span style=\"color: #535EB1;\">/</span></li>" +
                      "<li data-id=\"" + row.id + "\" ><a class=\"nav_root\" >" + row.documentNameDisplay + "列表</a></li>"
                        $(".ulNav").append(html);
                        $(".ulNav li").click(function () {
                            if ($(this).attr("data-id") != "index" && $(this).attr("data-id") != "disable") {
                                var li = $(this).index();
                                var fileID = $(this).attr("data-id");
                                var titleName = $(this).text();
                                $(".spanDocumentListTitle").text(titleName);
                                var liNum = $(".ulNav").children("li");
                                for (var i = li + 1; i < liNum.length; i++) {
                                    liNum[i].remove();
                                }
                                ClickTableRow(fileID);
                            }
                        });
                        //ratingRefresh();
                    }
                    else {
                        window.location.href = "DocumentDown.aspx?FileID=" + row.id + "";
                    }
                }
            },
            //onSort: function (name, order) { ratingRefresh(); },
            columns: [{
                field: 'id',
                title: 'ID',
                visible: false
            }, {
                field: 'typeRecord',
                title: '类型记录',
                visible: false
            }, {
                field: 'type',
                title: '类型',
                align: 'center',
                width: "50px"
            }, {
                field: 'documentNameDisplay',
                title: '隐藏文档名称',
                align: 'center',
                visible: false,
            }, {
                field: 'documentName',
                title: '文档名称',
                align: 'center',
                class: 'cursor',
                //width:'30%'
            }, {
                field: 'author',
                title: '作者',
                align: 'center',
                width: '100px'
            }, {
                field: 'uploadTime',
                title: '上传时间',
                align: 'center',
                sortable: true,
                width: '120px'
                //order: 'desc',
            }, {
                field: 'documentType',
                title: '所属类别',
                align: 'center',
                width: '120px'
            }, {
                field: 'clicks',
                title: '点击量',
                align: 'center',
                width: '100px'
            }, {
                field: 'downloads',
                title: '下载量',
                align: 'center',
                width: '100px'
            }, {
                field: 'comments',
                title: '评价次数',
                align: 'center',
                visible: false
            }, {
                field: 'downloadIntegral',
                title: '下载积分',
                align: 'center',
                width: '100px'
            }
            , {
                field: 'grade',
                title: '评分',
                align: 'center',
                width: '160px',
                sortable: true,
            }],
            data: tableData
        });
        //$('#documentListTable').on('sort.bs.table', function (name, order) {
        //    ratingRefresh();
        //});
        //ratingRefresh();
    }
    else {
        $('#documentListTable').bootstrapTable('load', tableData);
        //ratingRefresh();
    }
}



//测试数据
var tempData = [{
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}, {
    type: '<a class="aRecommendWord">W</a>',
    documentName: '如何成为一个大腿',
    author: 'heicaodan',
    uploadTime: '2016-6-17',
    documentType: '技术文档',
    clicks: '233',
    downloads: '233',
    comments: '233',
    downloadIntegral: '5',
    grade: '<input id="input-3" value="4.5" type="number" class="rating" min="0" max="5" step="0.5" data-size="ms" disabled="disabled" />'
}]
