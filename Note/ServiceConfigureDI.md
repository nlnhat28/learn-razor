# 📥 Services
```csharp
buider.Services.AddSomething()    // Đăng ký dịch vụ vào DI Container
builder.Services.AddOptions();    // Kích hoạt Options
builder.Services.AddTransient<IEmailSender, SendMailService>(); // Đăng ký dịch vụ SendMail : IEmailSender vào hệ thống
```
# ⚙️ Configure
```csharp
Services.Configure<Options>(options => )  // Chỉnh cấu hình cho Options
buider.Configuraion.Get...()              // Lấy các Configuration của hệ thống 
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
   // Trên 5 giây truy cập lại sẽ nạp lại thông tin User (Role)
   // SecurityStamp trong bảng User đổi -> nạp lại thông tin Security
   options.ValidationInterval = TimeSpan.FromSeconds(5);
});
```
# 💉 Dependency Injection
## DI Option
```csharp
public SendMailService(IOptions<MailSettings> _mailSettings)
{
   mailSettings = _mailSettings.Value;
}
```
## DI into View.cshtml 
```csharp
@inject AppDbContext context;
```
## DI ILogger
```csharp
private readonly ILogger<Product> _logger;
public Product(ILogger<Product> logger)
{
   _logger = logger;
}
```