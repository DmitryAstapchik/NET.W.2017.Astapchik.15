using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public class AccountOwner
    {
        public AccountOwner(string pid, string name, string email)
        {
            PassportID = pid;
            FullName = name;
            Email = email;
        }

        public string PassportID { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
