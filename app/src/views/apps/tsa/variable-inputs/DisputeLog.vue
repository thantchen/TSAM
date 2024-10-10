<!-- =========================================================================================

========================================================================================== -->


<template>
    <div id="dispute-log">
        <div class="vx-row" style="margin-top: 10px">
            <div class="vx-col w-full">
                <vx-card title="Dispute Invoice">
                    <div class="add_button"><vs-button color="primary" type="border" size="small" style="margin-bottom: 10px; margin-left: -10px" icon="add_box" @click="openRequestLogDialog">Submit Dispute</vs-button></div>     

                    <ejs-grid ref='grid' :dataSource="data" :allowSorting='true' :allowFiltering='true' :allowResizing='true' :filterSettings='filterSettings' :allowPaging='true' :pageSettings='pageSettings'>
                        <e-columns>
                            <e-column field='disputeId' headerText='DISPUTE ID' isPrimaryKey='true' :template='commandTemplate'></e-column>
                            <e-column field='tsaId' headerText='TSA ID'></e-column>
                            <e-column field='tsaSubId' headerText='TSA SUB ID' ></e-column>
                            <e-column field='submissionDate' headerText='SUBMISSION DATE/TIME' :formatter='formatDateTime'></e-column>
                            <e-column field='disputeTypeText' headerText='DISPUTE TYPE'></e-column>
                            <e-column field='disputeDiscrepancyText' headerText='DISCREPANCY'></e-column>
                            <e-column field='servicePeriod' headerText='SERVICE PERIOD' :formatter='formatMonthYear'></e-column>
                            <e-column field='requestedByUserText' headerText='REQUESTED BY'></e-column>
                            <e-column field='submittedByUserText' headerText='SUBMITTED BY'></e-column>
                            <e-column field='comments' headerText='COMMENTS' :formatter='formatComments'></e-column>
                            <e-column field='attachmentCount' headerText='ATTACHMENTS' :allowSorting='false' :allowFiltering='false' :template='attachmentTemplate'></e-column>
                        </e-columns>
                    </ejs-grid>
                </vx-card>
            </div>   
        </div>           
        <vs-prompt id="dialog" :active.sync="isDialogVisible" :title="dialogTitle" :is-valid="validate" @accept="submitRequest" @close="onCloseRequest" @cancel="cancelRequest" accept-text="Submit" :cancel-text="cancelText" >
            <div id="dialog-container" class="vs-con-loading__container">
                <vs-row>
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-select autocomplete style="width: 100%; margin-right: 16px" :disabled="!isNewSubmission"
                            label="TSA ID *"
                            v-model="logData.selectedTsaId">
                            <vs-select-item :key="index" :value="item" :text="item" v-for="(item,index) in tsaIdOptions" />
                        </vs-select>
                    </vs-col>
                    <vs-col vs-type="flex" vs-w="8">
                        <div class="vs-component" style="width: 100%;">
                            <label for="tsaSubIds" class="vs-select--label">TSA Sub ID *</label>
                            <v-select id="tsaSubIds" multiple :closeOnSelect="false" :disabled="!isNewSubmission"
                                      v-model="logData.selectedTsaSubId" :options="tsaSubIdOptions" label="tsaSubId" />                        
                        </div>
                    </vs-col>                    
                </vs-row>
               <vs-row>
                    <vs-col vs-type="flex" vs-w="4">
                        <div class="vs-component vs-con-input-label vs-select vs-select-primary">
                            <label for="" class="vs-select--label">Submission Date *</label>
                            <div class="vs-con-select">
                                <el-date-picker id="submissionDate" :disabled="!isNewSubmission"
                                    v-model="logData.submissionDate"
                                    type="date"
                                    placeholder="Pick a day">
                                </el-date-picker>
                            </div>
                        </div>
                    </vs-col>    
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-select autocomplete style="width: 100%; margin-right: 16px" :disabled="!isEditable"
                            label="Dispute Type *"
                            v-model="logData.selectedDisputeTypeId">
                            <vs-select-item :key="index" :value="item.id" :text="item.name" v-for="(item, index) in disputeTypeOptions" />
                        </vs-select>
                    </vs-col>
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-input label="Invoice # *" v-model="logData.invoiceNumber" style="width: 100%;" :disabled="!isNewSubmission"/>
                    </vs-col>
                </vs-row>             
                 <vs-row>
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-select autocomplete style="width: 100%; margin-right: 16px" :disabled="!isNewSubmission"
                            label="Requested By *"
                            v-model="logData.selectedRequestedBy">
                            <vs-select-item :key="index" :value="item.id" :text="item.email" v-for="(item,index) in userOptions" />
                        </vs-select>
                    </vs-col>
                    <vs-col vs-type="flex" vs-w="4">
                        <div class="vs-component" style="width: 100%; margin-right: 16px">
                            <label for="submittedBy" class="vs-input--label">Submitted By</label>
                            <vs-input v-model="logData.submittedBy" id="submittedBy" readonly style="width:100%" :disabled="!isNewSubmission"/>
                        </div>
                    </vs-col>    
                    <vs-col vs-type="flex" vs-w="4" v-if="logData.disputeId != 0">
                        <div class="vs-component" style="width: 100%; margin-right: ">
                            <label for="disputeId" class="vs-input--label">Dispute ID</label>
                            <vs-input v-model="logData.disputeId" id="disputeId" readonly style="width:100%" :disabled="!isNewSubmission"/>
                        </div>
                    </vs-col>                                     
                </vs-row>
                <vs-row>
                    <vs-col vs-type="flex" vs-w="8">
                        <vs-select autocomplete style="width: 100%; margin-right: 16px" :disabled="!isNewSubmission"
                            label="Discrepancy *"
                            v-model="logData.selectedDiscrepancy">
                            <vs-select-item :key="index" :value="item.id" :text="item.name" v-for="(item,index) in discrepancyOptions" />
                        </vs-select>
                    </vs-col>
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-select autocomplete style="width: 100%;" :disabled="!isNewSubmission"
                            label="Service Period *"
                            v-model="logData.selectedServicePeriod">
                            <vs-select-item :key="index" :value="item.value" :text="item.monthYear" v-for="(item,index) in monthYearOptions" />
                        </vs-select>
                    </vs-col>                     
                </vs-row>
                <vs-row v-bind:style="selectedDisputeTypeText.toLowerCase() == 'resolution' ? 'display: block' : 'display: none'">
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-select autocomplete style="width: 100%; margin-right: 16px" :disabled="!isEditable"
                            label="Currency *"
                            v-model="logData.selectedCurrency">
                            <vs-select-item :key="index" :value="item.name" :text="item.name" v-for="(item,index) in currencyOptions" />
                        </vs-select>                
                    </vs-col>
                    <vs-col vs-type="flex" vs-w="4">
                        <div class="vs-component" style="width: 100%; margin-right: 16px">
                            <vs-input v-model="logData.agreedCost" type="number" style="width: 100%;"  label="Agreed Cost ($) *" placeholder="Agreed cost" :disabled="!isEditable"/>
                        </div>
                    </vs-col>                    
                </vs-row>                   
                <vs-row v-if="(isInitialPendingStatus || !isNewSubmission) && isEditable" style="margin-bottom: -10px">
                    <vs-col vs-type="flex" vs-w="12">
                        <div class="vs-component" style="width: 100%;">
                            <label for="originalComments" class="vs-select--label">Previous Comments</label>
                            <vs-textarea v-model="logData.originalComments" id="originalComments" disabled style="opacity: .8"/>
                        </div>                       
                    </vs-col>
                </vs-row>                
                <vs-row>
                    <vs-col vs-type="flex" vs-w="12">
                        <div class="vs-component" style="width: 100%;">
                            <label for="comments" class="vs-select--label">Comments *</label>
                            <vs-textarea v-model="logData.comments" id="comments" :disabled="!isEditable" :label="commentsLabel"/>
                        </div> 
                    </vs-col>
                </vs-row>
                <vs-row>
                    <label for="uploadAttachements" class="vs-select--label" style="margin-top: -12px; ">Attachment(s) {{isFileAttachmentRequired}}</label>
                    <div style="max-height: 100px; overflow-y: auto; width: 100%;">                    
                        <vs-col v-if="isNewSubmission" vs-type="flex" vs-w="12">
                            <div class="vs-component" style="width: 500px;" id="uploadContainer">
                                <ejs-uploader ref="uploadAttachements" id='uploadAttachements' name="files"
                                            :autoUpload="autoUpload" :asyncSettings="uploadAttachmentsPath" :multiple="multipleUpload" :sequentialUpload='sequentialUpload'
                                            :success="onUploadSuccess" :uploading='onUploading' :actionComplete="onUploadActionComplete" :selected="onUploadSelected">
                                </ejs-uploader>
                            </div>
                        </vs-col>
                        <vs-col v-else vs-type="flex" vs-w="12">
                            <div class="vs-component" style="width: 500px;">
                                <vs-list>
                                    <vs-list-item icon-pack="feather" icon="icon-file" 
                                                :key="index" v-for="(item, index) in logData.attachments"
                                                :subtitle="item.file" >
                                        <vs-chip>
                                            <a :href="downloadAttachmentsPath + item.id">Download</a>
                                        </vs-chip>
                                    </vs-list-item>
                                </vs-list>                            
                            </div>
                        </vs-col>
                    </div>
                </vs-row>
            </div>
        </vs-prompt>
    </div>
</template>

<script>

import Vue from "vue";
import vSelect from 'vue-select';
import { UploaderComponent, UploaderPlugin } from '@syncfusion/ej2-vue-inputs';
import moment from "moment"

Vue.component(UploaderPlugin.name, UploaderComponent);

import { GridPlugin, Filter, Page, Selection, Sort, Resize } from "@syncfusion/ej2-vue-grids";
Vue.use(GridPlugin);

export default{
    components: {
        'v-select': vSelect,
    },  
    provide: {
        grid: [Filter, Page, Selection, Sort, Resize]
    },       
    data: function() {
        return {
            isDialogVisible: false,
            isNewSubmission: false,
            data: [],
            disputeTypeOptions: [],
            staticDisputeTypeOptions: [],
            discrepancyOptions: [],
            logData: {},
            userOptions: [],
            tsaLookupData: [],
            tsaIdOptions: [],
            monthYearOptions: [],
            tsaSubIdOptions: [],
            currencyOptions: [],
            uploadAttachmentsPath: {
                saveUrl: `${process.env.VUE_APP_SERVICE_API_URL}/api/upload/dispute-log-attachments`,
            },
            cancelText: 'Cancel',
            downloadAttachmentsPath: `${process.env.VUE_APP_SERVICE_API_URL}/api/download/file/`,
            autoUpload: false,
            multipleUpload: true,
            sequentialUpload: false,
            pageSettings: { pageSizes: [20,50,100,200], pageSize: 20, pageCount: 5 },
            filterSettings: { type: 'Excel',
                              columns: [ { field: 'disputeTypeText', operator: 'contains', value: 'Dispute' } ]
            },  
            commandTemplate: function () {
                return {
                    template: Vue.component('anchorTemplate', {
                        template:  '<a href="#" @click="onClick()">{{data.disputeId}}</a>',
                        data() { return { data: { data: {} } }; },
                        methods: {
                            onClick: function() {
                                this.$eventHub.$emit('openRequestLogDialog', this.data.disputeLogId);
                            }
                        }          
                    })
                }
            }, 
            isInitialPendingStatus: false,
            attachmentFileCount: 0,
            attachmentTemplate: function () {
                return {
                    template: Vue.component('viewTemplate', {
                        template:  `<div v-if='data.attachmentCount > 0'><img style='margin: 0 auto' :src='image' :title='data.attachmentCount + " attachment" + (data.attachmentCount > 1 ? "s" : "")' width='16px'></img></div>`,
                        data: function () { 
                            return { 
                                data: {} ,
                                image: require('@/assets/images/file-icons/attachment.png'),
                            }; 
                        },                     
                    })
                }
            }, 
            maxCommentsLength: 4000,        
        }
    },
    methods:{
        initiateSubmit() {
            // By default, submit will close the dialog. We need to reset "isDialogVisible" to true to keep the dialog open 
            // so we can handle dispute log submission and uploads events. We will set "isDialogVisible" to false after handling all events.
            this.isDialogVisible = true;

            this.startDialogLoading();
        },
        finalizeSubmit() {
            this.stopDialogLoading();
            this.isDialogVisible = false;
        },
        async submitRequest() {
             this.initiateSubmit();

            var formData = {
                disputeLogId: this.logData.disputeLogId,
                tsaId: this.logData.selectedTsaId,
                tsaSubId: this.logData.selectedTsaSubId.join(","),
                disputeId: this.logData.disputeId,
                submissionDate: this.logData.submissionDate,
                disputeTypeId: this.logData.selectedDisputeTypeId,
                invoiceNumber: this.logData.invoiceNumber,
                requestedByUserId: this.logData.selectedRequestedBy,
                submittedByUserId: this.userOptions.find(m => m.email == this.logData.submittedBy).id,
                disputeDiscrepancyId: this.logData.selectedDiscrepancy,
                currency: this.logData.selectedCurrency,
                agreedCost: parseFloat(this.logData.agreedCost) ? parseFloat(this.logData.agreedCost) : null,
                comments: this.logData.comments,
                servicePeriod: this.logData.selectedServicePeriod
            } 

            try {
                var isNewRecord = false;
                var attachmentCount = this.$refs.uploadAttachements ? this.$refs.uploadAttachements.getFilesData().length : 0;
                
                // Save dispute log data first
                let response = await this.$http.put(`${process.env.VUE_APP_SERVICE_API_URL}/api/input/dispute-log`, formData);
                response.data.data.forEach((item) => {
                    var index = this.data.findIndex(log => log.disputeLogId == item.disputeLogId);

                    if (index == -1) {
                        item.attachmentCount = attachmentCount;
                        this.data.unshift(item);
                        isNewRecord = true;
                    }
                    else {
                        item.attachmentCount = this.data[index].attachmentCount;
                        this.data[index] = item;
                    }
                });

                this.$refs.grid.refresh();

                this.logData.disputeLogIds = response.data.data.map(m => m.disputeLogId);

                if (isNewRecord && attachmentCount == 0) {  // If attachmentCount > 0, we'll send email after all attachment(s) finish uploading and then send email
                    // Call Add New API
                    this.sendEmail("newDisputeRequest", this.logData.disputeLogIds);
                }
                else if (isNewRecord == false ) {
                    // Call Add Update API
                    this.sendEmail("updatedDisputeRequest", this.logData.disputeLogIds);
                }

                // Upload if there are attachment files
                if (attachmentCount > 0) {
                    this.$refs.uploadAttachements.upload();
                }
                else {
                    this.finalizeSubmit();
                }
            }
            catch (error) {
                this.$vs.notify({color: 'error', title: 'Error',text: error})
                this.alert("Error", error, this.finalizeSubmit());
            }
        },
        cancelRequest() {
             this.resetLogData();
        },
        onCloseRequest() {
             this.$refs.grid.refresh();
        },        
        resetLogData() {
            this.logData = {
                disputeLogId: 0,
                selectedTsaId: '',
                selectedTsaSubId: [],
                disputeId: '',
                submissionDate: new Date(),
                selectedDisputeTypeId: null,
                invoiceNumber: '',
                selectedRequestedBy: null,
                submittedBy: this.$store.state.AppActiveUser.email,
                selectedDiscrepancy: null,
                selectedCurrency: null,
                agreedCost: null,
                comments: '',
                originalComments: '',
                disputeLogIds: [],
                attachments: [],
                selectedServicePeriod: null
            };

            if (this.disputeTypeOptions && this.disputeTypeOptions.length == 1) {
                this.logData.selectedDisputeTypeId = this.disputeTypeOptions[0].id;
            }            

            this.isInitialPendingStatus = false;
            this.attachmentFileCount = 0;
        },
        async loadLogData(log) {
            this.logData.disputeLogId = log.disputeLogId;
            this.logData.selectedTsaId = log.tsaId;
            this.logData.selectedTsaSubId.push(log.tsaSubId);
            this.logData.disputeId = log.disputeId;
            this.logData.submissionDate = log.submissionDate;
            this.logData.selectedDisputeTypeId = log.disputeTypeId;
            this.logData.invoiceNumber = log.invoiceNumber;
            this.logData.selectedRequestedBy = log.requestedByUserId;
            this.logData.submittedBy = log.submittedByUserText;

            var dateObject = moment(log.servicePeriod);
            this.logData.selectedServicePeriod = dateObject.format("YYYY-MM-01");

            // This has to be called after setting "selectedDisputeTypeId" and before setting "comments"
            this.isInitialPendingStatus = this.selectedDisputeTypeText.toLowerCase().includes("dispute") ? true : false;        

            this.logData.selectedDiscrepancy = log.disputeDiscrepancyId;
            this.logData.selectedCurrency = log.currency ? log.currency : 'USD';
            this.logData.agreedCost = log.agreedCost;
            this.logData.comments = !this.isEditable ? log.comments : ''; // Non-pending which mean it has already been confirmed or rejected which is final status, so no more edit.
            this.logData.originalComments = log.comments;
            this.logData.disputeLogIds = log.disputeLogIds;
            this.logData.attachments = log.attachments;

            if (this.logData.attachments == undefined) {
                this.logData.attachments = await this.httpGet(`/api/input/dispute-log/${this.logData.disputeLogId}/attachments`);
                
                var log1 = this.findLog(this.logData.disputeLogId);
                log1.attachments = this.logData.attachments;
            }

            if (this.isEditable) {
                this.cancelText = "Cancel";
            }
            else {
                // Hide Submit button
                document.querySelector(".vs-button.vs-button-primary").style = "display: none";

                this.cancelText = "Close";
            }
        },
        async loadTSAs() {
            this.tsaLookupData = await this.httpGet(`/api/lookup/tsa`);

            // Get distint TSA Ids
            if (this.tsaLookupData) {
                this.tsaIdOptions = [...new Set(this.tsaLookupData.map( x => x.tsaId))];
            }
        },
        async loadLogs() {
             this.data = await this.httpGet(`/api/input/dispute-log`);
        },
        async loadMonthYears() {
            var startDate = moment('2020-10-01');
            var endDate = moment('2022-09-30');
            while (startDate.isBefore(endDate)) {
                this.monthYearOptions.push( { value: startDate.format("YYYY-MM-01"), monthYear: startDate.format("MMM YYYY") } );
                startDate.add(1, 'month');
            }            
        },
        async getLookupData(apiEndpoint) {
            return this.httpGet(`${apiEndpoint}`);
        },
        findLog(logId) {
            return this.data.find(item => item.disputeLogId == logId)
        },
        openRequestLogDialog(logId) {
            this.isDialogVisible = true;

            if (isNaN(logId)) {
                this.isNewSubmission = true;
                this.disputeTypeOptions = this.staticDisputeTypeOptions.filter(function(item) {
                    return item.name.toLowerCase() != "resolution";
                })
                this.resetLogData();
            }
            else {
                this.isNewSubmission = false;
                this.disputeTypeOptions = this.staticDisputeTypeOptions;
                this.resetLogData();

                var id = parseInt(logId);
                this.loadLogData(this.findLog(id));
            }
        },
        onUploadSuccess: function(args) {
            const file = JSON.parse(args.e.target.response);
            if (file) {
                this.logData.disputeLogIds.forEach(function(logId) {
                    var log = this.findLog(logId);
                    if (log != null) {
                        if (!log.hasOwnProperty("attachments")) {
                            log["attachments"] = [];
                        }
                        log.attachments.push(file[0]);
                    }
                }, this);
            }
        },
        async onUploadActionComplete(args) {
            // Check for failed uploads
            const failed = args.fileData.filter(function(item) { return item.statusCode == 0; });
            if (failed.length > 0) {
                const errorMessages = failed.map(m => m.name + " - " + m.status).join("\r\n");

                this.alert("Error", errorMessages, this.finalizeSubmit);
            }
            else {
                this.logData.attachments = args.fileData;

                // Send email here; we only allow file attachment for new request
                await this.sendEmail("newDisputeRequest", this.logData.disputeLogIds);

                this.finalizeSubmit();          
            }
        },
        onUploading: function(args) {
            args.currentRequest.withCredentials = true;
            args.customFormData = [ { 'logIds': this.logData.disputeLogIds } ];
        },
        onUploadSelected: function(args) {
            // CSS style scoped ".e-upload-actions { display: none }" do not apply to dynamic content, so this is a workaround;
            // Will have to watch "attachmentFileCount" to hide the action buttons.
            this.attachmentFileCount = this.$refs.uploadAttachements.getFilesData().length + args.filesData.length;

            // Applying ".e-file-size" class to style scoped does not work on dynamic content. Applying globally to style 
            // will affect other component using Uploader. So we will remove File Size explicitly here instead.
            document.querySelectorAll('.e-file-size').forEach(element => element.style = "display: none");
        },
        formatDateTime(column, dataObject) {
            return this.getLocaleDateTime(dataObject.createdDate);
        }, 
        formatComments(column, dataObject) {
            return this.convertComments(dataObject.comments);
        }, 
        formatMonthYear(column, dataObject) {
            if (dataObject.servicePeriod) {
                var dateObject = moment(dataObject.servicePeriod)
                return dateObject.format("MMM YYYY");
            }
            else {
                return "";
            }
        }      
    },  
    computed: {
        dialogTitle: function() {
            return "Dispute Invoice" + (this.isNewSubmission ? " Request Form" : " - " + this.selectedDisputeTypeText);
        },
        validate: function() {
            if (this.logData.selectedTsaId && 
                (this.logData.selectedTsaSubId && this.logData.selectedTsaSubId.length >= 1) && 
                this.logData.submissionDate &&
                this.logData.selectedDisputeTypeId &&
                this.logData.invoiceNumber &&                   
                this.logData.selectedRequestedBy &&
                this.logData.selectedDiscrepancy &&
                this.logData.selectedServicePeriod &&
                this.logData.comments &&
                (this.selectedDisputeTypeText.toLowerCase() === "dispute" ? (this.$refs.uploadAttachements && this.$refs.uploadAttachements.getFilesData().length > 0) : true) &&
                (this.selectedDisputeTypeText.toLowerCase() === "dispute" ? true : (this.logData.selectedCurrency && this.selectedDisputeTypeText.toLowerCase() === "resolution" ? true : false)) &&
                (this.selectedDisputeTypeText.toLowerCase() === "dispute" ? true : (this.logData.agreedCost && this.selectedDisputeTypeText.toLowerCase() === "resolution" ? true : false)))
                return true;
            else 
                return false;
        },
        isFileAttachmentRequired: function() {
            return (this.selectedDisputeTypeText.toLowerCase() === "dispute") ? " *" : "";
        },
        selectedDisputeTypeText: function() {
            var disputeType = this.disputeTypeOptions.find(item => item.id == this.logData.selectedDisputeTypeId);
            return disputeType ? disputeType.name : "";
        },        
        isEditable: function() {
            var editable = false;

            if (this.isNewSubmission) {
                editable = true;
            }
            else {
                if (this.isInitialPendingStatus && this.$acl.check('tsaTeam')) {
                    editable = true;
                }
                else {
                    if (this.selectedDisputeTypeText.toLowerCase().includes("resolution")) {
                        editable = false;
                    }
                    else {
                        editable = this.selectedDisputeTypeText.toLowerCase().includes("dispute") && this.$acl.check('tsaTeam');
                    }
                }
            }

            return editable;
        },
        commentsCharactersRemaining: function() {
            if (this.isEditable) {
                var commentsLength = this.logData.comments ? this.logData.comments.length : 0;
                var originalComments = this.logData.originalComments ? this.logData.originalComments.length : 0;
                if (originalComments > this.maxCommentsLength) {
                    originalComments = this.maxCommentsLength; 
                }

                return this.maxCommentsLength - (commentsLength + originalComments);
            }
            else {
                return 0;
            }
        },
        commentsLabel: function() {
            if (this.isEditable) {
                return `${this.commentsCharactersRemaining} characters remaining`;
            }
            else {
                return "";
            }
        },        
    },
    watch: {
        'logData.selectedTsaId': function(tsaId) {
            if (this.isNewSubmission) {
                // Filter TSA Sub ID by TSA ID
                this.tsaSubIdOptions = this.tsaLookupData.filter(function(item) {
                    return item.tsaId == tsaId;
                }).map(m => m.tsaSubId);

                if (this.tsaSubIdOptions.length === 1) {
                    this.logData.selectedTsaSubId = [this.tsaSubIdOptions[0]];
                }
                else {
                    this.logData.selectedTsaSubId = [];
                }
            }
        },
        attachmentFileCount: function() {
            if (this.attachmentFileCount >= 0) {
                var selector = document.querySelector(".e-upload-actions");
                if (selector) {
                    document.querySelector(".e-upload-actions").style = "display: none";
                }
            }          
        },
        commentsCharactersRemaining: function() {
            if (this.commentsCharactersRemaining < 0) {
                var originalComments = this.logData.originalComments ? this.logData.originalComments.length : 0;
                var commentsAvailLength = this.maxCommentsLength - originalComments;

                // Maximum Comments length reached, truncate to max length
                this.logData.comments = this.logData.comments.substring(0, commentsAvailLength);

                this.alert("Error", `You can only enter a maximum of ${this.maxCommentsLength} characters for Comments.`);
            }
        }           
    },
    async mounted() {
        await this.loadTSAs();
        await this.loadLogs();
        await this.loadMonthYears();

        this.staticDisputeTypeOptions = await this.getLookupData("/api/lookup/dispute-types");
        this.discrepancyOptions = await this.getLookupData("/api/lookup/discrepancies");
        this.userOptions = await this.getLookupData("/api/lookup/users");
        this.currencyOptions = await this.getLookupData("/api/lookup/currencies");

        this.$eventHub.$on('openRequestLogDialog', this.openRequestLogDialog);
    },
    created() {
        
    },
    beforeDestroy() {
        this.$eventHub.$off('openRequestLogDialog');
    }
}
</script>

<style scoped>
.add_button {
    margin-left: 10px;
}
.con-exemple-prompt {
  padding: 10px;
  padding-bottom: 0px;
}
.e-upload.e-control-wrapper.e-lib.e-keyboard /deep/ .e-upload-actions {
    display: none !important;
}
.vs--disabled {
    opacity: .5
}
</style>

<style>
/* Workaround for hiding File Size since scoped style does not work on dynamic content*/
#uploadContainer > div > ul > li > span.e-file-container > .e-file-size {
    display: none;
}
.vs-dialog {
    max-width: 800px !important;
}
.con-input-upload {
    width: 550px;
    height: 100px;
    margin: -2px;
} 

</style>

<style>

li.e-upload-file-list:last-of-type {
    border-bottom: 0px !important;
}
.e-upload-file-list {
    min-height: fit-content !important;
}
.e-file-status {
    display: none !important;
}

</style>

