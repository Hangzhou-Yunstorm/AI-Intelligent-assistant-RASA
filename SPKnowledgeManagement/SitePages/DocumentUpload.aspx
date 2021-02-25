<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="../_catalogs/masterpage/head.Master" AutoEventWireup="true" CodeBehind="DocumentUpload.aspx.cs" Inherits="SPKnowledgeManagement.SitePages.DocumentUpload,SPKnowledgeManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4bc69ad3ebb6cdf7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">

    <link href="~/_Layouts/15/SPKnowledgeManagement/theme/css/DocumentUpload.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstrapvalidator-master/js/bootstrapValidator.js"></script>
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstrapvalidator-master/css/bootstrapValidator.css" rel="stylesheet" />
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstrap-fileinput/css/fileinput.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/bootstrap-fileinput/js/fileinput.js"></script>
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/fileupload/ajaxfileupload.js"></script>
    <%--<link href="~/_Layouts/15/SPKnowledgeManagement/theme/css/DocumentUpload.css" rel="stylesheet" />--%>
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/zTree_v3-master/js/jquery.ztree.core.js"></script>
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/zTree_v3-master/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <%--<script src="~/_layouts/15/SPKnowledgeManagement/plugin/zTree_v3-master/js/jquery.ztree.exhide.js"></script>--%>
    <link href="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/showLoading.css" rel="stylesheet" />
    <script src="~/_Layouts/15/SPKnowledgeManagement/plugin/loading/jquery.showLoading.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--    <div class="divTitle">
        <img src="~/_Layouts/15/SPKnowledgeManagement/theme/img/bg1.png" class="imgTitle" />
        <span class="spanTitle">文档上传</span>
    </div>--%>
    <div class="divUploadContent">
        <div class="divNav">
            <ul class="ulNav">
                <li data-id="disable"><span class="nav_root unNav_root" style="cursor: default">当前位置</span></li>
                <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                <li data-id="index"><a href="Index.aspx" class="nav_root">首页</a></li>
                <li data-id="disable"><span style="color: #C7C7C7;">/</span></li>
                <li data-id="disable"><a href="DocumentUpload.aspx" class="nav_root" style="cursor: default">文档上传</a></li>
            </ul>

        </div>
        <form id="uploadForm" style="width: 1100px; margin-top: 20px;" method="post" class="form-horizontal">
            <%--            <asp:HiddenField runat="server" ID="ChosenCategoryID"/>onsubmit="doUploadKnowledge()" --%>
            <div class="divUploadContent">
                <span class="spanDocumentName">1.选择您要上传的文档<span style="color: red;">*</span></span>
                <div class="form-group">
                    <label class="col-lg-2 control-label">选择文件</label>
                    <div class="col-lg-8">
                        <input id="kFile" class="file file-loading" type="file" name="kFile" required data-bv-notempty-message="请选择上传文件" data-show-upload="false" data-show-caption="true" data-show-preview="false" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-2 control-label"></label>
                    <div class="col-lg-8">
                        <div class="input-group">
                            <label style="font-size: 12px; font-weight: 100; color: rgb(134, 126, 126);">
                                最大支持300MB，文件名无效或文件为空。文件名不能包含以下任何字符: \ / : * ? " &lt; &gt; | # { } % ~ &amp;，且不能以特殊字符开头(例如".")</label>
                        </div>
                    </div>
                </div>
                <span class="spanDocumentName">2.完善内容信息
                </span>
                <br />
                <div class="form-group">
                    <label class="col-lg-2 control-label">标题</label>
                    <div class="col-lg-8">
                        <input id="kTitle" type="text" maxlength="80" required data-bv-notempty-message="标题不可为空" class="form-control" name="kTiltle" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-2 control-label">作者</label>
                    <div class="col-lg-8">
                        <input id="kAuthor" readonly="readonly" type="text" class="form-control" name="kAuthor" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-2 control-label">所属分类</label>
                    <div class="col-lg-8">
                        <div class="input-group">
                            <input id="kCategory" readonly="readonly" type="text" class="form-control" required data-bv-notempty-message="所属分类不可为空" chosencategoryid="" name="kCategory" />
                            <span class="input-group-btn">
                                <button class="btn btn-default" type="button" onclick="showCategoryTree()">选择知识目录</button>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="form-group" style="display: none" data-toggle="buttons">
                    <label class="col-lg-2 control-label">是否活动</label>
                    <div class="col-lg-8">
                        <div class="btn-group" data-toggle="buttons">
                            <label class="btn btn-default ">
                                <input type="radio" name="options" id="option1" autocomplete="off" />
                                是
                            </label>
                            <label class="btn btn-default active">
                                <input type="radio" name="options" id="option2" autocomplete="off" checked />
                                否
                            </label>
                        </div>
                    </div>

                </div>
                <div class="form-group">
                    <label class="col-lg-2 control-label">描述</label>
                    <div class="col-lg-8">
                        <textarea id="kRemark" maxlength="500" class="form-control" required data-bv-notempty-message="知识描述不可为空" style="height: 110px; max-height: 110px; resize: none" name="kRemark" placeholder="文档描述不可为空且字数不能超过500"></textarea>
                    </div>
                </div>
                <br />
                <div class="form-group">
                    <div class="col-lg-9 col-lg-offset-3">
                        <button type="submit" class="btn btn-primary btnUpload">确认上传</button>
                    </div>
                </div>
            </div>

        </form>
        <div class="modal fade" id="myModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">知识目录选择</h4>
                        <div id="ajaxLoading"></div>
                    </div>
                    <div class="modal-body">
                        <ul id="categoryTree" class="ztree" style="width: 100%; height: 280px; overflow: auto;"></ul>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-primary" onclick="getCategory()">选择</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </div>
    <script type="text/javascript" language="javascript">
        $('#uploadForm').bind('submit', function () {
            Upload();
            return false;
        });


        //文件上传事件方法
        function Upload() {
            //如果有需要验证的可以在这里操作
            UploadFile();
        }
        //文件上传逻辑方法
        function UploadFile() {
            var path = document.getElementById("kFile").value;
            var destiFolder = document.getElementById("kCategory").value;
            var title = document.getElementById("kTitle").value;
            title = title.replace(/ /g, "");

            var arr = path.split('.');
            var extension = "." + arr[arr.length - 1];
            var extensionArr = ".ade.adp.asa.ashx.asmx.asp.bas.bat.cdx.cer.chm.class.cmd.cnt.com.config.cpl.crt.csh.der.dll.exe.fxp.gadget.grp.hlp.hpj.hta.htr.htw.ida.idc.idq.ins.isp.its.jse.json.ksh.lnk.mad.maf.mag.mam.maq.mar.mas.mat.mau.mav.maw.mcf.mda.mdb.mde.mdt.mdw.mdz.ms-one-stub.msc.msh.msh1.msh1xml.msh2.msh2xml.mshxml.msi.msp.mst.ops.pcd.pif.pl.prf.prg.printer.ps1.ps1xml.ps2.ps2xml.psc1.psc2.pst.reg.rem.scf.scr.sct.shb.shs.shtm.shtml.soap.stm.svc.url.vb.vbe.vbs.vsix.ws.wsc.wsf.wsh.xamlx";

            if (extensionArr.indexOf(extension) != -1) { alert("本系统不支持该文件上传"); return; }
            if (title == "") { alert("标题不能为纯空格"); return; }
            var isactive = false;
            if ($.trim(path) == "") { alert("请选择要上传的文件"); return; }
            if ($.trim(destiFolder) == "") { alert("请选择所属目录，即上传目标目录"); return; }
            if ($('#option1').checked)
                isactive = true;
            else
                isactive = false;
            var result_msg = "";
            $(".divUploadContent").showLoading();
            $.ajaxFileUpload({
                url: '~/_Layouts/15/SPKnowledgeManagement/FileUpload.ashx',
                type: 'post',
                async: false,
                secureuri: false, //一般设置为false
                fileElementId: 'kFile', // 上传文件的id、name属性名
                dataType: 'json', //返回值类型，一般设置为json、application/json.replace(/\n/g, "<br/>").replace('<br />','/n')
                data: { title: $('#kTitle').val(), category: $('#kCategory').val(), author: $('#kAuthor').val(), remark: $('#kRemark').val(), isActivity: isactive, destiFolderId: chosenNode.id }, //传递参数到服务器
                success: function (data, status) {
                    $(".divUploadContent").hideLoading();
                    var uploadStatus = data.ResponseState;
                    var uploadRemark = data.ResponseRemark;
                    //uploadStatus===0:完全失败,   1:完全上传成功,   2:上传成功，邮件发送失败，记录插入失败, ,3:上传成功，邮件发送成功，记录插入失败 4:上传成功，邮件发送失败，记录插入成功
                    if (uploadStatus == "1") {
                        alert('上传成功,等待管理员审批');
                        $('#kCategory').val("");
                        window.location.href = "Index.aspx";
                    }
                    else if (uploadStatus == "0") {
                        if (confirm("文件上传失败，" + uploadRemark)) {
                            $('#kCategory').val("");
                            location.reload();
                        }
                        else {
                            $('#kCategory').val("");
                            location.reload();
                        }
                    }
                    else {
                        if (confirm("文件上传成功，" + uploadRemark)) {
                            $('#kCategory').val("");
                            window.location.href = "Index.aspx";
                        }
                        else {
                            $('#kCategory').val("");
                            window.location.href = "Index.aspx";
                        }
                    }

                },
                error: function (data, status, e) {
                    // alert(e);
                    if (status == 'error') {
                        if (confirm('发生意外错误：' + data))
                            window.location.reload();
                        else
                            window.location.reload();
                    }
                    $(".divUploadContent").hideLoading();
                    resutlData = eval(data);
                    var uploadStatus = resutlData.uploadStatus;
                    var uploadRemark = resutlData.remark;
                    if (uploadStatus == "1") {
                        alert('上传成功,等待管理员审批');
                        $('#kCategory').val("");
                        window.location.href = "Index.aspx";
                        //location.reload();
                    }
                    else if (uploadStatus == "0") {
                        if (confirm("文件上传失败，" + uploadRemark)) {
                            $('#kCategory').val("");
                            location.reload();
                        }
                        else {
                            $('#kCategory').val("");
                            location.reload();
                        }
                    }
                    else {
                        if (confirm("文件上传成功，" + uploadRemark)) {
                            $('#kCategory').val("");
                            window.location.href = "Index.aspx";
                        }
                        else {
                            $('#kCategory').val("");
                            window.location.href = "Index.aspx";
                        }
                    }
                }
            });
        }
    </script>
    <script>
        //创建文件目录树形选择器
        function showCategoryTree() {
            if (treeNodesData == null)
                $.ajax({
                    url: 'DocumentUpload.aspx/GetFoldersCategory',// 跳转到 action  
                    type: 'post',
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    beforeSend: function (XMLHttpRequest) {
                        $("#ajaxLoading").html("<img src='~/_Layouts/15/SPKnowledgeManagement/theme/loading/img/loading.gif' />");
                    },
                    success: function (data) {
                        treeNodesData = eval(data.d);
                        $.fn.zTree.init($("#categoryTree"), setting, treeNodesData);
                        $("#ajaxLoading").empty();
                    },
                    error: function () {
                        $("#ajaxLoading").empty();
                    },
                    complete: function (XMLHttpRequest, textStatus) {
                        $("#ajaxLoading").empty();
                    }
                });

            $('#myModal').modal("show");
        }
        var treeNodesData;
        var parentNodes;
        var chosenNode;
        //chosencategoryid
        //点击保存按钮时，获取当前选择目录节点
        function getCategory() {

            var treeObj = $.fn.zTree.getZTreeObj("categoryTree");
            var nodes = treeObj.getSelectedNodes();
            if (nodes.length == 0) {
                alert("请选择目录节点"); return;
            }
            if (nodes[0].pId == 0) {
                alert("不可上传至根目录"); return;
            }
            var categoryPath = '';
            parentNodes = nodes[0].getPath();
            chosenNode = nodes[0];
            //$('#hiddenFiled').value(nodes[0].id);
            for (var index in parentNodes) {
                //if (parentNodes[index].name != "Shared Documents")
                categoryPath += "/" + parentNodes[index].name;
                //else
                //    categoryPath += "/" + node.name;
                //$('#kCategory')
            }
            categoryPath = categoryPath.substr(1, categoryPath.length - 1);
            document.getElementById("kCategory").value = categoryPath;
            //$('#kCategory').val(categoryPath);
            $('#myModal').modal("hide");
        }
    </script>
    <script type="text/javascript">
        $(function () {
            //根据当前屏幕分辨率适配
            var width = document.documentElement.clientWidth;
            var height = document.documentElement.clientHeight;
            var offset = $("#divFooter").offset().top;
            if (offset < height && height > 768) {
                $("#divFooter").css('top', '' + height - offset + 'px')
            }
            var margin;
            if (width > 1560) {
                margin = (width - 1560) / 2;
                $('.spanTitle').css('left', margin);
                $('.divUploadContent').css('left', margin);
            }
            else {
                var left = $("#uploadForm").width();
                var marginLeft = width - left;
                $('.spanTitle').css('left', marginLeft / 2);
                $('.divUploadContent').css('margin-left', marginLeft / 2);
            }
        });
    </script>
    <script>
        var maxfilesize = <%= MaxFileLength%>;
        //初始化页面校验元素及校验事件
        $(document).ready(function () {

            $('#kFile').fileinput({
                language: 'zh', //设置语言
                showUpload: false, //是否显示上传按钮
                showCaption: true,//是否显示标题
                browseClass: "btn btn-primary", //按钮样式     
                allowedPreviewTypes: false,
                browseClass: "btn btn-primary",
                browseLabel: "选择文件",
                browseIcon: "<i class=\"glyphicon glyphicon-folder-open\"></i> ",
                removeClass: "btn btn-danger",
                removeLabel: "删除文件",
                removeIcon: "<i class=\"glyphicon glyphicon-trash\"></i> ",
                //previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
            });

            $('#kAuthor').val('<%= CurrentUserName%>');
            $('#kFile').on('change', function (event) {
                if ((parseInt(event.currentTarget.files[0].size)/1024)>maxfilesize)
                {
                    if(confirm('所选文件超出平台上传文件大小限制，限制大小：51200KB'))
                    {
                        window.location.reload();
                        return;
                    }
                    else
                    {
                        window.location.reload();
                        return;
                    }
                }
                var chosenFilePath = event.target.value;
                var array = chosenFilePath.split('\\');
                var fileTitle = array[array.length - 1];
                $('#kTitle').val(fileTitle);
            });
        })


    </script>
    <script>
        //树形图settings配置
        var zTree;
        var demoIframe;

        var setting = {
            view: {
                selectedMulti: false
            },
            edit: {
                enable: true,
                showRemoveBtn: false,
                showRenameBtn: false
            },
            data: {
                keep: {
                    parent: true,
                    leaf: true
                },
                simpleData: {
                    enable: true
                }
            },
            callback: {
                //onClick: poNodeClick
            }
        };
        //属性单击事件
        function poNodeClick(event, treeId, treeNode) {
            alert(treeNode.name);
        }



    </script>
</asp:Content>
