@echo off
@echo This cmd file creates a Data API Builder configuration based on the chosen database objects.
@echo To run the cmd, create an .env file with the following contents:
@echo dab-connection-string=your connection string
@echo ** Make sure to exclude the .env file from source control **
@echo **
dotnet tool install -g Microsoft.DataApiBuilder
dab init -c dab-config.json --database-type mssql --connection-string "@env('dab-connection-string')" --host-mode Development
@echo Adding tables
dab add "AddLog" --source "[dbo].[add_log]" --fields.include "add_log_id,tsa_id,tsa_sub_id,change_date,change_log_type_id,change_log_status_id,requested_by_user_id,submitted_by_user_id,comments,created_date,created_user_id,last_modified_date,last_modified_user_id,revision,tsa_sub_ids,add_type_id,add_id" --permissions "anonymous:*" 
dab add "AddLogFile" --source "[dbo].[add_log_file]" --fields.include "add_log_file_id,add_log_id,file_repository_id" --permissions "anonymous:*" 
dab add "AddType" --source "[dbo].[add_type]" --fields.include "add_type_id,name,is_active" --permissions "anonymous:*" 
dab add "ChangeLog" --source "[dbo].[change_log]" --fields.include "change_log_id,tsa_id,tsa_sub_id,change_id,change_date,change_log_type_id,change_log_status_id,requested_by_user_id,submitted_by_user_id,comments,created_date,last_modified_date,created_user_id,last_modified_user_id,revision" --permissions "anonymous:*" 
dab add "ChangeLogFile" --source "[dbo].[change_log_file]" --fields.include "change_log_file_id,change_log_id,file_repository_id" --permissions "anonymous:*" 
dab add "ChangeResolutionLog" --source "[dbo].[change_resolution_log]" --fields.include "tsa_id,tsa_sub_id,change_id,change_log_type_id,effective_date,function,sub_function,service_name,detailed_service_description,tsa_vs_rtsa,primary_owner,receiver_owner,service_exceptions,service_location,estimated_duration_text,assumptions,exceptions_to_monthly_invoice,exceptions_to_30_day_payment,notice_of_termination_text,exceptions_re_ownership_ip,tsa_exit_approach,known_tsa_exit_costs,known_dependencies,unit_description,num_of_units,price_per_unit,cost_type,additional_pricing_comments,tax_determination,option_exercised_flag,active_option_duration_exercised,monthly_cost_forecast,created_date,created_user_id,last_modified_date,last_modified_user_id,file_repository_id,change_resolution_log_id,active_end_date,currency,exit_cost_amount,max_option_duration,notice_of_term_num,option_flag,provider_owner,separation_or_standalone" --permissions "anonymous:*" 
dab add "DisputeDiscrepancy" --source "[dbo].[dispute_discrepancy]" --fields.include "dispute_discrepancy_id,name,is_active" --permissions "anonymous:*" 
dab add "DisputeLog" --source "[dbo].[dispute_log]" --fields.include "dispute_log_id,tsa_sub_id,tsa_id,submission_date,dispute_type_id,dispute_id,invoice_number,requested_by_user_id,submitted_by_user_id,dispute_discrepancy_id,comments,agreed_cost,created_date,created_user_id,last_modified_date,last_modified_user_id,revision,currency,service_period" --permissions "anonymous:*" 
dab add "DisputeLogFile" --source "[dbo].[dispute_log_file]" --fields.include "dispute_log_file_id,dispute_log_id,file_repository_id" --permissions "anonymous:*" 
dab add "DisputeType" --source "[dbo].[dispute_type]" --fields.include "dispute_type_id,name,is_active" --permissions "anonymous:*" 
dab add "FileRepository" --source "[dbo].[file_repository]" --fields.include "file_repository_id,file_name,file_stream,created_date,created_user_id,file_type_id" --permissions "anonymous:*" 
dab add "FileType" --source "[dbo].[file_type]" --fields.include "file_type_id,name,is_active" --permissions "anonymous:*" 
dab add "Invoice" --source "[dbo].[invoice]" --fields.include "id,tsa_sub_id,tsa_id,invoice_number,invoice_date,invoice_frequency,invoice_period_start_date,invoice_period_end_date,invoice_due_date,service_period,markup,created_date,created_user_id,file_repository_id,currency,markup_usd,usd_exchange_rate,number_of_units,unit_of_measure,cost,cost_usd,invoice_amount,invoice_amount_usd,ankura_invoice,reference_invoice_number" --permissions "anonymous:*" 
dab add "LogStatus" --source "[dbo].[log_status]" --fields.include "log_status_id,log_type_id,name,is_active" --permissions "anonymous:*" 
dab add "LogType" --source "[dbo].[log_type]" --fields.include "log_type_id,name,is_active" --permissions "anonymous:*" 
dab add "Payment" --source "[dbo].[payment]" --fields.include "payment_id,tsa_sub_id,tsa_id,invoice_number,payment_type,payment_amount,payment_date,transaction_id,created_date,created_user_id,file_repository_id,bank_account_from,bank_account_To,currency,exchange_rate,usd_conversion,ankura_invoice" --permissions "anonymous:*" 
dab add "Role" --source "[dbo].[role]" --fields.include "id,name,disabled,disabled_at" --permissions "anonymous:*" 
dab add "SellSideNotificationChange" --source "[dbo].[sell_side_notification_change]" --fields.include "notification_change_id,tsa_id,tsa_sub_id,primary_owner,service_name,status,action_item,file_repository_id,last_modified_date,last_modified_user_id" --permissions "anonymous:*" 
dab add "SellSideNotificationDispute" --source "[dbo].[sell_side_notification_dispute]" --fields.include "notification_dispute_id,tsa_id,tsa_sub_id,primary_owner,service_name,type,invoice_number,invoice_amount,action_item,file_repository_id,last_modified_date,last_modified_user_id,invoice_period_end_date,invoice_period_start_date,invoice_currency" --permissions "anonymous:*" 
dab add "TsaSchedule" --source "[dbo].[tsa_schedule]" --fields.include "tsa_sub_id,tsa_id,function,sub_function,service_name,detailed_service_description,tsa_vs_rtsa,primary_owner,receiver_owner,service_exceptions,service_location,estimated_duration_text,assumptions,exceptions_to_monthly_invoice,exceptions_to_30_day_payment,notice_of_termination_text,exceptions_re_ownership_ip,tsa_exit_approach,known_tsa_exit_costs,known_dependencies,unit_description,num_of_units,price_per_unit,monthly_pricing,USD,INR,CRC,MXN,PHP,BGN,BRL,cost_type,additional_pricing_comments,tax_determination,estimated_duration,option_flag,max_option_duration,option_exercised_flag,active_option_duration_exercised,exit_cost_amount,change_log_type,monthly_cost_forecast,start_date,original_end_date,active_end_date,created_date,created_user_id,last_modified_date,last_modified_user_id,file_repository_id,revision,AUD,CAD,CNY,currency,CZK,DKK,EUR,GBP,HUF,PLN,separation_or_standalone,SGD,provider_owner,monthly_pricing_text,notice_of_term_num" --permissions "anonymous:*" 
dab add "User" --source "[dbo].[user]" --fields.include "id,user_name,first_name,last_name,email,disabled,disabled_at,created_date,last_modified_date" --permissions "anonymous:*" 
dab add "UserRole" --source "[dbo].[user_role]" --fields.include "id,role_id,user_id" --permissions "anonymous:*" 
@echo Adding views and tables without primary key
@echo No primary key found for table/view 'v_monthly_estimates_actuals_2021_0407', using first Id column (tsa_sub_id) as key field
dab add "VMonthlyEstimatesActuals20210407" --source "[archive].[v_monthly_estimates_actuals_2021_0407]" --fields.include "tsa_sub_id,tsa_id,function,sub_function,service_name,detailed_service_description,tsa_vs_rtsa,primary_owner,receiver_owner,unit_description,start_date,active_end_date,separation_or_standalone,cost_type,MONTH,YEAR,END_OF_MONTH,exception_pricing,exception_forecast,monthly_pricing,monthly_cost_forecast,monthly_cost_forecast_prorated,monthly_cost_forecast_incl_actuals,monthly_cost_forecast_prorated_incl_actuals,invoice_amount_usd,payment_usd_conversion,Dispute_Category,agreed_cost,comments,change_log_type,Cancelled_Flag,orig_invoice_amount_usd,credit_memo_amount_usd,num_credits,pos_inv_amount_usd,num_pos_inv" --source.type "view" --source.key-fields "tsa_sub_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'ExcelFailures', using Id column (Id) as key field
dab add "ExcelFailure" --source "[dbo].[ExcelFailures]" --fields.include "Id,Errors,StackTrace,FileName,FileType,ExcelRow,CellData,CreatedDate,CreatedUserId" --source.type "view" --source.key-fields "Id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'exceptions', using first Id column (tsa_sub_id) as key field
dab add "Exception" --source "[dbo].[exceptions]" --fields.include "end_of_month,tsa_sub_id,tsa_id,monthly_pricing,monthly_cost_forecast,monthly_cost_forecast_prorated" --source.type "view" --source.key-fields "tsa_sub_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_activity_report', using Id column (id) as key field
dab add "VActivityReport" --source "[dbo].[v_activity_report]" --fields.include "id,created_date,tsa_id,tsa_sub_id,function,primary_owner,service_name,tsa_vs_rtsa,effective_date,log_type,log_status,comments,submitted_by,invoice_period_end_date,invoice_number,invoice_amount,agreed_cost" --source.type "view" --source.key-fields "id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_add_log', using first Id column (add_log_id) as key field
dab add "VAddLog" --source "[dbo].[v_add_log]" --fields.include "add_log_id,tsa_id,tsa_sub_id,change_date,change_log_type_id,change_log_status_id,requested_by_user_id,submitted_by_user_id,comments,created_date,created_user_id,last_modified_date,last_modified_user_id,revision,ValidFrom,ValidTo,tsa_sub_ids,add_type_id,add_id,change_log_type,change_log_status,primary_owner,receiver_owner,service_name,function,tsa_vs_rtsa" --source.type "view" --source.key-fields "add_log_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_change_log', using first Id column (change_log_id) as key field
dab add "VChangeLog" --source "[dbo].[v_change_log]" --fields.include "change_log_id,tsa_id,tsa_sub_id,change_id,change_date,change_log_type_id,change_log_status_id,requested_by_user_id,submitted_by_user_id,comments,created_date,last_modified_date,created_user_id,last_modified_user_id,revision,ValidFrom,ValidTo,change_log_type,change_log_status,primary_owner,receiver_owner,service_name,function,tsa_vs_rtsa" --source.type "view" --source.key-fields "change_log_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_dispute_log', using first Id column (dispute_log_id) as key field
dab add "VDisputeLog" --source "[dbo].[v_dispute_log]" --fields.include "dispute_log_id,tsa_sub_id,tsa_id,submission_date,dispute_type_id,dispute_id,invoice_number,requested_by_user_id,submitted_by_user_id,dispute_discrepancy_id,comments,agreed_cost,created_date,created_user_id,last_modified_date,last_modified_user_id,revision,ValidFrom,ValidTo,currency,dispute_type,dispute_discrepancy,service_name,function,unit_description,primary_owner,tsa_vs_rtsa,monthly_pricing,monthly_cost_forecast_incl_actuals,monthly_cost_forecast_prorated_incl_actuals,invoice_amount_usd,service_period" --source.type "view" --source.key-fields "dispute_log_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_input_stream_summary', using first Id column (FileRepositoryId) as key field
dab add "VInputStreamSummary" --source "[dbo].[v_input_stream_summary]" --fields.include "FileRepositoryId,FileTypeId,FileTypeName,FileName,UploadedDate,UploadedBy,TotalCount" --source.type "view" --source.key-fields "FileRepositoryId" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_inv_est_pay_disp', using first Id column (tsa_sub_id) as key field
dab add "VInvEstPayDisp" --source "[dbo].[v_inv_est_pay_disp]" --fields.include "tsa_sub_id,tsa_id,invoice_number,invoice_period_start_date,invoice_period_end_date,service_period,invoice_amount,invoice_amount_usd,separation_or_standalone,function,sub_function,service_name,tsa_vs_rtsa,primary_owner,receiver_owner,unit_description,cost_type,monthly_pricing,monthly_cost_forecast,monthly_cost_forecast_prorated,monthly_cost_forecast_prorated_invoice_period,Dispute_Category,agreed_cost,comments,payment_amount,payment_usd_conversion,payment_date,payment_type,transaction_id" --source.type "view" --source.key-fields "tsa_sub_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_inv_pay', using first Id column (tsa_sub_id) as key field
dab add "VInvPay" --source "[dbo].[v_inv_pay]" --fields.include "tsa_sub_id,tsa_id,invoice_number,ankura_invoice,invoice_date,invoice_period_start_date,invoice_period_end_date,service_period,currency,invoice_amount,invoice_amount_usd,separation_or_standalone,function,sub_function,service_name,tsa_vs_rtsa,primary_owner,receiver_owner,Dispute_Category,agreed_cost,comments,payment_amount,payment_usd_conversion,payment_date,payment_type,transaction_id" --source.type "view" --source.key-fields "tsa_sub_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_log_type_status', using Id column (id) as key field
dab add "VLogTypeStatus" --source "[dbo].[v_log_type_status]" --fields.include "id,log_type_id,log_type_name,log_status_id,log_status_name,description" --source.type "view" --source.key-fields "id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_monthly_estimates', using first Id column (tsa_sub_id) as key field
dab add "VMonthlyEstimate" --source "[dbo].[v_monthly_estimates]" --fields.include "tsa_sub_id,tsa_id,function,sub_function,service_name,detailed_service_description,tsa_vs_rtsa,primary_owner,receiver_owner,unit_description,start_date,original_end_date,active_end_date,separation_or_standalone,cost_type,MONTH,YEAR,END_OF_MONTH,monthly_pricing,monthly_cost_forecast,monthly_cost_forecast_prorated" --source.type "view" --source.key-fields "tsa_sub_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_monthly_estimates_actuals', using first Id column (tsa_sub_id) as key field
dab add "VMonthlyEstimatesActual" --source "[dbo].[v_monthly_estimates_actuals]" --fields.include "tsa_sub_id,tsa_id,function,sub_function,service_name,detailed_service_description,tsa_vs_rtsa,primary_owner,receiver_owner,unit_description,start_date,active_end_date,separation_or_standalone,cost_type,MONTH,YEAR,END_OF_MONTH,exception_pricing,exception_forecast,monthly_pricing,monthly_cost_forecast,monthly_cost_forecast_prorated,monthly_cost_forecast_incl_actuals,monthly_cost_forecast_prorated_incl_actuals,invoice_amount_usd,payment_usd_conversion,Dispute_Category,agreed_cost,comments,change_log_type,Cancelled_Flag,orig_invoice_amount_usd,credit_memo_amount_usd,num_credits,pos_inv_amount_usd,num_pos_inv" --source.type "view" --source.key-fields "tsa_sub_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_monthly_inv_est_pay_disp', using first Id column (tsa_sub_id) as key field
dab add "VMonthlyInvEstPayDisp" --source "[dbo].[v_monthly_inv_est_pay_disp]" --fields.include "tsa_sub_id,tsa_id,service_period,separation_or_standalone,function,cost_type,tsa_vs_rtsa,monthly_invoice_amount,monthly_invoice_usd_conversion,monthly_pricing,monthly_cost_forecast,monthly_cost_forecast_prorated,payment_amount,payment_usd_conversion" --source.type "view" --source.key-fields "tsa_sub_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_payment_approval_report', using first Id column (tsa_id) as key field
dab add "VPaymentApprovalReport" --source "[dbo].[v_payment_approval_report]" --fields.include "tsa_id,tsa_description,currency,original_invoice_number,service_period,original_invoice_amount_usd,credit_memo_number,credit_memo_amount_usd,disputed_amount_usd,approved_proposed_release_wo_tax,total_invoiced_amount_for_service_period,tsa_baseline,difference_w_b" --source.type "view" --source.key-fields "tsa_id" --permissions "anonymous:*" 
@echo No primary key found for table/view 'v_TSA_status', using first Id column (tsa_sub_id) as key field
dab add "VTsaStatus" --source "[dbo].[v_TSA_status]" --fields.include "tsa_sub_id,tsa_id,function,sub_function,service_name,detailed_service_description,tsa_vs_rtsa,primary_owner,receiver_owner,service_exceptions,service_location,estimated_duration_text,assumptions,exceptions_to_monthly_invoice,exceptions_to_30_day_payment,notice_of_termination_text,exceptions_re_ownership_ip,tsa_exit_approach,known_tsa_exit_costs,known_dependencies,unit_description,num_of_units,price_per_unit,monthly_pricing,USD,INR,CRC,MXN,PHP,BGN,BRL,cost_type,additional_pricing_comments,tax_determination,estimated_duration,option_flag,max_option_duration,option_exercised_flag,active_option_duration_exercised,exit_cost_amount,change_log_type,monthly_cost_forecast,start_date,original_end_date,active_end_date,created_date,created_user_id,last_modified_date,last_modified_user_id,file_repository_id,revision,ValidFrom,ValidTo,AUD,CAD,CNY,currency,CZK,DKK,EUR,GBP,HUF,PLN,separation_or_standalone,SGD,provider_owner,monthly_pricing_text,notice_of_term_num,Cancelled_Flag" --source.type "view" --source.key-fields "tsa_sub_id" --permissions "anonymous:*" 
@echo Adding relationships
dab update AddLog --relationship AddType --target.entity AddType --cardinality one
dab update AddType --relationship AddLog --target.entity AddLog --cardinality many
dab update AddLog --relationship LogStatus --target.entity LogStatus --cardinality one
dab update LogStatus --relationship AddLog --target.entity AddLog --cardinality many
dab update AddLog --relationship LogType --target.entity LogType --cardinality one
dab update LogType --relationship AddLog --target.entity AddLog --cardinality many
dab update AddLog --relationship TsaSchedule --target.entity TsaSchedule --cardinality one
dab update TsaSchedule --relationship AddLog --target.entity AddLog --cardinality many
dab update AddLog --relationship User --target.entity User --cardinality one
dab update User --relationship AddLog --target.entity AddLog --cardinality many
dab update AddLog --relationship User --target.entity User --cardinality one
dab update User --relationship AddLog --target.entity AddLog --cardinality many
dab update AddLog --relationship User --target.entity User --cardinality one
dab update User --relationship AddLog --target.entity AddLog --cardinality many
dab update AddLog --relationship User --target.entity User --cardinality one
dab update User --relationship AddLog --target.entity AddLog --cardinality many
dab update AddLogFile --relationship AddLog --target.entity AddLog --cardinality one
dab update AddLog --relationship AddLogFile --target.entity AddLogFile --cardinality many
dab update AddLogFile --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship AddLogFile --target.entity AddLogFile --cardinality many
dab update ChangeLog --relationship TsaSchedule --target.entity TsaSchedule --cardinality one
dab update TsaSchedule --relationship ChangeLog --target.entity ChangeLog --cardinality many
dab update ChangeLog --relationship User --target.entity User --cardinality one
dab update User --relationship ChangeLog --target.entity ChangeLog --cardinality many
dab update ChangeLog --relationship User --target.entity User --cardinality one
dab update User --relationship ChangeLog --target.entity ChangeLog --cardinality many
dab update ChangeLog --relationship User --target.entity User --cardinality one
dab update User --relationship ChangeLog --target.entity ChangeLog --cardinality many
dab update ChangeLog --relationship User --target.entity User --cardinality one
dab update User --relationship ChangeLog --target.entity ChangeLog --cardinality many
dab update ChangeLog --relationship LogStatus --target.entity LogStatus --cardinality one
dab update LogStatus --relationship ChangeLog --target.entity ChangeLog --cardinality many
dab update ChangeLog --relationship LogType --target.entity LogType --cardinality one
dab update LogType --relationship ChangeLog --target.entity ChangeLog --cardinality many
dab update ChangeLogFile --relationship ChangeLog --target.entity ChangeLog --cardinality one
dab update ChangeLog --relationship ChangeLogFile --target.entity ChangeLogFile --cardinality many
dab update ChangeLogFile --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship ChangeLogFile --target.entity ChangeLogFile --cardinality many
dab update ChangeResolutionLog --relationship LogType --target.entity LogType --cardinality one
dab update LogType --relationship ChangeResolutionLog --target.entity ChangeResolutionLog --cardinality many
dab update ChangeResolutionLog --relationship TsaSchedule --target.entity TsaSchedule --cardinality one
dab update TsaSchedule --relationship ChangeResolutionLog --target.entity ChangeResolutionLog --cardinality many
dab update ChangeResolutionLog --relationship User --target.entity User --cardinality one
dab update User --relationship ChangeResolutionLog --target.entity ChangeResolutionLog --cardinality many
dab update ChangeResolutionLog --relationship User --target.entity User --cardinality one
dab update User --relationship ChangeResolutionLog --target.entity ChangeResolutionLog --cardinality many
dab update ChangeResolutionLog --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship ChangeResolutionLog --target.entity ChangeResolutionLog --cardinality many
dab update DisputeLog --relationship DisputeDiscrepancy --target.entity DisputeDiscrepancy --cardinality one
dab update DisputeDiscrepancy --relationship DisputeLog --target.entity DisputeLog --cardinality many
dab update DisputeLog --relationship DisputeType --target.entity DisputeType --cardinality one
dab update DisputeType --relationship DisputeLog --target.entity DisputeLog --cardinality many
dab update DisputeLog --relationship TsaSchedule --target.entity TsaSchedule --cardinality one
dab update TsaSchedule --relationship DisputeLog --target.entity DisputeLog --cardinality many
dab update DisputeLog --relationship User --target.entity User --cardinality one
dab update User --relationship DisputeLog --target.entity DisputeLog --cardinality many
dab update DisputeLog --relationship User --target.entity User --cardinality one
dab update User --relationship DisputeLog --target.entity DisputeLog --cardinality many
dab update DisputeLog --relationship User --target.entity User --cardinality one
dab update User --relationship DisputeLog --target.entity DisputeLog --cardinality many
dab update DisputeLog --relationship User --target.entity User --cardinality one
dab update User --relationship DisputeLog --target.entity DisputeLog --cardinality many
dab update DisputeLogFile --relationship DisputeLog --target.entity DisputeLog --cardinality one
dab update DisputeLog --relationship DisputeLogFile --target.entity DisputeLogFile --cardinality many
dab update DisputeLogFile --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship DisputeLogFile --target.entity DisputeLogFile --cardinality many
dab update ExcelFailure --relationship User --target.entity User --cardinality one
dab update User --relationship ExcelFailure --target.entity ExcelFailure --cardinality many
dab update FileRepository --relationship FileType --target.entity FileType --cardinality one
dab update FileType --relationship FileRepository --target.entity FileRepository --cardinality many
dab update FileRepository --relationship User --target.entity User --cardinality one
dab update User --relationship FileRepository --target.entity FileRepository --cardinality many
dab update Invoice --relationship User --target.entity User --cardinality one
dab update User --relationship Invoice --target.entity Invoice --cardinality many
dab update Invoice --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship Invoice --target.entity Invoice --cardinality many
dab update Invoice --relationship TsaSchedule --target.entity TsaSchedule --cardinality one
dab update TsaSchedule --relationship Invoice --target.entity Invoice --cardinality many
dab update LogStatus --relationship LogType --target.entity LogType --cardinality one
dab update LogType --relationship LogStatus --target.entity LogStatus --cardinality many
dab update Payment --relationship User --target.entity User --cardinality one
dab update User --relationship Payment --target.entity Payment --cardinality many
dab update Payment --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship Payment --target.entity Payment --cardinality many
dab update Payment --relationship TsaSchedule --target.entity TsaSchedule --cardinality one
dab update TsaSchedule --relationship Payment --target.entity Payment --cardinality many
dab update SellSideNotificationChange --relationship User --target.entity User --cardinality one
dab update User --relationship SellSideNotificationChange --target.entity SellSideNotificationChange --cardinality many
dab update SellSideNotificationChange --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship SellSideNotificationChange --target.entity SellSideNotificationChange --cardinality many
dab update SellSideNotificationChange --relationship TsaSchedule --target.entity TsaSchedule --cardinality one
dab update TsaSchedule --relationship SellSideNotificationChange --target.entity SellSideNotificationChange --cardinality many
dab update SellSideNotificationDispute --relationship User --target.entity User --cardinality one
dab update User --relationship SellSideNotificationDispute --target.entity SellSideNotificationDispute --cardinality many
dab update SellSideNotificationDispute --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship SellSideNotificationDispute --target.entity SellSideNotificationDispute --cardinality many
dab update SellSideNotificationDispute --relationship TsaSchedule --target.entity TsaSchedule --cardinality one
dab update TsaSchedule --relationship SellSideNotificationDispute --target.entity SellSideNotificationDispute --cardinality many
dab update TsaSchedule --relationship FileRepository --target.entity FileRepository --cardinality one
dab update FileRepository --relationship TsaSchedule --target.entity TsaSchedule --cardinality many
dab update UserRole --relationship Role --target.entity Role --cardinality one
dab update Role --relationship UserRole --target.entity UserRole --cardinality many
dab update UserRole --relationship User --target.entity User --cardinality one
dab update User --relationship UserRole --target.entity UserRole --cardinality many
@echo Adding stored procedures
dab add "GetActivityReport" --source "[dbo].[get_activity_report]" --source.type "stored-procedure" --permissions "anonymous:execute" --rest.methods "get" --graphql.operation "query" 
@echo **
@echo ** run 'dab validate' to validate your configuration **
@echo ** run 'dab start' to start the development API host **
