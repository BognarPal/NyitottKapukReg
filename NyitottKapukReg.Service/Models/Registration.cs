using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NyitottKapukReg.Service.Models
{
    public class Registration
    {
        public int Id { get; set; }
        [Required]
        public Day Day { get; set; }
        /// <summary>
        /// Kitöltve: csoport, null esetén: előjegyzés
        /// </summary>
        public VisitorGroup VisitorGroup { get; set; }
        //Todo egyediség éves szinten
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ParentName1 { get; set; }
        public string ParentName2 { get; set; }
        public string StudentName1 { get; set; }
        public string StudentName2 { get; set; }
        public string StudentName3 { get; set; }
        public string StudentName4 { get; set; }

        public int CountOfVisitors()
        {
            int count = 0;
            count += !string.IsNullOrWhiteSpace(ParentName1) ? 1 : 0;
            count += !string.IsNullOrWhiteSpace(ParentName2) ? 1 : 0;
            count += !string.IsNullOrWhiteSpace(StudentName1) ? 1 : 0;
            count += !string.IsNullOrWhiteSpace(StudentName2) ? 1 : 0;
            count += !string.IsNullOrWhiteSpace(StudentName3) ? 1 : 0;
            count += !string.IsNullOrWhiteSpace(StudentName4) ? 1 : 0;
            return count;
        }

        [NotMapped]
        public string Parents
        {
            get
            {
                if (string.IsNullOrEmpty(ParentName1))
                    return ParentName2;
                if (string.IsNullOrEmpty(ParentName2))
                    return ParentName1;
                return ParentName1 + ", " + ParentName2;
            }
        }

        [NotMapped]
        public string Students
        {
            get
            {
                if (string.IsNullOrEmpty(StudentName1))
                    return "";
                
                if (string.IsNullOrEmpty(StudentName2))
                    return $"{StudentName1}";
                
                if (string.IsNullOrEmpty(StudentName3))
                    return $"{StudentName1}, {StudentName2}";
                
                if (string.IsNullOrEmpty(StudentName4))
                    return $"{StudentName1}, {StudentName2}, {StudentName3}";
                
                return $"{StudentName1}, {StudentName2}, {StudentName3}, {StudentName4}";
            }
        }

    }
}
