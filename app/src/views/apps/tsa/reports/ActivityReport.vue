<template>
    <div id="activity-report">
        <div class="vx-row">
            <div class="vx-col w-full">
                <vx-card title="Search Criteria" :collapse-action="true" class="vs-con-loading__container">
                    <vs-divider style="margin-top: -20px"/>
                    <vs-row>
                        <vs-col vs-w="1" style="width:100px">
                            <label for="chip-filter" class="vs-select--label">Log Types:</label>
                        </vs-col>
                        <vs-col vs-w="11">
                            <ejs-chiplist id="chip-filter" selection="Multiple" :selectedChips='selectedLogTypes' @click="onLogTypeSelected">
                                <e-chips>
                                    <e-chip cssClass="e-primary" :key="index" :text="item.text" v-for="(item,index) in logTypes" />
                                </e-chips>
                            </ejs-chiplist>  
                        </vs-col>                        
                    </vs-row>   
                    <vs-row>
                        <vs-col vs-w="1" style="width: 100px">
                            <label class="vs-select--label">Date Range:</label>
                        </vs-col>                        
                        <vs-col vs-w="11">
                            <span style="display: inline-block">
                                <el-date-picker id="fromDate" v-model="fromDate" type="date" placeholder="From date"></el-date-picker>                         
                            </span>
                            <span style="display: inline-block; margin-left: 10px">
                                <el-date-picker id="toDate" v-model="toDate" type="date" placeholder="To date"></el-date-picker>   
                            </span>
                        </vs-col>                        
                    </vs-row>
                    <vs-row>
                        <vs-col vs-w="1" style="width: 100px">
                            <label class="vs-select--label">Log Statuses:</label>
                        </vs-col>                        
                        <vs-col vs-w="11">
                            <ejs-multiselect id='logStatuses' ref='logStatuses' :dataSource='logStatuses' mode='Box' placeholder='Select log status' :fields="fields" v-model='selectedLogStatuses' cssClass="multiselect-chip-custom"></ejs-multiselect>
                        </vs-col>                        
                    </vs-row>   
                    <vs-row style="margin-top: 4px; margin-bottom: 0px">
                        <vs-col vs-w="1">
                            <vs-button color="primary" type="filled" icon="search" @click="onSearch">Search</vs-button>
                        </vs-col>                        
                 
                    </vs-row>
                </vx-card>
            </div>
        </div>
        <div class="vx-row" style="margin-top: 10px">
            <div class="vx-col w-full">
                <vx-card title="Activity Report">
                    <ejs-grid ref='grid' :dataSource="data" :allowSorting='true' :allowFiltering='true' :allowResizing='true' :filterSettings='filterSettings' :allowPaging='true' :pageSettings='pageSettings' 
                                :columns='columns'  :showColumnChooser='true' :toolbar="toolbar" :actionFailure='onActionFailure' 
                                 :allowExcelExport='true' :allowPdfExport='true' :toolbarClick='onToolbarClick' :excelQueryCellInfo="excelQueryCellInfo">
                    </ejs-grid>
                </vx-card>
            </div>   
        </div> 
    </div>
</template>

<script>

import TsMessageBox from "@/components/ts-message-box/TsMessageBox.vue"

import Vue from "vue";
import { MultiSelectComponent, MultiSelectPlugin } from '@syncfusion/ej2-vue-dropdowns';
import { GridPlugin, Filter, Page, Selection, Sort, Resize, ColumnChooser, Toolbar, PdfExport, ExcelExport } from "@syncfusion/ej2-vue-grids";
import { ChipListPlugin } from "@syncfusion/ej2-vue-buttons";
Vue.component(MultiSelectPlugin.name, MultiSelectComponent);
Vue.use(GridPlugin);
Vue.use(ChipListPlugin);


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
        onLogTypeSelected: function(e) {
            if (e.selected) {
                var selected = this.logTypes[e.index];
                var logStatuses = this.logTypeStatuses.filter(item => { return item.id == selected.id}).map(m => { return m.logStatusId; });
                logStatuses.forEach(item => this.selectedLogStatuses.push(item));

                this.selectedLogTypes.push(e.index);
            }
            else {
                var index = this.selectedLogTypes.indexOf(e.index);
                this.selectedLogTypes.splice(index, 1);
            }
        },
        async onSearch() {
            var params = "logStatuses=" + this.selectedLogStatuses.join(",");

            if (this.fromDate) {
                params = params + "&fromDate=" + this.getShortDate(this.fromDate);
                params = params + "&toDate=" + (this.toDate ? this.getShortDate(this.toDate) : this.getShortDate(new Date));
            }

            this.$vs.loading({ container: '.vs-con-loading__container', scale: 0.6 });
            this.data = [];
            this.data = await this.httpGet("/api/data/activity-report?" + params);
            this.$vs.loading.close('.vs-con-loading__container > .con-vs-loading');
        },
        onToolbarClick: function (args) {
            switch (args.item.text) {
                case 'PDF Export':
                    this.$refs.grid.pdfExport();
                    break;
                case 'Excel Export':
                    this.$refs.grid.excelExport(this.getExcelExportProperties());
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
                                { index: 1, colSpan: 2, value: 'ACTIVITY REPORT', style: { fontColor: '#662d91', fontSize: 20, hAlign: 'Left', bold: true } },
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
                footer: {
                    footerRows: 4,
                    rows: [
                        {
                            index: 1,
                            cells: [
                                { index: 1, value: 'Search Criteria', style: { fontColor: '#626262', fontSize: 18, hAlign: 'Left', bold: true } }
                            ]
                        },
                        {
                            index: 2,
                            cells: [
                                { index: 1, value: 'Log Types:', style: { fontColor: '#626262', hAlign: 'Left', bold: true } },
                                { index: 2, colSpan: 16, value: this.selectedLogTypesText, style: { fontColor: '#626262', hAlign: 'Left' } },
                            ]
                        },
                        {
                            index: 3,
                            cells: [
                                /* tslint:disable-next-line:max-line-length */
                                { index: 1, value: 'Date Range:', style: { fontColor: '#626262', hAlign: 'Left', bold: true } },
                                { index: 2, colSpan: 3, value: this.selectedDateRangeText, style: { fontColor: '#626262', hAlign: 'Left' } },
                            ]
                        },
                        {
                            index: 4,
                            cells: [
                                /* tslint:disable-next-line:max-line-length */
                                { index: 1, value: 'Log Statuses:', style: { fontColor: '#626262', hAlign: 'Left', bold: true } },
                                { index: 2, colSpan: 16, value: this.selectedLogStatusText, style: { fontColor: '#626262', hAlign: 'Left' } },
                            ]
                        },                             
                    ]
                },     
                fileName: `activity_report_${(this.getShortDate(new Date)).replace(/-/g, '.')}.xlsx`
            }
        },
        excelQueryCellInfo(args) { 
            if (args.column.field === 'createdDate' || args.column.field === 'effectiveDate' || args.column.field === 'invoicePeriodEndDate') { 
                if (args.value) {
                    args.value = this.getShortDate((new Date(args.value)));
                }
            }  
    
        }      
    },
    provide: {
        grid: [Filter, Page, Selection, Sort, Resize, ColumnChooser, Toolbar, PdfExport, ExcelExport]
    },  
    data: function() {
        return {
            errorContent: '',
            data: {},            
            pageSettings: { pageSizes: [20,50,100,200], pageSize: 20, pageCount: 5 },
            filterSettings: { type: 'Excel' },
            toolbar: ['ColumnChooser', 'ExcelExport'],
            columns: [
                { field: 'id', headerText: 'ID', width: 90 },
                { field: 'createdDate', headerText: 'CREATED DATE', width: 120, type: 'date', format: 'yyyy-MM-dd' },
                { field: 'tsaId', headerText: 'TSA ID', width: 100 },
                { field: 'tsaSubId', headerText: 'TSA SUB ID', width: 130 },
                { field: 'function', headerText: 'FUNCTION', width: 130 },
                { field: 'primaryOwner', headerText: 'PRIMARY OWNER', width: 130 },
                { field: 'serviceName', headerText: 'SERVICE NAME', width: 130 },
                { field: 'effectiveDate', headerText: 'EFFECTIVE DATE', width: 120, type: 'date', format: 'yyyy-MM-dd' },
                { field: 'logType', headerText: 'LOG TYPE', width: 130 },
                { field: 'logStatus', headerText: 'LOG STATUS', width: 130 },
                { field: 'daysOpen', headerText: 'DAYS OPEN', width: 120 },
                { field: 'comments', headerText: 'COMMENTS', width: 130 },
                { field: 'submittedBy', headerText: 'SUBMITTED BY', width: 130 },
                { field: 'invoicePeriodEndDate', headerText: 'INVOICE PERIOD END DATE', width: 130, type: 'date', format: 'yyyy-MM-dd' },
                { field: 'invoiceNumber', headerText: 'INVOICE NUMBER', width: 130 },
                { field: 'invoiceAmount', headerText: 'INVOICE AMOUNT', width: 130 },
                { field: 'agreedCost', headerText: 'AGREED COST', width: 130 },
            ],
            selectedLogTypes: [0,1,2,3,4,5],
            selectedLogStatuses: [],
            fromDate: '',
            toDate: '',
            logTypeStatuses: [],
            logTypes: [],
            logStatuses: [],
            fields: { text: 'logStatusName', value: 'logStatusId' },
            closePopupOnSelect: false,
        }
    },
    async mounted() {
        this.logTypeStatuses = await this.httpGet("/api/lookup/log-type-status");

        var logTypes = this.logTypeStatuses.map((m) => {
            return { id: m.id, text: m.description }
        });
        // Get distinct log types
        this.logTypes = logTypes.filter((item, index, selfArray) =>
            index === selfArray.findIndex((p) => (
                p.id === item.id && p.description === item.description
            ))
            
        );

        // Set all log types as default. Have to set the array index number.
        //this.selectedLogTypes = this.logTypes.map((val, index) => index);

        // Set all statuses as default
        this.selectedLogStatuses = this.logTypeStatuses.map(m => { return m.logStatusId; }); 
        
        this.onSearch();
    },
    computed: {
        selectedLogTypeIds: function() {
            if (this.selectedLogTypes && this.logTypes && this.logTypes.length > 0) {
                var items = [];
                this.selectedLogTypes.forEach(index => items.push(this.logTypes[index]));

                return items.map(m => m.id);
            }
            else {
                return [];
            }
        },
        selectedLogTypesText: function() {
            return this.logTypes.filter((item, index) => { 
                return this.selectedLogTypes.findIndex(i => i == index) >= 0 }).map(m => m.text).join("; ");
        },
        selectedLogStatusText: function() {
            return this.logStatuses.filter((item) => { 
                return this.selectedLogStatuses.findIndex(i => i == item.logStatusId) >= 0 }).map(m => m.logStatusName).join("; ");
        },
        selectedDateRangeText: function() {
            var dateRangeText = "";
            if (this.fromDate || this.toDate) {
                dateRangeText = `${this.fromDate ? this.getShortDate(this.fromDate) : ""} to ${this.toDate ? this.getShortDate(this.toDate) : ""}`;
            }
            return dateRangeText;
        },
    },
    watch: {
        selectedLogTypeIds: function(newValue) {
            // Filter log status option list for log type ID (A, E, S, M, C, D)
            this.logStatuses = this.logTypeStatuses.filter(item => {
                return (newValue.findIndex((id) => { return item.id == id })) >= 0;
            });
            
            // Get list of currently selected log status corresponding to deselected log type
            var removed = [];
            this.selectedLogStatuses.forEach(logStatusId => {
                var index = this.logStatuses.findIndex(item => {
                    return item.logStatusId == logStatusId;
                });
                if (index < 0) {
                    removed.push(logStatusId);
                }
            });
            if (removed.length > 0) {
                // Remove currently selected log status if their corresponding log type is deselected
                removed.forEach(logStatusId => {
                    var index = this.selectedLogStatuses.findIndex(id => {
                        return id == logStatusId;
                    });
                    this.selectedLogStatuses.splice(index, 1);     
                });

                // Have to explicity call these statements to clear all selection
                if (this.selectedLogStatuses.length == 0) {
                    //this.$refs.logStatuses.value = [];
                    this.$refs.logStatuses.selectAll(false);
                    this.selectedLogStatuses = [];
                }
            }
        }
    }
}
</script>

<style scoped>
.vs-row {
    line-height: 38px;
}
</style>
