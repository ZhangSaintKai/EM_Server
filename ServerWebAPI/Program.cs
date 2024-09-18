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
//����������HTTPS����������ǩ��֤��
//if (environment != "Development")
{
    //����������IPv4��ַ
    var LocalIpAddress = NetworkInterface.GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
            .SelectMany(nic => nic.GetIPProperties().UnicastAddresses)
            .Where(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .Select(addr => addr.Address).First();
    // �� appsettings.json �ж�ȡ�˿�
    int port = builder.Configuration.GetValue<int>("SSLPort");
    // �� secret.json �ж�ȡ֤��
    var configurationbuilder = new ConfigurationBuilder();
    configurationbuilder.AddUserSecrets<Program>();
    var configurationSecrets = configurationbuilder.Build();
    var certificatePath = configurationSecrets.GetRequiredSection("SSLCertificate").GetValue<string>("Path");
    var certificatePassword = configurationSecrets["SSLCertificate:Password"];

    // ����Kestrelʹ��֤��
    builder.WebHost.ConfigureKestrel(options =>
    {

        options.Listen(LocalIpAddress, 5244);

        // ��ǩ��֤��(��������)���ǿ�������launchSettings.json(֤��������)
        options.Listen(IPAddress.Loopback, 7275, listenOptions =>
        {
            listenOptions.UseHttps(certificatePath, certificatePassword);
        });

        // ��������ʹ��
        options.Listen(LocalIpAddress, port, listenOptions =>
        {
            listenOptions.UseHttps(certificatePath, certificatePassword);
        });

    });
}


// Add services to the container.
builder.Services.AddDbContext<DbEmContext>();
// ע����������ΪScoped�ķ���
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
// ע����������ΪAddSingleton�ķ���
builder.Services.AddSingleton<UseSHA>();
builder.Services.AddSingleton<WebSocketService>();

builder.Services.AddControllers();

builder.Services.AddMvc(options =>
{
    // ע��ȫ��������
    options.Filters.Add(typeof(GlobalInterceptor));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// ����Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // �������ͷ��Ϣ
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // ���ȫ�ְ�ȫҪ��
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
    Console.WriteLine($"Swagger��https://localhost:{7275}/swagger/index.html");
}

//app.UseHttpsRedirection(); //httpתhttps

// ���ÿ���
app.UseCors(options =>
{
    options.AllowAnyOrigin(); // ���������κ�Դ������
    options.AllowAnyMethod(); // �����κ� HTTP ����
    options.AllowAnyHeader(); // �����κ� HTTP ͷ��
});

//app.UseAuthorization();

// ����WebSocket
var webSocketOptions = new WebSocketOptions
{
    // ��ͻ��˷��͡�ping����Ƶ�ʣ���ȷ�����������Ӵ��ڴ�״̬��Ĭ��ֵΪ2����
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

app.MapControllers();

app.Run();
