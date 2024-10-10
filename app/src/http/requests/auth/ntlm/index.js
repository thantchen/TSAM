//import axios from "../../../axios/index.js"

export default {
  async init($this) {
    try {
      localStorage.removeItem('userInfo');

      await $this.$http.get(`${process.env.VUE_APP_SERVICE_API_URL}/api/user`)
        .then((response) => {
          const userData = {
            displayName: `${response.data.data.firstName} ${response.data.data.lastName}`,
            email: response.data.data.email,
            uid: response.data.data.id,
            userRole: response.data.data.roles[0]
          }
    
          // Update user details
          $this.$store.commit('UPDATE_USER_INFO', userData, {root: true})

          $this.$store.dispatch('updateUserRole', {aclChangeRole: $this.$acl.change, userRole: response.data.data.roles[0]})

          /*var reg = /.+?:\/\/.+?(\/.+?)(?:#|\?|$)/;
          // If current URL is root (/), then redirect based on role
          if (reg.exec(location.href) == null)
          {
            if (response.data.data.roles[0] == "personnel") {
              $this.$router.push("/apps/tsa/reports/insights")
            }
            else {
              $this.$router.push("/key-metrics");
            }
          }*/
        })
        .catch((error) => { 
          $this.alert("ERROR", error)
          $this.$router.push("/pages/not-authorized");
        });
    }
    catch (error) {
      $this.alert("ERROR", error);
    }   
  },

}
