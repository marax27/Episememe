<template>
  <v-card dense>
    <v-card-title>Search</v-card-title>

    <v-card-text>

        <v-row dense>
            <v-autocomplete
              v-model="values" :items="items"
              prepend-icon='mdi-magnify'
              outlined dense chips small-chips label='Search' multiple
              v-on:update:search-input='onInputChange'
            ></v-autocomplete>
        </v-row>

        <v-row dense justify='end'>
          <v-btn :disabled="!valid" color='primary' @click='search'>
            Search
          </v-btn>
        </v-row>
    </v-card-text>
  </v-card>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';

@Component({
  name: 'Home'
})
export default class Home extends Vue {

  valid = true;

  name = '';

  items = ['Politics', 'History', 'Poland', 'United States', 'Something else', 'Lorem ipsum', 'XYZ', 'Even more', 'Extremely long chip', 'Science']
  values = ['Poland', 'History']

  search() {
    console.log('val');
  }

  onInputChange($event: string) {
    if ($event == null)  return;
  }
}
</script>

