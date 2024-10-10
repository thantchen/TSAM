ALTER TABLE [dispute_log] ADD [service_period] date NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201222002000_AddDisputeLog_Service_Period_Field', N'3.1.7');

GO

