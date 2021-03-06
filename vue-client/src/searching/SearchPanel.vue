<template>
  <v-card dense>
    <v-card-title>Search</v-card-title>

    <v-card-text>
      Select tags to construct a query.
      <span class='font-weight-bold'>Click</span> on a tag to make it an
      <span class='success--text text-darken-2'>include</span>/<span class='error--text text-darken-2'>exclude</span> tag.
      You can also specify a <span class='font-weight-bold'>time range</span>.
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
            @change='timeFrom = $event;'
            icon='mdi-clock-start'
            label='From:'>
          </MonthPicker>
        </v-col>

        <v-col cols='6' sm='3'>
          <MonthPicker
            @change='timeTo = $event;'
            icon='mdi-clock-end'
            label='To:'>
          </MonthPicker>
        </v-col>

        <v-btn
          class='search-button'
          :loading='loading'
          :disabled="!isValid" color='primary' @click='submitSearchQuery'>
          Search
        </v-btn>
      </v-row>

    </v-card-text>

    <v-card-text align='left'>
      <InlineErrorNotification
        v-for='item in validationErrors' :key='item'
        :message='item' />
    </v-card-text>
  </v-card>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ISearchSpecification } from '../searching/interfaces/ISearchSpecification';
import MonthPicker from './components/MonthPicker.vue';
import TagInputField from './components/TagInputField.vue';
import InlineErrorNotification from '../shared/components/inline-notifications/InlineErrorNotification.vue';

@Component({
  components: {
    MonthPicker,
    InlineErrorNotification,
    TagInputField
  }
})
export default class SearchPanel extends Vue {

  @Prop({ default: false })
  loading!: boolean;

  timeFrom: Date | null = null;
  timeTo: Date | null = null;

  includeTags: string[] = [];
  excludeTags: string[] = [];

  get isValid(): boolean {
    return this.validationErrors.length === 0
      && (this.includeTags.length > 0
      || this.excludeTags.length > 0
      || this.timeFrom != null
      || this.timeTo != null);
  }

  get validationErrors(): string[] {
    const result = [];
    if (this.timeFrom != null && this.timeTo != null && this.timeFrom > this.timeTo)
      result.push('The "From" date must be older than the "To" date.');
    return result;
  }

  submitSearchQuery() {
    const specification: ISearchSpecification = {
      includeTags: this.includeTags,
      excludeTags: this.excludeTags,
      timeFrom: this.timeFrom,
      timeTo: this.timeTo
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
