using Microsoft.AspNetCore.Mvc;
using iakademi47_proje.Models;
using PagedList.Core;
using System.Collections.Specialized;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.CodeAnalysis.Differencing;
using Newtonsoft.Json;
using System.Diagnostics;

namespace iakademi47_proje.Controllers
{
    public class HomeController : Controller
    {

       Cls_Product p = new Cls_Product();
       MainPageModel mpm = new MainPageModel();
        iakademi47Context context = new iakademi47Context();
		Cls_Order cls_order = new Cls_Order();
        

        public IActionResult Index()
        {
            mpm.SliderProducts = p.ProductSelect("slider", "", 0);
            mpm.ProductsNew = p.ProductSelect("new","", 0); //new= ana sayfa, ""= alt sayfa, 0= ajax için parametre
            mpm.Productofday = p.ProductDetails();
            mpm.SpecialProducts = p.ProductSelect("Special","", 0);
			mpm.DiscountedProducts = p.ProductSelect("Discounted","", 0);
			mpm.HigligtedProducts = p.ProductSelect("Higligted", "", 0);
			mpm.TopSeller = p.ProductSelect("TopSeller", "", 0);
			mpm.StarProducts = p.ProductSelect("star", "", 0);
			mpm.FeaturedProducts = p.ProductSelect("featured", "", 0);
			mpm.NotableProducts = p.ProductSelect("notable", "", 0);
			return View(mpm);
        }

        public IActionResult Cart()
        {
            List<Cls_Order> MyCart;

            //silme butonu ile gelirse
            if (HttpContext.Request.Query["scid"].ToString() !="")
            {
                int scid = Convert.ToInt32(HttpContext.Request.Query["scid"].ToString());
                cls_order.MyCart = Request.Cookies["sepetim"];
                cls_order.DeleteFromMyCart(scid.ToString());

                var cookieOptions = new CookieOptions();
				Response.Cookies.Append("sepetim", cls_order.MyCart, cookieOptions);
				cookieOptions.Expires = DateTime.Now.AddDays(7); //7 günlük çerez süresi
				TempData["Message"] = "Ürün Sepetinizden Silindi.";
                MyCart = cls_order.SelectMyCart();
                ViewBag.MyCart = MyCart;
                ViewBag.MyCart_Table_Details = MyCart;


			}
            else
            {
                //sepet butonu ile geldim
                var cookie = Request.Cookies["sepetim"];
                
                if (cookie == null)
                {
                    //sepette ürün olmayabilir
					var cookieOptions = new CookieOptions();
                    cls_order.MyCart = "";
                    MyCart= cls_order.SelectMyCart();
                    ViewBag.MyCart = MyCart;
                    ViewBag.MyCart_Table_Details = MyCart;

                }
                else
                {
                    //sepette ürün var
                    var cookieOptions = new CookieOptions();
                    cls_order.MyCart = Request.Cookies["sepetim"];
                    MyCart= cls_order.SelectMyCart();
                    ViewBag.MyCart = MyCart;
                    ViewBag.MyCart_Table_Details = MyCart;

                }
            }

            if (MyCart.Count == 0)
            {
                ViewBag.MyCart = null;
            }

            return View();
        }

		public IActionResult CartProcess(int id)
		{
			//sepetim
			//10=1&
			//20=1&
			//30=4
			//ürün detayına tıklanınca,sepete eklenince HighLigted kolonunun değerini 1 arttıracagız
			Cls_Product.Highlighted_Increase(id);
			
			cls_order.ProductID = id;
			cls_order.Quantity = 1;



			var cookieOptions = new CookieOptions();
			//tarayıcıdan okuma
			var cookie = Request.Cookies["sepetim"];
			if (cookie == null)
			{
				//sepet boş
				cookieOptions = new CookieOptions();
				cookieOptions.Expires = DateTime.Now.AddDays(7); //7 günlük çerez süresi
				cookieOptions.Path = "/";
				cls_order.MyCart = "";
				cls_order.AddToMyCart(id.ToString());
				Response.Cookies.Append("sepetim", cls_order.MyCart, cookieOptions);
				HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
				TempData["Message"] = "Ürün Sepetinize Eklendi.";
			}
			else
			{
				//sepet doluysa
				cls_order.MyCart = cookie; //tarayıcıdan aldım,property ye koydum
				if (cls_order.AddToMyCart(id.ToString()) == false)
				{
					//sepet dolu,aynı ürün değil
					Response.Cookies.Append("sepetim", cls_order.MyCart, cookieOptions);
					cookieOptions.Expires = DateTime.Now.AddDays(7);
					HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
					TempData["Message"] = "Ürün Sepetinize Eklendi.";
					//o an hangi sayfadaysam sayfanın linkini yakalıyorum
				}
				else
				{
					HttpContext.Session.SetString("Message", "Ürün Sepetinize Zaten Var");
					TempData["Message"] = "Ürün Sepetinize Zaten Var.";
				}
			}
			string url = Request.Headers["Referer"].ToString();
			return Redirect(url);
		}

	

        [HttpGet]
        public IActionResult Order()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                User? user = Cls_User.SelectMemberInfo(HttpContext.Session.GetString("Email").ToString());
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        
        }


        //method overload = aynı parametre sırasıyla, aynı isimli meyhod yazılmaz,
        //overload etmek için parametre sırası farklı olmalı

        [HttpPost]

        public IActionResult Order(IFormCollection frm)
        {
            //string? KrediKartNo = Request.Form["KrediKartNo"]; IFormCollection olmadan yakalamak için (method overload sebiyle string deneme gibi birşey yazmak lazım)
            string? KrediKartNo = frm["KrediKartNo"].ToString();
            string? İsimSoyisim = frm["İsimSoyisim"].ToString();
            string? Ay = frm["Ay"].ToString();
            string? Yıl = frm["Yıl"].ToString();
            string? CVC = frm["CVC"].ToString();


            string? txttckimlikno = frm["txttckimlikno"].ToString();
            string? txtvergino = frm["txtvergino"].ToString();

            if (txttckimlikno != "")
            {
                WebServiceController.tckimlikno = txttckimlikno;
            }
            else
            {
                WebServiceController.vergino= txtvergino;
            }


            NameValueCollection data = new NameValueCollection();
            string url = "https://www.onurcaliskan.com/backref";

            data.Add("BACK_REF", url);
            data.Add("CC_CVV", CVC);
            data.Add("CC_NUMBER", KrediKartNo);
            data.Add("EXP_MONTH", Ay);
            data.Add("EXP_YEAR", Yıl);

            var deger = "";
            foreach (var item in data)
            {
                var value = item as string;
                var byteCount = Encoding.UTF8.GetBytes(data.Get(value));
                deger += byteCount + data.Get(value);
            }

            var signatureKey = "payu üyeliğinde size verilen script_key buraya gelecek";
            var hash = HashWithSignature(deger, signatureKey);

            data.Add("ORDER_HASH", hash);

            var x =POSTFormPAYU("https://secure.payu.com.tr/order/..", data);


            //sanal kart
            if (x.Contains("<STATUS>SUCCES</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
            {
                //SANAL KART OK
            }
            else
            {
                //kredi kartı

            }

            return RedirectToAction("backref");
        }

        public static string HashWithSignature(string deger, string signatureKey)
        {
            return "";
        }


        public IActionResult backref()
        {
            Confirm_Order();

            return RedirectToAction("Confirm");
        }


        public static string OderGroupGUID = "";

        public IActionResult Confirm_Order()
        {

            //sipariş tablosuna kayıt
            //cookie sepetini sileceğiz
            //efatura oluşturacağız, e fatura oluşturan xml methodu cağıracağız

            var cookieOptions = new CookieOptions();
            var cookie = Request.Cookies["sepetim"];
            if (cookie != null)
            {

                cls_order.MyCart = cookie; //tarayıcıdan aldım,property ye koydum
                OderGroupGUID = cls_order.WriteToOrderTable(HttpContext.Session.GetString("Email"));
                cookieOptions.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Delete("sepetim");

                bool result = Cls_User.SendSms(OderGroupGUID);
                if (result == false)
                {
                    //Orders tablosunda sms kolonuna false değeri basılır, admin panelde menü yapılır,
                    //Orders tablosunda sms kolonu = false olan siparişleri getir
                }

                //Cls_User.SendEmail(OderGroupGUID);


            }

            return RedirectToAction("Confirm");
        }

        public IActionResult Confirm()
        {
            ViewBag.OderGroupGUID = OderGroupGUID;
            return View();
        }

        public static string POSTFormPAYU(string url, NameValueCollection data)
        {
            return "";
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Register(User user)
        {
            if (Cls_User.loginEmailControl(user) == false)
            {
                bool answer = Cls_User.AddUser(user);



                if (answer)
                {
                    TempData["Message"] = "Kaydedildi.";
                    return RedirectToAction("Login");
                }
                TempData["Message"] = "Hata.Tekrar deneyiniz.";
            }
            else
            {
                TempData["Message"] = "Bu Email Zaten mevcut.Başka Deneyiniz.";
            }
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            string answer = Cls_User.MemberControl(user);

            if (answer == "error")
            {
                TempData["Message"] = "Hata.Email ve/veya Şifre Hatalı";
            
            }
            else if (answer == "admin")
            {
                //email ve şifre doğru ama admin mi yoksa normal kullanıcı mı?
                HttpContext.Session.SetString("Admin", "Admin");
                HttpContext.Session.SetString("Email", answer);
                return RedirectToAction("Index", "Admin");

            }
            else
            {
                //normal kullanıcı
                HttpContext.Session.SetString("Email", answer);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

   


        public IActionResult NewProducts()
        {
            mpm.ProductsNew = p.ProductSelect("new", "new", 0);
            return View(mpm);
        }

        public PartialViewResult _partialNewProducts(string nextpagenumber)
        {
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.ProductsNew = p.ProductSelect("new", "new", pagenumber);
            return PartialView(mpm);
        }


        public IActionResult SpecialProducts()
        {
            mpm.SpecialProducts = p.ProductSelect("Special", "Special", 0);
            return View(mpm);
        }

        public PartialViewResult _partialSpecialProducts(string nextpagenumber)
        {
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.SpecialProducts = p.ProductSelect("Special", "Special", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult DiscountedProducts()
        {
            mpm.DiscountedProducts = p.ProductSelect("Discounted", "Discounted", 0);
            return View(mpm);
        }

        public PartialViewResult _partialDiscountedProducts(string nextpagenumber)
        {
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.DiscountedProducts = p.ProductSelect("Discounted", "Discounted", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult HighlightedProducts()
        {
            mpm.HigligtedProducts = p.ProductSelect("Higligted", "Higligted", 0);
            return View(mpm);
        }

        public PartialViewResult _partialHighlightedProducts(string nextpagenumber)
        {
            int pagenumber = Convert.ToInt32(nextpagenumber);
            mpm.HigligtedProducts = p.ProductSelect("Higligted", "Higligted", pagenumber);
            return PartialView(mpm);
        }

        public IActionResult TopsellerProducts(int page = 1, int pageSize = 4)
        {
            PagedList<Product> model = new PagedList<Product>(context.Products.OrderByDescending(p => p.TopSeller), page, pageSize);
            return View("TopsellerProducts", model);
        }

        public IActionResult MyOrders()
        {
            if (HttpContext.Session.GetString("Email") != null)
            {
                List<vw_MyOrders> orders = cls_order.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
                return View(orders);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult DetailedSearch()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Suppliers = context.Suppliers.ToList();
            return View();
        }

        public IActionResult DProducts(int CategoryID, string[] SupplierID, string price, string IsInStock)
        {
            price = price.Replace(" ", "");
            string[] PriceArray = price.Split('-');
            string startprice = PriceArray[0];
            string endprice = PriceArray[1];

            string sign = ">";
            if (IsInStock == "0")
            {
                sign = ">=";
            }

            int count = 0;
            string suppliervalue = ""; //1,2,4
            for (int i = 0; i < SupplierID.Length; i++)
            {
                if (count == 0)
                {
                    suppliervalue = "SublierID =" + SupplierID[i];
                    count++;
                }
                else
                {
                    suppliervalue += " or SublierID =" + SupplierID[i];
                }
            }

            string query = "select * from Products where  CategoryID = " + CategoryID + " and (" + suppliervalue + ") and (UnitPrice > " + startprice + " and UnitPrice < " + endprice + ") and Stock " + sign + " 0 order by ProductName";

            ViewBag.Products = p.SelectProductsByDetails(query);
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index");

        }

        public IActionResult CategoryPage(int id)
        {
            
                List<Product> products = p.ProductSelectWithCategoryID(id);
                return View(products);
           
        }

        public IActionResult SupplierPage(int id)
        {
            List<Product> products = p.ProductSelectWithSupplierID(id);
            return View(products);
        }



        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            //efcore
            //mpm.ProductDetails = context.Products.FirstOrDefault(p => p.ProductID == id);

            //select * from Products where ProductID = id  ado.net , dapper

            //linq  - 4 nolu ürünün bütün kolon (sütün) bilgileri elimde
            mpm.ProductDetails = (from p in context.Products where p.ProductID == id select p).FirstOrDefault();

            //linq
            mpm.CategoryName = (from p in context.Products
                                join c in context.Categories
                              on p.CategoryID equals c.CategoryID
                                where p.ProductID == id
                                select c.CategoryName).FirstOrDefault();

            //linq
            mpm.BrandName = (from p in context.Products
                             join s in context.Suppliers
                           on p.SublierID equals s.SupplierID
                             where p.ProductID == id
                             select s.BrandName).FirstOrDefault();

            //select * from Products where Related = 2 and ProductID != 4
            mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.ProductDetails!.Related && p.ProductID != id).ToList();

            Cls_Product.Highlighted_Increase(id);

            return View(mpm);
        }


        //ARAMA BARI İÇİN YAZILDI
		public PartialViewResult gettingProducts(string id)
		{
			id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
			List<sp_arama> ulist = Cls_Product.gettingSearchProducts(id);
			string json = JsonConvert.SerializeObject(ulist);
			var response = JsonConvert.DeserializeObject<List<Search>>(json);
			return PartialView(response);
		}

        public IActionResult PharmacyOnDuty()
        {
            /*
            https://openfiles.izmir.bel.tr/111324/docs/ibbapi-WebServisKullanimDokumani_1.0.pdf
            https://openapi.izmir.bel.tr/api/ibb/cbs/wizmirnetnoktalari
            https://acikveri.bizizmir.com/dataset/kablosuz-internet-baglanti-noktalari/resource/982875a4-2bb6-4178-8ee2-3f07641156bb
            https://acikveri.bizizmir.com/dataset/izban-banliyo-hareket-saatleri
            */

            //https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler

            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler");

            var pharmacy = JsonConvert.DeserializeObject<List<Pharmacy>>(json);

            return View(pharmacy);
        }

        public IActionResult ArtAndCulture()
        {
            //https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler

            string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler");

            var activite = JsonConvert.DeserializeObject<List<Activite>>(json);

            return View(activite);
        }

    }
}
