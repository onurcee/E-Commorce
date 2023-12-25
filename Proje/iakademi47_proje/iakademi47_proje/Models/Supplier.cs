using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi47_proje.Models
{
	public class Supplier
	{

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int SupplierID { get; set; }

		[Required(ErrorMessage = "Marka Adı Zorunludur")]
		[StringLength(100, ErrorMessage = "En Fazla 100 Karakter Girilebilir")]
		[DisplayName("Marka Adı")]
		public string? BrandName { get; set; }

		[DisplayName("Aktif")]
		public bool Active { get; set; }

        [DisplayName("Resim")]
        public string? PhotoPath { get; set; }

    }
}
