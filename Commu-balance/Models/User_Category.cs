using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commu_balance.Models
{
	public class User_Category
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("User Category ID")]
        public int UserCat_ID { get; set; }

        [Required(ErrorMessage = "Bank name is required")]
        [DisplayName("Category")]
        public string Category_Name { get; set; }
        public string Profile_ID { get; set; }
        public virtual Profile Profile { get; set; }
    }
}