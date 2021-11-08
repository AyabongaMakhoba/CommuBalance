using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Commu_balance.Models
{
	public class Request
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DisplayName("Request ID")]
		public int Request_ID { get; set; }
 
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[DisplayName("Desired  delivery date")]
		public DateTime Request_Date { get; set; }
		public string Profile_ID { get; set; }
		public virtual Profile Profile { get; set; }

		public int RequestStatus_ID { get; set; }
		public string name { get; set; }

		public virtual RequestStatus RequestStatus { get; set; }

		public virtual List<Schedule> Schedules { get; set; }

		ApplicationDbContext db = new ApplicationDbContext();

		public string GetName()
		{
			var name = (from n in db.Profiles
						where n.Profile_ID == Profile_ID
						select n.Profile_Name.ToString()).FirstOrDefault();
			return name;
		}
	}
}