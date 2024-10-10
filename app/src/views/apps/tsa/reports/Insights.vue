<!-- =========================================================================================

========================================================================================== -->


<template>
    <div id="insights">
        <div class="vx-row">
            <div class="vx-col w-full">
                <vx-card title="Insights">
                        <div class="figureContainer" v-if="$acl.check('primaryOwner')">
                            <figure>
                                <!--a :href="$router.resolve({name: 'executive-summary'}).href"><img src="https://tableau.ankura.com/vizportal/api/rest/v1/views/414/thumbnail?7ad602ba-4561-411b-a57a-3a2c80f21dbb" title="Executive Summary" /></a-->
                                <router-link v-bind:to="'executive-summary'"><img src="@/assets/images/report/executive-summary.png" title="Executive Summary" /></router-link>
                                <!--a href='/apps/tsa/reports/executive-summary'><img src="https://tableau.ankura.com/vizportal/api/rest/v1/workbooks/103/thumbnail?1595614680497" title="Executive Summary" /></a-->
                                <figcaption>Executive Summary</figcaption>
                            </figure>  
                        </div>
                        <div class="figureContainer" v-if="$acl.check('primaryOwner')">
                            <figure>
                                <router-link v-bind:to="'primary-owner'"><img src="@/assets/images/report/primary-owner.png" title="Primary Owner" /></router-link>
                                <figcaption>Primary Owner</figcaption>
                            </figure>  
                        </div>
                        <div class="figureContainer" v-if="$acl.check('primaryOwner')">
                            <figure>
                                <!--a :href="$router.resolve({name: 'tsa-life-cycle'}).href"><img src="https://tableau.ankura.com/vizportal/api/rest/v1/workbooks/105/thumbnail?1595614778002" title="TSA Life Cycle" /></a-->
                                <router-link v-bind:to="'tsa-life-cycle'"><img src="@/assets/images/report/tsa-life-cycle.png" title="TSA Life Cycle" /></router-link>
                                <figcaption>TSA Life Cycle</figcaption>
                            </figure>  
                        </div>   
                        <!-- <div class="figureContainer" v-if="$acl.check('personnel')">
                            <figure>
                                <router-link v-bind:to="'sell-side-notification'"><img src="@/assets/images/report/sell-side-notification.png" title="Sell Side Notification" /></router-link>
                                <figcaption>Sell Side Notification</figcaption>
                            </figure>  
                        </div>  -->
                        <div class="figureContainer" v-if="$acl.check('personnel')">
                            <figure>
                                <router-link v-bind:to="'activity-report'"><img src="@/assets/images/report/activity_report.png" title="Activity Report" /></router-link>
                                <figcaption>Activity Report</figcaption>
                            </figure>  
                        </div>                                                              
                </vx-card>
            </div>
        </div>         


    </div>
</template>

<script>


export default{

}
</script>

<style scoped>

div.figureContainer {
    display: inline-block;
    margin: 20px;
    text-align: center;
} 

img {
    background-repeat: no-repeat;
    border: 1px solid #d8d8d8;
    border-radius: 1px;
    box-shadow: 0px 1px 2px rgba(0,0,0,.15);
    display: block;
    /*height: 184px;*/
    position: relative;
}

</style>