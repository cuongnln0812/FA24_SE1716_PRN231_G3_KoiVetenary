using KoiVetenary.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Writers;
using KoiVetenary.Data.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace KoiVetenary.APIService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Data.Models.Service>("Services");
            //
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureServiceService(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           
            builder.Services.AddControllers().AddOData(opt =>
            opt.AddRouteComponents("odata", modelBuilder.GetEdmModel())
               .Select()
               .Filter()
               .OrderBy()
               .Expand()
               .SetMaxTop(100)
               .Count());
            
            var app = builder.Build();


            ///////////
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
