<template>
    <div id="user-management">
        <div class="vx-row" style="margin-top: 10px">
            <div class="vx-col w-full">
                <vx-card title="User Management">
                    <ejs-grid ref='grid' :dataSource="data" :allowSorting='true' :allowFiltering='true' :allowResizing='true' :filterSettings='filterSettings' :allowPaging='true' :pageSettings='pageSettings' 
                                :actionFailure='onActionFailure' :editSettings='editSettings' :toolbar='toolbar'
                                :actionBegin="onActionBegin" :actionComplete = 'onActionComplete'>
                        <e-columns>
                            <e-column field='id' headerText='ID' width="75"></e-column>
                            <e-column field='userName' headerText='USER NAME'></e-column>
                            <e-column field='firstName' headerText='FIRST NAME'></e-column>
                            <e-column field='lastName' headerText='LAST NAME'></e-column>
                            <e-column field='email' headerText='EMAIL'></e-column>
                            <e-column field='isActive' headerText='IS ACTIVE' width="100" :template='statusTemplate' :allowFiltering="false"></e-column>
                            <e-column field='role.name' headerText='ROLE'></e-column>
                        </e-columns>                                
                    </ejs-grid>
                </vx-card>
            </div>   
        </div> 
    </div>
</template>

<script>

import TsMessageBox from "@/components/ts-message-box/TsMessageBox.vue"

import Vue from "vue";
import { GridPlugin, Filter, Page, Selection, Sort, Resize, Toolbar, Edit } from "@syncfusion/ej2-vue-grids";
Vue.use(GridPlugin);

export default{
    components: {
        TsMessageBox,
    },  
    methods:{
        onActionFailure(args) {
            var errorMessage = args.error.message;  //extract the message from args 
            this.$vs.notify({color:'error',title:'Grid loading Error',text: errorMessage})
        },
        onActionBegin (args) {
            if (args.requestType === 'save') {
                args.data['role'] = this.roles.find(item => item.name == args.data['role']); 
                // Somehow isActive value is not automatically set in checkbox even though it's binded
                args.data['isActive'] = args.form.querySelector("input.vs-checkbox--input").value === 'true' ? true : false;
                args.data['disabled'] = args.form.querySelector("input.vs-checkbox--input").value === 'true' ? false : true;
            }
        },        
        async onActionComplete(args) {
            if ((args.requestType === 'beginEdit' || args.requestType === 'add')) {
                args.form.ej2_instances[0].addRules('firstName', { required: true });
                args.form.ej2_instances[0].addRules('lastName', { required: true });
                args.form.ej2_instances[0].addRules('userName', { required: true });
                args.form.ej2_instances[0].addRules('email', { required: true });
                //args.form.ej2_instances[0].addRules('role', { required: true });
                //args.form.ej2_instances[0].addRules('disabled', { required: true });
                
                let dialog = args.dialog;
                // change the header of the dialog
                dialog.header = args.requestType === 'beginEdit' ? args.rowData['firstName'] + ' ' + args.rowData['lastName'] : 'New User';
            }

            try {
                if (args.action && args.action === "edit") {
                    await this.$http.put(`${process.env.VUE_APP_SERVICE_API_URL}/api/settings/user`, args.data)
                    this.$vs.notify({color: 'success', title: 'Update Success', text: 'User updated successfully.'});
                }
                else if (args.action && args.action === "add") {
                    let response = await this.$http.post(`${process.env.VUE_APP_SERVICE_API_URL}/api/settings/user`, args.data);

                    args.data.id = response.data.data.id;
                    this.$refs.grid.refresh();

                    this.$vs.notify({color: 'success', title: 'Add Success', text: 'User added successfully.'});
                }
            }
            catch (error) {
                this.$vs.notify({color: 'error', title: 'Error',text: error})
                this.alert("Error", error);
            }
        }        
    },
    provide: {
        grid: [Filter, Page, Selection, Sort, Resize, Toolbar, Edit]
    },  
    data: function() {
        return {
            errorContent: '',
            data: {}, 
            roles: {},
            pageSettings: { pageSizes: [20,50,100,200], pageSize: 20, pageCount: 5 },
            filterSettings: { type: 'Excel' },
            statusTemplate: function () {
                    return {
                        template: Vue.component('statusTemplate', {
                            template: `<div id="status" :class="{'statustemp': true, 'e-activecolor': data.isActive == true, 'e-inactivecolor': data.isActive == false}">\
                                                  <span :class="{'statustxt': true, 'e-activecolor': data.isActive == true, 'e-inactivecolor': data.isActive == false}">{{data.isActive ? "Active" : "Inactive"}}</span></div>`,
                            data: function () { return { data: {} }; }                       
                        })
                    }
                }, 
            toolbar: ['Add', 'Edit'],  
            editSettings: { allowEditing: true, allowAdding: true, mode: 'Dialog', template: function () {
                    return {
                        template: Vue.component('userForm', {
                            template: 
                                `
                                <div id="dialog-container" class="vs-con-loading__container" style="width: 600px" >
                                    <vs-row>
                                        <vs-col vs-type="flex" vs-w="6">
                                            <vs-input name="firstName" v-model="data.firstName" class="col1" label="First Name" placeholder="First Name"/>                  
                                        </vs-col>                    
                                        <vs-col vs-type="flex" vs-w="6">
                                            <vs-input name="lastName" v-model="data.lastName" class="col2" label="Last Name" placeholder="Last Name"/>                  
                                        </vs-col>                  
                                    </vs-row>                             
                                    <vs-row>
                                        <vs-col vs-type="flex" vs-w="6">
                                            <vs-input name="userName" v-model="data.userName" class="col1" label="User Name" placeholder="User Name"/>                  
                                        </vs-col>                    
                                        <vs-col vs-type="flex" vs-w="6">
                                            <vs-input name="email" v-model="data.email" class="col2" label="Email" placeholder="Email"/>                  
                                        </vs-col>                  
                                    </vs-row>
                                    <vs-row>
                                        <vs-col vs-type="flex" vs-w="6">
                                            <vs-select ref="role" autocomplete name="role" class="col1" label="Role" v-model="data.role.id">
                                                <vs-select-item :key="item.id" :value="item.id" :text="item.name" v-for="(item,index) in roleOptions" />
                                            </vs-select>             
                                        </vs-col>                    
                                        <vs-col vs-type="flex" vs-w="6">
                                            <vs-checkbox v-model="data.isActive" class="col2">Is Active</vs-checkbox>                 
                                        </vs-col> 
                                    </vs-row>
                                </div>                                    
                                `,
                            data: function () { 
                                return { 
                                    data: {},
                                    roleOptions: [],
                                }; 
                            },
                            async mounted() {
                                this.roleOptions = await this.httpGet('/api/lookup/roles');
                                if (this.data.id === undefined) {
                                    this.data.role.id = 4; // Default to primaryOwner role
                                    this.data.isActive = true;
                                }
                            } 
                        })
                    }
                }
            },
        }
    },
    async mounted() {
        this.data = await this.httpGet('/api/settings/users');
        this.roles = await this.httpGet('/api/lookup/roles');
    }      
}
</script>

<style>
.statustemp.e-activecolor {
    background-color: #ccffcc;
    width: 57px;
}
.e-control, .e-control [class^='e-'], .e-control [class*=' e-'] {
    box-sizing: border-box;
}
.statustemp {
    position: relative;
    height: 19px;
    border-radius: 5px;
    text-align: center;
}
.statustemp.e-inactivecolor {
    background-color: #ffd7cc;
    width: 64px;
}
#overviewgrid td.e-rowcell .statustxt.e-activecolor {
    color: #00cc00;
    position: relative;
    top: 9px;
}
.statustxt.e-activecolor {
    color: #00cc00;
}
#overviewgrid td.e-rowcell .statustxt.e-inactivecolor {
    color: #e60000;
    position: relative;
    top: 9px;
}
.statustxt.e-inactivecolor {
    color: #e60000;
}

.col1 { width: 95%; margin-right: 5%; }
.col2 { width: 95%; margin-left: 5%; }
</style>