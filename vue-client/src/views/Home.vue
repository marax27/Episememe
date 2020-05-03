<template>
  <ContentWrapper>
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
  </ContentWrapper>
</template>

<script lang='ts'>
import { Component, Vue, Mixins } from 'vue-property-decorator';
import router from '../router';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';
import ApiClientService from '../shared/mixins/api-client/api-client.service';

@Component({
  name: 'Home',
  components: {
    ContentWrapper
  }
})
export default class Home extends Mixins(ApiClientService) {

  valid = true;

  name = '';

  items = ['Politics', 'History', 'Poland', 'United States', 'Something else', 'Lorem ipsum', 'XYZ', 'Even more', 'Extremely long chip', 'Science']
  values = ['Poland', 'History']

  search() {
    this.refreshBrowseToken(() => {
      router.push({ name: 'Gallery', params: { data: '123' } });
    });
  }

  onInputChange($event: string) {
    if ($event == null)  return;
  }

  private refreshBrowseToken(afterCompletion: () => void) {
    return this.$api
      .post<string>('authorization', {})
      .then(response => {
        this.$store.dispatch('refreshBrowseToken', response.data)
          .then(_ => afterCompletion());
      })
      .catch(err => {
        console.error(`Failed to retrieve the browser token.`);
      });
  }
}
</script>

