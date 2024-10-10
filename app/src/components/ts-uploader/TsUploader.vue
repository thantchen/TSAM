<template>
  <div id="uploader">
    <ejs-uploader ref="uploadObj" id='defaultfileupload' name="file" 
                  :autoUpload="autoUpload" :asyncSettings="path" :multiple="multiple"
                  :success= "onUploadSuccess" :failure= "onUploadFailed" :removing= "onFileRemove" :uploading="onUploading">
    </ejs-uploader>
  </div>
</template>

<script>
import Vue from "vue";
import { UploaderComponent, UploaderPlugin } from '@syncfusion/ej2-vue-inputs';

Vue.component(UploaderPlugin.name, UploaderComponent);

export default {
  props: {
    url: { 
      type: String, 
      required: true 
    },
    onSuccess: {
      type: Function,
      required: true,
    },
    onFailed: {
      type: Function,
      required: true,
    },
    onRemoved: {
      type: Function,
      required: false,
    }        
  },
  data() {
    return {
      path:  {
        saveUrl: this.url,
      },
      autoUpload: false,
      multiple: true,
    }
  },
  methods:{
      onUploadSuccess: function(args) {
        this.stopContainerLoading("#uploader");
        this.onSuccess && this.onSuccess(args.e, args.file);
      },
      onUploadFailed: function(args) {
        this.stopContainerLoading("#uploader");
        this.onFailed && this.onFailed(args.e, args.file);
      },
      onFileRemove: function(args) {
        this.onRemoved && this.onRemoved(args.e, args.file);
      },
      onUploading: function(args) {
        this.startContainerLoading("#uploader");
        args.currentRequest.withCredentials = true;
        //args.currentRequest.setRequestHeader("Authorization", `Bearer ${localStorage.getItem("accessToken")}`);
        //args.customFormData = [{'name': 'Syncfusion INC'}, {'first': 'Than'}, {'last': 'Chen'}, {'email': 'than.chen@ankura.com'}];    
      }
  },
}
</script>


