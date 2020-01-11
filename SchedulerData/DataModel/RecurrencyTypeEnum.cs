using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerData.DataModel
{
    public enum RecurrencyTypeEnum 
    {
        Weekly = 0,
        One_Off = 1
    }

}