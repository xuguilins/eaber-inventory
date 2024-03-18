using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.QueriesDtos
{
    public record UserInfoDto
    {
        public UserInfoDto(string id,string userName,string tel,string address,string remark,bool enable) {
            Id = id;
            UserName = userName;
            UserTel = tel;
            UserAddress = address;
            Remark = remark;
            Enable = enable;
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserTel { get; set; }
        public string UserAddress { get; set; }
        public string Remark { get; set; }
        public bool Enable { get; set; }
    }
}