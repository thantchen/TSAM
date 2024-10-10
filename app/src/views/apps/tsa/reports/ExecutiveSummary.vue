<!-- =========================================================================================
https://help.tableau.com/current/pro/desktop/en-us/link_view.htm
https://help.tableau.com/current/pro/desktop/en-us/embed_dashboard.htm
https://help.tableau.com/current/pro/desktop/en-us/embed_list.htm#url-parameters-for-iframe-tags

https://tableau.ankura.com/t/ProjectMilano/views/TSAWeeklyBuy-SideReporting/ExecSummary?iframeSizedToWindow=true&:embed=yes&:tabs=no&:toolbar=no&:showVizHome=no&:increment_view_count=no&:format=png

https://viziblydiffrnt.wordpress.com/2016/12/14/tableaus-undocumented-api-made-easy-with-python/
tableau simulate account login rest api vizportal/api/web/v1/login
========================================================================================== -->

<template>
    <div id="executive-summary">
        <div class="vx-row">
            <div class="vx-col w-full">
                <vx-card>
                    <div style="float: right">
                        <a :href="printUri" target="_blank" download @click='onClick'>
                          <img title="Download PDF" src="@/assets/images/elements/printer_preview.png" style="width:20px" />
                        </a>
                    </div>
                    <vs-tabs>
                        <vs-tab label="Executive Summary" v-on:click="onTabClick1">
                          <iframe frameBorder="0" v-if="ticket1 != ''" :src="reportUrl1"></iframe>
                        </vs-tab>
                        <vs-tab label="Executive Drilldown" v-on:click="onTabClick2">
                          <iframe frameBorder="0" v-if="ticket2 != ''" :src="reportUrl2"></iframe>
                        </vs-tab>
                        <vs-tab label="TSA Cost Comparison" v-on:click="onTabClick3">
                          <iframe frameBorder="0" v-if="ticket3 != ''" :src="reportUrl3"></iframe>
                        </vs-tab>
                        <vs-tab label="Functional/Primary Owner Summary" v-on:click="onTabClick4">
                          <iframe frameBorder="0" v-if="ticket4 != ''" :src="reportUrl4"></iframe>
                        </vs-tab>
                        <vs-tab label="Termination Summary" v-on:click="onTabClick5">
                          <iframe frameBorder="0" v-if="ticket5 != ''" :src="reportUrl5"></iframe>
                        </vs-tab>
                    </vs-tabs>
                </vx-card>
            </div>
        </div>
    </div>
</template>

<script>
export default{
  data() {
    return {
    ticket1: '',
    ticket2: '',
    ticket3: '',
    ticket4: '',
    ticket5: '',
    printUri: '',
    activePrintTicket: '',
    isPdf: true,
    }
  },
  methods: {
      async onTabClick1() {
        this.ticket1 = "";
        this.ticket1 = await this.generateTrustedTicket();
        await this.refreshPrintUri(this.reportUrl1, this.ticket1, true);
      },
      async onTabClick2() {
        this.ticket2 = "";
        this.ticket2 = await this.generateTrustedTicket();
        await this.refreshPrintUri(this.reportUrl2, this.ticket2, false);
      },
      async onTabClick3() {
        this.ticket3 = "";
        this.ticket3 = await this.generateTrustedTicket();
        await this.refreshPrintUri(this.reportUrl3, this.ticket3, true);
      },
      async onTabClick4() {
        this.ticket4 = "";
        this.ticket4 = await this.generateTrustedTicket();
        await this.refreshPrintUri(this.reportUrl4, this.ticket4, true);
      },
      async onTabClick5() {
        this.ticket5 = "";
        this.ticket5 = await this.generateTrustedTicket();
        await this.refreshPrintUri(this.reportUrl5, this.ticket5, true);
      },
      async refreshPrintUri(uri, trustedTicket, isPdf) {
        this.isPdf = isPdf;
        this.activePrintTicket = await this.generateTrustedTicket();
        uri = uri.replace(trustedTicket, this.activePrintTicket);
        if (isPdf) {
          if (this.printUri.indexOf('?') >= 0) {
            this.printUri = uri.substring(0, uri.indexOf('?')) + '.pdf';
          }
          else {
            this.printUri = uri;
          }
        }
        else {
          this.printUri = uri.replace(":toolbar=no", ":toolbar=top");
        }
      },
      async onClick() {
        await this.refreshPrintUri(this.printUri, this.activePrintTicket, this.isPdf);
      }   
  },
  computed :{
    reportUrl1 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket1 + process.env.VUE_APP_REPORT_SITE + '/ExecutiveSummary?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=yes&:display_spinner=yes&:toolbar=no&:tabs=no';
    },
    reportUrl2 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket2 + process.env.VUE_APP_REPORT_SITE + '/ExecutiveDrilldown?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=no&:display_spinner=yes&:toolbar=no&:tabs=no&:showAskData=false&:subscriptions=no&:showShareOptions=false&:customViews=no';
    },
    reportUrl3 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket3 + process.env.VUE_APP_REPORT_SITE + '/TSASpend?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=yes&:display_spinner=yes&:toolbar=no&:tabs=no';
    },
    reportUrl4 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket4 + process.env.VUE_APP_REPORT_SITE + '/ExecutiveFunctionReport?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=yes&:display_spinner=yes&:toolbar=no&:tabs=no';
    },
    reportUrl5 : function(){
        return process.env.VUE_APP_REPORT_SERVER_URL + '/trusted/' + this.ticket5 + process.env.VUE_APP_REPORT_SITE + '/Expirations?iframeSizedToWindow=true&:embed=yes&:showAppBanner=false&:display_count=no&:showVizHome=no&refresh=yes&:display_spinner=yes&:toolbar=no&:tabs=no';
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
      overflow: hidden; /*scroll;*/
      -webkit-overflow-scrolling: touch;
      display: block;
      margin: 0px;
      padding: 0px;
      border: none;
  }

</style>
