<template>
  <v-navigation-drawer
    v-model='isOpen'
    color='secondary darken-1'
    absolute temporary>

    <span v-if='instance == null'>No media instance specified.</span>
    <v-list v-else class='ma-1'>
      <v-chip small color='secondary' class='tag'
        v-for='item in instance.tags' :key='item'>
        {{ item }}
      </v-chip>
    </v-list>

    <template v-slot:append>
      <RevisionHistoryPanel />
    </template>

  </v-navigation-drawer>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';
import { IMediaInstance } from '../../../shared/models/IMediaInstance';
import RevisionHistoryPanel from './revision-history/RevisionHistoryPanel.vue';

@Component({
  components: {
    RevisionHistoryPanel
  }
})
export default class MediaSidebar extends Vue {
  @Prop()
  instance?: IMediaInstance;

  @Prop({ default: false })
  value!: boolean;

  get isOpen() {
    return this.value;
  }
  set isOpen(newValue: boolean) {
    this.$emit('input', newValue);
  }
}
</script>

<style scoped>
.tag {
  margin: 2px;
}
</style>
