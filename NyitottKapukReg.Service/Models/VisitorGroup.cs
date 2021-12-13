using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NyitottKapukReg.Service.Models
{
    public class VisitorGroup
    {
        [Key]
        public int Id { get; set; }
        public int GroupNumber { get; set; }
        [Required]
        public string ClassroomNumber { get; set; }
        //Todo egyedi -> de lehet null
        public Day Day { get; set; }
    }
}
