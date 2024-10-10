<template>
    <div id="tsa-schedules">
        <div class="vx-row" style="margin-top: 10px">
            <div class="vx-col w-full">
                <vx-card title="TSA Schedule">
                    <ejs-grid ref='grid' :dataSource="data" :allowSorting='true' :allowFiltering='true' :allowResizing='true' :filterSettings='filterSettings' :allowPaging='true' :pageSettings='pageSettings' 
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
import { Workbook } from '@syncfusion/ej2-excel-export'; 
Vue.use(GridPlugin);

export default{
    components: {
        TsMessageBox,
    },  
    methods:{
        onActionFailure(args) {
            var errorMessage = args.error[0].error.responseText.split("Exception:")[1].split('<br>')[0];  //extract the message from args 
            this.$vs.notify({color:'error',title:'Grid loading Error',text: errorMessage})
        },
        formatDateTime(column, dataObject) {
            return this.getLocaleDateTime(dataObject.uploadedDateTime);
        },
        onToolbarClick: function (args) {
            switch (args.item.text) {
                case 'PDF Export':
                    this.$refs.grid.pdfExport();
                    break;
                case 'Excel Export':
                    this.$refs.grid.excelExport(this.getExcelExportProperties(), true).then(function (workbook) {
                        workbook.worksheets[0].name = "TSA Schedule";
                        var book = new Workbook(workbook, 'xlsx');
                        var filename = `tsa_schedule_${(this.getShortDate(new Date)).replace(/-/g, '.')}.xlsx`;
                        
                        book.save(filename);
                    }.bind(this));

                    break;
            }
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
                                { index: 1, colSpan: 2, value: 'TSA SCHEDULE REPORT', style: { fontColor: '#662d91', fontSize: 20, hAlign: 'Left', bold: true } },
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
                //fileName: `tsa_schedule_${(this.getShortDate(new Date)).replace(/-/g, '.')}.xlsx`
            }
        }, 
        excelQueryCellInfo(args) { 
            if (args.column.field === 'startDate' || args.column.field === 'originalEndDate' || args.column.field === 'activeEndDate') { 
                if (args.value) {
                    args.value = this.getShortDate((new Date(args.value)));
                }
            }  
    
        }                     
    },
    provide: {
        grid: [Filter, Page, Selection, Sort, Resize, ColumnChooser, Toolbar, ExcelExport]
    },  
    data: function() {
        return {
            errorContent: '',
            data: {},            
            pageSettings: { pageSizes: [20,50,100,200], pageSize: 20, pageCount: 5 },
            filterSettings: { type: 'Excel' },
            toolbar: ['ColumnChooser', 'ExcelExport'],
            columns: [
                { field: 'tsaId', headerText: 'TSA ID', width: 130 },
                { field: 'tsaSubId', headerText: 'TSA Sub-ID', width: 130 },
                { field: 'status', headerText: 'Status', width: 130 },
                { field: 'function', headerText: 'Function', width: 130 },
                { field: 'subFunction', headerText: 'Sub-Function', width: 130 },
                { field: 'serviceName', headerText: 'Service Name', width: 130 },
                { field: 'providerOwner', headerText: 'Provider Owner', width: 130 },
                { field: 'receiverOwner', headerText: 'Receiver Owner', width: 130 },
                { field: 'serviceExceptions', headerText: 'Service Exceptions', width: 130 },
                { field: 'serviceLocation', headerText: 'Service Location', width: 130 },
                { field: 'estimatedDurationText', headerText: 'Estimated Duration (Months)', width: 130 },
                { field: 'assumptions', headerText: 'Assumptions', width: 130 },
                { field: 'exceptionsToMonthlyInvoice', headerText: 'Exceptions to monthly invoicing', width: 130 },
                { field: 'exceptionsTo30DayPayment', headerText: 'Exceptions to 30 day payment terms', width: 130 },
                { field: 'noticeOfTerminationText', headerText: 'Notice of termination', width: 130 },
                { field: 'exceptionsReOwnershipIp', headerText: 'Exceptions regarding ownership of Intellectual Property', width: 130 },
                { field: 'tsaExitApproach', headerText: 'TSA Exit Approach', width: 130 },
                { field: 'knownTsaExitCosts', headerText: 'Known TSA Exit Costs', width: 130 },
                { field: 'knownDependencies', headerText: 'Known Interdependencies', width: 130 },
                { field: 'detailedServiceDescription', headerText: 'Detailed Service Description', width: 130 },
                { field: 'tsaVsRtsa', headerText: 'TSA vs RTSA', width: 130 },
                { field: 'unitDescription', headerText: 'Unit Description', width: 130 },
                { field: 'numOfUnits', headerText: '# of Units', width: 130 },
                { field: 'pricePerUnit', headerText: 'Price / Unit', width: 130 },
                { field: 'monthlyPricing', headerText: 'Monthly Pricing', width: 130 },
                { field: 'usd', headerText: 'USD', width: 130 },
                { field: 'inr', headerText: 'INR', width: 130 },
                { field: 'cad', headerText: 'CAD', width: 130 },
                { field: 'costType', headerText: 'Cost Type', width: 130 },
                { field: 'additionalPricingComments', headerText: 'Additional Pricing Comments', width: 130 },
                { field: 'taxDetermination', headerText: 'Tax Determination', width: 130 },
                //{ field: 'estimatedDuration', headerText: 'Estimated Duration (Number of Months)', width: 130 },
                //{ field: 'noticeOfTermination', headerText: 'Notice of termination (Number of Days)', width: 130 },
                { field: 'optionFlag', headerText: 'Option (Y/N)', width: 130 },
                { field: 'maxOptionDuration', headerText: 'Maximum Option Duration (Months)', width: 130 },
                { field: 'optionExercisedFlag', headerText: 'Option Exercised (Y/N/TBD)', width: 130 },
                { field: 'activeOptionDurationExercised', headerText: 'Active Option Duration Exercised (Months)', width: 130 },
                { field: 'exitCostAmount', headerText: 'Exit Cost Amount', width: 130 },
                { field: 'changeLogType', headerText: 'Change Log Type', width: 130 },
                { field: 'monthlyCostForecast', headerText: 'Monthly Cost Forecast', width: 130 },
                { field: 'startDate', headerText: 'Start Date', width: 130, type: 'date', format: 'yyyy-MM-dd' },
                { field: 'originalEndDate', headerText: 'Original End Date', width: 130, type: 'date', format: 'yyyy-MM-dd' },
                { field: 'activeEndDate', headerText: 'Active End Date', width: 130, type: 'date', format: 'yyyy-MM-dd' },
                { field: 'separationOrStandalone', headerText: 'Separation Or Standalone' }
            ],
        }
    },
    mounted() {
        let serviceUri = `${process.env.VUE_APP_SERVICE_API_URL}/api/data/tsa-schedules`;
        this.$http.get(serviceUri)
            .then((response) => { 
                if (response.data.data == null && response.data.errors.length > 0) {
                    this.$vs.notify({color:'error',title:'Data Loading Error',text: response.data.errors[0]})
                }
                else {
                    this.data = response.data.data }
                }
            )
            .catch((error) => { console.log(error) })
    }      
}
</script>
