<template>
  <div style='display:block; margin: auto; width: 24px'>
    <!--a @click='onClick' :href="location" ><img :src="icon" width="24px" title="View Details" /></a-->
    <a href='#' @click='onClick'><img :src="icon" width="24px" title="Download File" /></a>
  </div>
</template>

<script>

export default {
  name: 'ts-download',
  props: {
    icon: { type: String, default: require('@/assets/images/file-icons/document-excel-icon.png') },
    location: { type: String, required: true},
  },
  data() {
    return {

    }
  },
  methods: {
    onClick() {
      const config = { responseType: 'blob' };
      this.$http.get(this.location, config)
        .then((response) => {
          const type = response.headers['content-type'];
          const filename = response.headers["content-disposition"].split("filename=")[1];
          const blob = new Blob([response.data], { type: type });
          const link = document.createElement('a');
          link.href = window.URL.createObjectURL(blob);
          link.download = filename;
          link.click();
          link.remove();          
        });
    }
  },  
}
</script>

