using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using AXL.Api.Utiy;
using AXL.Commons;
using AXL.Entitys.Base;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using Npgsql;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("autofac.json", true);
 builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())//ʹ��autofac����MS����
    .ConfigureContainer<ContainerBuilder>(container =>
    {
        var connection = new SqlConnection(builder.Configuration.GetConnectionString("Default"));
        var sqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect()));
        container.Register(p => new AsyncDatabase(connection, sqlGenerator)).AsImplementedInterfaces().InstancePerLifetimeScope();


        var SqlConnection = new SqlConnection(builder.Configuration.GetConnectionString("SqlServer"));
        var SqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect()));
        container.Register(x => new AsyncDatabase(SqlConnection, SqlGenerator))
               .Keyed<IAsyncDatabase>("SqlDb")
               .InstancePerLifetimeScope();


        //var Oracleconnection = new OracleConnection(builder.Configuration.GetConnectionString("Oracle"));
        //var OraclesqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new OracleDialect()));
        //container.Register(x => new AsyncDatabase(Oracleconnection, OraclesqlGenerator))
        //       .Keyed<IAsyncDatabase>("OracleDb")
        //       .InstancePerLifetimeScope();

        var Pgconnection = new NpgsqlConnection(builder.Configuration.GetConnectionString("Pg"));
        var PgsqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new PostgreSqlDialect()));
        container.Register(x => new AsyncDatabase(Pgconnection, PgsqlGenerator))
               .Keyed<IAsyncDatabase>("PgDb")
               .InstancePerLifetimeScope();
        container.RegisterModule(new ConfigurationModule(builder.Configuration));//ע��ģ��
    }).ConfigureLogging((option, logg) => { //ʹ��Log4net Ĭ��·��Ϊ��Ŀ��log4net.config
        logg.AddLog4Net();
    });
// Add services to the container.
builder.Services.AddAutoMapper(typeof(TransForMationProfile));//���ʵ������DTO��ӳ���ϵ
builder.Services.AddMvc();
builder.Services.AddCors(cors =>//��ӿ���
{
    cors.AddPolicy("Any", policy => {
        policy.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//���http�����ĵ�ȫ��
builder.Services.AddControllersWithViews(option => {//ȫ���쳣����
    option.Filters.Add(typeof(CustomExceptionFilterAttribute));
}).AddJsonOptions(option => {//ȫ��json���л�
    option.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    option.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    option.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
});
builder.Services.AddAuthentication("Bearer").AddJwtBearer(optins => {
    //optins.RequireHttpsMetadata = false;
    //optins.SaveToken = true;
    optins.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,//�Ƿ���֤ǩ��,����֤�Ļ����Դ۸����ݣ�����ȫ
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Secret").ToString())),//���ܵ���Կ
        ValidateIssuer = true,//�Ƿ���֤�����ˣ�������֤�غ��е�Iss�Ƿ��ӦValidIssuer����
        ValidIssuer = builder.Configuration.GetValue<string>("Issuer"),//������
        ValidateAudience = true,//�Ƿ���֤�����ˣ�������֤�غ��е�Aud�Ƿ��ӦValidAudience����
        ValidAudience = builder.Configuration.GetValue<string>("Audience"),//������
        ValidateLifetime = true,//�Ƿ���֤����ʱ�䣬�����˾;ܾ�����
        ClockSkew = TimeSpan.Zero,
        RequireExpirationTime = true,
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AXL.API",
        Version = "v1",
        Contact = new OpenApiContact()
        {
            Name = "AXL",
            Email = "AXL59148402@163.com",
            Url = null
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath,true); // true : ��ʾ��������ע��
    c.OrderActionsBy(o => o.RelativePath);
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AXL.Dto.xml"),true);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
        Name = "Authorization",//jwtĬ�ϵĲ�������
        In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
             new OpenApiSecurityScheme{
             Reference = new OpenApiReference {
             Type = ReferenceType.SecurityScheme,
             Id = "Bearer"
                }
             },new string[] { }
         }
    });
});

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 2;
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AXL.API v1");
    });
}
app.UseRateLimiter();

app.UseAuthentication();//ʹ�ü�Ȩ

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Any");

app.MapControllers();
static string GetTicks() => (DateTime.Now.Ticks & 0x11111).ToString("00000");
//app.MapGet("/User/GetUsers", () => Results.Ok($"Hello {GetTicks()}")).RequireRateLimiting("fixed");
app.Run();
