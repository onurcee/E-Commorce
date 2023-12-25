using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi47_proje.Models
{
	public class Status
	{

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int StatusID { get; set; }

		[Required(ErrorMessage = "Statü Adı Zorunludur")]
		[StringLength(100, ErrorMessage = "En Fazla 100 Karakter Girilebilir")]
		[DisplayName("Statü Adı")]
		public string? StatuName { get; set; }

		[DisplayName("Aktif")]
		public bool Active { get; set; }

	}
}
