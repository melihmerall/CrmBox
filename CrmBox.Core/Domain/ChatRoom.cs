using CrmBox.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmBox.Core.Domain
{
    public class ChatRoom:BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
