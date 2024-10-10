using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace TamsApi.Data
{
    public static class MigrationExtensions
    {
        public static MigrationBuilder AddTemporalTable(this MigrationBuilder migrationBuilder, string name)
        {
            migrationBuilder.Sql($@"
                alter table {name} add
                     [ValidFrom] datetime2 generated always as row start NOT null
                    ,[ValidTo] datetime2 generated always as row end not null
                    ,period for SYSTEM_TIME (ValidFrom, ValidTo);");
            migrationBuilder.Sql($@"alter table {name} set (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.{name}_history));");
            return migrationBuilder;
        }

        public static MigrationBuilder RemoveTemporalTable(this MigrationBuilder migrationBuilder, string name)
        {
            migrationBuilder.Sql($@"alter table {name} set (SYSTEM_VERSIONING = OFF);");
            migrationBuilder.Sql($"alter table {name} drop period for SYSTEM_TIME;");
            
            return migrationBuilder;
        }

        public static MigrationBuilder CreateView(this MigrationBuilder migrationBuilder, string viewName)
        {
            var filePath = Environment.CurrentDirectory + $@"\Migrations\Views\{viewName}.sql";

            var viewContent = File.ReadAllText(filePath);
            migrationBuilder.Sql(viewContent); ;

            return migrationBuilder;
        }

        public static MigrationBuilder DropView(this MigrationBuilder migrationBuilder, string viewName)
        {
            migrationBuilder.Sql($"DROP VIEW {viewName}");

            return migrationBuilder;
        }
    }
}
