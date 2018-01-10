using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface;
using DAL.Interface;

namespace BLL
{
    internal static class AccountOwnerMapper
    {
        public static AccountOwner FromDTO(this AccountOwnerDTO dto)
        {
            return new AccountOwner(dto.PassportID, dto.FullName, dto.Email);
        }

        public static AccountOwnerDTO ToDTO(this AccountOwner owner)
        {
            return new AccountOwnerDTO(owner.PassportID, owner.FullName, owner.Email);
        }
    }
}
