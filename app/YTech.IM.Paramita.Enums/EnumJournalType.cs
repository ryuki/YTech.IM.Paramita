using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTech.IM.Paramita.Enums
{
    public enum EnumJournalType
    {
        [StringValue("GL")]
        GeneralLedger,
        [StringValue("KM")]
        CashIn,
        [StringValue("KK")]
        CashOut
    }
}
