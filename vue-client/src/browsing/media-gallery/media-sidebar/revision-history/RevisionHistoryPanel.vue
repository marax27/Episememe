<template>
  <v-card class='ma-1 pa-1' color='secondary darken-2'>
    <v-card-title class='ma-0 pa-0'>
      <v-icon left>mdi-history</v-icon> History
    </v-card-title>

    <v-progress-circular
      v-show='loadingInProgress'
      class='loading-spinner'
      indeterminate>
    </v-progress-circular>
    <v-list-item
      v-show='!loadingInProgress'
      v-for='item in items' :key='item.id'
      two-line>

      <v-list-item-content>
        <v-list-item-title>
          {{ item.date }} <span class='caption'>{{ item.time }}</span>
        </v-list-item-title>
        <v-list-item-subtitle>
          {{ item.description }}
        </v-list-item-subtitle>
      </v-list-item-content>
    </v-list-item>

  </v-card>
</template>

<script lang='ts'>
import { Component, Vue, Prop, Mixins, Watch } from 'vue-property-decorator';
import ApiClientService from '../../../../shared/mixins/api-client/api-client.service';
import { IMediaRevisionHistoryDto, ChangeType } from '../../../interfaces/IMediaRevisionHistoryDto';
import { formatDate, formatTime } from '../../../../shared/helpers/time-format';
import { RevisionHistoryViewModel } from './RevisionHistoryViewModel';

@Component
export default class RevisionHistoryPanel extends Mixins(ApiClientService) {

  @Prop({ default: false })
  public visible!: boolean;

  items: RevisionHistoryViewModel[] = [];
  loadingInProgress = false;

  @Watch('visible')
  onVisibleChange(isVisible: boolean) {
    if (isVisible)
      this.loadHistory();
  }

  private get currentMediaId(): string {
    return this.$store.getters['gallery/currentMediaId'];
  }

  private loadHistory() {
    const url = `media/${this.currentMediaId}/history`;
    this.loadingInProgress = true;

    this.$api.get<IMediaRevisionHistoryDto[]>(url)
      .then(response => response.data)
      .then(records => {
        this.items = records.map(this.mapHistoryDto);
        this.loadingInProgress = false;
      })
      .catch(err => {
        this.$store.dispatch('reportError', 'Failed to load history: ' + err);
        this.loadingInProgress = false;
      });
  }

  private mapHistoryDto(dto: IMediaRevisionHistoryDto, id: number): RevisionHistoryViewModel {
    const timestamp = new Date(dto.timeStamp);
    return {
      id: id,
      date: formatDate(timestamp),
      time: formatTime(timestamp),
      description: this.getChangeDescription(dto.mediaChangeType)
    };
  }

  private getChangeDescription(changeType: ChangeType) {
    switch(changeType) {
      case ChangeType.Create:  return "Created";
      case ChangeType.Update:  return "Updated";
      default:  return "Unknown";
    }
  }
}
</script>

<style scoped>
.loading-spinner {
  display: block;
  margin: 0 auto;
}
</style>
