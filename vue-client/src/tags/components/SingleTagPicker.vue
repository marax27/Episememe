<template>
  <v-autocomplete
    v-model='selectedItem'
    :items='allTags'
    :search-input.sync='userInput'
    outlined
    dense
    item-text='fullName'
    item-value='name'
    return-object
    label='Selected tag'
    persistent-hint
    :hint='hint'>

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
import { Component, Vue, Prop, Watch, Mixins } from 'vue-property-decorator';
import { TagViewModel } from '../view-models/TagViewModel';
import TagsProviderService from '../mixins/tags-provider.service';

@Component
export default class SingleTagPicker extends Mixins(TagsProviderService) {
  selectedItem: TagViewModel | null = null;
  userInput = '';

  allItems: TagViewModel[] = [];

  @Prop({ default: undefined })
  hint?: string;

  @Prop({ default: () => [] })
  value!: TagViewModel;

  @Watch('value')
  onValueChange(newValue: TagViewModel | null) {
    this.selectedItem = newValue;
  }

  remove() {
    this.selectedItem = null;
  }

  @Watch('selectedItem')
  onSelectedItemChange(newValue: TagViewModel | null) {
    this.$emit('input', newValue);
  }
}
</script>
