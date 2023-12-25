using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iakademi47_proje.Models
{
	public class Product
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProductID { get; set; }

		[Required(ErrorMessage = "Ürün Adı Zorunludur")]
		[StringLength(100, ErrorMessage = "En Fazla 100 Karakter Girilebilir")]
		[DisplayName("Ürün Adı")]
		public string? ProductName { get; set; }

		[Required]
		[DisplayName("Fiyat")]
		public decimal UnitPrice { get; set; }

		[DisplayName("Kategori")]
		public int CategoryID { get; set; }

		[DisplayName("Marka")]
		public int SublierID { get; set; }

		[DisplayName("Stok")]
		public int Stock { get; set; }

		[DisplayName("İndirim")]
		public int Discount { get; set; }

		[DisplayName("Statü")]
		public int StatusID { get; set; }

		public DateTime AddDate { get; set; }

		[DisplayName("Anahtar Kelimeler")]
		public string? Keywords { get; set; }

		private int _Kdv { get; set; }
		public int Kdv
		{
			get { return _Kdv; }
			set { _Kdv = Math.Abs(value); }
		}

		
		public int HighLighted { get; set; } //Öne Çıkanlar

		public int TopSeller { get; set; } //Çok Satanlar

		[DisplayName("Buna Bakanlar")]
		public int Related { get; set; } //Buna bakanlar buna da baktı

		[DisplayName("Notlar")]
		public string? Notes { get; set; }

		[DisplayName("Resim")]
		public string? PhotoPath { get; set; }

		[DisplayName("Aktif")]
		public bool Active { get; set; }


        [DisplayName("Üst Marka")]
        public int ParentID { get; set; }
    }
}
