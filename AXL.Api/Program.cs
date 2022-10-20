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

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(option =>
    {
        option.AddJsonFile("autofac.json", true);//添加autofac注入的json文件
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())//使用autofac代替MS容器
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

        //var Pgconnection = new NpgsqlConnection(builder.Configuration.GetConnectionString("Pg"));
        //var PgsqlGenerator = new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new PostgreSqlDialect()));
        //container.Register(x => new AsyncDatabase(Pgconnection, PgsqlGenerator))
        //       .Keyed<IAsyncDatabase>("PgDb")
        //       .InstancePerLifetimeScope();
        container.RegisterModule(new ConfigurationModule(builder.Configuration));//注入模块
    }).ConfigureLogging((option, logg) => { //使用Log4net 默认路径为项目的log4net.config
        logg.AddLog4Net();
    });
// Add services to the container.
builder.Services.AddAutoMapper(typeof(TransForMationProfile));//添加实体类与DTO的映射关系
builder.Services.AddMvc();
builder.Services.AddCors(cors =>//添加跨域
{
    cors.AddPolicy("Any", policy => {
        policy.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//添加http上下文到全局
builder.Services.AddControllersWithViews(option => {//全局异常处理
    option.Filters.Add(typeof(CustomExceptionFilterAttribute));
}).AddJsonOptions(option => {//全局json序列化
    option.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    option.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    option.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
});
builder.Services.AddAuthentication("Bearer").AddJwtBearer(optins => {
    //optins.RequireHttpsMetadata = false;
    //optins.SaveToken = true;
    optins.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,//是否验证签名,不验证的画可以篡改数据，不安全
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Secret").ToString())),//解密的密钥
        ValidateIssuer = true,//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
        ValidIssuer = builder.Configuration.GetValue<string>("Issuer"),//发行人
        ValidateAudience = true,//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
        ValidAudience = builder.Configuration.GetValue<string>("Audience"),//订阅人
        ValidateLifetime = true,//是否验证过期时间，过期了就拒绝访问
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
    c.IncludeXmlComments(xmlPath); // true : 显示控制器层注释
    c.OrderActionsBy(o => o.RelativePath);
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AXL.Dto.xml"));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
        Name = "Authorization",//jwt默认的参数名称
        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AXL.API v1");
    });
}
app.UseAuthentication();//使用鉴权

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
