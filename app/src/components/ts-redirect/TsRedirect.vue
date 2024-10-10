<template>
  <div style='display:block; margin: auto; width: 24px'>
    <a href="#" @click='onClick'><img :src="icon" width="24px" title="View Details" /></a>
  </div>
</template>

<script>

import router from '../../router'

export default {
  name: 'ts-redirect',
  props: {
    icon: { type: String, default: '/view_details.png' },
    location: { type: Object, required: true},
  },
  data() {
    return {

    }
  },
  methods: {
    onClick() {
      router.push(this.location);
    }
  },
}
</script>
