<!-- =========================================================================================

========================================================================================== -->


<template>
    <div id="tsa-life-cycle">
        <div class="vx-row">
            <div class="vx-col w-full">
                <vx-card>
                    <div style="float: right">
                        <a :href="printUri" target="_blank" download @click='onClick'>
                          <img title="Download PDF" src="@/assets/images/elements/printer_preview.png" style="width:20px" />
                        </a>
                    </div>
                    <iframe frameBorder="0" v-if="ticket != ''" :src="reportUrl"></iframe>
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
        ticket: '',
        printUri: '',
    }
  },
  methods: {
      async refreshPrintUri(uri, trustedTicket) {
        uri = uri.replace(trustedTicket, await this.generateTrustedTicket());
        this.printUri = uri.replace(":toolbar=no", ":toolbar=top");
      },
      async onClick() {
        await this.refreshPrintUri(this.reportUrl, this.ticket);
      }
  },
  computed :{
    reportUrl : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket + process.env.VUE_APP_REPORT_SITE + '/TSALifeCycle?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=no&:display_spinner=yes&:toolbar=no&:tabs=no&:showAskData=false&:subscriptions=no&:showShareOptions=false&:customViews=no';
    }
  },
  async created() {
      this.ticket = await this.generateTrustedTicket();
      await this.refreshPrintUri(this.reportUrl, this.ticket);
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
        overflow: hidden; /*scroll;*/
        -webkit-overflow-scrolling: touch;
        display: block;
        margin: 0px;
        padding: 0px;
        border: none;
    }
    .tabErrorDialog {
        display: none !important;
    }
</style>
