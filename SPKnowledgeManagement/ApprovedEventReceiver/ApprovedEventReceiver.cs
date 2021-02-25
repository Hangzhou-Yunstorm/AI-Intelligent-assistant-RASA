using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace SPKnowledgeManagement.ApprovedEventReceiver
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class ApprovedEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// 正在更新项.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {

            //base.ItemUpdating(properties);
        }


    }
}