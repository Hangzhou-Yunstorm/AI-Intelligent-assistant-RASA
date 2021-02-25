$(function () {
    var width = document.documentElement.clientWidth;
    var margin;
    if (width > 1560) {
        margin = (width - 1560) / 2;
        if (margin < 100) {
            $('.divUserCenter').css("width", "76%");
            $('.divUserCenter').css("margin", "0 12% 0 12%");
        }
        $('.divUserCenter').css("left", margin);
    }
    else {
        $('.divUserCenter').css("width", "76%");
        $('.divUserCenter').css("margin", "0 12% 0 12%");
    }
    //下载列表data生成
    var downData = "[";
    for (var i = 0; i < downList.length; i++) {
        downData += "{" +
                               "id:'" + downList[i]["Id"] + "'," +
                               "type:'<img src=\"~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/" + downList[i]["extension"] + ".png\" />'," +
                               "documentName:'<a style=\"cursor: pointer;\">" + downList[i]["Title"] + "</a>'," +
                               "author:'" + downList[i]["Author"] + "'," +
                               "downTime:'" + downList[i]["DownloadTime"] + "'," +
                               "documentType:'" + downList[i]["KnowledgeType"] + "'," +
                               "clicks:'" + downList[i]["Clicks"] + "'," +
                               "downloads:'" + downList[i]["Downloads"] + "'," +
                               "comments:'" + downList[i]["Comments"] + "'," +
                               "downloadIntegral:'" + downList[i]["DownloadIntegral"] + "'," +
                               "grade:'<input id=\"input-3\" value=\"" + downList[i]["Grade"] + "\" title=\"" + downList[i]["Grade"] + "\"  type=\"number\" class=\"rating\" min=\"0\" max=\"5\" step=\"0.5\" data-size=\"ms\" disabled=\"disabled\" />'" +
                               "},";
    }
    if (downData.charAt(downData.length - 1) == ",") {
        downData = downData.substring(0, downData.length - 1);
    }
    downData += "]";
    //上传列表data生产
    var uploadData = "[";
    for (var i = 0; i < uploadList.length; i++) {
        uploadData += "{" +
                               "id:'" + uploadList[i]["Id"] + "'," +
                               "typeRecord:'" + uploadList[i]["extension"] + "'," +
                               "type:'<img src=\"~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/" + uploadList[i]["extension"] + ".png\" />'," +
                               "documentName:'<a style=\"cursor: pointer;\">" + uploadList[i]["Title"] + "</a>'," +
                               "author:'" + uploadList[i]["Author"] + "'," +
                               "uploadTime:'" + uploadList[i]["UploadTime"] + "'," +
                               "documentType:'" + uploadList[i]["KnowledgeType"] + "'," +
                               "clicks:'" + uploadList[i]["Clicks"] + "'," +
                               "downloads:'" + uploadList[i]["Downloads"] + "'," +
                               "comments:'" + uploadList[i]["Comments"] + "'," +
                               "downloadIntegral:'" + uploadList[i]["DownloadIntegral"] + "'," +
                               "grade:'<input id=\"input-3\" value=\"" + uploadList[i]["Grade"] + "\" title=\"" + uploadList[i]["Grade"] + "\"  type=\"number\" class=\"rating\" min=\"0\" max=\"5\" step=\"0.5\" data-size=\"ms\" disabled=\"disabled\" />'," +
                               "share:'<button class=\"btn btn-default\" type=\"button\" onclick=\"OpenShare(this);\" data-id=\"" + uploadList[i]["Id"] + "\" >分享</button>'" +
                               "},";
    }
    if (uploadData.charAt(uploadData.length - 1) == "|") {
        uploadData = uploadData.substring(0, uploadData.length - 1);
    }
    uploadData += "]";
    //未读分享data
    var unReadData = "[";
    for (var i = 0; i < unReadList.length; i++) {
        unReadData += "{" +
                               "id:'" + unReadList[i]["ShareDocumentId"] + "'," +
                               "typeRecord:'" + unReadList[i]["extension"] + "'," +
                               "type:'<img src=\"~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/" + unReadList[i]["extension"] + ".png\" />'," +
                               "documentName:'<a style=\"cursor: pointer;\">" + unReadList[i]["Title"] + "</a>'," +
                               "shareTime:'" + unReadList[i]["ShareTime"] + "'," +
                               "ownerName:'" + unReadList[i]["OwnerName"] + "'" +
                               "},";
    }
    if (unReadData.charAt(unReadData.length - 1) == "|") {
        unReadData = unReadData.substring(0, unReadData.length - 1);
    }
    unReadData += "]";
    //所有分享data
    var readData = "[";
    for (var i = 0; i < readList.length; i++) {
        readData += "{" +
                               "id:'" + readList[i]["ShareDocumentId"] + "'," +
                               "typeRecord:'" + readList[i]["extension"] + "'," +
                               "type:'<img src=\"~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/" + readList[i]["extension"] + ".png\" />'," +
                               "documentName:'<a style=\"cursor: pointer;\">" + readList[i]["Title"] + "</a>'," +
                               "shareTime:'" + readList[i]["ShareTime"] + "'," +
                               "ownerName:'" + readList[i]["OwnerName"] + "'," +
                               "isRead:'" + readList[i]["IsRead"] + "'" +
                               "},";
    }
    if (readData.charAt(readData.length - 1) == "|") {
        readData = readData.substring(0, readData.length - 1);
    }
    readData += "]";

    //未通过审批文档data
    var examineData = "[";
    for (var i = 0; i < examineList.length; i++) {
        examineData += "{" +
                               "id:'" + examineList[i]["Id"] + "'," +
                               "typeRecord:'" + examineList[i]["extension"] + "'," +
                               "type:'<img src=\"~/_Layouts/15/SPKnowledgeManagement/theme/fileIcon/" + examineList[i]["extension"] + ".png\" />'," +
                               "documentName:'<a style=\"cursor: pointer;\">" + examineList[i]["Title"] + "</a>'," +
                               "author:'" + examineList[i]["Author"] + "'," +
                               "uploadTime:'" + examineList[i]["UploadTime"] + "'," +
                               "documentType:'" + examineList[i]["KnowledgeType"] + "'," +
                               "examineState:'" + examineList[i]["ExamineState"] + "'" +
                               "},";
    }
    if (examineData.charAt(examineData.length - 1) == "|") {
        examineData = examineData.substring(0, examineData.length - 1);
    }
    examineData += "]";

    GetListTable(listName, uploadData);

    //绑定按钮
    //上传列表
    $("#liUploadList").click(function (event) {
        $("#liUploadList").css("background-color", "#d4d4d4");
        $("#liUploadList").css("font-size", "22px");
        $("#liDownList").css("background-color", "#fff");
        $("#liDownList").css("font-size", "15px");
        $("#liUnReadList").css("background-color", "#fff");
        $("#liUnReadList").css("font-size", "15px");
        $("#liReadList").css("background-color", "#fff");
        $("#liReadList").css("font-size", "15px");
        $("#liExamineList").css("background-color", "#fff");
        $("#liExamineList").css("font-size", "15px");
        listName = "上传列表";
        GetListTable(listName, uploadData);
        ratingRefresh();
    });
    //下载列表
    $("#liDownList").click(function (event) {
        $("#liDownList").css("background-color", "#d4d4d4");
        $("#liDownList").css("font-size", "22px");
        $("#liUploadList").css("background-color", "#fff");
        $("#liUploadList").css("font-size", "15px");
        $("#liUnReadList").css("background-color", "#fff");
        $("#liUnReadList").css("font-size", "15px");
        $("#liReadList").css("background-color", "#fff");
        $("#liReadList").css("font-size", "15px");
        $("#liExamineList").css("background-color", "#fff");
        $("#liExamineList").css("font-size", "15px");
        listName = "下载列表";
        GetListTable(listName, downData);
        ratingRefresh();
    });
    //未读分享
    $("#liUnReadList").click(function (event) {
        $("#liDownList").css("background-color", "#fff");
        $("#liDownList").css("font-size", "15px");
        $("#liUploadList").css("background-color", "#fff");
        $("#liUploadList").css("font-size", "15px");
        $("#liUnReadList").css("background-color", "#d4d4d4");
        $("#liUnReadList").css("font-size", "22px");
        $("#liReadList").css("background-color", "#fff");
        $("#liReadList").css("font-size", "15px");
        $("#liExamineList").css("background-color", "#fff");
        $("#liExamineList").css("font-size", "15px");
        listName = "未读分享";
        GetListTable(listName, unReadData);
        ratingRefresh();
    });
    //已读分享
    $("#liReadList").click(function (event) {
        $("#liDownList").css("background-color", "#fff");
        $("#liDownList").css("font-size", "15px");
        $("#liUploadList").css("background-color", "#fff");
        $("#liUploadList").css("font-size", "15px");
        $("#liUnReadList").css("background-color", "#fff");
        $("#liUnReadList").css("font-size", "15px");
        $("#liReadList").css("background-color", "#d4d4d4");
        $("#liReadList").css("font-size", "22px");
        $("#liExamineList").css("background-color", "#fff");
        $("#liExamineList").css("font-size", "15px");
        listName = "所有分享";
        GetListTable(listName, readData);
        ratingRefresh();
    });
    //未审批文档
    $("#liExamineList").click(function (event) {
        $("#liDownList").css("background-color", "#fff");
        $("#liDownList").css("font-size", "15px");
        $("#liUploadList").css("background-color", "#fff");
        $("#liUploadList").css("font-size", "15px");
        $("#liUnReadList").css("background-color", "#fff");
        $("#liUnReadList").css("font-size", "15px");
        $("#liReadList").css("background-color", "#fff");
        $("#liReadList").css("font-size", "15px");
        $("#liExamineList").css("background-color", "#d4d4d4");
        $("#liExamineList").css("font-size", "22px");
        listName = "未审批文档";
        GetListTable(listName, examineData);
        ratingRefresh();
    });
});


function ShowShareLoading() {
    $('.modal-content').showLoading();
}

function HideShareLoading() {
    $('.modal-content').hideLoading();
}
//列表生成
function GetListTable(listName, data) {
    var tableData = eval(data);
    if (listName == "上传列表") {
        $('#userCenterTable').bootstrapTable('destroy');
        $('#userCenterTable').bootstrapTable({
            locale: 'zh-CN',
            method: 'get',
            cache: false,
            striped: true,
            pagination: true,
            search: true,
            //showPaginationSwitch: true,
            sortable: true,
            sortOrder: 'desc',
            sortName: 'grade',
            height: 730,
            //height: document.body.clientHeight,
            pageNumber: 1,
            pageSize: 15,
            pageList: [15, 25, 50, 100],
            minimumCountColumns: 2,
            onAll: function (name, args) {
                ratingRefresh();
            },
            onClickCell: function (field, value, row, $element) {
                if (field == "documentName") {
                    window.location.href = "DocumentDown.aspx?FileID=" + row.id + "";
                }
            },
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
                field: 'documentName',
                title: '文档名称',
                align: 'center',
                class: 'cursor',
            }, {
                field: 'author',
                title: '作者',
                align: 'center',
                visible: false
            }, {
                field: 'uploadTime',
                title: '上传时间',
                align: 'center',
                sortable: true
            }, {
                field: 'documentType',
                title: '所属类别',
                align: 'center',
            }, {
                field: 'clicks',
                title: '点击量',
                align: 'center',
                sortable: true
            }, {
                field: 'downloads',
                title: '下载量',
                align: 'center',
                sortable: true
            }, {
                field: 'comments',
                title: '评价次数',
                align: 'center',
                visible: false
            }, {
                field: 'downloadIntegral',
                title: '下载积分',
                align: 'center',
                width: '90px',
                visible: false
            }
            , {
                field: 'grade',
                title: '评分',
                align: 'center',
                width: '160px',
            }
            , {
                field: 'share',
                title: '分享',
                align: 'center',
                width:'100px'
            }],
            data: tableData
        });
    }
    else if (listName == "下载列表") {
        $('#userCenterTable').bootstrapTable('destroy');
        $('#userCenterTable').bootstrapTable({
            locale: 'zh-CN',
            method: 'get',
            cache: false,
            striped: true,
            pagination: true,
            search: true,
            //showPaginationSwitch: true,
            sortable: true,
            sortOrder: 'desc',
            sortName: 'grade',
            height: 730,
            //height: document.body.clientHeight,
            pageNumber: 1,
            pageSize: 15,
            pageList: [15, 25, 50, 100],
            minimumCountColumns: 2,
            onAll: function (name, args) {
                ratingRefresh();
            },
            onClickCell: function (field, value, row, $element) {
                if (field == "documentName") {
                    window.location.href = "DocumentDown.aspx?FileID=" + row.id + "";
                }
            },
            columns: [{
                field: 'id',
                title: 'ID',
                visible: false
            }, {
                field: 'type',
                title: '类型',
                align: 'center',
                width: "50px"
            }, {
                field: 'documentName',
                title: '文档名称',
                align: 'center',
                class: 'cursor',
            }, {
                field: 'author',
                title: '作者',
                align: 'center',
            }, {
                field: 'downTime',
                title: '下载时间',
                align: 'center',
                width: '120px',
                sortable: true
            }, {
                field: 'documentType',
                title: '所属类别',
                align: 'center',
            }, {
                field: 'clicks',
                title: '点击量',
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: 'downloads',
                title: '下载量',
                align: 'center',
                sortable: true,
                visible: false
            }, {
                field: 'comments',
                title: '评价次数',
                align: 'center',
                visible: false
            }, {
                field: 'downloadIntegral',
                title: '下载积分',
                align: 'center',
                width: '90px',
            }
            , {
                field: 'grade',
                title: '评分',
                align: 'center',
                width: '160px',
                sortable: true
            }],
            data: tableData
        });
    }
    else if (listName == "未读分享") {
        $('#userCenterTable').bootstrapTable('destroy');
        $('#userCenterTable').bootstrapTable({
            locale: 'zh-CN',
            method: 'get',
            cache: false,
            striped: true,
            pagination: true,
            search: true,
            //showPaginationSwitch: true,
            sortable: true,
            sortOrder: 'desc',
            sortName: 'shareTime',
            height: 730,
            //height: document.body.clientHeight,
            pageNumber: 1,
            pageSize: 15,
            pageList: [15, 25, 50, 100],
            minimumCountColumns: 2,
            onAll: function (name, args) {
                ratingRefresh();
            },
            onClickCell: function (field, value, row, $element) {
                if (field == "documentName") {
                    window.location.href = "DocumentDown.aspx?Shared=1&FileID=" + row.id + "";
                }
            },
            columns: [{
                field: 'id',
                title: 'ID',
                visible: false
            }, {
                field: 'typeRecord',
                title: '分享文档类型记录',
                visible: false
            }, {
                field: 'type',
                title: '分享文档类型',
                align: 'center',
                width: "50px"
            }, {
                field: 'documentName',
                title: '分享文档名称',
                align: 'center',
                class: 'cursor',
            }, {
                field: 'shareTime',
                title: '分享时间',
                align: 'center',
                width: "160px",
                sortable: true
            }, {
                field: 'ownerName',
                title: '分享人',
                align: 'center',
            }],
            data: tableData
        });
    }
    else if (listName == "所有分享") {
        $('#userCenterTable').bootstrapTable('destroy');
        $('#userCenterTable').bootstrapTable({
            locale: 'zh-CN',
            method: 'get',
            cache: false,
            striped: true,
            pagination: true,
            search: true,
            //showPaginationSwitch: true,
            sortable: true,
            sortOrder: 'desc',
            sortName: 'shareTime',
            height: 730,
            //height: document.body.clientHeight,
            pageNumber: 1,
            pageSize: 15,
            pageList: [15, 25, 50, 100],
            minimumCountColumns: 2,
            onAll: function (name, args) {
                ratingRefresh();
            },
            onClickCell: function (field, value, row, $element) {
                if (field == "documentName") {
                    window.location.href = "DocumentDown.aspx?Shared=1&FileID=" + row.id + "";
                }
            },
            columns: [{
                field: 'id',
                title: 'ID',
                visible: false
            }, {
                field: 'typeRecord',
                title: '分享文档类型记录',
                visible: false
            }, {
                field: 'type',
                title: '分享文档类型',
                align: 'center',
                width: "50px"
            }, {
                field: 'documentName',
                title: '分享文档名称',
                align: 'center',
                class: 'cursor',
            }, {
                field: 'shareTime',
                title: '分享时间',
                align: 'center',
                width: "160px",
                sortable: true
            }, {
                field: 'ownerName',
                title: '分享人',
                align: 'center',
            }
            , {
                field: 'isRead',
                title: '是否已读',
                align: 'center',
            }],
            data: tableData
        });
    }
    else if (listName == "未审批文档") {
        $('#userCenterTable').bootstrapTable('destroy');
        $('#userCenterTable').bootstrapTable({
            locale: 'zh-CN',
            method: 'get',
            cache: false,
            striped: true,
            pagination: true,
            search: true,
            //showPaginationSwitch: true,
            sortable: true,
            sortOrder: 'desc',
            sortName: 'uploadTime',
            height: 730,
            //height: document.body.clientHeight,
            pageNumber: 1,
            pageSize: 15,
            pageList: [15, 25, 50, 100],
            minimumCountColumns: 2,
            onAll: function (name, args) {
                ratingRefresh();
            },
            onClickCell: function (field, value, row, $element) {
                //if (field == "documentName")
                //{
                //    window.location.href = "DocumentDown.aspx?FileID=" + row.id + "";
                //}
            },
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
                field: 'documentName',
                title: '文档名称',
                align: 'center',
                class: 'cursor',
            }, {
                field: 'author',
                title: '作者',
                align: 'center',
            }, {
                field: 'uploadTime',
                title: '上传时间',
                align: 'center',
                width: '160px',
                sortable: true
            }, {
                field: 'documentType',
                title: '所属类别',
                align: 'center',
            }, {
                field: 'examineState',
                title: '审批状态',
                align: 'center',
                sortable: true
            }],
            data: tableData
        });
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

var data1 = [{ type: '<img src="theme/fileIcon/doc.png" />', documentName: '业内动态1', author: '系统帐户', uploadTime: '2016-06-22 01:06:10', documentType: '业内动态', clicks: '0', downloads: '0', comments: '0', downloadIntegral: '6', grade: '0' }, { type: '<img src="theme/fileIcon/doc.png" />', documentName: '战略规划1', author: '系统帐户', uploadTime: '2016-06-22 01:06:28', documentType: '战略规划', clicks: '0', downloads: '0', comments: '0', downloadIntegral: '2', grade: '0' }, { type: '<img src="theme/fileIcon/txt.png" />', documentName: '活动1', author: '系统帐户', uploadTime: '2016-06-22 01:06:46', documentType: '活动', clicks: '0', downloads: '0', comments: '0', downloadIntegral: '0', grade: '0' }]