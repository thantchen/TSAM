<template>
    <div id="tsa-schedules">
        <div class="vx-row" style="margin-top: 10px">
            <div class="vx-col w-full">
                <vx-card title="Payment Approval Report">
                    <div style="text-align: right; font-size: 12px; margin-top: -35px !important">
                        <label>Select Original Invoice Number:</label>
                        <ejs-dropdownlist ref='ddlInvoiceNumber' width="200" :dataSource='invoiceNumberData' placeholder='Select original invoice number' :change="onInvoiceNumberChange" noRecordsTemplate="No invoice # available"></ejs-dropdownlist>
                    </div>
                    <ejs-grid ref='grid' :dataSource="data" :dataBound="onDataBound" :allowSorting='true' :allowFiltering='true' :allowResizing='true' :filterSettings='filterSettings' :allowPaging='true' :pageSettings='pageSettings'
                                 :allowExcelExport='true' :columns='columns'  :showColumnChooser='true' :toolbar="toolbar" :toolbarClick='onToolbarClick' :actionFailure='onActionFailure' :excelQueryCellInfo="excelQueryCellInfo">
                    </ejs-grid>
                </vx-card>
            </div>
        </div>
    </div>
</template>

<script>

import TsMessageBox from "@/components/ts-message-box/TsMessageBox.vue"

import Vue from "vue";
import { GridPlugin, Filter, Page, Selection, Sort, Resize, ColumnChooser, Toolbar, ExcelExport } from "@syncfusion/ej2-vue-grids";
import { DropDownListPlugin} from "@syncfusion/ej2-vue-dropdowns";
import { Workbook } from '@syncfusion/ej2-excel-export';

Vue.use(GridPlugin);
Vue.use(DropDownListPlugin);

export default{
    components: {
        TsMessageBox,
    },
    methods:{
        onActionFailure(args) {
            var errorMessage = args.error[0].error.responseText.split("Exception:")[1].split('<br>')[0];  //extract the message from args
            this.$vs.notify({color:'error',title:'Grid loading Error',text: errorMessage})
        },
        formatCurrency(field, data) {
            var formatter = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' });
            const fieldValue = data[field.field];

            if (isNaN(fieldValue)) {
                return fieldValue;
            }
            else {
                return formatter.format(fieldValue);
            }
        },
        formatNumber(field, data) {
            var formatter = new Intl.NumberFormat('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            const fieldValue = data[field.field];

            if (isNaN(fieldValue)) {
                return fieldValue;
            }
            else {
                return formatter.format(fieldValue);
            }
        },
        onToolbarClick: function (args) {
            switch (args.item.text) {
                case 'PDF Export':
                    this.$refs.grid.pdfExport();
                    break;
                case 'Excel Export':
                    this.$refs.grid.excelExport(this.getExcelExportProperties(), true).then(function (workbook) {
                        workbook.worksheets[0].name = "Payment Approval Report";
                        var book = new Workbook(workbook, 'xlsx');
                        var filename = `payment_approval_report_${(this.getShortDate(new Date)).replace(/-/g, '.')}.xlsx`;

                        book.save(filename);
                    }.bind(this));

                    break;
            }
        },
        onDataBound: function() {
            /*this.columns = this.sourceColumns;

            var autoFitColumns = {};
            if (this.settings && this.settings.tsaScheduleSettings) {
                autoFitColumns = this.settings.tsaScheduleSettings.filter(c => c.width == 0).map(m => m.columnName);
            }
            else {
                autoFitColumns = ['tsaId', 'tsaDescription', 'currency', 'originalInvoiceNumber', 'servicePeriod', 'originalInvoiceAmountUsd', 'creditMemoNumber', 'creditMemoAmountUsd', 'disputedAmountUsd', 'approvedProposedReleaseWOTax', 'notes', 'totalInvoiceAmountForServicePeriod', 'tsaBaseline', 'differenceWB'];
            }
            this.$refs.grid.autoFitColumns(autoFitColumns);
            */
        },
        getExcelExportProperties: function() {
            return {
                header: {
                    headerRows: 3,
                    rows: [
                        {
                            index: 1,
                            cells: [
                                /* tslint:disable-next-line:max-line-length */
                                { index: 1, colSpan: 2, value: 'PAYMENT APPROVAL REPORT', style: { fontColor: '#662d91', fontSize: 20, hAlign: 'Left', bold: true } },
                            ]
                        },
                        {
                            index: 2,
                            cells: [
                                /* tslint:disable-next-line:max-line-length */
                                { index: 1, colSpan: 2, value: 'As of ' + this.getShortDate(new Date), width: 150 },
                            ]
                        },
                    ]
                },
                //fileName: `payment_approval_report_${(this.getShortDate(new Date)).replace(/-/g, '.')}.xlsx`
            }
        },
        excelQueryCellInfo(args) {
            if (args.column.field === 'servicePeriod') {
                if (args.value) {
                    args.value = this.getShortDate((new Date(args.value)));
                }
            }
        },
        onInvoiceNumberChange: function(e) {
            if (e != null) {
                this.selectedInvoiceNumber = e.value;
                if (e.value === 'All') {
                    this.$refs.grid.clearFiltering();
                } else {
                    this.$refs.grid.filterByColumn('originalInvoiceNumber', 'equal', e.value);
                }
            }
        },
    },
    provide: {
        grid: [Filter, Page, Selection, Sort, Resize, ColumnChooser, Toolbar, ExcelExport]
    },
    data: function() {
        return {
            errorContent: '',
            data: {},
            invoiceNumberData: {},
            selectedInvoiceNumber: '',
            settings: {},
            pageSettings: { pageSizes: [20,50,100,200], pageSize: 20, pageCount: 5 },
            filterSettings: { type: 'Excel' },
            toolbar: ['ColumnChooser', 'ExcelExport'],
            columns: ['tsaId', 'tsaDescription', 'currency', 'originalInvoiceNumber', 'servicePeriod', 'originalInvoiceAmountUsd', 'creditMemoNumber', 'creditMemoAmountUsd', 'disputedAmountUsd', 'approvedProposedReleaseWOTax', 'totalInvoiceAmountForServicePeriod', 'tsaBaseline', 'differenceWB'],
            sourceColumns: [
                { field: 'tsaId', headerText: 'TSA ID', width: 130 },
                { field: 'tsaDescription', headerText: 'TSA Description', width: 130 },
                { field: 'currency', headerText: 'Currency', width: 130 },
                { field: 'originalInvoiceNumber', headerText: 'Original Invoice Number', width: 130 },
                { field: 'servicePeriod', headerText: 'Service Period', type: 'date', format: 'yyyy-MM-dd', width: 130 },
                { field: 'originalInvoiceAmountUsd', headerText: 'Original Invoice Amount (USD)', textAlign:'Right', type: 'number', format: 'C2', width: 130 },
                { field: 'creditMemoNumber', headerText: 'Credit Memo Number', width: 130 },
                { field: 'creditMemoAmountUsd', headerText: 'Credit Memo Amount (USD)', textAlign:'Right', type: 'number', format: 'C2', width: 130 },
                { field: 'disputedAmountUsd', headerText: 'Disputed Amount (USD)', textAlign:'Right', type: 'number', format: 'C2', width: 130 },
                { field: 'approvedProposedReleaseWOTax', headerText: 'Approved/Proposed/Release w/o Tax', textAlign:'Right', type: 'number', format: 'C2',  width: 130 },
                { field: 'totalInvoiceAmountForServicePeriod', headerText: 'Total Invoice Amount for Service Period', textAlign:'Right', type: 'number', format: 'C2',  width: 130 },
                { field: 'tsaBaseline', headerText: 'TSA Baseline', textAlign:'Right', type: 'number', format: 'C2', width: 130 },
                { field: 'differenceWB', headerText: 'Difference B/(W)', textAlign:'Right', type: 'number', format: 'C2', width: 130 },
            ],
        }
    },
    async mounted() {
        /*this.settings = await this.httpGet("/api/lookup/custom-settings");

        this.sourceColumns.forEach(column => {
            const setting = this.settings.tsaScheduleSettings.find(c => c.columnName == column.field);
            column.visible = setting.isVisible;
            column.width = setting.width;
        });*/

        // Set initial filter to return nothing as user has not selected invoice number filtering yet. (Have to set this.columns names variable in data declaration first)
        this.columns = this.sourceColumns;
        this.$refs.grid.filterByColumn('originalInvoiceNumber', 'equal', '');
        this.data = await this.httpGet("/api/data/payment-approval");

        // Get distinct invoice number sorted by originalInvoiceNumber
        if (this.data != null) {
          this.invoiceNumberData = [...new Map(this.data.map(item => [item.originalInvoiceNumber, item.originalInvoiceNumber])).values()].sort((a, b) => (a > b) ? 1 : -1);
        }
    }
}
</script>

<style scoped>
label {
    font-size: .857rem;
    margin-right: 10px;
}
</style>
