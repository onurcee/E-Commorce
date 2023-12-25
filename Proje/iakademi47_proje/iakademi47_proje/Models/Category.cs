using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi47_proje.Models
{
	public class Category
	{
		[Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]


		[DisplayName("ID")]
		public int CategoryID { get; set; }

		[DisplayName("Üst Kategori")]
		public int ParentID { get; set; }

		
		[Required(ErrorMessage ="Kategori Adı Zorunludur")]
		[StringLength(50,ErrorMessage ="En Fazla 50 Karakter Girilebilir")]
		[DisplayName("Kategori Adı")]
		public string? CategoryName { get; set; }

		[DisplayName("Resim")]
		public string? PhotoPath { get; set; }

		[DisplayName("Aktif")]
		public bool Active { get; set; }
	}
}
