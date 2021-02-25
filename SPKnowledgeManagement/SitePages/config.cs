using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPKnowledgeManagement.SitePages
{
    public class config
    {
     
        public static string siteUrl = "http://win-udb590rbc5f:81/sites/testlist/";
        public static string managementSiteUrl = " http://win-udb590rbc5f:81/sites/testlist/DocLib6/Forms/mod-view.aspx";
        public static string siteName = "sites/testlist";
        public static string previewSiteName = "/sites/testlist/";
        public static string knowledgeBase = "大华知识库";
        public static string tmpEmailReciever = "liu_fayong@dahuatech.com";
        public static string AllMemberMail = "contoso@dahuatech.com";
        public static string userUrl = "http://win-udb590rbc5f:81/sites/testlist/SitePages/UserCenter.aspx";
        public static string officialUrl = "http://www.dahuatech.com";
        public static string picDoclib = "知识平台图片库";



        public static string adminManagement = "DocLib6";
        public static string logOutUrl = "_layouts/15/SignOut.aspx";
        public static string mailConfigList = "MailConfig";
        public static string approverList = "文档审批者配置";
        public static string folderIntegralMapList = "积分目录对应表";
        public static string userList = "用户积分";
        public static string uploadList = "UserUpload";
        public static string downList = "UserDownload";
        public static string userComment = "UserComments";
        public static string gradeList = "DocGrade";
        public static string docShareList = "DocShare";
        public static string mailGroup = "MailGroup";
        public static string defaultIntegral = "20";
        public static int defaultDownIntegral = 10;
        public static string addIntegral = "10";
        public static int permissionLevel = 0;
        public static int maxFileSize = 307200;
        public static string mailBodyHeaderPic = "http://www.km.com/DocLib/mailHeader.jpg";

        //首页6块区域
        public static string rowLimit1 = "5";
        //最新上传限制
        public static string rowLimit2 = "5";
        //排行榜过滤条件
        public static string rowLimit3 = "10";
        //首页轮播非活动显示行数
        public static string rowLimit4 = "6";
        ////猜你喜欢限制
        //public static string rowLimit5 = "3";

    }
}
