ALTER TABLE [invoice] ADD [reference_invoice_number] nvarchar(24) NULL;

GO

CREATE VIEW [dbo].[v_payment_approval_report]
AS

WITH i1 AS
(
    SELECT *,
           ROW_NUMBER() OVER (PARTITION BY tsa_sub_id, service_period, MONTH(invoice_date) ORDER BY invoice_amount_usd DESC ) AS rn
    FROM invoice WITH(NOLOCK)
)
, i2 AS
(
    SELECT tsa_sub_id,
           service_period,
           SUM(invoice_amount_usd) AS invoice_amount_usd_total
    FROM invoice WITH(NOLOCK)
    GROUP BY tsa_sub_id, service_period
)
, mea AS
(
    SELECT tsa_sub_id,
           end_of_month,
           monthly_pricing AS baseline
    FROM v_monthly_estimates_actuals
)
SELECT i1.tsa_sub_id AS tsa_id,
       ts.service_name AS tsa_description,
       i1.currency AS currency,
       i1.invoice_number AS original_invoice_number,
       i1.service_period AS service_period,
       i1.invoice_amount_usd AS original_invoice_amount_usd,
       COALESCE(i1.reference_invoice_number, '') AS credit_memo_number,					--Replace null with Credit memo number ALL good
       COALESCE(NULL, 0.00) AS credit_memo_amount_usd,									--Replace null with Credit memo amount usd
       -1 * COALESCE(NULL, 0.00) AS disputed_amount_usd,								--Replace null with Credit memo amount usd
       i1.invoice_amount_usd - ISNULL(NULL, 0.00) AS approved_proposed_release_wo_tax,	--Replace null with Credit memo amount usd
       --COALESCE(NULL, '') AS notes,													--Either to systematically add common notes, or to leave as empty string for manual edit in Excel
       i2.invoice_amount_usd_total AS total_invoiced_amount_for_service_period,
       mea.baseline AS tsa_baseline,
       --CASE WHEN i1.rn > 1 THEN 0 ELSE mea.baseline END AS tsa_baseline,
       mea.baseline - i2.invoice_amount_usd_total AS difference_w_b
       --CASE WHEN i1.rn > 1 THEN 0 ELSE mea.baseline END - i2.invoice_amount_usd_total AS difference_w_b
FROM i1
    LEFT JOIN tsa_schedule AS ts WITH(NOLOCK)
        ON i1.tsa_sub_id = ts.tsa_sub_id
    LEFT JOIN i2
        ON i1.tsa_sub_id = i2.tsa_sub_id
           AND i1.service_period = i2.service_period
    LEFT JOIN mea
        ON i1.tsa_sub_id = mea.tsa_sub_id
           AND DATEADD(DAY, -1, DATEADD(MONTH, 1, i1.service_period)) = mea.end_of_month




GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210524212358_PaymentApprovalReportView', N'3.1.7');

GO

