const { forEach, push } = require("core-js/fn/array");
const { log10 } = require("core-js/fn/number");
const { MessageSquareIcon } = require("vue-feather-icons");

<!-- =========================================================================================

========================================================================================== -->


<template>
    <div id="generate-data">
        <vx-card>
            <div class="centerx">
                <div class="fill-row">
                  <vs-table :data="data" noDataText="">
                    <vs-tr>
                      <vs-td style="width: 280px; vertical-align: top">
                        <vs-button @click="openLoadingInDiv" type="relief" vslor="primary">Generate Data for Reporting</vs-button>
                      </vs-td>
                      <vs-td>
                        <vs-table :data="data" noDataText="">
                          <template slot-scope="{data}">
                            <vs-tr :key="indextr" v-for="(tr, indextr) in data" style='line-height: 1'>
                              <vs-td :data="data[indextr].timestamp" style='width:210px'>
                                {{data[indextr].timestamp}}
                              </vs-td>

                              <vs-td :data="data[indextr].message">
                                {{data[indextr].message}}
                              </vs-td>
                            </vs-tr>
                          </template>
                        </vs-table>                
                      </vs-td>
                    </vs-tr>
                  </vs-table>
                </div>
            </div>




        </vx-card>
    </div>
</template>

<script>
export default {
  data(){
    return {
        data: [],
        messages: [
            'Initiating data transformation process...',
            'Reading data...',
            'Merging data from views...',
            'Processing and transforming data...', 
            'Writing data to staging tables...', 
            'Cleaning up data...', 
            'Data transformation completed successfully...'
        ],
        interval: 0,
    }
  },
  methods:{
    openLoadingInDiv(){
      this.data = [];
      this.$vs.loading({
        container: '.fill-row',
        background: 'rgba(255, 255, 255,.8)',
        scale: 0.6,
        text: 'Transforming data for reports...'
      })

      var index = 0;
      this.interval = setInterval(() => {
        console.log(this.messages[index]);
        this.data.push({
          timestamp: (new Date).toLocaleDateString("en-US", { year: "numeric", month: "2-digit", day: "2-digit" }) + " " + (new Date).toLocaleString([], { hour: '2-digit', minute: '2-digit', second: '2-digit' }),
          message: this.messages[index]
        });            
        index++;
        if (index === this.messages.length) {
          clearInterval(this.interval);
          this.$vs.loading.close('.fill-row > .con-vs-loading');
          this.$vs.notify({ title: 'Data Transformaton', text: 'Generating data for reporting completed successfully.', color: 'success', position: 'top-center' })             
        }
      }, 1000 * Math.floor(Math.random() * Math.floor(5)));
    },
  }
}
</script>

<style>
.fill-row {
  flex: 0 0 100%;
  margin-top: 20px;
  margin-bottom: 20px;
  height: 320px;
}

.vs-con-table .vs-con-tbody {
    background: #f8f8f8;
    border: 1px solid #f8f8f8;
}
</style>
