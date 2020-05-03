<template>
  <v-autocomplete
    v-model='selectedItems'
    :items='allItems'
    :search-input.sync='userInput'
    prepend-icon='mdi-magnify'
    outlined dense multiple
    item-text='description'
    item-value='name'
    return-object
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

        <v-icon small left>mdi-tag</v-icon> {{ data.item.name }}
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

  </v-autocomplete>
</template>

<script lang='ts'>
import { Component, Vue, Watch } from 'vue-property-decorator';
import { ITag } from '../../shared/models/ITag';

@Component
export default class TagInputField extends Vue {
  selectedItems: ITag[] = [];
  excludedTagNames: string[] = [];
  userInput = '';

  get allItems(): ITag[] {
    return [
      { description: 'Harold Hide the Pain', name: 'Harold Hide the Pain' },
      { description: 'Poland', name: 'Poland' },
      { description: 'Sample text', name: 'Sample text' },
      { description: 'USA: United States of America', name: 'USA' },
      { description: 'Star Wars', name: 'Star Wars' },
      { description: 'Pseudoscience', name: 'Pseudoscience' },
      { description: 'Antivaxxers, anti-vaxxers, antyszczepionkowcy - you name it', name: 'Anti-vaxxers' },
    ];
  }

  isExcluded(item: ITag): boolean {
    return this.excludedTagNames.includes(item.name);
  }

  handleItemClick(item: ITag) {
    const index = this.excludedTagNames.indexOf(item.name);
    if (index === -1)
      this.excludedTagNames.push(item.name);
    else
      this.excludedTagNames.splice(index, 1);
  }

  remove(item: ITag) {
    const index = this.selectedItems.findIndex(x => x.name === item.name);
    if (index >= 0)
      this.selectedItems.splice(index, 1);

    const excludeIndex = this.excludedTagNames.indexOf(item.name);
    if (excludeIndex >= 0)
      this.excludedTagNames.splice(excludeIndex, 1);
  }

  @Watch('selectedItems')
  onSelectedItemsChange(value: ITag[]) {
    this.userInput = '';
    this._emitItems();
  }

  @Watch('excludedTagNames')
  onExcludedTagNamesChange(value: string[]) {
    this._emitItems();
  }

  private _emitItems() {
    const selectedNames = this.selectedItems.map(tag => tag.name);
    const excludedNames = this.excludedTagNames
      .filter(name => selectedNames.includes(name));
    const includedNames = selectedNames
      .filter(name => !excludedNames.includes(name));

    this.$emit('changeInclude', includedNames);
    this.$emit('changeExclude', excludedNames);
  }
}
</script>
