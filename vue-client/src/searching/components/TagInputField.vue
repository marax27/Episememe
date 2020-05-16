<template>
  <v-combobox
    v-model='selectedItems'
    :items='allTags'
    :search-input.sync='userInput'
    prepend-icon='mdi-magnify'
    outlined multiple
    item-text='fullName'
    item-value='name'
    :return-object='false'
    label='Search'>

    <template v-slot:selection='data'>
      <v-chip
        v-bind='data.attrs'
        :input-value='data.selected'
        close
        small
        :color='isExcluded(data.item) ? "error darken-2" : "success darken-2"'
        @click='handleItemClick(data.item, $event); data.select($event)'
        @click:close='remove(data.item)'>

        <v-icon small left>mdi-tag</v-icon> {{ data.item }}
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
import { Component, Watch, Mixins } from 'vue-property-decorator';
import { ITag } from '../../shared/models/ITag';
import TagsProviderService from '../../tags/mixins/tags-provider.service';

@Component
export default class TagInputField extends Mixins(TagsProviderService) {
  selectedItems: string[] = [];
  excludedTagNames: string[] = [];
  userInput = '';

  isExcluded(item: string): boolean {
    return this.excludedTagNames.includes(item);
  }

  handleItemClick(item: string) {
    const index = this.excludedTagNames.indexOf(item);
    if (index === -1)
      this.excludedTagNames.push(item);
    else
      this.excludedTagNames.splice(index, 1);
  }

  remove(item: string) {
    const index = this.selectedItems.findIndex(x => x === item);
    if (index >= 0)
      this.selectedItems.splice(index, 1);

    const excludeIndex = this.excludedTagNames.indexOf(item);
    if (excludeIndex >= 0)
      this.excludedTagNames.splice(excludeIndex, 1);
  }

  @Watch('selectedItems')
  onSelectedItemsChange(value: string[]) {
    this.userInput = '';
    this._emitItems();
  }

  @Watch('excludedTagNames')
  onExcludedTagNamesChange(value: string[]) {
    this._emitItems();
  }

  private _emitItems() {
    const excludedNames = this.excludedTagNames
      .filter(name => this.selectedItems.includes(name));
    const includedNames = this.selectedItems
      .filter(name => !excludedNames.includes(name));

    this.$emit('changeInclude', includedNames);
    this.$emit('changeExclude', excludedNames);
  }
}
</script>
