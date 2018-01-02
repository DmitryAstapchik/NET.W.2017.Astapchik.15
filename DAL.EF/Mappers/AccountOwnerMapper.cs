using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using ORM.EF;

namespace DAL.DB
{
    public static class AccountOwnerMapper
    {
        public static AccountOwner FromDTO(this AccountOwnerDTO dto)
        {
            return new AccountOwner() { Email = dto.Email, FullName = dto.FullName, PassportID = dto.PassportID };
        }

        public static AccountOwnerDTO ToDTO(this AccountOwner owner)
        {
            return new AccountOwnerDTO() { Email = owner.Email, FullName = owner.FullName, PassportID = owner.PassportID };
        }
    }
}
