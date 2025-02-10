using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectPos.Data.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentMethod
    {
        None,
        Cash,
        EcoCash,
        CreditCard,
        BankDeposit,
        Cheque,
        Voucher,
        CreditMemo,
        DPO
    }
}
