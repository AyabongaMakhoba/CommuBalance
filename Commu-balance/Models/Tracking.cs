using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commu_balance.Models
{
	public class Tracking
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Tracking number")]
        public int Track_ID { get; set; }

        [DisplayName("Tracking Message")]
        public string Track_Message { get; set; }
        public string Delivery_Status { get; set; }

        public int Delivery_ID { get; set; }
        public virtual Delivery Delivery { get; set; }
    }
}