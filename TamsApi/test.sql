ALTER TABLE [change_resolution_log] ADD [monthly_pricing] decimal(18, 2) NULL;

GO

ALTER TABLE [change_resolution_log] ADD [monthly_pricing_text] nvarchar(32) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210513215514_AddMonthlyPricingAndMonthlyPricingTextToChangeResolutionLog', N'3.1.7');

GO

