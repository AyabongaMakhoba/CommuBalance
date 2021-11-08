using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace Commu_balance.Models
{
	public class Donation
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Donation_ID { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Donation Amount")]
        public decimal Donation_Amt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Donation Date")]
        public DateTime Donation_Date { get; set; }

        [DisplayName("Profile ID")]
        public string Profile_ID { get; set; }
        public virtual Profile Profile { get; set; }

        //ApplicationDbContext db = new ApplicationDbContext();

        //public void UpdateFunds()
        //{
        //    Funds funds = (from i in db.Funds
        //                   where i.IsActive == true
        //                   select i).SingleOrDefault();
        //    funds.Current_Funds += Donation_Amt;
        //    db.SaveChanges();

        //}
}

}