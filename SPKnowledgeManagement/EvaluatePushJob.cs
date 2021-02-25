using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Threading.Tasks;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;
using SPKnowledgeManagement.SitePages;

namespace SPKnowledgeManagement
{
    class EvaluatePushJob : SPJobDefinition
    {
        public EvaluatePushJob()
            : base()
        { }


        public EvaluatePushJob(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "EVALUATEPUSHJOB";
        }
        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }
        public override void Execute(Guid targetInstanceId)
        {
            System.Diagnostics.Trace.Assert(false);

            try
            {
                //SPTools.InsertRecord("Eval-start");
                SPWebApplication webApplication = this.Parent as SPWebApplication;
                SPContentDatabase contentDb = webApplication.ContentDatabases[targetInstanceId];

                SPList list;
                SPList downList;
                SPList gradeList;
                SPWeb web;
                if (config.siteName == "")
                {
                    list = contentDb.Sites[0].OpenWeb().Lists.TryGetList(config.mailConfigList);
                    downList = contentDb.Sites[0].OpenWeb().Lists.TryGetList(config.downList);
                    gradeList = contentDb.Sites[0].OpenWeb().Lists.TryGetList(config.gradeList);
                    web = contentDb.Sites[0].OpenWeb();
                }
                else
                {
                    list = contentDb.Sites[config.siteName].OpenWeb().Lists.TryGetList(config.mailConfigList);
                    downList = contentDb.Sites[config.siteName].OpenWeb().Lists.TryGetList(config.downList);
                    gradeList = contentDb.Sites[config.siteName].OpenWeb().Lists.TryGetList(config.gradeList);
                    web = contentDb.Sites[config.siteName].OpenWeb();
                }
                SPQuery spDown = new SPQuery();
                SPListItemCollection downItem = downList.GetItems(spDown);

                if (downItem.Count > 0)
                {
                    for (int i = 0; i < downItem.Count; i++)
                    {
                        string userAccount = SPTools.CheckStringIsNull(downItem[i]["UserAccount"]);
                        string documentId = SPTools.CheckStringIsNull(downItem[i]["DocumentId"]);
                        string downTime = Convert.ToDateTime(downItem[i]["Created"]).ToString("yyyy-MM-dd HH:mm:ss");

                        SPList docList = web.Lists.TryGetList(config.knowledgeBase);
                        SPQuery spQuery = new SPQuery();
                        spQuery.ViewAttributes = "Scope=\"Recursive\"";
                        spQuery.Query = "<Where>" +
                                                        "<Eq>" +
                                                            "<FieldRef Name='ID' />" +
                                                            "<Value Type='Counter'>" + documentId + "</Value>" +
                                                        "</Eq>" +
                                                    "</Where>";
                        SPListItemCollection docItems = docList.GetItems(spQuery);
                        string author = "-----";
                        string docRemark = "-----";
                        if (docItems.Count > 0)
                        {
                            author = docItems[0]["Owner"].ToString();
                            docRemark = docItems[0]["Remark"].ToString();
                        }

                        //SPTools.InsertRecordByTimer(web, "3-" + userAccount + "-" + documentId);
                        SPQuery spGrade = new SPQuery();
                        spGrade.Query = "<Where>" +
                                                              "<And>" +
                                                                    "<Eq>" +
                                                                          "<FieldRef Name='UserAccount' />" +
                                                                          "<Value Type='Text'>" + userAccount + "</Value>" +
                                                                    "</Eq>" +
                                                                    "<Eq>" +
                                                                          "<FieldRef Name='DocumentId' />" +
                                                                          "<Value Type='Number'>" + documentId + "</Value>" +
                                                                     "</Eq>" +
                                                                "</And>" +
                                                         "</Where>";
                        SPListItemCollection gradeItem = gradeList.GetItems(spGrade);
                        //获取用户邮箱
                        string emailAdd = web.EnsureUser(SPTools.CheckStringIsNull(downItem[i]["TotalAccount"])).Email;
                        //SPTools.InsertRecordByTimer(web, "5-" + emailAdd);
                        string emailTitle = SPTools.CheckStringIsNull(downItem[i]["Title"]);
                        //SPTools.InsertRecordByTimer(web, "6-" + gradeItem.Count.ToString());
                        if (gradeItem.Count == 0)
                        {
                            MailHelper email = new MailHelper(SPTools.CheckStringIsNull(list.Items[0]["MailFrom"]), SPTools.CheckStringIsNull(list.Items[0]["MailPWD"]), SPTools.CheckStringIsNull(list.Items[0]["MailSMTP"]));
                            email.mailSubject = "知识管理平台未评分文档";
                            string mailHeader = MailHelper.GeneratorMailHeader("您好，知识管理平台有您一篇未评分的文档");
                            string mailContent = MailHelper.GeneratorMailContent(documentId, emailTitle, downTime, author, docRemark);
                            string mailFooter = MailHelper.BasicFooter;
                            email.mailBody = mailHeader + mailContent + mailFooter;                          
                            email.isbodyHtml = true;    //是否是HTML
                            if (emailAdd != "")
                                email.mailToArray = new string[] { emailAdd };//接收者邮件集合
                            else
                                email.mailToArray = new string[] { config.tmpEmailReciever };//接收者邮件集合
                            email.Send("未评分");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }


        }
    }
}