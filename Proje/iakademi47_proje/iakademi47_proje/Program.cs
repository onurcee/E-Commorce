using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//ben ekledim. Oturum s�resi.
builder.Services.AddSession(option =>
{
	option.IOTimeout = TimeSpan.FromSeconds(1);
});

//ben ekledim. T�rk�e karakter sorunu
builder.Services.AddWebEncoders(o => {
	o.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(UnicodeRanges.All);
});


//ben ekledim. Layoutta kullan�c�n�n session (email) g�sterebilmek i�in
builder.Services.AddHttpContextAccessor();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();//Ben ekledim.

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
