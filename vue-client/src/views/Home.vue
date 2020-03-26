<template>
  <v-container>
    <v-img
      :src="require('../assets/logo.svg')"
      contain height="200"/>
  </v-container>
</template>

<script lang='ts'>
import { Component, Mixins, Vue } from 'vue-property-decorator';

@Component({
  name: 'Home'
})
export default class Home extends Vue {
}
</script>

