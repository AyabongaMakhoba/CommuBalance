using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commu_balance.Models
{
	public class Profile
	{
		
        [Key]
        [DisplayName("Profile ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Profile_ID { get; set; }

        //[Required(ErrorMessage = "ID number is required")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "This field must be 13 characters")]
        [DisplayName("ID number")]
        public string User_IDNo { get; set; }

        //[Required(ErrorMessage = "First name is required")]
        [DisplayName("First Name")]
        public string Profile_Name { get; set; }

        //[Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last Name")]
        public string Profile_Surname { get; set; }

     
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Number")]
        //[Required(ErrorMessage = "Contact Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
          ErrorMessage = "Entered Contact number format is not valid.")]
        public string Profile_Cellnumber { get; set; }

        //[Required(ErrorMessage = "Residence address is required")]
        [DisplayName("Residence address")]
        public string Profile_Address { get; set; }

        //[Required(ErrorMessage = "Email address is required")]
        [DisplayName("Email address")]
        [EmailAddress]
        public string Profile_Email { get; set; }

        [DisplayName("User Category ID")]
        
       
    
        public virtual List<User_Category> User_Categories { get; set; }
        public virtual List<Document> Documents { get; set; }
        public virtual List<Donation> Donations { get; set; }
        public virtual List<Request> Requests { get; set; }

    }
}