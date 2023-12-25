namespace iakademi47_proje.Models
{
    public class MainPageModel
    {

        public List<Product>? SliderProducts { get; set; }
        public List<Product>? ProductsNew { get; set; }

        public Product? Productofday { get; set; }

        public List<Product>? SpecialProducts { get; set; }

		public List<Product>? DiscountedProducts { get; set; }

		public List<Product>? HigligtedProducts { get; set; }

		public List<Product>? TopSeller { get; set; }

		public List<Product>? StarProducts { get; set; }

		public List<Product>? FeaturedProducts { get; set; }

		public List<Product>? NotableProducts { get; set; }


        //Ürün Detay sayfası için
        public Product? ProductDetails { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public List<Product>? RelatedProducts { get; set; }
    }
}
