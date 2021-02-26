<template>
  <v-card class='card'>
    <v-card-title>{{ "#" + identifier }}</v-card-title>

    <v-card-text>
      <a :href='url' download target='_blank'>
        <v-btn text>
          <v-icon>mdi-download</v-icon>
          <span>Download</span>
        </v-btn>
      </a>
    </v-card-text>
  </v-card>
</template>

<script lang='ts'>
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class DownloadLinkComponent extends Vue {
  @Prop()
  url?: string;

  @Prop()
  identifier?: string;
}
</script>

<style scoped>
.card {
  width: auto !important;
  height: auto !important;
}
</style>
