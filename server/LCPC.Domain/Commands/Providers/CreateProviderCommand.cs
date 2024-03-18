using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LCPC.Share.Response;
using MediatR;

namespace LCPC.Domain.Commands
{
    public class CreateProviderCommand : IRequest<ReturnResult>
    {
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