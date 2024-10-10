<template>
  <div class="control-section dialog-components">
      <ejs-dialog id="componentsDialog" :buttons='dialogButtons' ref="dialog" :visible='false' @close="onClose()" :header='title' 
                                        :animationSettings='animationSettings' :content='content' showCloseIcon=true :width='width' :height='height' :minHeight='minHeight'>
      </ejs-dialog>        
  </div>   
</template>

<script>

import Vue from "vue";
import { DialogPlugin } from '@syncfusion/ej2-vue-popups';
import { ButtonPlugin } from "@syncfusion/ej2-vue-buttons";

Vue.use(DialogPlugin);
Vue.use(ButtonPlugin);

export default {
  name: 'ts-message-box',
 // components: {
 //     DialogPlugin,
 //     ButtonPlugin,
 // },  
  props: {
    title: { type: String, default: 'Alert' },
    content: { type: String, required: true },
    visible: { type: Boolean, default: false },
    width: { type: String, default: '600px'},
    height: { type: String, default: '600px'},
    minHeight: { type: String, default: '300px'},
  },
  data() {
    return {

      animationSettings: { effect: 'Zoom' },
      dialogButtons: [{ click: this.onCloseDialog, buttonModel: { content: 'Close' } }],
    }
  },
  methods: {
    onOpenDialog : function() {
      this.$refs.dialog.show();
    },
    onCloseDialog: function() {
      this.$refs.dialog.hide();
      this.$emit("close", event);
      this.$emit('update:visible',false)
    },
    onClose: function() {
      this.$emit("close", event);
      this.$emit('update:visible',false)
    }
  },  
  watch: {
    visible: function() {
      if (this.visible) {
        this.onOpenDialog();
      }
      else {
        this.onCloseDialog();
      }
    }
  }
}
</script>

<style scoped>
.control-section {
    left: 0px !important;
    top:0% !important;
}
.dlgbtn {
    margin-right: 10px;
}
/* .control-section {
    height: 100%;
    min-height: 480px;
} */

.dialog-components {
    overflow-y: auto;
}
</style>
