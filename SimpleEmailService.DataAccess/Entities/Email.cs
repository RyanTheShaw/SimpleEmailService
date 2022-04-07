using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleEmailService.DataAccess.Entities
{
    public class Email
    {
        public long Id { get; set; }

        public bool IsPrimary { get; set; }

        public string Address { get; set; } = default!;

        #region Navigation Properties

        //[JsonIgnore]
        //public virtual Contact Contact { get; set; }

        #endregion
    }
}
