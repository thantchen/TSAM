<!-- =========================================================================================

========================================================================================== -->


<template>
    <div id="dispute-log">
        <div class="vx-row">
            <div class="vx-col w-full">
                <vx-card title="File Upload">
                    <div class="mb-8">
                        <ts-uploader :onSuccess="onUploadSuccess" :onFailed="onUploadFailed" :url="uploadUrl" />
                    </div>    
                </vx-card>
            </div>
        </div> 
        <div class="vx-row" style="margin-top: 10px">
            <div class="vx-col w-full">
                <vx-card title="Upload History">
                    <ejs-grid ref='grid' :dataSource="data" :allowSorting='true' :allowFiltering='true' :allowResizing='true' :filterSettings='filterSettings' :allowPaging='true' :pageSettings='pageSettings'>
                        <e-columns>
                            <e-column field='DateTime' headerText='DATE/TIME' format='MM/dd/yyyy hh:mm a' width='100'></e-column>
                            <e-column field='Filename' headerText='FILE NAME' width='100'></e-column>
                            <e-column field='UploadedBy' headerText='UPLOADED BY' width='100'></e-column>
                            <e-column field='Filename' headerText='DOWNLOAD' width='50' textAlign='Center' :allowSorting='false' :allowFiltering='false' :template='downloadTemplate'></e-column>
                            <e-column headerText='VIEWS DETAIL' width='50' textAlign='Center' :allowSorting='false' :allowFiltering='false' :template='viewTemplate'></e-column>
                        </e-columns>
                    </ejs-grid>
                </vx-card>
            </div>   
        </div>     
        <ts-message-box :title="'Error Message'" :content="errorContent" :visible="dialogVisible" @close="closeDialog()"></ts-message-box>

    </div>
</template>

<script>

import TsUploader from "@/components/ts-uploader/TsUploader.vue"
import TsRedirect from "@/components/ts-redirect/TsRedirect.vue"
import TsDownload from "@/components/ts-download/TsDownload.vue"
import TsMessageBox from "@/components/ts-message-box/TsMessageBox.vue"

import Vue from "vue";
import { GridPlugin, Filter, Page, Selection, Sort, Resize } from "@syncfusion/ej2-vue-grids";
Vue.use(GridPlugin);

export default{
    components: {
        TsUploader,
        TsRedirect,
        TsDownload,
        TsMessageBox,
    },  
    methods:{
        openDialog: function(content) {
            this.errorContent = content;
            this.dialogVisible = true;
        },
        closeDialog: function() {
            //this.dialogVisible = false;
        },        
        onUploadSuccess(event, file) {
            if (event && event.returnValue) {
                var data = JSON.parse(event.target.response);
                let uploadedData = {
                    Id: data.id,
                    DateTime: this.getLocaleDateTime(data.uploadedDate),
                    Filename: data.file,
                    UploadedBy: data.uploadedBy
                };

                this.data.unshift(uploadedData); 
                this.data = [...this.data]; // Refresh the Grid dataSource.      
            }
            // Hide Delete icon after successful upload
            document.querySelector('.e-file-delete-btn').style = "display: none";
            
            this.$vs.notify({color:'success',title:'Upload Success',text:'You have successfully upload "' + file.name + '" file.'})
        },
        onUploadFailed(arg) {
            if (arg.name) {
                this.$vs.notify({color:'error',title:'Upload Failed',text:'You have failed to upload "' + arg.name + '" file.'});
            }
            
            var errorMessage = "";
            try {
                var data = JSON.parse(event.target.response);
                if (data.errors && data.errors.length > 0) {
                    errorMessage = data.errors.join('<br/>')
                }
            }
            catch {
                errorMessage = event.target.responseText != "" ? event.target.responseText : event.target.status + " status error";
            }
            this.openDialog(errorMessage);
        }  
    },  
    provide: {
        grid: [Filter, Page, Selection, Sort, Resize]
    },       
    data: function() {
        return {
            errorContent: '',
            dialogVisible: false,            
            data: [
                {
                    Id: 2,
                    DateTime: '07/20/2020 05:50 PM PDT',
                    Filename: 'DisputeLog.xlsx',
                    UploadedBy: 'jsmith@ankura.com'
                },
                {
                    Id: 2,
                    DateTime: '07/19/2020 05:51 PM PDT',
                    Filename: 'DisputeLog.xlsx',
                    UploadedBy: 'jsmith@ankura.com'
                },
                {
                    Id: 3,
                    DateTime: '07/18/2020 05:45 PM PDT',
                    Filename: 'DisputeLog.xlsx',
                    UploadedBy: 'jsmith@ankura.com'
                },
                {
                    Id: 4,
                    DateTime: '07/16/2020 05:55 PM PDT',
                    Filename: 'DisputeLog.xlsx',
                    UploadedBy: 'jsmith@ankura.com'
                },
                {
                    Id: 5,
                    DateTime: '07/15/2020 05:52 PM PDT',
                    Filename: 'DisputeLog.xlsx',
                    UploadedBy: 'jsmith@ankura.com'
                },
                {
                    Id: 6,
                    DateTime: '07/14/2020 05:49 PM PDT',
                    Filename: 'DisputeLog.xlsx',
                    UploadedBy: 'jsmith@ankura.com'
                }                                                                                      
            ],
            uploadUrl: "https://localhost:44353/api/upload/dispute-requests",
            pageSettings: { pageSizes: [20,50,100,200], pageSize: 10, pageCount: 5 },
            filterSettings: { type: 'Excel' },
            downloadTemplate: function () {
                return {
                    template: Vue.component('downloadTemplate', {
                        template: `<ts-download :location="'https://localhost:44353/api/download/file/' + data.Id"></ts-download>`,
                        components: { TsDownload },                          
                        data: function () { return { data: {} }; },                     
                    })
                }
            }, 
            viewTemplate: function () {
                return {
                    template: Vue.component('viewTemplate', {
                        template: `<ts-redirect :location="{ name: 'dispute-log-details' }"></ts-redirect>`,
                        components: { TsRedirect },                          
                        data: function () { return { data: {} }; },                     
                    })
                }
            },             
        }
    }
}
</script>
