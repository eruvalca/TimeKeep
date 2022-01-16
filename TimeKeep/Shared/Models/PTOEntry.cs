using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeep.Shared.Enums;

namespace TimeKeep.Shared.Models
{
    public class PTOEntry
    {
        public int PTOEntryId { get; set; }
        [Required]
        public decimal PTOHours { get; set; }
        [Required]
        public PTOType PTOType { get; set; }
        [Required]
        public DateTime PTODate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string TimeKeepUserId { get; set; }
    }
}
