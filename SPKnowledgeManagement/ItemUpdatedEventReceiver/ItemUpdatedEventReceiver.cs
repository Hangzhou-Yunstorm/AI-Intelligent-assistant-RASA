using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using SPKnowledgeManagement.SitePages;

namespace SPKnowledgeManagement.ItemUpdatedEventReceiver
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class ItemUpdatedEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// 已更新项.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {

            try
            {
                this.EventFiringEnabled = false;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                   {
                       SPWeb docWeb = properties.OpenWeb();
                       docWeb.AllowUnsafeUpdates = true;
                       SPListItem item = properties.ListItem;
                       int originState = int.Parse(item["HiddenState"].ToString());
                       int currentState = int.Parse(item["_ModerationStatus"].ToString());
                       if (originState != currentState && currentState == 0)
                       {
                           string totalAccount = item.Properties["OwnerAccount"].ToString();
                           int start = totalAccount.IndexOf("\\");
                           string authorAccount = totalAccount.Substring(start, totalAccount.Length - start).TrimStart('\\');


                           using (SPSite site = new SPSite(config.siteUrl))
                           {
                               SPWeb web = site.OpenWeb();
                               web.AllowUnsafeUpdates = true;
                               SPList userList = web.Lists.TryGetList(config.userList);
                               SPQuery spUser = new SPQuery();
                               spUser.Query = "<Where><Eq><FieldRef Name='UserAccount' /><Value Type='Text'>" + authorAccount + "</Value></Eq></Where>";
                               SPListItemCollection userItems = userList.GetItems(spUser);
                               if (userItems.Count > 0)
                               {
                                   int docId = item.ID;
                                   int addIntegral = 0;
                                   SPList uploadList = web.Lists[config.uploadList];
                                   SPQuery spUpload = new SPQuery();
                                   spUpload.Query = "<Where><Eq><FieldRef Name='DocumentId' /><Value Type='Number'>" + docId + "</Value></Eq></Where>";
                                   SPListItemCollection uploadItems = uploadList.GetItems(spUpload);

                                   if (uploadItems.Count > 0)
                                   {
                                       var uploadItem = uploadItems[0];
                                       addIntegral = Convert.ToInt32(uploadItem["UploadIntegral"]);

                                       switch (currentState)
                                       {
                                           case 0:
                                               uploadItem["ApproveState"] = "已批准";
                                               break;
                                           case 1:
                                               uploadItem["ApproveState"] = "未通过";
                                               break;
                                           case 2:
                                               uploadItem["ApproveState"] = "待定";
                                               break;
                                           case 3:
                                               uploadItem["ApproveState"] = "草稿";
                                               break;
                                       }
                                       uploadItem.SystemUpdate(false);


                                       var currentItem = userItems[0];
                                       currentItem["Integral"] = Convert.ToInt32(currentItem["Integral"]) + addIntegral;
                                       currentItem.SystemUpdate(false);
                                   }
                               }
                               web.AllowUnsafeUpdates = false;

                           }
                           
                           item["ApprovedDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                           item["HiddenState"] = currentState;
                           
                           item.SystemUpdate();
                           docWeb.AllowUnsafeUpdates = false;

                       }
                   });
                this.EventFiringEnabled = true;
            }
            catch { }

        }


    }
}