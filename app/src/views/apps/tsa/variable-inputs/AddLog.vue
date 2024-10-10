<!-- =========================================================================================

========================================================================================== -->


<template>
    <div id="Add-log">
        <div class="vx-row" style="margin-top: 10px">
            <div class="vx-col w-full">
                <vx-card title="Add New">
                    <div class="add_button"><vs-button color="primary" type="border" size="small" style="margin-bottom: 10px; margin-left: -10px" icon="add_box" @click="openRequestLogDialog">Submit New Request</vs-button></div>     

                    <ejs-grid ref='grid' :dataSource="data" :allowSorting='true' :allowFiltering='true' :allowResizing='true' :filterSettings='filterSettings' :allowPaging='true' :pageSettings='pageSettings'>
                        <e-columns>
                            <e-column field='addLogId' headerText='ID' isPrimaryKey='true' :template='commandTemplate'></e-column>
                            <e-column field='tsaId' headerText='TSA ID'></e-column>
                            <e-column field='tsaSubIds' headerText='TSA SUB ID' ></e-column>
                            <e-column field='changeDate' headerText='CHANGE DATE/TIME' :formatter='formatDateTime'></e-column>
                            <e-column field='addTypeText' headerText='ADD TYPE'></e-column>
                            <e-column field='changeLogTypeText' headerText='LOG TYPE'></e-column>
                            <e-column field='changeLogStatusText' headerText='STATUS'></e-column>
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
            <div id="dialog-container" class="vs-con-loading__container" >
                <vs-row>
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-select autocomplete style="width: 100%; margin-right: 16px" :disabled="!isNewSubmission"
                            label="Type of Add *" 
                            v-model="logData.selectedAddType">
                            <vs-select-item :key="index" :value="item.id" :text="item.name" v-for="(item,index) in typeOfAddOptions" />
                        </vs-select>                  
                    </vs-col>                    
                    <vs-col vs-type="flex" vs-w="4">
                        <div class="vs-component" style="width: 100%; margin-right: 16px">
                            <label for="tsaId" class="vs-select--label">TSA ID *</label>
                            <v-select id="tsaId" v-model="logData.selectedTsaId" :options="tsaIdOptions" label="tsaId" :placeholder="tsaPlaceholder" 
                                :disabled="!isEditable || (isNewSubmission && logData.selectedAddType == 1) || (!isNewSubmission && logData.selectedAddType == 2) || 
                                            (!isNewSubmission && logData.selectedAddType == 3) || (!isNewSubmission && logData.selectedStatus == 13)"/>
                        </div>                        
                    </vs-col>
                    <vs-col vs-type="flex" vs-w="4" >
                        <div class="vs-component" style="width: 100%;">
                            <label for="tsaSubIds" class="vs-select--label">TSA Sub ID *</label>
                            <v-select id="tsaSubIds" :multiple='logData.selectedAddType == 3 ? false : true' :closeOnSelect="logData.selectedAddType == 3 ? true : false" 
                                      v-model="logData.selectedTsaSubId" :options="tsaSubIdOptions" label="tsaSubId" :placeholder="subTsaPlaceholder" 
                                      :disabled="!isEditable || (isNewSubmission && (logData.selectedAddType == 1 || logData.selectedAddType == 2)) ||
                                                 (!isNewSubmission && logData.selectedAddType == 3) || (!isNewSubmission && logData.selectedStatus == 13)"/>
                        </div>
                    </vs-col>                    
                </vs-row>
                <vs-row>
                    <vs-col vs-type="flex" vs-w="4">
                        <div class="vs-component vs-con-input-label vs-select vs-select-primary">
                            <label for="" class="vs-select--label">Change Date *</label>
                            <div class="vs-con-select">
                                <el-date-picker id="changeLogDate" :disabled="!isNewSubmission"
                                    v-model="logData.changeLogDate"
                                    type="date"
                                    placeholder="Pick a day">
                                </el-date-picker>
                            </div>
                        </div>
                    </vs-col>                    
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-select autocomplete style="width: 100%; margin-right: 16px" :disabled="!isNewSubmission"
                            label="Change Type *"
                            v-model="logData.selectedChangeLogType">
                            <vs-select-item :key="index" :value="item.id" :text="item.name" v-for="(item, index) in logTypeOptions" />
                        </vs-select>
                    </vs-col>
                    <vs-col vs-type="flex" vs-w="4">
                        <vs-select autocomplete style="width: 100%;" :disabled="!isEditable"
                            label="Status *"
                            v-model="logData.selectedStatus">
                            <vs-select-item :key="index" :value="item.id" :text="item.name" v-for="(item,index) in statusOptions" />
                        </vs-select>
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
                            <vs-input v-model="logData.submittedBy" id="submittedBy" readonly style="width: 100%;" :disabled="!isNewSubmission"/>
                        </div>
                    </vs-col>
                    <vs-col vs-type="flex" vs-w="4" v-if="logData.addId != 0">
                        <div class="vs-component" style="width: 100%;">
                            <label for="addId" class="vs-input--label">Add ID</label>
                            <vs-input v-model="logData.addId" id="addId" readonly style="width: 100%;" :disabled="!isNewSubmission"/>
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
                            <label for="comments" class="vs-select--label">Comments * <span v-if="isNewSubmission">(Please provide a short explanation of your change request.)</span></label>
                            <vs-textarea v-model="logData.comments" id="comments" :disabled="!isEditable" :label="commentsLabel"/>
                        </div> 
                    </vs-col>
                </vs-row>
                <vs-row>
                    <label for="uploadAttachements" class="vs-select--label" style="margin-top: -12px; ">Attachment(s) {{isFileAttachmentRequired}}</label>
                    <div style="max-height: 100px; overflow-y: auto; width: 100%;">
                        <vs-col v-if="isNewSubmission" vs-type="flex" vs-w="12">
                            <div class="vs-component" style="width: 500px" id="uploadContainer">
                                <!-- <label for="uploadAttachements" class="vs-select--label">Attachment(s) {{isFileAttachmentRequired}}</label> -->
                                <ejs-uploader ref="uploadAttachements" id='uploadAttachements' name="files" 
                                            :autoUpload="autoUpload" :asyncSettings="uploadAttachmentsPath" :multiple="multipleUpload" :sequentialUpload='sequentialUpload'
                                            :success="onUploadSuccess" :uploading='onUploading' :actionComplete="onUploadActionComplete" :selected="onUploadSelected">
                                </ejs-uploader>
                            </div>
                        </vs-col>
                        <vs-col v-else vs-type="flex" vs-w="12">
                            <div class="vs-component" style="width: 500px;">
                                <!-- <label for="uploadAttachements" class="vs-select--label">Attachment(s)</label> -->
                                <vs-list>
                                    <vs-list-item icon-pack="feather" icon="icon-file" 
                                                :key="index" v-for="(item, index) in logData.attachments"
                                                :subtitle="item.file">
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
            logTypeOptions: [],
            staticLogTypes: [],
            statusOptions: [],
            staticStatusOptions: [],
            logData: {},
            userOptions: [],
            tsaLookupData: [],
            tsaIdOptions: [],
            tsaSubIdOptions: [],
            uploadAttachmentsPath: {
                saveUrl: `${process.env.VUE_APP_SERVICE_API_URL}/api/upload/add-log-attachments`,
            },
            cancelText: 'Cancel',
            downloadAttachmentsPath: `${process.env.VUE_APP_SERVICE_API_URL}/api/download/file/`,
            autoUpload: false,
            multipleUpload: true,
            sequentialUpload: false,
            pageSettings: { pageSizes: [20,50,100,200], pageSize: 20, pageCount: 5 },
            filterSettings: { type: 'Excel',
                              columns: [ { field: 'changeLogStatusText', operator: 'contains', value: 'Pending' } ]
            },  
            commandTemplate: function () {
                return {
                    template: Vue.component('anchorTemplate', {
                        template:  '<a href="#" @click="onClick()">{{data.addId}}</a>',
                        data() { return { data: { data: {} } }; },
                        methods: {
                            onClick: function() {
                                this.$eventHub.$emit('openRequestLogDialog', this.data.addLogId);
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
            typeOfAddOptions: [],
            maxCommentsLength: 4000,
        }
    },
    methods:{
        initiateSubmit() {
            // By default, submit will close the dialog. We need to reset "isDialogVisible" to true to keep the dialog open 
            // so we can handle change log submission and uploads events. We will set "isDialogVisible" to false after handling all events.
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
                addLogId: this.logData.addLogId,
                tsaId: this.logData.selectedTsaId == "" ? null : this.logData.selectedTsaId,
                tsaSubId: this.logData.selectedTsaSubId.length == 0 ? null : (Array.isArray(this.logData.selectedTsaSubId) ? this.logData.selectedTsaSubId[0] : this.logData.selectedTsaSubId),
                tsaSubIds: this.logData.selectedTsaSubId.length == 0 ? null : (Array.isArray(this.logData.selectedTsaSubId) ? this.logData.selectedTsaSubId.join(",") : this.logData.selectedTsaSubId),
                changeDate: this.logData.changeLogDate,
                changeLogTypeId: this.logData.selectedChangeLogType,
                changeLogStatusId: this.logData.selectedStatus,
                requestedByUserId: this.logData.selectedRequestedBy,
                submittedByUserId: this.userOptions.find(m => m.email == this.logData.submittedBy).id,
                comments: this.logData.comments,
                addTypeId: this.logData.selectedAddType
            } 

            try {
                var isNewRecord = false;
                var attachmentCount = this.$refs.uploadAttachements ? this.$refs.uploadAttachements.getFilesData().length : 0;
                
                // Save change log data first
                let response = await this.$http.put(`${process.env.VUE_APP_SERVICE_API_URL}/api/input/add-log`, formData);
                response.data.data.forEach((item) => {
                    var index = this.data.findIndex(log => log.addLogId == item.addLogId);

                    if (index == -1) {          // Adding new record
                        item.attachmentCount = attachmentCount;
                        this.data.unshift(item);
                        isNewRecord = true;
                    }
                    else {                      // Updating existing record
                        item.attachmentCount = this.data[index].attachmentCount;
                        this.data[index] = item;
                    }
                });

                this.$refs.grid.refresh();

                this.logData.addLogIds = response.data.data.map(m => m.addLogId);

                if (isNewRecord && attachmentCount == 0) {  // If attachmentCount > 0, we'll send email after all attachment(s) finish uploading and then send email
                    // Call Add New API
                    this.sendEmail("newAddRequest", this.logData.addLogIds);
                }
                else if (isNewRecord == false ) {
                    // Call Add Update API
                    this.sendEmail("updatedAddRequest", this.logData.addLogIds);
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
                addLogId: 0,
                selectedTsaId: '',
                selectedTsaSubId: [],
                addId: '',
                changeLogDate: new Date(),
                selectedChangeLogType: '',
                selectedStatus: '',
                selectedRequestedBy: '',
                submittedBy: this.$store.state.AppActiveUser.email,
                comments: '',
                originalComments: '',
                addLogIds: [],
                attachments: [],
                selectedAddType: null,
            };

            this.isInitialPendingStatus = false;
            this.attachmentFileCount = 0;
        },
        async loadLogData(log) {
            this.logData.addLogId = log.addLogId;
            this.logData.selectedTsaId = log.tsaId;

            this.logData.addId = log.addId;
            this.logData.selectedAddType = log.addTypeId;
            this.logData.changeLogDate = log.changeDate;
            this.logData.selectedChangeLogType = log.changeLogTypeId;
            this.logData.selectedStatus = log.changeLogStatusId;
            this.logData.selectedRequestedBy = log.requestedByUserId;
            this.logData.submittedBy = log.submittedByUserText;
            
            // This has to be called after setting "selectedtatus" and before setting "comments"
            this.isInitialPendingStatus = this.selectedStatus.toLowerCase().includes("pending") ? true : false;
            
            this.logData.comments = !this.isEditable ? log.comments : ''; // Non-pending which mean it has already been confirmed or rejected which is final status, so no more edit.
            this.logData.originalComments = log.comments;
            this.logData.addLogIds = log.addLogIds;
            this.logData.attachments = log.attachments;

            if (this.logData.attachments == undefined) {
                this.logData.attachments = await this.httpGet(`/api/input/add-log/${this.logData.addLogId}/attachments`);
                
                var log1 = this.findLog(this.logData.addLogId);
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

            this.filterLogStatus(log.changeLogTypeId);
            this.filterLogType(log.addTypeId);
            //this.filterTsaSubId(log.tsaId);       
            
            // Select Multiple selection is in array format, single is in String format.
            if (this.logData.selectedAddType == 3 /*Service Request*/) { // Service Request allows only single selection
                this.logData.selectedTsaSubId = log.tsaSubIds;
            }
            else {
                if (log.tsaSubIds) {
                    log.tsaSubIds.split(",").forEach(item => {
                       this.logData.selectedTsaSubId.push(item);
                    });
                } 
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
            this.data = await this.httpGet(`/api/input/add-log`);
        },
        async getLookupData(apiEndpoint) {
            return this.httpGet(`${apiEndpoint}`);
        },
        findLog(logId) {
            return this.data.find(item => item.addLogId == logId)
        },
        openRequestLogDialog(logId) {
            this.isDialogVisible = true;

            if (isNaN(logId)) {
                this.isNewSubmission = true;
                this.resetLogData();
            }
            else {
                this.isNewSubmission = false;
                this.resetLogData();

                var id = parseInt(logId);
                this.loadLogData(this.findLog(id));
            }
        },
        onUploadSuccess: function(args) {
            const file = JSON.parse(args.e.target.response);
            if (file) {
                this.logData.addLogIds.forEach(function(logId) {
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
                await this.sendEmail("newAddRequest", this.logData.addLogIds);

                this.finalizeSubmit();          
            }
        },
        onUploading: function(args) {
            args.currentRequest.withCredentials = true;
            args.customFormData = [ { 'logIds': this.logData.addLogIds } ];
        },
        onUploadSelected: function(args) {
            // CSS style scoped ".e-upload-actions { display: none }" do not apply to dynamic content, so this is a workaround;
            // Will have to watch "attachmentFileCount" to hide the action buttons.
            this.attachmentFileCount = this.$refs.uploadAttachements.getFilesData().length + args.filesData.length;
        },        
        formatDateTime(column, dataObject) {
            return this.getLocaleDateTime(dataObject.createdDate);
        }, 
        formatComments(column, dataObject) {
            return this.convertComments(dataObject.comments);
        },
        filterLogStatus: function(logTypeId) {
            // Filter statuses by selected log type
            if (this.isNewSubmission) {
                this.statusOptions = this.staticStatusOptions.filter(function(item) {
                    return (item.referenceId == logTypeId || item.referenceId === null) && item.name.toLowerCase().includes("pending") ? true : false;
                });                

                if (this.statusOptions && this.statusOptions.length == 1) {
                    this.logData.selectedStatus = this.statusOptions[0].id;
                }
                else {
                    // Clear currently selected status
                    this.logData.selectedStatus = "";
                }
            }
            else {
                this.statusOptions = this.staticStatusOptions.filter(function(item) {
                    return item.referenceId == logTypeId || item.referenceId === null;
                });                   
            }            
        },
        filterLogType: function(addTypeId) {
            if (addTypeId == 1 || addTypeId == 2) {  // TSA / TSA Sub ID
                this.logTypeOptions = this.staticLogTypes.filter(function(item) {
                    return item.name.toLowerCase() == "add";
                })
            }
            else if (addTypeId == 3) {  // Service Request
                this.logTypeOptions = this.staticLogTypes.filter(function(item) {
                    return item.name.toLowerCase() == "service request";
                })                    
            }                   
        },
        filterTsaSubId: function(tsaId) {
            // Filter TSA Sub ID by TSA ID
            this.tsaSubIdOptions = this.tsaLookupData.filter(function(item) {
                return item.tsaId == tsaId;
            }).map(m => m.tsaSubId);   
            
            if (this.logData.selectedAddType == 1) {
                if (this.tsaSubIdOptions.length === 1) {
                    this.logData.selectedTsaSubId = [this.tsaSubIdOptions[0]];
                }
                else {
                    this.logData.selectedTsaSubId = [];
                }
            }
            else if (this.logData.selectedAddType == 3) {
                if (tsaId == null) {
                    this.logData.selectedTsaSubId = [];
                }
                else
                {
                    if (this.tsaSubIdOptions.length === 1) {
                        this.logData.selectedTsaSubId = [this.tsaSubIdOptions[0]];
                    }
                    else {
                        if (!this.tsaSubIdOptions.includes(this.logData.selectedTsaSubId))
                        {
                            this.logData.selectedTsaSubId = [];
                        }
                    }
                }
            }
        }
    },  
    computed: {
        dialogTitle: function() {
            return "Add New Request" + (this.isNewSubmission ? " Form" : "");
        },
        validate: function() {
            return this.logData.selectedChangeLogType !== "" &&
                   this.logData.changeLogDate !== "" &&
                   this.logData.selectedRequestedBy !== "" &&
                   this.logData.selectedAddType != null &&
                   
                   (this.logData.selectedAddType == 1 /*Add a New TSA*/ ||
                    (this.logData.selectedAddType == 2 /*Add a New Sub-TSA*/ && (this.logData.selectedTsaId != "" && this.logData.selectedTsaId != null)) ||
                    (this.logData.selectedAddType == 3 /*Add a New Service Request*/ && (this.logData.selectedTsaId != "" && this.logData.selectedTsaSubId.length >= 1))) &&
                   (this.isNewSubmission || ((!this.isNewSubmission && this.logData.selectedTsaId != "" && (this.logData.selectedStatus == 2 /*Add Confirmed*/ || this.logData.selectedStatus == 12 /*Service Request Resolved*/)) ||
                                             (!this.isNewSubmission && this.logData.selectedStatus == 13 /*Add Rejected*/))) &&
                   (this.isNewSubmission || ((!this.isNewSubmission && this.logData.selectedTsaSubId.length >= 1 && (this.logData.selectedStatus == 2 /*Add Confirmed*/ || this.logData.selectedStatus == 12 /*Service Request Resolved*/)) ||
                                             (!this.isNewSubmission && this.logData.selectedStatus == 13 /*Add Rejected*/))) &&
                   this.logData.selectedStatus != "" && 
                   this.logData.comments != "" &&
                   (this.selectedLogType.toLowerCase() === "cancellation" ? (this.$refs.uploadAttachements && this.$refs.uploadAttachements.getFilesData().length > 0) : true);
        },
        isFileAttachmentRequired: function() {
            return (this.selectedLogType.toLowerCase() === "cancellation") ? " *" : "";
        },
        selectedLogType: function() {
            var logType = this.logTypeOptions.find(item => item.id == this.logData.selectedChangeLogType);
            return logType ? logType.name : "";
        },
        selectedStatus: function() {
            var logStatue = this.staticStatusOptions.find(item => item.id == this.logData.selectedStatus);
            return logStatue ? logStatue.name : "";
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
                    if (this.selectedStatus.toLowerCase().includes("confirmed") || this.selectedStatus.toLowerCase().includes("resolved") || this.selectedStatus.toLowerCase().includes("rejected")) {
                        editable = false;
                    }
                    else {
                        editable = this.selectedStatus.toLowerCase().includes("pending") && this.$acl.check('tsaTeam');
                    }
                }
            }

            return editable;
        },
        tsaPlaceholder: function() {
            var placeholder = "";
            if (this.logData.selectedAddType) {
                if (this.logData.selectedAddType == 1) {
                    placeholder = "NEW";
                }
                else {
                    placeholder = "Select a TSA"
                }
            }
            return placeholder;
        },
        subTsaPlaceholder: function() {
            var placeholder = "";
            if (this.logData.selectedTsaId) {
                if (this.logData.selectedAddType == 3) {
                    placeholder = "Select a TSA Sub ID";
                }
                else {
                    placeholder = "TBD";
                }
            }
            return placeholder;
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
        'logData.selectedChangeLogType': function(logTypeId) {
            this.filterLogStatus(logTypeId);
        },
        'logData.selectedAddType': function(newAddTypeId/*, oldAddTypeId*/) {
            this.filterLogType(newAddTypeId);    

            if (this.isNewSubmission) {
                this.logData.selectedTsaId = '';

                // if (((newAddTypeId == 1 || newAddTypeId == 2) && oldAddTypeId == 3) ||
                //     (newAddTypeId == 3 && (oldAddTypeId == 1 || oldAddTypeId == 2))) {
                //     this.logData.selectedChangeLogType = '';
                //     this.logData.selectedStatus = '';
                // }

                if (this.logTypeOptions && this.logTypeOptions.length == 1) {
                    this.logData.selectedChangeLogType = this.logTypeOptions[0].id;
                }                  
            }
        },        
        'logData.selectedTsaId': async function(tsaId) {
            if (!this.isNewSubmission || this.logData.selectedAddType == 3 /*Service Request*/) {
                // Filter TSA Sub ID by TSA ID
                this.filterTsaSubId(tsaId);
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
        },     
    },
    async mounted() {
        await this.loadTSAs();
        await this.loadLogs();

        this.staticStatusOptions = await this.getLookupData("/api/lookup/log-statuses");
        this.staticLogTypes = await this.getLookupData("/api/lookup/log-types");
        this.typeOfAddOptions = await this.getLookupData("/api/lookup/add-types");
        this.userOptions = await this.getLookupData("/api/lookup/users");

        this.$eventHub.$on('openRequestLogDialog', this.openRequestLogDialog);
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

