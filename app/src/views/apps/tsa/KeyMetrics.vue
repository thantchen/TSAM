
<!--
  Pie chart https://jsfiddle.net/3umqape1/
  green #8DC63F
blue #1D75BC
-->
<template>
  <div id="dashboard">
<!--             <p>Your current role is <strong>{{ $acl.get[0] }}</strong>.</p>
            <div class="demo-alignment mb-base">
                <vs-radio v-model="userRole" vs-value="primaryOwner">primaryOwner</vs-radio>
                <vs-radio v-model="userRole" vs-value="personnel">personnel</vs-radio>
                <vs-radio v-model="userRole" vs-value="tsaTeam">tsaTeam</vs-radio>
                <vs-radio v-model="userRole" vs-value="admin">admin</vs-radio>
            </div>  -->   
    <div class="vx-row">
      <div class="vx-col w-full lg:w-1/2 mb-base">
        <vx-card title="TSA Tracker">
          <img src='/tsa-tracker.png' />
        </vx-card>
      </div>
      <div class="vx-col w-full lg:w-1/2 mb-base">
        <vx-card :title='title' slot="no-body" class="text-center">
          <img src='/tsa-cost-summary.png' style="width: 81% !important; " />
        </vx-card>
      </div>      
    </div>
    <div class="vx-row">
      <div class="vx-col w-full lg:w mb-base">
        <vx-card title='TSA Change Log Summary to Date' slot="no-body" class="text-center">
          <img src='/tsa-change-log-summary.png' style="width: 90% !important; height: 90% !important" />
        </vx-card>
      </div>
    </div>
    <div class="vx-row">
      <div class="vx-col w-full lg:w mb-base">
        <vx-card :title='billingTitle' slot="no-body" class="text-center">
          <img src='/tsa-billing-dispute-summary.png' style="width: 90% !important; height: 90% !important"/>
        </vx-card>  
      </div>
    </div>
    <div class="vx-row" v-if="$acl.check('tsaTeam')">
      <div class="vx-col w-full lg:w mb-base">
        <vx-card title="Input Stream History Log">
          <div class="vs-con-tbody vs-table--tbody">
            <div slot="no-body" class="mt-4">
                <vs-table :data="data" stripe  class="table-dark-inverted">
                    <template slot="thead">
                        <vs-th>INPUT STREAM</vs-th>
                        <vs-th>FILE NAME</vs-th>
                        <vs-th>UPLOADED BY</vs-th>
                        <vs-th>COUNT</vs-th>
                        <vs-th>LAST UPLOADED</vs-th>
                    </template>
                    <template slot-scope="{data}">
                        <vs-tr :key="indextr" v-for="(tr, indextr) in data">
                            <vs-td :data="data[indextr].fileTypeName">
                                <span>{{data[indextr].fileTypeName}}</span>
                            </vs-td>
                            <vs-td :data="data[indextr].fileName">
                                <span>{{data[indextr].fileName}}</span>
                            </vs-td>
                            <vs-td :data="data[indextr].uploadedBy">
                                <span>{{data[indextr].uploadedBy}}</span>
                            </vs-td>   
                            <vs-td :data="data[indextr].totalCount">
                                <span>{{data[indextr].totalCount == 0 ? "" : data[indextr].totalCount}}</span>
                            </vs-td>
                            <vs-td :data="data[indextr].uploadedDate">
                                <span>{{getLocaleDateTime(data[indextr].uploadedDate)}}</span>
                            </vs-td>                                                                                                                                    
                        </vs-tr>
                    </template>
                </vs-table>
            </div>
          </div>
        </vx-card>
      </div>
    
    </div>
  </div>
</template>

<script>

import Vue from "vue";
import { GridPlugin } from "@syncfusion/ej2-vue-grids";
//import axios from 'axios'

Vue.use(GridPlugin);

export default {
  data: function() {
    const datetime = (new Date).toLocaleDateString("en-US", { // you can skip the first argument
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
    });

    return {
      title: 'TSA Cost Summary as of ' + datetime,
      billingTitle: 'TSA Billing Dispute Summary - Current Month',
      data: [],
      userRole: this.$acl.get[0]
    }
  },
  methods: {
    formatDateTime(column, dataObject) {
        return this.getLocaleDateTime(dataObject.uploadedDate);
    },  
  },
  computed: {

  },
  async mounted() {
    this.data = await this.httpGet("/api/input/input-stream-summary");

    // const instance = axios.create({
    //   withCredentials: false,
    //   headers: {
    //     'Access-Control-Allow-Origin' : '*',
    //     'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS',
    //     }
    // });

    // const response = await this.$http.post("https://tableau.ankura.com/vizportal/api/web/v1/generatePublicKey", { "method": "generatePublicKey","params":{}}, { headers: { 'Access-Control-Allow-Origin': '*', 'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,PATCH,OPTIONS'} });

    // console.log(response);

  },
}
</script>

<style scoped>
img {
  max-width: 100%;
  max-height: 100%;
}


</style>

