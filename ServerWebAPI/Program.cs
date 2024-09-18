using Microsoft.OpenApi.Models;
using ServerWebAPI.BLL;
using ServerWebAPI.Commons.Algorithm;
using ServerWebAPI.DAL;
using ServerWebAPI.Interceptors;
using ServerWebAPI.Models;
using ServerWebAPI.Services;
using System.Net;
using System.Net.NetworkInformation;

var builder = WebApplication.CreateBuilder(args);


var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Console.WriteLine(environment);
//生产环境的HTTPS额外配置自签名证书
//if (environment != "Development")
{
    //本机局域网IPv4地址
    var LocalIpAddress = NetworkInterface.GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
            .SelectMany(nic => nic.GetIPProperties().UnicastAddresses)
            .Where(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .Select(addr => addr.Address).First();
    // 从 appsettings.json 中读取端口
    int port = builder.Configuration.GetValue<int>("SSLPort");
    // 从 secret.json 中读取证书
    var configurationbuilder = new ConfigurationBuilder();
    configurationbuilder.AddUserSecrets<Program>();
    var configurationSecrets = configurationbuilder.Build();
    var certificatePath = configurationSecrets.GetRequiredSection("SSLCertificate").GetValue<string>("Path");
    var certificatePassword = configurationSecrets["SSLCertificate:Password"];

    // 配置Kestrel使用证书
    builder.WebHost.ConfigureKestrel(options =>
    {

        options.Listen(LocalIpAddress, 5244);

        // 自签名证书(不受信任)覆盖开发配置launchSettings.json(证书受信任)
        options.Listen(IPAddress.Loopback, 7275, listenOptions =>
        {
            listenOptions.UseHttps(certificatePath, certificatePassword);
        });

        // 生产环境使用
        options.Listen(LocalIpAddress, port, listenOptions =>
        {
            listenOptions.UseHttps(certificatePath, certificatePassword);
        });

    });
}


// Add services to the container.
builder.Services.AddDbContext<DbEmContext>();
// 注册生命周期为Scoped的服务
builder.Services.AddScoped<UserDAL>();
builder.Services.AddScoped<UserBLL>();
builder.Services.AddScoped<PrivateConversationDAL>();
builder.Services.AddScoped<PrivateConversationBLL>();
builder.Services.AddScoped<ContactDAL>();
builder.Services.AddScoped<ContactBLL>();
builder.Services.AddScoped<FileBLL>();
builder.Services.AddScoped<FileDAL>();
builder.Services.AddScoped<PrivateMemberBLL>();
builder.Services.AddScoped<PrivateMemberDAL>();
builder.Services.AddScoped<PrivateMessageBLL>();
builder.Services.AddScoped<PrivateMessageDAL>();
// 注册生命周期为AddSingleton的服务
builder.Services.AddSingleton<UseSHA>();
builder.Services.AddSingleton<WebSocketService>();

builder.Services.AddControllers();

builder.Services.AddMvc(options =>
{
    // 注册全局拦截器
    options.Filters.Add(typeof(GlobalInterceptor));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 配置Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // 添加请求头信息
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // 添加全局安全要求
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine($"Swagger：https://localhost:{7275}/swagger/index.html");
}

//app.UseHttpsRedirection(); //http转https

// 配置跨域
app.UseCors(options =>
{
    options.AllowAnyOrigin(); // 允许来自任何源的请求
    options.AllowAnyMethod(); // 允许任何 HTTP 方法
    options.AllowAnyHeader(); // 允许任何 HTTP 头部
});

//app.UseAuthorization();

// 配置WebSocket
var webSocketOptions = new WebSocketOptions
{
    // 向客户端发送“ping”的频率，以确保代理保持连接处于打开状态。默认值为2分钟
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

app.MapControllers();

app.Run();
