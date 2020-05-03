<template>
  <v-card dense>
    <v-card-title>Search</v-card-title>

    <v-card-text>
      Select tags to construct a query. <span class='font-weight-bold'>Click</span> on a tag to make it an <span class='success--text text-darken-2'>include</span>/<span class='error--text text-darken-2'>exclude</span> tag. You can also specify a <span class='font-weight-bold'>time range</span>.
    </v-card-text>

    <v-card-text>
      <v-row dense>
        <TagInputField
          @changeInclude='includeTags = $event'
          @changeExclude='excludeTags = $event'>
        </TagInputField>
      </v-row>

      <v-row dense>
        <v-col cols='6' sm='3'>
          <MonthPicker
            @change='dateFrom = $event;'
            icon='mdi-clock-start'
            label='From:'>
          </MonthPicker>
        </v-col>

        <v-col cols='6' sm='3'>
          <MonthPicker
            @change='dateTo = $event;'
            icon='mdi-clock-end'
            label='To:'>
          </MonthPicker>
        </v-col>

        <v-btn
          class='search-button'
          :disabled="!isValid" color='primary' @click='submitSearchQuery'>
          Search
        </v-btn>
      </v-row>

    </v-card-text>
  </v-card>
</template>

<script lang='ts'>
import { Component, Vue, Watch } from 'vue-property-decorator';
import { ITag } from '../shared/models/ITag';
import { ISearchSpecification } from '../shared/models/ISearchSpecification';
import MonthPicker from './components/MonthPicker.vue';
import TagInputField from './components/TagInputField.vue';

@Component({
  components: {
    MonthPicker,
    TagInputField
  }
})
export default class SearchPanel extends Vue {

  dateFrom: Date | null = null;
  dateTo: Date | null = null;

  includeTags: string[] = [];
  excludeTags: string[] = [];

  get isValid(): boolean {
    return this.includeTags.length > 0
        || this.excludeTags.length > 0
        || this.dateFrom != null
        || this.dateTo != null;
  }

  submitSearchQuery() {
    const specification: ISearchSpecification = {
      includeTags: this.includeTags,
      excludeTags: this.excludeTags,
      timeFrom: this.dateFrom,
      timeTo: this.dateTo
    };
    this.$emit('submit', specification);
  }
}
</script>

<style scoped>
.search-button {
  margin-left: auto;
}
</style>
