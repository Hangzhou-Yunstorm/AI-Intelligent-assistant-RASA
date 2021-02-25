/**分页js插件
var ListPager = new listPaging();
先调用start方法加载上下文
然后调用dataLoad方法查询第一页数据
需要设置几个属性值
            ListPager.property.pageSize = 4;
            ListPager.start();
            ListPager.property.prevControlID = "prevBtn";//上一页按钮id
            ListPager.property.nextControlID = "nextBtn";//下一页按钮id
            ListPager.property.controlActionType = "css";//操作类型attr为禁用 css为显示隐藏
然后在success这个成功回调的函数中设置上一页的状态
                var id = ListPager.property.listItems.itemAt(0).get_id();
                ListPager.property.PagingState = ["p_ID=" + id];
                p_ID是必填项。否则无法返回上一页
                如果查询的数据 需要排序需要在PagingState的数组中添加你的排序字段
                例如：var title = ListPager.property.listItems.itemAt(0).get_item("Title");
                ListPager.property.PagingState = ["p_ID=" + id,"p_Title="+title];
                如果排序字段为时间类型，比如2013-01-01 请转成20130101
分页方法调用：ListPager.getListPager
*/

var listPaging = function () { };
    listPaging.prototype={
        property: {
            clientContext:null,
            webSite : null,
            siteUrl:"",
            listItems : null,
            query : null,
            targetList : null,
            position : null,
            lastState : null,
            pageState : "",
            itemIndex : 1,
            pageIndex : 1,
            pageSize : 15,
            prevControlID : "",
            nextControlID : "",
            PagingState:null,//Array 格式 ["key=value","key1=value1"],value 为时间格式的话，比如2013-01-01 请转成20130101
            /*   @ControlActionType: 分页操作控件启用和禁用的方式css/attr。*/
            controlActionType : "css"
        },
        start:function(){        
            this.property.clientContext = new SP.ClientContext.get_current();
            this.property.webSite = this.property.clientContext.get_web();
        },
        /*加载列表数据*/
        /**
        *   @listName: 列表名称。
        *   @camlQuery: Caml语句
        *   @isUserList: 是不是用户列表true/false
        *   @success: 成功执行方法。
        *   @error: 失败执行方法
        */
        dataLoad: function (listName, camlQuery, isUserList,success, error) {
            this.property.pageIndex = 1;
            this.property.pageState = "";
            if (isUserList)
                this.property.targetList = this.property.webSite.get_siteUserInfoList();
            else
                this.property.targetList = this.property.webSite.get_lists().getByTitle(listName);
            this.property.query = new SP.CamlQuery();
            this.property.query.set_viewXml(camlQuery);
            this.property.listItems = this.property.targetList.getItems(this.property.query);
            this.property.clientContext.load(this.property.listItems);
            this.property.clientContext.executeQueryAsync(Function.createDelegate(this, success), Function.createDelegate(this, error));
        },
        getListPager:function (result, camlQuery, success, error) {
            this.property.pageState = result;
            this.property.lastState = this.property.position;//保存当前页的状态
            if (this.property.pageState == "prev") {//上一页
                this.property.itemIndex = this.property.itemIndex - (this.property.listItems.get_count() + this.property.pageSize);
                if (this.property.position == null) {
                    this.property.position = new SP.ListItemCollectionPosition();
                }
                var prevPageString = this.property.position.get_pagingInfo();
                var prevString = "PagedPrev=TRUE";
                var searchStr = prevPageString.split("&");
                for (var i = 0; i < searchStr.length; i++) {
                    var key = searchStr[i].split("=")[0];
                    var value = searchStr[i].split("=")[1];
                    for (var p = 0; p < this.property.PagingState.length; p++) {
                        var state = this.property.PagingState[p].split("=");
                        if (key == state[0]) {
                            value = state[1];
                        }
                    }
                    prevString += "&"+key+"="+value;
                }
                this.property.position.set_pagingInfo(prevString);//设置上一页读取状态
                this.property.pageIndex--;
                if (this.property.pageIndex == 1) {//如果到第一页，上一页禁用
                    //删除下一页的禁用状态
                    if (this.property.controlActionType == "css") {
                        $("#" + this.property.prevControlID).css("display", "none");
                        $("#" + this.property.nextControlID).css("display", "block");
                    }
                    if (this.property.controlActionType == "attr") {
                        $("#" + this.property.nextControlID).removeAttr("disabled");
                        $("#" + this.property.prevControlID).attr("disabled", "disabled");
                    }
                }
                else {
                    if (this.property.controlActionType == "css") {
                        $("#" + this.property.nextControlID).css("display", "block");
                    }
                    if (this.property.controlActionType == "attr") {
                        $("#" + this.property.nextControlID).removeAttr("disabled");
                    }
                }
            }
            else {//下一页
                if (this.property.position == null)
                    this.property.position = this.property.lastState;//赋值上一页状态
                if (this.property.controlActionType == "css")
                    $("#" + this.property.prevControlID).css("display", "block");
                if (this.property.controlActionType == "attr")
                    $("#" + this.property.prevControlID).removeAttr("disabled");
                //var prevPageString = this.property.position.get_pagingInfo();
                //prevPageString = prevPageString.replace("PagedPrev", "Paged");
                //this.property.position.set_pagingInfo(prevPageString);//设置上一页读取状态
                this.property.pageIndex++;
            }
            this.property.query = new SP.CamlQuery();
            this.property.query.set_viewXml(camlQuery);
            this.property.query.set_listItemCollectionPosition(this.property.position);
            this.property.listItems = this.property.targetList.getItems(this.property.query);
            this.property.clientContext.load(this.property.listItems);
            this.property.clientContext.executeQueryAsync(Function.createDelegate(this, success), Function.createDelegate(this, error));
        },
        /*设置分页控件状态*/
        pagerControlHandle:function () {
            this.property.position = this.property.listItems.get_listItemCollectionPosition();
            if (this.property.position == null) {
                if (this.property.pageState == "next")//如果是最后一页,禁用下一页
                {
                    if (this.property.controlActionType == "css")
                        $("#" + this.property.nextControlID).css("display", "none");
                    if (this.property.controlActionType == "attr")
                        $("#" + this.property.nextControlID).attr("disabled", "disabled");
                }
                else if (this.property.pageState == "prev") {//如果是第一页,禁用上一页
                    if (this.property.controlActionType == "css")
                        $("#" + this.property.prevControlID).css("display", "none");
                    if (this.property.controlActionType == "attr")
                        $("#" + this.property.prevControlID).attr("disabled", "disabled");
                }
                else {//不足一页数据,上一页下一页按钮都禁用
                    if (this.property.controlActionType == "css") {
                        $("#" + this.property.prevControlID).css("display", "none");
                        $("#" + this.property.nextControlID).css("display", "none");
                    }
                    if (this.property.controlActionType == "attr") {
                        $("#" + this.property.prevControlID).attr("disabled", "disabled");
                        $("#" + this.property.nextControlID).attr("disabled", "disabled");
                    }
                }
                this.property.position = this.property.lastState;
            }
            else {
                if (this.property.pageIndex == 1) {//如果到第一页，上一页禁用
                    //删除下一页的禁用状态
                    if (this.property.controlActionType == "css") {
                        $("#" + this.property.prevControlID).css("display", "none");
                        $("#" + this.property.nextControlID).css("display", "block");
                    }
                    if (this.property.controlActionType == "attr") {
                        $("#" + this.property.nextControlID).removeAttr("disabled");
                        $("#" + this.property.prevControlID).attr("disabled", "disabled");
                    }
                }
            }
        },
        isEmpty:function(value){
            if ($.trim(value).length != 0) {
                return false;
            }
            return true;
        }
    }