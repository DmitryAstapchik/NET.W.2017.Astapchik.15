using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    /// <summary>
    /// generates bank account IBAN
    /// </summary>
    public class IBANGenerator : IIBANGenerator
    {
        /// <summary>
        /// generates new IBAN
        /// </summary>
        /// <returns>IBAN</returns>
        public string GenerateIBAN()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
