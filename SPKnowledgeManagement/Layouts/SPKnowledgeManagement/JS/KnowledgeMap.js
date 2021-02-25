function BuildMap() {
    Build_Empty();
    $.ajax({
        url: 'KnowledgeMap.aspx/GetFoldersCategoryValue',// 跳转到 action  
        type: 'post',
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        beforeSend: function (XMLHttpRequest) {
            $("#ajaxLoading").html("<img src='~/_Layouts/15/SPKnowledgeManagement/theme/loading/img/loading.gif' />");
        },
        success: function (data) {
            generatorMindMap(data);
        },
        error: function () {
            $("#ajaxLoading").empty();
        },
        complete: function (XMLHttpRequest, textStatus) {
            $("#ajaxLoading").empty();
        }
    });
}

function generatorMindMap(data) {
    try {
        kmmapdata = eval(data.d);
        //var mapOption = IntiTreeOption(kmmapdata);
        open_jsMind(kmmapdata[0]);

        //treeNodesData = eval(data.d);
        //$.fn.zTree.init($("#categoryTree"), setting, treeNodesData);
        $("#ajaxLoading").empty();
    }
    catch (e) {
        alert(e);
        $("#ajaxLoading").empty();
    }
}

var _jm = null;//new jsMind(options);
function Build_Empty() {
    var options = {
        container: 'jsmind_container',
        theme: 'info',
        editable: false,
        view: {
            hmargin: 50,        // 思维导图距容器外框的最小水平距离
            vmargin: 30,         // 思维导图距容器外框的最小垂直距离
            line_width: 2,       // 思维导图线条的粗细
            line_color: '#555'   // 思维导图线条的颜色
        },
        layout: {
            hspace: 40,          // 节点之间的水平间距
            vspace: 10,          // 节点之间的垂直间距
            pspace: 13           // 节点收缩/展开控制器的尺寸
        },
    };
    _jm = new jsMind(options);
}

$(window).load(function () {
    BuildMap();
});

function open_jsMind(data) {


    var mind = {
        container: 'jsmind_container',
        theme: 'info',
        editable: false,
        "meta": {
            "name": "jsMind",
            "author": "km",
            "version": "1.0"
        },
        "format": "node_tree",
        "data": data
    }
    _jm.show(mind);
    $('.jsmind-inner').height($('.theme-info').height())
    $('#jsmind_container').height($('.theme-info').height())
    adaptScreen();
    $('jmnode').each(
   function () {
       $(this).click(
           function () {
               var listId = $(this).attr('nodeid');
               //if (parseInt(listId) > 0)
               window.location.href = "documentlist.aspx?folderid=" + listId;
               //else
               //    window.location.href = "index.aspx";
               //alert($(this).attr('nodeid'))
           })
   });
}
