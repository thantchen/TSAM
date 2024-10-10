using Microsoft.Extensions.DependencyInjection;
using TamsApi.Core.Excel;
using TamsApi.Core.Excel.Mappings;
using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Core.Tsas
{
    public static class DependencyRegistry
    {
        public static IServiceCollection AddExcelFileHandling(this IServiceCollection services)
        {
            services.AddScoped<IDataFileRepository<TsaSchedule>, TsaRepository>();
            services.AddScoped<IDataFileRepository<Invoice>, TsaRepository>();
            services.AddScoped<IDataFileRepository<ChangeResolutionLog>, TsaRepository>();
            services.AddScoped<IDataFileRepository<Payment>, TsaRepository>();
            services.AddScoped<IDataFileRepository<SellSideNotificationChange>, TsaRepository>();
            services.AddScoped<IDataFileRepository<SellSideNotificationDispute>, TsaRepository>();

            services.AddTransient(typeof(DataAnnotationsValidator<>));
            services.AddTransient<IExcelMapProvider<TsaSchedule>, TsaScheduleMap>();
            services.AddTransient<IExcelMapProvider<Invoice>, InvoiceMap>();
            services.AddTransient<IExcelMapProvider<ChangeResolutionLog>, ChangeResolutionMap>();
            services.AddTransient<IExcelMapProvider<Payment>, PaymentMap>();
            services.AddTransient<IExcelMapProvider<SellSideNotificationChange>, SellSideChangeMap>();
            services.AddTransient<IExcelMapProvider<SellSideNotificationDispute>, SellSideDisputeMap>();

            services.AddTransient<IExcelRowValidator<TsaSchedule>, TsaRowValidator>();
            services.AddTransient<IExcelRowValidator<Invoice>, InvoiceImportValidator>();
            services.AddTransient<IExcelRowValidator<ChangeResolutionLog>, ChangeResolutionImportValidator>();
            services.AddTransient<IExcelRowValidator<Payment>, PaymentImportValidator>();
            services.AddTransient<IExcelRowValidator<SellSideNotificationDispute>, SellSideDisputeValidator>();
            services.AddTransient<IExcelRowValidator<SellSideNotificationChange>, SellSideChangeValidator>();
            
            services.AddTransient(typeof(ISheetParser<>), typeof(SheetParser<>));
            services.AddTransient<IExcelImporter<ChangeResolutionLog>, ChangeResolutionImporter>();
            services.AddTransient(typeof(IExcelImporter<>), typeof(ExcelImporter<>));
            return services;
        }
    }
}
