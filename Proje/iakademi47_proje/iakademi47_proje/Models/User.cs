using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace iakademi47_proje.Models
{
	public class User
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int UserID { get; set; }

		[Required(ErrorMessage = "İsim Soyisim Zorunludur")]
		[StringLength(50, ErrorMessage = "En Fazla 50 Karakter Girilebilir")]
		[DisplayName("Ad Soyad")]
		public string? NameSurname { get; set; }


		[Required(ErrorMessage = "EMail Zorunludur")]
		[StringLength(100, ErrorMessage = "En Fazla 100 Karakter Girilebilir")]
		[DisplayName("Email")]
		[EmailAddress]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Parola Zorunludur")]
		[StringLength(100, ErrorMessage = "En Fazla 100 Karakter Girilebilir")]
		[DataType(DataType.Password)]
		[DisplayName("Şifre")]

		public string? Password { get; set; }

		[DisplayName("Telefon")]
		public string? Telephone { get; set; }

		[DisplayName("Fatura Adresi")]
		public string? InvoiceAddress { get; set; }

		public bool IsAdmin { get; set; }

		[DisplayName("Aktif")]
		public bool Active { get; set; }

	}
}
