using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commu_balance.Models
{
	public class Delivery
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DisplayName("Delivery ID")]
		public int Delivery_ID { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[DisplayName("Donation Date")]
		public DateTime Delivery_Date { get; set; }

		public int Schedule_ID { get; set; }
		public virtual Schedule Schedule { get; set; }

		public virtual List<Tracking> Trackings { get; set; }

		public virtual List<Beneficiary_Signature> Beneficiary_Signatures { get; set; }

	}
}