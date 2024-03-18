using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class UpdateProviderCommand : IRequest<ReturnResult>
    {
        public string Id { get; set; }
        public string SupileCode { get; set; }
        public string SupileName { get; set; }
        public string TelONE { get; set; }
        public string TelTWO { get; set; }
        public string PhoneONE { get; set; }
        public string PhoneTWO { get; set; }
        public string UserONE { get; set; }
        public string UserTWO { get; set; }
        public string Address { get; set; }
        public bool Enable { get; set; }
        public string Remark { get; set; }
    }
}