using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commu_balance.Models
{
	public class RequestStatus
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[DisplayName("Request ID")]
		public int RequestStatus_ID { get; set; }

		[DisplayName("Request status")]
		public string Request_Satus { get; set; }
		public virtual List<Request> Requests { get; set; }
	}
}