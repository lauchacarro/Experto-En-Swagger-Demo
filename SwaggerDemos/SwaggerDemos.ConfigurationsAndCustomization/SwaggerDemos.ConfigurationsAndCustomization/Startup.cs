using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;

using SwaggerDemos.ConfigurationsAndCustomization.Filters;
using SwaggerDemos.ConfigurationsAndCustomization.Middlewares;
using SwaggerDemos.ConfigurationsAndCustomization.Options;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SwaggerDemos.ConfigurationsAndCustomization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SecurityOptions securityOptions = Configuration.GetSection("Security").Get<SecurityOptions>();

            services.AddControllers();


            services.AddApiVersioning(
                 options =>
                 {
                     // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                     options.ReportApiVersions = true;
                 });
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Audience = securityOptions.Audience;
                    options.Authority = securityOptions.Authority;

                });




            OpenApiInfo openApiInfo = Configuration.GetSection(nameof(OpenApiInfo)).Get<OpenApiInfo>();

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AuthorizeOperationFilter>();
                c.SwaggerDoc("v1", openApiInfo);
                c.SwaggerDoc("v2", openApiInfo);

                c.IgnoreObsoleteActions();

                c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{securityOptions.Authority}connect/authorize"),
                            TokenUrl = new Uri($"{securityOptions.Authority}connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "api" , "E-Commerce Server HTTP Api" }
                            },
                        }
                    },
                    Description = "E-Commerce Server OpenId Security Scheme"
                });

                c.IncludeXmlComments(XmlCommentsFilePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SecurityOptions securityOptions = Configuration.GetSection("Security").Get<SecurityOptions>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<SwaggerBasicAuthMiddleware>();

            app.UseStaticFiles();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/{documentName}/ecommerce-swagger.json";

                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                });


            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api-docs/v1/ecommerce-swagger.json", "E-Commerce v1.0");
                c.SwaggerEndpoint("/api-docs/v2/ecommerce-swagger.json", "E-Commerce v2.0");

                c.InjectStylesheet("/swagger-custom/swagger-custom-styles.css");  //Added Code

                c.InjectJavascript("/swagger-custom/swagger-custom-script.js", "text/javascript");  //Added Code

                c.RoutePrefix = "docs";
                c.DocumentTitle = "E-Commerce - Docs";


                c.DisplayOperationId();

                c.DisplayRequestDuration();

                c.EnableFilter();

                c.UseRequestInterceptor("(request) => { return requestInterceptor(request); }");
                c.UseResponseInterceptor("(response) => { return responseInterceptor(response); }");




                c.OAuthClientId(securityOptions.ClientId);
                c.OAuthClientSecret(securityOptions.ClientSecret);
                c.OAuthAppName("E-Commerce API");
                c.OAuthScopeSeparator(" ");
                c.OAuthUsePkce();
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
