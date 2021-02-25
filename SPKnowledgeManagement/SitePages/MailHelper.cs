using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPKnowledgeManagement.SitePages
{
    public class MailHelper
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public string mailFrom { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string[] mailToArray { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public string[] mailCcArray { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string mailSubject { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string mailBody { get; set; }

        /// <summary>
        /// 发件人密码
        /// </summary>
        public string mailPwd { get; set; }

        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public bool isbodyHtml { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string[] attachmentsPath { get; set; }
        public static string GeneratorMailHeader(string headerTitle)
        {
            int tbWidth = 750;
            if (headerTitle == "您好，知识管理平台中有您待审批的知识")
                tbWidth = 860;
            string BasicHeaderContent =
               "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"zhwd-mobile-fullwidth\" style=\"background-color: #f7f9fa;margin:0 auto;\" width=\"" + tbWidth + "\">" +
               "<tbody>" +
                   "<tr>" +
                       "<td align=\"center\">" +
                           "<table cellpadding=\"0\" cellspacing=\"0\" class=\"zhwd-mobile-no-radius\" style=\"border-radius: 4px; border: 1px solid #dedede; margin: 30px auto; background-color: #ffffff\">" +
                              "<tbody>" +
                                   "<tr>" +
                                       "<td style=\"padding: 25px 35px 45px 35px\" class=\"zhwd-mobile-collapse-padding\" align=\"left\">" +
                                           "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family:'Microsoft YaHei'\">" +
                                              "<tbody>" +
                                                   "<tr>" +
                                                       "<td>" +
                                                           "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">" +
                                                               "<tbody>" +
                                                                   "<tr>" +
                                                                       "<td align=\"center\">" +
                                                                       "<div style=\"border-radius: 10px;width:590px;margin:0 auto;\">" +
                                                                           "<div style=\"width:590px;height:80px;color:#fff;margin:0 auto\">" +
                                                                               "<img style=\"width:590px;height:80px\" src='cid:pic'/>" +
                                                                           "</div></div>" +
                                                                       "</td>" +
                                                                   "</tr>" +
                                                               "</tbody>" +
                                                           "</table>" +
                                                       "</td>" +
                                                   "</tr>";
            return BasicHeaderContent;
        }
        #region mailFooter
        public static string BasicFooter =
                                                    "</tbody>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</tbody>" +
                                "</table>" +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>" +
                                "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">" +
                                    "<tbody>" +
                                        "<tr>" +
                                            "<td style=\"padding: 20px 25px 45px 25px; font-size: 16px; color: #575757; line-height: 25px\" class=\"zhwd-mobile-small-footer\" align=\"center\">" +
                                                "此邮件由 知识管理平台 自动发出，请勿回复" +
                                            "</td>" +
                                        "</tr>" +
                                    "</tbody>" +
                                "</table>" +
                            "</td>" +
                        "</tr>" +
                    "</tbody>" +
                "</table>";
        #endregion
        /// <summary>
        /// 新文档推送邮件
        /// </summary>
        /// <param name="knowledgeList"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GeneratorMailContent(SPListItemCollection knowledgeList, string type)
        {

            string content = "";
            foreach (SPListItem item in knowledgeList)
            {
                string documentId = item.ID.ToString();
                string docTitle = item["Title"].ToString();
                string uploadTime = Convert.ToDateTime(item["Created"]).ToString("yyyy-MM-dd HH:mm:ss");
                string author = item["Owner"].ToString();
                string docRemark = item["Remark"].ToString();
                if (docRemark.Length > 100)
                    docRemark = docRemark.Substring(0, 100) + "......";
                else
                    docRemark += "......";
                if (docTitle == "")
                {
                    docTitle = item.Name;
                }
                content += "<tr>" +
                                                     "<td height=\"25\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                 "</tr>" +
                                                 "<tr>" +
                                                     "<td>" +
                                                         "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                             "<tbody>" +
                                                                 "<tr>" +
                                                                     "<td height=\"35\" style=\"font-size: 0px; line-height: 0px; border-top: 1px #f1f4f6 solid\">&nbsp;</td>" +
                                                                 "</tr>" +
                                                                 "<tr>" +
                                                                     "<td>" +
                                                                         "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                             "<tbody>" +
                                                                                 "<tr>" +
                                                                                     "<td><a href=\"" + config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + documentId + " \" style=\"font-size: 17px; font-weight: bold; text-decoration: none; color: #259; border: none; outline: none\" target=\"_blank\">" + docTitle + "</a></td>" +
                                                                                 "</tr>" +
                                                                                 "<tr>" +
                                                                                     "<td>" +
                                                                                         "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"zhwd-mobile-no-fullwidth\">" +
                                                                                             "<tbody>" +
                                                                                                 "<tr>" +
                                                                                                     "<td height=\"15\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                                 "</tr>" +
                                                                                                 "<tr>" +
                                                                                                     "<td width=\"50\">" +
                                                                                                         "<span style=\"font-size:14px\">作者：</span>" +
                                                                                                     "</td>" +
                                                                                                     "<td align=\"left\" width=\"100\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\"><strong>" + author + "</strong></a></td>" +
                                                                                                     "<td width=\"35\"></td>" +
                                                                                                     "<td width=\"80\">" +
                                                                                                        "<span style=\"font-size:14px\">上传时间：</span>" +
                                                                                                     "</td>" +
                                                                                                     "<td align=\"left\" width=\"180\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\">" + uploadTime + "</a></td>" +

                                                                                                 "</tr>" +
                                                                                                 "<tr>" +
                                                                                                     "<td height=\"12\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                                 "</tr>" +
                                                                                             "</tbody>" +
                                                                                         "</table>" +
                                                                                     "</td>" +
                                                                                 "</tr>" +
                                                                                 "<tr>" +
                                                                                     "<td>" +
                                                                                         "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                                             "<tbody>" +
                                                                                                 "<tr>" +
                                                                                                     "<td style=\"word-break: break-all\"><a href=\"" + config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + documentId + "\" style=\"font-size: 13px; line-height: 22px; text-decoration: none; color: #333; display: block\" target=\"_blank\">" + docRemark + "<span style=\"font-size: 13px; display: inline-block; color: #259\">&nbsp;阅读全文</span></a></td>" +
                                                                                                     "<td width=\"35\" class=\"zhwd-mobile-hide\">&nbsp;</td>" +
                                                                                                 "</tr>" +
                                                                                             "</tbody>" +
                                                                                       "</table>" +
                                                                                     "</td>" +
                                                                                 "</tr>" +
                                                                                 "<tr>" +
                                                                                     "<td height=\"28\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                 "</tr>" +
                                                                             "</tbody>" +
                                                                         "</table>" +
                                                                     "</td>" +
                                                                 "</tr>" +
                                                             "</tbody>" +
                                                         "</table>" +
                                                     "</td>" +
                                                 "</tr>";
            }
            return content;
        }

        /// <summary>
        /// 未评分文档邮件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        /// <param name="owner"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string GeneratorMailContent(string id, string title, string time, string owner, string remark)
        {
            string content = "";

            string documentId = id;
            string docTitle = title;
            string uploadTime = time;
            string author = owner;
            string docRemark = remark;
            if (docRemark.Length > 100)
                docRemark = docRemark.Substring(0, 100) + "......";
            else
                docRemark += "......";
            content += "<tr>" +
                                                 "<td height=\"25\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                             "</tr>" +
                                             "<tr>" +
                                                 "<td>" +
                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                         "<tbody>" +
                                                             "<tr>" +
                                                                 "<td height=\"35\" style=\"font-size: 0px; line-height: 0px; border-top: 1px #f1f4f6 solid\">&nbsp;</td>" +
                                                             "</tr>" +
                                                             "<tr>" +
                                                                 "<td>" +
                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                         "<tbody>" +
                                                                             "<tr>" +
                                                                                 "<td><a href=\"" + config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + documentId + " \" style=\"font-size: 17px; font-weight: bold; text-decoration: none; color: #259; border: none; outline: none\" target=\"_blank\">" + docTitle + "</a></td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td>" +
                                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"zhwd-mobile-no-fullwidth\">" +
                                                                                         "<tbody>" +
                                                                                             "<tr>" +
                                                                                                 "<td height=\"15\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                             "<tr>" +
                                                                                                 "<td width=\"50\">" +
                                                                                                     "<span style=\"font-size:14px\">作者：</span>" +
                                                                                                 "</td>" +
                                                                                                 "<td align=\"left\" width=\"100\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\"><strong>" + author + "</strong></a></td>" +
                                                                                                 "<td width=\"35\"></td>" +
                                                                                                 "<td width=\"80\">" +
                                                                                                    "<span style=\"font-size:14px\">下载时间：</span>" +
                                                                                                 "</td>" +
                                                                                                 "<td align=\"left\" width=\"180\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\">" + uploadTime + "</a></td>" +

                                                                                             "</tr>" +
                                                                                             "<tr>" +
                                                                                                 "<td height=\"12\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                         "</tbody>" +
                                                                                     "</table>" +
                                                                                 "</td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td>" +
                                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                                         "<tbody>" +
                                                                                             "<tr>" +
                                                                                                 "<td style=\"word-break: break-all\"><a href=\"" + config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + documentId + "\" style=\"font-size: 13px; line-height: 22px; text-decoration: none; color: #333; display: block\" target=\"_blank\">" + docRemark + "<span style=\"font-size: 13px; display: inline-block; color: #259\">&nbsp;点击前往评分</span></a></td>" +
                                                                                                 "<td width=\"35\" class=\"zhwd-mobile-hide\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                         "</tbody>" +
                                                                                   "</table>" +
                                                                                 "</td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td height=\"28\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                             "</tr>" +
                                                                         "</tbody>" +
                                                                     "</table>" +
                                                                 "</td>" +
                                                             "</tr>" +
                                                         "</tbody>" +
                                                     "</table>" +
                                                 "</td>" +
                                             "</tr>";

            return content;
        }

        /// <summary>
        /// 审批提醒邮件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        /// <param name="owner"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string GeneratorApproveMailContent(string title, string time, string owner, string category, string remark)
        {
            string content = "";

            string docTitle = title;
            string uploadTime = time;
            string author = owner;
            string docRemark = remark;
            string truePath = category.Replace(config.knowledgeBase, config.adminManagement);
            if (docRemark.Length > 100)
                docRemark = docRemark.Substring(0, 100) + "......";
            else
                docRemark += "......";
            content += "<tr>" +
                                                 "<td height=\"25\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                             "</tr>" +
                                             "<tr>" +
                                                 "<td>" +
                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                         "<tbody>" +
                                                             "<tr>" +
                                                                 "<td height=\"35\" style=\"font-size: 0px; line-height: 0px; border-top: 1px #f1f4f6 solid\">&nbsp;</td>" +
                                                             "</tr>" +
                                                             "<tr>" +
                                                                 "<td>" +
                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                         "<tbody>" +
                                                                             "<tr>" +
                                                                                 "<td><a href=\"" + config.managementSiteUrl + "?RootFolder=" + HttpUtility.UrlEncode(config.previewSiteName + truePath) + " \" style=\"font-size: 17px; font-weight: bold; text-decoration: none; color: #259; border: none; outline: none\" target=\"_blank\">" + docTitle + "</a></td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td>" +
                                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"zhwd-mobile-no-fullwidth\">" +
                                                                                         "<tbody>" +
                                                                                             "<tr>" +
                                                                                                 "<td height=\"15\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                             "<tr>" +
                                                                                                 "<td width=\"50\">" +
                                                                                                     "<span style=\"font-size:14px\">作者：</span>" +
                                                                                                 "</td>" +
                                                                                                 "<td align=\"left\" width=\"100\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\"><strong>" + author + "</strong></a></td>" +
                                                                                                 "<td width=\"35\"></td>" +
                                                                                                 "<td width=\"80\">" +
                                                                                                    "<span style=\"font-size:14px\">上传时间：</span>" +
                                                                                                 "</td>" +
                                                                                                 "<td align=\"left\" width=\"180\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\">" + uploadTime + "</a></td>" +
                                                                                                 "<td width=\"35\"></td>" +
                                                                                                  "<td width=\"80\">" +
                                                                                                    "<span style=\"font-size:14px\">上传目录：</span>" +
                                                                                                 "</td>" +
                                                                                                 "<td align=\"left\" width=\"300\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\">" + category + "</a></td>" +
                                                                                             "</tr>" +
                                                                                             "<tr>" +
                                                                                                 "<td height=\"12\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                         "</tbody>" +
                                                                                     "</table>" +
                                                                                 "</td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td>" +
                                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                                         "<tbody>" +
                                                                                             "<tr>" +
                                                                                                 "<td style=\"word-break: break-all\"><a href=\"" + config.managementSiteUrl + "?RootFolder=" + HttpUtility.UrlEncode(config.previewSiteName + truePath) + "\" style=\"font-size: 13px; line-height: 22px; text-decoration: none; color: #333; display: block\" target=\"_blank\">" + docRemark + "<span style=\"font-size: 13px; display: inline-block; color: #259\">&nbsp;点击前往审批</span></a></td>" +
                                                                                                 "<td width=\"35\" class=\"zhwd-mobile-hide\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                         "</tbody>" +
                                                                                   "</table>" +
                                                                                 "</td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td height=\"28\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                             "</tr>" +
                                                                         "</tbody>" +
                                                                     "</table>" +
                                                                 "</td>" +
                                                             "</tr>" +
                                                         "</tbody>" +
                                                     "</table>" +
                                                 "</td>" +
                                             "</tr>";

            return content;
        }

        /// <summary>
        /// 分享提醒邮件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        /// <param name="owner"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        /// <summary>
        public static string GeneratorShareMailContent(string id, string title, string time, string docauthor, string owner, string remark)
        {
            string content = "";

            string documentId = id;
            string docTitle = title;
            string uploadTime = time;
            string author = docauthor;
            string sharer = owner;
            string docRemark = remark;
            if (docRemark.Length > 100)
                docRemark = docRemark.Substring(0, 100) + "......";
            else
                docRemark += "......";
            content += "<tr>" +
                                                 "<td height=\"25\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                             "</tr>" +
                                             "<tr>" +
                                                 "<td>" +
                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                         "<tbody>" +
                                                             "<tr>" +
                                                                 "<td height=\"35\" style=\"font-size: 0px; line-height: 0px; border-top: 1px #f1f4f6 solid\">&nbsp;</td>" +
                                                             "</tr>" +
                                                             "<tr>" +
                                                                 "<td>" +
                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                         "<tbody>" +
                                                                             "<tr>" +
                                                                                 "<td><a href=\"" + config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + documentId + " \" style=\"font-size: 17px; font-weight: bold; text-decoration: none; color: #259; border: none; outline: none\" target=\"_blank\">" + docTitle + "</a></td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td>" +
                                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"zhwd-mobile-no-fullwidth\">" +
                                                                                         "<tbody>" +
                                                                                             "<tr>" +
                                                                                                 "<td height=\"15\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                             "<tr>" +
                                                                                                 "<td width=\"50\">" +
                                                                                                     "<span style=\"font-size:14px\">作者：</span>" +
                                                                                                 "</td>" +
                                                                                                 "<td align=\"left\" width=\"100\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\"><strong>" + author + "</strong></a></td>" +
                                                                                                 "<td width=\"35\"></td>" +
                                                                                                 "<td width=\"60\">" +
                                                                                                    "<span style=\"font-size:14px\">分享者：</span>" +
                                                                                                 "</td>" +
                                                                                                 "<td align=\"left\" width=\"100\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\">" + sharer + "</a></td>" +
                                                                                                 "<td width=\"35\"></td>" +
                                                                                                  "<td width=\"80\">" +
                                                                                                    "<span style=\"font-size:14px\">分享时间：</span>" +
                                                                                                 "</td>" +
                                                                                                 "<td align=\"left\" width=\"180\"><a href=\"#\" style=\"font-size: 14px; color: #333333; text-decoration: none\" target=\"_blank\">" + uploadTime + "</a></td>" +
                                                                                             "</tr>" +
                                                                                             "<tr>" +
                                                                                                 "<td height=\"12\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                         "</tbody>" +
                                                                                     "</table>" +
                                                                                 "</td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td>" +
                                                                                     "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                                                                         "<tbody>" +
                                                                                             "<tr>" +
                                                                                                 "<td style=\"word-break: break-all\"><a href=\"" + config.siteUrl + "SitePages/DocumentDown.aspx?FileID=" + documentId + "\" style=\"font-size: 13px; line-height: 22px; text-decoration: none; color: #333; display: block\" target=\"_blank\">" + docRemark + "<span style=\"font-size: 13px; display: inline-block; color: #259\">&nbsp;点击前往查看</span></a></td>" +
                                                                                                 "<td width=\"35\" class=\"zhwd-mobile-hide\">&nbsp;</td>" +
                                                                                             "</tr>" +
                                                                                         "</tbody>" +
                                                                                   "</table>" +
                                                                                 "</td>" +
                                                                             "</tr>" +
                                                                             "<tr>" +
                                                                                 "<td height=\"28\" style=\"font-size: 0px; line-height: 0px\">&nbsp;</td>" +
                                                                             "</tr>" +
                                                                         "</tbody>" +
                                                                     "</table>" +
                                                                 "</td>" +
                                                             "</tr>" +
                                                         "</tbody>" +
                                                     "</table>" +
                                                 "</td>" +
                                             "</tr>";

            return content;
        }
        /// <summary>
        /// 生成文章评论通知
        /// </summary>
        /// <param name="author">文章作者</param>
        /// <param name="reader">评论读者</param>
        /// <param name="docTitle">文章标题</param>
        /// <param name="url">文章链接</param>
        /// <returns></returns>
        public static string GeneratorEvaluateNotification(string author, string reader, string docTitle, string url)
        {
            string content =
                "<span>" + author + "，您好</span>" +
                "<br/><br/>" +
                "知识管理平台中，<span style='color:#0094ff;font-weight:bold'>" + reader + "</span>在您的文章：《" + docTitle + "》下留言，快去看看吧。<a href='" + url + "'>点击查看 </a><br/><br/>此邮件由 知识管理平台 自动发出，请勿回复";
            return content;
        }

        /// <summary>
        /// 生成文章评论回复通知
        /// </summary>
        /// <param name="author">评论作者</param>
        /// <param name="reader">评论回复者</param>
        /// <param name="replyFloor">回复楼数</param>
        /// <param name="docTitle">文章标题</param>
        /// <param name="url">文章链接</param>
        /// <returns></returns>
        public static string GeneratorReplyEvaluateNotification(string author, string reader, string replyFloor, string docTitle, string url)
        {
            string content =
                "<span>" + author + "，您好</span>" +
                "<br/><br/>" +
                "知识管理平台中，<span style='color:#0094ff;font-weight:bold'>" + reader + "</span>在文章：《" + docTitle + "》下第" + replyFloor + "楼回复了您的留言，快去看看吧。<a href='" + url + "'>点击查看 </a><br/><br/>此邮件由 知识管理平台 自动发出，请勿回复";
            return content;
        }
        public MailHelper(string mMailFrom, string mMailPwd, string mMailSMTP)
        {
            this.mailFrom = mMailFrom;
            this.mailPwd = mMailPwd;
            this.host = mMailSMTP;
        }
        public bool Send(string type)
        {
            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(mailFrom, "知识管理平台", Encoding.UTF8);

            //初始化MailMessage实例
            MailMessage myMail = new MailMessage();


            //向收件人地址集合添加邮件地址
            if (mailToArray != null)
            {
                for (int i = 0; i < mailToArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(mailToArray[i]))
                        myMail.To.Add(mailToArray[i].ToString());
                }
            }


            //向抄送收件人地址集合添加邮件地址
            if (mailCcArray != null)
            {
                for (int i = 0; i < mailCcArray.Length; i++)
                {
                    myMail.CC.Add(mailCcArray[i].ToString());
                }
            }


            //在有附件的情况下添加附件
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite spSite = new SPSite(config.siteUrl))
                    {

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where>" +
                                                        "<Eq>" +
                                                            "<FieldRef Name='Title' />" +
                                                            "<Value Type='Text'>" + type + "</Value>" +
                                                        "</Eq>" +
                                                    "</Where>";

                        SPList list = spSite.OpenWeb().Lists.TryGetList(config.picDoclib);
                        SPListItemCollection items = list.GetItems(spQuery);
                        SPListItem item;
                        if (items.Count > 0)
                        {
                            item = items[0];
                            var headerPicBytes = item.File.OpenBinary(SPOpenBinaryOptions.Unprotected);
                            Stream picStream = new MemoryStream(headerPicBytes);

                            System.Net.Mime.ContentDisposition condisp = new ContentDisposition();
                            condisp.Size = picStream.Length;
                            System.Net.Mime.ContentType contype = new ContentType();
                            contype.MediaType = System.Net.Mime.MediaTypeNames.Image.Jpeg;


                            Attachment attach = new Attachment(picStream, contype);
                            attach.ContentId = "pic";
                            attach.ContentDisposition.Inline = true;
                            attach.ContentDisposition.Size = picStream.Length;
                            attach.TransferEncoding = TransferEncoding.Base64;
                            myMail.Attachments.Add(attach);
                        }
                    }
                });
            }
            catch (Exception err)
            {
                throw new Exception("在添加附件时有错误:" + err);
            }

            //发件人地址
            myMail.From = maddr;
            //电子邮件的标题
            myMail.Subject = mailSubject;

            //电子邮件的主题内容使用的编码
            myMail.SubjectEncoding = Encoding.UTF8;

            //电子邮件正文
            myMail.Body = mailBody;

            //电子邮件正文的编码
            myMail.BodyEncoding = Encoding.Default;

            myMail.Priority = MailPriority.Normal;

            myMail.IsBodyHtml = isbodyHtml;



            SmtpClient smtp = new SmtpClient(host, 25);
            //smtp.EnableSsl = true;
            //smtp.UseDefaultCredentials = false;
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.Credentials = new System.Net.NetworkCredential(mailFrom, mailPwd);


            //设置SMTP邮件服务器
            //smtp.Host = host;

            try
            {
                //将邮件发送到SMTP邮件服务器
                smtp.Send(myMail);
                return true;

            }
            catch (System.Net.Mail.SmtpException ex)
            {
                return false;
            }

        }
        public bool Send()
        {
            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(mailFrom, "知识管理平台", Encoding.UTF8);

            //初始化MailMessage实例
            MailMessage myMail = new MailMessage();


            //向收件人地址集合添加邮件地址
            if (mailToArray != null)
            {
                for (int i = 0; i < mailToArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(mailToArray[i]))
                        myMail.To.Add(mailToArray[i].ToString());
                }
            }


            //向抄送收件人地址集合添加邮件地址
            if (mailCcArray != null)
            {
                for (int i = 0; i < mailCcArray.Length; i++)
                {
                    myMail.CC.Add(mailCcArray[i].ToString());
                }
            }         
            //发件人地址
            myMail.From = maddr;
            //电子邮件的标题
            myMail.Subject = mailSubject;

            //电子邮件的主题内容使用的编码
            myMail.SubjectEncoding = Encoding.UTF8;

            //电子邮件正文
            myMail.Body = mailBody;

            //电子邮件正文的编码
            myMail.BodyEncoding = Encoding.Default;

            myMail.Priority = MailPriority.Normal;

            myMail.IsBodyHtml = isbodyHtml;



            SmtpClient smtp = new SmtpClient(host, 25);
            //smtp.EnableSsl = true;
            //smtp.UseDefaultCredentials = false;
            //指定发件人的邮件地址和密码以验证发件人身份
            smtp.Credentials = new System.Net.NetworkCredential(mailFrom, mailPwd);


            //设置SMTP邮件服务器
            //smtp.Host = host;

            try
            {
                //将邮件发送到SMTP邮件服务器
                smtp.Send(myMail);
                return true;

            }
            catch (System.Net.Mail.SmtpException ex)
            {
                return false;
            }

        }

    }
}
