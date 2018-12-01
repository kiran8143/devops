using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using OnePointRestAPI.Middlewares;
using Swashbuckle.AspNetCore.Swagger;

namespace OnePointRestAPI
{
    public class Startup
    {
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes((string)CommonUtils.AppConfig.EncryptionKey));
        public static readonly ILogger LogManager = UtilsFactory.Logger;
        public Startup(IHostingEnvironment env)
        {
            try
            {
                var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

                if (env.IsEnvironment("Development"))
                {
                    // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                  //  builder.AddApplicationInsightsSettings(developerMode: true);
                }

                builder.AddEnvironmentVariables();
                Configuration = builder.Build();
            }
            catch (Exception ex)
            {
               
            }
        }


        public IConfiguration Configuration { get; }
       

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                
                // Add framework services.
                services.AddOptions();
               // services.AddApplicationInsightsTelemetry(Configuration);
                services.AddMvc(config =>
                {

                    var policy = new AuthorizationPolicyBuilder()
                                     .RequireAuthenticatedUser()
                                     .Build();
                    //  config.Filters.Add(new AuthorizeFilter(policy));
                    //Setting up request interceptor
               
                    config.Filters.Add(new RequestFilter());
                    //Setting up response interceptor
                    config.Filters.Add(new ResponseFilter());

                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                   // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

                //Enabling CORS, Creating Policy
                services.AddCors(o => o.AddPolicy("OnePointRestAPIPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));

                //JWT -Settings -Satrts

                // Get options from app settings
                var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

                //Configure JwtIssuerOptions
                services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = "OnePointRestAPI_Server";
                    options.Audience = "*";//Restricting to a particular url
                    options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                });
                //services.AddDistributedRedisCache(options => {
                //    options.Configuration = Configuration.GetConnectionString("RedisConnection");
                //    options.InstanceName = "OnePointRestAPI_RedisServer"; // Your DNS Name  
                //});
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                });
                dynamic swaggerInfo = CommonUtils.AppConfig.Swagger_Configuration.Info;
                // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc((string)swaggerInfo.Version, new Info
                    {
                        Version = (string)swaggerInfo.Version,
                        Title = (string)swaggerInfo.Title,
                        Description = (string)swaggerInfo.Description,
                        TermsOfService = (string)swaggerInfo.TermsOfService,
                        Contact = new Contact
                        {
                            Name = (string)swaggerInfo.Contact.Name,
                            Email = (string)swaggerInfo.Contact.Email,
                            Url = (string)swaggerInfo.Contact.Url
                        },
                        License = new License
                        {
                            Name = (string)swaggerInfo.License.Name,
                            Url = (string)swaggerInfo.License.Url
                        }                      
                           
                    });
                   
                    //Determine base path for the application.

                    var basePath = AppContext.BaseDirectory;
                    var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                    var fileName = System.IO.Path.GetFileName(assemblyName + ".xml");
                    //Set the comments path for the swagger json and ui.
                    c.IncludeXmlComments(System.IO.Path.Combine(basePath, fileName));

                    // c.DocumentFilter<EnumDocumentFilter>();
                    c.DescribeAllEnumsAsStrings();
                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter Bearer into field.Format Bearer 123XXXXXXXX56", Name = "Authorization", Type = "apiKey"  });
                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                      { "Bearer", Enumerable.Empty<string>() },
                     });

                });

                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            }
            catch (Exception ex)
            {
                LogManager.Log(ex, LogType.Error);
            }
        }

       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //JWT MiddleWare Settings -Satrts
            //var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuer = true,
            //    ValidIssuer = "OnePointRestAPI_Server",

            //    ValidateAudience = true,
            //    ValidAudience = "*",

            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = _signingKey,

            //    RequireExpirationTime = true,
            //    ValidateLifetime = true,

            //    ClockSkew = TimeSpan.Zero
            //};

            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    TokenValidationParameters = tokenValidationParameters
            //});

            //JWT JWT MiddleWare Settings -Ends
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors((string)CommonUtils.AppConfig.Cors.OnePointRestAPIPolicy);

            //     app.UseApplicationInsightsRequestTelemetry();

            //      app.UseApplicationInsightsExceptionTelemetry();       
             app.UseMiddleware<HttpRequestFilter>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/docs/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
                {
                    swaggerDoc.BasePath = (string)CommonUtils.AppConfig.Swagger_Configuration.API_PREFIX;
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint((string)CommonUtils.AppConfig.Swagger_Configuration.Route, (string)CommonUtils.AppConfig.Swagger_Configuration.AppName);
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;
                    LogManager.Log(exception, LogType.Error);

                });
            });
            //Add our new middleware to the pipeline
            app.UseMiddleware<ReqRespLogMiddleware>();
            app.UseMvc();
        }

       
    }
}
