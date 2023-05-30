```csharp
Services.Add...()                         // Đăng ký dịch vụ vào DI Container
Services.Configure<Options>(options => )  // Chỉnh cấu hình cho Options
buider.Configuraion.Get...()              // Lấy các Configuration của hệ thống 
Services.Configure<Options>(options)      // Chỉnh cấu hình cho Options


builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
   // Trên 30 giây truy cập lại sẽ nạp lại thông tin User (Role)
   // SecurityStamp trong bảng User đổi -> nạp lại thông tinn Security
   options.ValidationInterval = TimeSpan.FromSeconds(5);
});


builder.Services.AddOptions();            // Kích hoạt Options

// Đăng ký dịch vụ SendMail : Iemail vào hệ thống
builder.Services.AddTransient<IEmailSender, SendMailService>();

// DI Option
public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
{
   mailSettings = _mailSettings.Value;
   logger = _logger;
   logger.LogInformation("Create SendMailService");
}

// DI into view
@inject ...
```