<!-- =========================================================================================

========================================================================================== -->


<template>
    <div id="sell-side-notification-disputes">
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
                    <ejs-grid ref='grid' :dataSource="data" :allowSorting='true' :allowFiltering='true' :allowResizing='true' :filterSettings='filterSettings' :allowPaging='true' :pageSettings='pageSettings' :actionFailure='onActionFailure'>
                        <e-columns>
                            <e-column field='id' headerText='ID' width='40' isPrimaryKey='true'></e-column>
                            <e-column field='uploadedDateTime' headerText='DATE/TIME' width='100' :formatter='formatDateTime'></e-column>
                            <e-column field='fileName' headerText='FILE NAME' width='100'></e-column>
                            <e-column field='uploadedBy' headerText='UPLOADED BY' width='100'></e-column>
                            <e-column field='id' headerText='DOWNLOAD' width='50' textAlign='Center' :allowSorting='false' :allowFiltering='false' :template='downloadTemplate'></e-column>
                            <!--e-column headerText='VIEWS DETAIL' width='50' textAlign='Center' :allowSorting='false' :allowFiltering='false' :template='viewTemplate'></e-column-->
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
                    id: data.data.fileRepositoryId,
                    uploadedDateTime: data.data.uploadedDate,
                    fileName: data.data.file,
                    uploadedBy: data.data.uploadedBy
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
        }, 
        onActionFailure(args) {
            var errorMessage = args.error[0].error.responseText.split("Exception:")[1].split('<br>')[0];  //extract the message from args 
            this.$vs.notify({color:'error',title:'Grid loading Error',text: errorMessage})
        },
        formatDateTime(column, dataObject) {
            return this.getLocaleDateTime(dataObject.uploadedDateTime);
        }
    },   
    provide: {
        grid: [Filter, Page, Selection, Sort, Resize]
    },      
    data: function() {
        return {
            errorContent: '',
            dialogVisible: false,            
            data: {}, 
            uploadUrl: `${process.env.VUE_APP_SERVICE_API_URL}/api/upload/sell-side-notification-disputes`,
            dateTimeFormat: 'yyyy-MM-dd hh:mm tt',
            pageSettings: { pageSizes: [20,50,100,200], pageSize: 10, pageCount: 5 },
            filterSettings: { type: 'Excel' },
            downloadTemplate: function () {
                return {
                    template: Vue.component('downloadTemplate', {
                        template: `<ts-download :location="this.serviceApiUrl"></ts-download>`,
                        components: { TsDownload },                          
                        data: function () { return { data: {} }; }, 
                        computed: {
                            serviceApiUrl() {
                                return `${process.env.VUE_APP_SERVICE_API_URL}/api/download/file/` + this.data.id;
                            }
                        }
                    })
                }
            }, 
            // viewTemplate: function () {
            //     return {
            //         template: Vue.component('viewTemplate', {
            //             template: `<ts-redirect :location="{ name: 'tsa-schedules-details' }"></ts-redirect>`,
            //             components: { TsRedirect }, 
            //             data: function () { return { data: {} }; },                     
            //         })
            //     }
            // },                          
        }
    },
    created() {
        let serviceUri = `${process.env.VUE_APP_SERVICE_API_URL}/api/file-repository/file-type/9`;
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
