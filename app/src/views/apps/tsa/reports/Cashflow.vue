<!-- =========================================================================================

========================================================================================== -->


<template>
    <div id="cashflow">
        <div class="vx-row">
            <div class="vx-col w-full">
                <vx-card title="File Upload">
                    <div class="mb-8">
                        <import-excel :onSuccess="successUpload" />
                    </div>    
                </vx-card>
            </div>
        </div> 

        <div class="vx-row" style="margin-top: 10px">
            <div class="vx-col w-full">
                <vx-card title="Upload History">
                    <div slot="no-body" class="mt-4">
                        <vs-table :data="data" max-items="5" pagination class="table-dark-inverted">
                            <template slot="thead">
                                <vs-th>DATE/TIME</vs-th>
                                <vs-th>FILE NAME</vs-th>
                                <vs-th>UPLOADED BY</vs-th>
                                <vs-th>DOWNLOAD</vs-th>
                                <vs-th>DETAILS</vs-th>
                            </template>
                            <template slot-scope="{data}">
                                <vs-tr :key="indextr" v-for="(tr, indextr) in data">
                                    <vs-td :data="data[indextr].DateTime">
                                        <span>{{data[indextr].DateTime}}</span>
                                    </vs-td>
                                    <vs-td :data="data[indextr].Filename">
                                        <span>{{data[indextr].Filename}}</span>
                                    </vs-td>
                                    <vs-td :data="data[indextr].UploadedBy">
                                        <span>{{data[indextr].UploadedBy}}</span>
                                    </vs-td>   
                                    <vs-td>
                                        <a href="#"><img src="/document-excel-icon.png" width="24px" title="Download" /></a>
                                    </vs-td>    
                                    <vs-td>
                                    <router-link v-bind:to="'cashflow-details'"><img src="/view_details.png" width="24px" title="View Details" /></router-link>
                                    </vs-td>                                                                                                                                           
                                </vs-tr>
                            </template>

                        </vs-table>
                    </div>

                </vx-card>
            </div>   
        </div>          
    </div>
</template>

<script>

import ImportExcel from "@/components/excel/ImportExcel.vue"

export default{
    components: {
        ImportExcel
    },  
    methods:{
        successUpload({meta}){
            this.data.unshift({
                DateTime: (new Date).toLocaleDateString("en-US", { year: "numeric", month: "2-digit", day: "2-digit" }) + " " + (new Date).toLocaleString([], { hour: '2-digit', minute: '2-digit' }),
                Filename: meta.fileName,
                UploadedBy: "John"
            });

            this.$vs.notify({color:'success',title:'Upload Success',text:'You have successfully upload "' + meta.fileName + '" file.'})
        }
    },    
    data: function() {
        return {
            data: [
                {
                    DateTime: '07/20/2020 05:50 PM',
                    Filename: 'Cashflow.xlsx',
                    UploadedBy: 'tchen'
                },
                {
                    DateTime: '07/19/2020 05:51 PM',
                    Filename: 'Cashflow.xlsx',
                    UploadedBy: 'tchen'
                },
                {
                    DateTime: '07/18/2020 05:45 PM',
                    Filename: 'Cashflow.xlsx',
                    UploadedBy: 'tchen'
                },
                {
                    DateTime: '07/16/2020 05:55 PM',
                    Filename: 'Cashflow.xlsx',
                    UploadedBy: 'tchen'
                },
                {
                    DateTime: '07/15/2020 05:52 PM',
                    Filename: 'Cashflow.xlsx',
                    UploadedBy: 'tchen'
                },
                {
                    DateTime: '07/14/2020 05:49 PM',
                    Filename: 'Cashflow.xlsx',
                    UploadedBy: 'tchen'
                }                                                                                      
            ]
        }
    },
}
</script>
