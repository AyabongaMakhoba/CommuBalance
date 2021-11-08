using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commu_balance.Models
{
	public class Document
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Document ID")]
        public int Documents_ID { get; set; }

        [Required]
        [DisplayName("ID Document")]
        public byte[] Documents { get; set; }
        
        [Required]
        [DisplayName("Proof of income")]
        public byte[] Proof_Of_Income { get; set; }
        public string Profile_ID { get; set; }
        public virtual Profile Profile { get; set; }
  
    }
}