<template>
  <v-combobox
    v-model='selectedItems'
    :items='allTags'
    :search-input.sync='userInput'
    outlined multiple
    item-text='fullName'
    item-value='name'
    :return-object='false'
    :label='label'
    :readonly='readonly'>

    <template v-slot:selection='data'>
      <v-chip
        v-bind='data.attrs'
        :input-value='data.selected'
        close
        small
        color='secondary'
        @click:close='remove(data.item)'>

        <v-icon small left>mdi-tag</v-icon> {{ data.item | truncate(35) }}
      </v-chip>
    </template>

    <template v-slot:item='data'>
      <template v-if='typeof data.item !== "object"'>
        <v-list-item-content v-text='data.item'></v-list-item-content>
      </template>
      <template v-else>
        <v-list-item-content>
          <v-list-item-title>{{ data.item.name }}</v-list-item-title>
          <v-list-item-subtitle v-if='data.item.name !== data.item.description'>{{ data.item.description }}</v-list-item-subtitle>
        </v-list-item-content>
      </template>
    </template>

  </v-combobox>
</template>

<script lang='ts'>
import { Component, Watch, Mixins, Prop } from 'vue-property-decorator';
import { truncateFilter } from '../../shared/filters/truncate.filter';
import TagsProviderService from '../mixins/tags-provider.service';

@Component({
  filters: {
    truncate: truncateFilter
  }
})
export default class BasicTagPicker extends Mixins(TagsProviderService) {
  selectedItems: string[] = [];
  userInput = '';

  @Prop({ default: 'Items' })
  label!: string;

  @Prop({ default: false })
  readonly!: boolean;

  @Prop({ default: () => [] })
  value!: string[];

  @Watch('value')
  onValueChange(newValue: string[]) {
    this.selectedItems = newValue;
  }

  remove(item: string) {
    const index = this.selectedItems.findIndex(x => x === item);
    if (index >= 0)
      this.selectedItems.splice(index, 1);
  }

  @Watch('selectedItems')
  onSelectedItemsChange(_newValue: string[]) {
    this.userInput = '';
    this._emitItems();
  }

  private _emitItems() {
    this.$emit('input', this.selectedItems);
  }
}
</script>
