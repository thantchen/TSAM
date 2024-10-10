<!-- =========================================================================================
    File Name: Checkbox.vue
    Description: Checkbox Element - Imports all page portions.
    ----------------------------------------------------------------------------------------
    Item Name: Vuexy - Vuejs, HTML & Laravel Admin Dashboard Template
      Author: Pixinvent
    Author URL: http://www.themeforest.net/user/pixinvent
========================================================================================== -->


<template>
    <div id="seller-notification">
        <div class="vx-row">
            <div class="vx-col w-full">
                <vx-card>
                    <vs-tabs>
                        <vs-tab label="Change Log" v-on:click="onTabClick1">
                            <iframe frameBorder="0" v-if="ticket1 != ''" :src="reportUrl1"></iframe>
                        </vs-tab>
                        <vs-tab label="Dispute Log" v-on:click="onTabClick2">
                            <iframe frameBorder="0" v-if="ticket2 != ''" :src="reportUrl2"></iframe>
                        </vs-tab>                    
                    </vs-tabs>
                </vx-card>
            </div>
        </div> 
    </div>
</template>

<script>
import Vue from 'vue'
Vue.loadScript("https://tableau.ankura.com/javascripts/api/viz_v1.js")

export default{
  data() {
    return {
        ticket1: '',
        ticket2: '',
    }
  },
  methods: {
      async onTabClick1() {
        this.ticket1 = "";
        this.ticket1 = await this.generateTrustedTicket();
      },
      async onTabClick2() {
        this.ticket2 = "";
        this.ticket2 = await this.generateTrustedTicket();
      },              
  },
  computed :{
    reportUrl1 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket1 + process.env.VUE_APP_REPORT_SITE + '/SellSideNotif_Changes?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&:origin=viz_share_link&:refresh=yes&:display_spinner=yes&:toolbar=no&tabs-no';
    },
    reportUrl2 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket2 + process.env.VUE_APP_REPORT_SITE + '/SellSideNotif_Disputes?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&:origin=viz_share_link&:refresh=yes&:display_spinner=yes&:toolbar=no&tabs-no';
    },      
  },  
  async created() {
        this.ticket1 = await this.generateTrustedTicket();
  },
}
</script>

<style scoped>
  html, body { height: 100% }
  iframe { 
      width: 95vw;
      height: 100vh;
      max-width: 100%;
      max-height: 100%;
      overflow: scroll;
      -webkit-overflow-scrolling: touch;
  }
</style>