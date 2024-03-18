using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.QueriesDtos
{
    public class SupilerSearch:DataSearch {
        public string TelOne { get; set; }
        public string Address { get; set; }
        public string PhoneOne { get; set; }
        public string UserOne { get; set; }
        public string Remark { get; set; }

    }

    public record SupilerSelect
    {
        public string Name { get; private set; }
        public string SupileId { get;private  set; }

        public SupilerSelect(string name,string id)
        {
            Name = name;
            SupileId = id;
        }
    }
    public record SupilerDto
    {
        public SupilerDto(SupplierInfo info) {
            Address = info.Address;
            Id = info.Id;
            SupileCode = info.SupNumber;
            SupileName = info.SupName;
            TelONE = info.SupTel;
            TelTWO = info.SupTelT;
            PhoneONE = info.SupPhone;
            PhoneTWO = info.SupPhoneT;
            UserONE = info.ProviderUser;
            UserTWO  = info.ProviderUserT;
            Enable = info.Enable;
            Remark = info.Remark;
        }
        public string Id { get; private set; }
        public string SupileCode {get;private set;}
        public string SupileName { get; private  set; }
        public string TelONE { get;  private set; }
        public string TelTWO { get;private  set; }
        public string PhoneONE { get; private set; }
        public string PhoneTWO { get; private set; }
        public string UserONE { get;  private set; }
        public string UserTWO { get; private set; }
        public string Address { get; private set; }
        public bool Enable { get; private set; }
        public string Remark { get; private set; }
    }

    public record EnableSupiler
    {
        public string Id { get; set; }
        public string SupileCode { get; set; }
        public string SupileName { get; set; }
        public string SupileUser { get; set; }
        public string SupileUTEN { get; set; }
        public string SupileTel { get; set; }
        public string SupileSen { get; set; }
    }
    
}