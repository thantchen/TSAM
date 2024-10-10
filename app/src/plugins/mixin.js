
import Vue from 'vue'

Vue.mixin({
    methods: {
        getLocaleDateTime(arg) {
            if (arg == null)
                return "";
            else {
                var date = new Date(arg);
                return date.toLocaleDateString("en-US", { year: "numeric", month: "2-digit", day: "2-digit" }) + " " + date.toLocaleString([], { hour: '2-digit', minute: '2-digit' }) +
                                    " "  + date.toLocaleTimeString('en-us',{timeZoneName:'short'}).split(' ')[2];
            }
        },
        alert(title, text, acceptCallback) {
            this.$vs.dialog({
                type: 'alert',
                color: 'danger',
                title: title,
                text: text,
                acceptText: 'Close',
                accept: acceptCallback,
                cancel: acceptCallback  // to accommodate and handle user clicking on X icon
            });

            var checkExist = setInterval(function() {
                var element = document.querySelector(".con-vs-dialog.vs-dialog-danger");
                if (element != null) {
                    document.querySelector(".vs-dialog-danger > .vs-dialog").style = "width: 700px";    // Set alert dialog width
                    document.querySelector(".con-vs-dialog.vs-dialog-danger > .vs-dialog > .vs-dialog-text").style = "white-space: pre-line";   // Set style to recognize newline in dialog text
                    element.setAttribute("style", "z-index:99999"); // Bring forth error dialog

                    clearInterval(checkExist);
                }
            }, 100);                
        },  
        startDialogLoading() {
            // Add "vs-con-loading__container" class to "vs-dialog" element to add loading within dialog containter
            document.getElementsByClassName("vs-dialog")[0].classList.add("vs-con-loading__container");
            this.$vs.loading({ container: '.vs-dialog', scale: 0.6 });
        },
        stopDialogLoading() {
            this.$vs.loading.close('.vs-con-loading__container > .con-vs-loading');
            // Remove "vs-con-loading__container" class to "vs-dialog" element to close loading within dialog containter
            document.getElementsByClassName("vs-dialog")[0].classList.remove("vs-con-loading__container");
        },
        async httpGet(endpoint) {
            try {
                let response = await this.$http.get(`${process.env.VUE_APP_SERVICE_API_URL}${endpoint}`);
                return response.data.data;
            }
            catch (error) {
                this.alert("ERROR", error + "\r\nError querying API");
            }             
        },
        startContainerLoading(containerSelector) {
            this.$vs.loading({
                //background: 'primary',
                //color: '#fff',
                container: containerSelector,
                scale: 0.45
              });
        },
        stopContainerLoading(containerSelector) {
            this.$vs.loading.close(containerSelector + ' > .con-vs-loading');
        },
        async generateTrustedTicket() {
            try {
                let response = await this.$http.get(`${process.env.VUE_APP_SERVICE_API_URL}/api/data/trusted-ticket`);
                return response.data.data;
            }
            catch (error) {
                this.alert("ERROR", error);
            }  
        },
        async sendEmail(emailTemplate, idList) {
            if (idList) {
                var method = "";
                var url = "";

                switch (emailTemplate) {
                    case "newAddRequest":
                        method = "post";
                        url = "/api/emails/add-log";
                        break;
                    case "updatedAddRequest":
                        method = "put";
                        url = "/api/emails/add-log";
                        break;
                    case "newChangeRequest":
                        method = "post";
                        url = "/api/emails/change-log";
                        break;
                    case "updatedChangeRequest":
                        method = "put";
                        url = "/api/emails/change-log";
                        break;
                    case "newDisputeRequest":
                        method = "post";
                        url = "/api/emails/dispute-log";
                        break;
                    case "updatedDisputeRequest":
                        method = "put";
                        url = "/api/emails/dispute-log";
                        break;
                    default:
                        this.alert("ERRROR", `Email template '${emailTemplate}' not supported.`);
                }

                try {
                    let response = await this.$http({
                                                    method: method,
                                                    url: `${process.env.VUE_APP_SERVICE_API_URL}${url}`,
                                                    data: {idList}
                                                });
                    return response.data.data;
                }
                catch (error) {
                    this.alert("ERROR", error);
                }  
            }           
        }, 
        getShortDate(datetime)    {
            return datetime.toJSON().slice(0,10);
        },
        convertComments(comments) {
            var result = comments;
            result = result.replace(/\[[\s\S]*?\]/g, '[~]');

            var count = (result.match(/[~]/g) || []).length;

            for (var i = 1; i <= count; i++)
            {
                result = result.replace(/[~]/, i)
            }
            return result;
        },        
    }
})