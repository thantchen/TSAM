using System;
using System.Collections.Generic;
using System.Text;

namespace TamsApi.Models.Lookups
{
    public enum FileTypeId
    {
        ChangeLog = 1, 
        DisputeLog = 2,
        Invoice = 3,
        Payment = 4, 
        TSASchedule = 5,
        Other = 6,
        AddLogAttachment = 7,
        SellSideNotificationChange = 8,
        SellSideNotificationDispute = 9,
        ChangeLogAttachment = 10,
        DisputeLogAttachment = 11
    }
}
