using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPKnowledgeManagement.SitePages
{
    /// <summary>
    /// 文档审批状态枚举。0：已批准，1：拒绝，2：待定，3：草稿
    /// SPListItem中，审批状态字段为   “_ModerationStatus”
    /// 例：item["_ModerationStatus"] = Convert.ToInt32(ModerationStatus.Approved)，设置item审批状态为已批准
    /// </summary>
    public enum ModerationStatus
    {
        Approved,//0
        Denied,//1
        Pending,//2
        Draft//3
    }



    class SPTools
    {
        public delegate void StartDownload();
        public event StartDownload fileDownload;

        /// <summary>  
        /// 判断是否是数字  
        /// </summary>  
        /// <param name="str">字符串</param>  
        /// <returns>bool</returns>  
        public static bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);
            foreach (byte c in bytestr)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        } 

        public static bool IsItemExisted(int id, SPList list)
        {
            try
            {
                var existed = list.GetItemById(id);
                if (existed != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// //获取用户登录账号，根据Web
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        public static string GetLoginUserAccount(SPWeb web)
        {
            string loginName = web.CurrentUser.LoginName;
            int start = loginName.IndexOf("\\");
            string loginAccount = loginName.Substring(start, loginName.Length - start).TrimStart('\\');
            return loginAccount;
        }

        
        /// <summary>
        /// //获取用户登录账，根据LoginName
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        public static string GetLoginUserAccount(string loginName)
        {
            int start = loginName.IndexOf("\\");
            string loginAccount = loginName.Substring(start, loginName.Length - start).TrimStart('\\');
            return loginAccount;
        }

        public static void InsertRecord(string content)
        {
            try
            {
                using (SPSite site = new SPSite(config.siteUrl))
                {
                    SPWeb web = site.OpenWeb();
                    SPList list = web.Lists.TryGetList("TimerJobLog");
                    SPListItem item = list.Items.Add();
                    item["Title"] = content;
                    web.AllowUnsafeUpdates = true;
                    item.Update();
                    web.AllowUnsafeUpdates = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void InsertRecordByTimer(SPWeb web, string content)
        {
            try
            {
                SPList list = web.Lists.TryGetList("TimerJobLog");
                SPListItem item = list.Items.Add();
                item["Title"] = content;
                web.AllowUnsafeUpdates = true;
                item.Update();
                web.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {

            }
        }

        public static string CheckStringIsNull(object content)
        {
            string value;
            if (content == null)
            {
                value = "";
            }
            else
            {
                value = content.ToString();
            }
            return value;
        }
    }
}
