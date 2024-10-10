<!-- =========================================================================================

========================================================================================== -->

<template>
    <div id="primary-owner">
        <div class="vx-row">
            <div class="vx-col w-full">
                <vx-card>
                    <div style="float: right">
                        <a :href="printUri" target="_blank" download @click='onClick'>
                          <img title="Download PDF" src="@/assets/images/elements/printer_preview.png" style="width:20px" />
                        </a>
                    </div>
                    <vs-tabs>
                        <vs-tab label="POR Summary" v-on:click="onTabClick1">
                            <iframe frameBorder="0" v-if="ticket1 != ''" :src="reportUrl1"></iframe>
                        </vs-tab>
                        <vs-tab label="POR Change Log" v-on:click="onTabClick2">
                            <iframe frameBorder="0" v-if="ticket2 != ''" :src="reportUrl2"></iframe>
                        </vs-tab>
                        <vs-tab label="POR Dispute Log" v-on:click="onTabClick3">
                            <iframe frameBorder="0" v-if="ticket3 != ''" :src="reportUrl3"></iframe>
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
      ticket3: '',
      printUri: '',
      activePrintTicket: '',
    }
  },
  methods: {
    async onTabClick1() {
      this.ticket1 = "";
      this.ticket1 = await this.generateTrustedTicket();
      await this.refreshPrintUri(this.reportUrl1, this.ticket1);
    },
    async onTabClick2() {
      this.ticket2 = "";
      this.ticket2 = await this.generateTrustedTicket();
      await this.refreshPrintUri(this.reportUrl2, this.ticket2);
    },
    async onTabClick3() {
      this.ticket3 = "";
      this.ticket3 = await this.generateTrustedTicket();
      await this.refreshPrintUri(this.reportUrl3, this.ticket3);
    },
    async refreshPrintUri(uri, trustedTicket) {
      this.activePrintTicket = await this.generateTrustedTicket();
      uri = uri.replace(trustedTicket, this.activePrintTicket);
      this.printUri = uri.replace(":toolbar=no", ":toolbar=top");
    },
    async onClick() {
      await this.refreshPrintUri(this.printUri, this.activePrintTicket);
    }
  },
  computed :{
    reportUrl1 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket1 + process.env.VUE_APP_REPORT_SITE + '/FunctionPrimaryOwnerReport1?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=no&:display_spinner=yes&:toolbar=no&:tabs=no&:showAskData=false&:subscriptions=no&:showShareOptions=false&:customViews=no';
    },
    reportUrl2 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket2 + process.env.VUE_APP_REPORT_SITE + '/FunctionPrimaryOwnerReport2?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=no&:display_spinner=yes&:toolbar=no&:tabs=no&:showAskData=false&:subscriptions=no&:showShareOptions=false&:customViews=no';
    },
    reportUrl3 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket3 + process.env.VUE_APP_REPORT_SITE + '/FunctionPrimaryOwnerReport3?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=no&:display_spinner=yes&:toolbar=no&:tabs=no&:showAskData=false&:subscriptions=no&:showShareOptions=false&:customViews=no';
    },

  },
  async created() {
    await this.onTabClick1();
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
