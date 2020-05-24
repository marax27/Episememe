<template>
  <v-card class='ma-1 pa-1' color='secondary darken-2'>
    <v-card-title class='ma-0 pa-0'>
      <v-icon left>mdi-history</v-icon> History
    </v-card-title>

    <v-list-item v-for='item in items' :key='item.id'
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
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class RevisionHistoryPanel extends Vue {

  items = [
    { id: '1', date: '2020-02-01', time: '12:05', description: 'Created' },
    { id: '2', date: '2020-02-10', time: '08:54', description: 'Added 2, deleted 1 tag' },
    { id: '3', date: '2020-03-19', time: '07:33', description: 'Added 4, deleted 2 tags' },
    { id: '4', date: '2020-02-01', time: '12:05', description: 'Added 1, deleted 1 tag' },
  ];
}
</script>
