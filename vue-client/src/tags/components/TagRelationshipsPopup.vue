<template>
  <v-dialog
    v-model='isOpen'
    width='80%'
    persistent>

    <v-card outlined>
      <v-card-title>
        <v-icon left>mdi-tag-multiple-outline</v-icon>
        Tag Relationships
      </v-card-title>

      <v-card-text class='pa-2'>
        <BasicTagPicker
          v-model='allAncestors'
          label='All ancestors'
          :disabled='true' />

        <BasicTagPicker
          v-model='parents'
          label='Parents'
          :disabled='selectedTag == null' />
      </v-card-text>

      <v-divider></v-divider>

      <v-card-text class='pa-2'>
        <SingleTagPicker
          v-if='isOpen'
          v-model='selectedTag'
          hint='Pick one of the existing tags to edit it.' />

        <v-text-field
          dense
          label='Description'
          v-model='description'
          hide-details='auto'
          outlined>
        </v-text-field>
      </v-card-text>

      <v-divider></v-divider>

      <v-card-text class='pa-2'>
        <BasicTagPicker
          v-model='children'
          label='Children'
          :disabled='selectedTag == null' />

        <BasicTagPicker
          v-model='allSuccessors'
          label='All successors'
          :disabled='true' />
      </v-card-text>

      <v-card-actions>
        <v-btn
          color='secondary'
          @click='close'>
          <v-icon left>mdi-close</v-icon> Close
        </v-btn>

        <v-spacer></v-spacer>

        <v-btn
          color='primary'
          @click='applyChanges'>
          <v-icon left>mdi-arrow-up-circle-outline</v-icon> Apply changes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang='ts'>
import { Component, Watch, Mixins } from 'vue-property-decorator';
import SingleTagPicker from './SingleTagPicker.vue';
import BasicTagPicker from './BasicTagPicker.vue';
import { ITag } from '../../shared/models/ITag';
import TagsProviderService from '../mixins/tags-provider.service';

@Component({
  components: {
    BasicTagPicker,
    SingleTagPicker,
  }
})
export default class TagRelationshipsPopup extends Mixins(TagsProviderService) {
  get isOpen(): boolean {
    return this.$store.state.popups.tagRelationships.isOpen;
  }
  set isOpen(newValue: boolean) {
    const actionName = newValue ? 'popups/openTagRelationships' : 'popups/closeTagRelationships';
    this.$store.dispatch(actionName);
    if (newValue)
      this.refreshTags();
  }

  selectedTag: ITag | null = null;
  description?: string = '';
  children: string[] = [];
  parents: string[] = [];

  allAncestors: string[] = [];
  allSuccessors: string[] = [];

  close() {
    this.isOpen = false;
  }

  applyChanges() {
    this.isOpen = false;
  }

  @Watch('selectedTag')
  onSelectedTagChange() {
    this.description = this.selectedTag?.description;
    this.children = this.selectedTag?.children ?? [];
    this.parents = this.selectedTag?.parents ?? [];
    this.allSuccessors = this.findAllSuccessors();
    this.allAncestors = this.findAllAncestors();
  }

  @Watch('children')
  onChildrenChange() {
    this.allSuccessors = this.findAllSuccessors();
  }

  @Watch('parents')
  onParentsChange() {
    this.allAncestors = this.findAllAncestors();
  }

  findAllSuccessors(): string[] {
    const pool = Array.from(this.children);
    for(let i = 0; i < pool.length; ++i) {
      const tag = this.findTagByName(pool[i]);
      this.pushUnique(pool, tag.children);
    }
    return pool;
  }

  findAllAncestors(): string[] {
    const pool = Array.from(this.parents);
    for(let i = 0; i < pool.length; ++i) {
      const tag = this.findTagByName(pool[i]);
      this.pushUnique(pool, tag.parents);
    }
    return pool;
  }

  private findTagByName(name: string): ITag {
    const result = this.allTags.find(tag => tag.name === name);
    if (result == null)
      throw new Error(`Tag named '${name}' does not exist.`);
    return result;
  }

  private pushUnique<T>(collection: T[], newElements: T[]) {
    newElements.forEach(element => {
      if (collection.indexOf(element) === -1)
        collection.push(element);
    });
  }
}
</script>
