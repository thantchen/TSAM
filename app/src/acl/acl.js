import Vue from "vue"
import { AclInstaller, AclCreate, AclRule } from "vue-acl"
import router from "@/router"

Vue.use(AclInstaller)

let initialRole = "primaryOwner"; //"public";

let userInfo = JSON.parse(localStorage.getItem("userInfo"))
if(userInfo && userInfo.userRole) initialRole = userInfo.userRole

export default new AclCreate({
  initial: initialRole,
  notfound: "/pages/not-authorized",
  router,
  acceptLocalRules: true,
  globalRules: {
    public: new AclRule("public").generate(),
    admin: new AclRule("admin").or("public").generate(),
    tsaTeam: new AclRule("tsaTeam").or("admin").or("public").generate(),
    executive: new AclRule("executive").or("tsaTeam").or("admin").or("public").generate(),
    primaryOwner: new AclRule("primaryOwner").or("executive").or("tsaTeam").or("admin").or("public").generate(),
    personnel: new AclRule("personnel").or("primaryOwner").or("executive").or("tsaTeam").or("admin").or("public").generate(),
  },
})
