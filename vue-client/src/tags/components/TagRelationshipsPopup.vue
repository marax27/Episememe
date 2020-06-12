<template>
  <v-dialog
    v-model='isOpen'
    fullscreen
    persistent>

    <v-card class='d-flex flex-column'>
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
          :disabled='isDisabled()' />
      </v-card-text>

      <v-divider></v-divider>

      <v-card-text class='pa-2'>
        <v-row dense>
          <v-col cols='6'>
            <SingleTagPicker
              v-if='isOpen'
              v-model='selectedTag'
              hint='Pick one of the existing tags to edit it.' />
          </v-col>

          <v-col cols='6'>
            <v-text-field
              dense
              label='New name'
              v-model='newName'
              hide-details='auto'
              :disabled='isDisabled()'
              outlined>
            </v-text-field>
          </v-col>

          <v-col cols='12'>
            <v-text-field
              dense
              label='Description'
              v-model='description'
              hide-details='auto'
              :disabled='isDisabled()'
              outlined>
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-divider></v-divider>

      <v-card-text class='pa-2'>
        <BasicTagPicker
          v-model='children'
          label='Children'
          :disabled='isDisabled()' />

        <BasicTagPicker
          v-model='allSuccessors'
          label='All successors'
          :disabled='true' />
      </v-card-text>

      <v-card-text align='left'>
        <p v-for='item in getValidationErrors()' :key='item'>
          <v-icon left color='error'>mdi-alert-circle-outline</v-icon>
          <span class='error--text'>{{ item }}</span>
        </p>
      </v-card-text>

      <v-spacer></v-spacer>
      <v-divider></v-divider>

      <v-card-actions>
        <v-btn
          color='secondary'
          @click='close'>
          <v-icon left>mdi-close</v-icon> Close
        </v-btn>

        <v-btn
          color='secondary'
          @click='reloadPopupData'>
          <v-icon left>mdi-trash-can-outline</v-icon> Reset
        </v-btn>

        <v-spacer></v-spacer>

        <v-btn
          color='primary'
          :disabled='!canSubmit()'
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
import { pushUnique, intersectionOf } from '../../shared/helpers/collections';
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
  newName: string | null = null;
  description: string | null = null;
  children: string[] = [];
  parents: string[] = [];

  allAncestors: string[] = [];
  allSuccessors: string[] = [];

  isDisabled(): boolean {
    return this.selectedTag == null;
  }

  canSubmit(): boolean {
    return !this.isDisabled() && this.getValidationErrors().length === 0;
  }

  close() {
    this.clearValues();
    this.isOpen = false;
  }

  applyChanges() {
    this.isOpen = false;
  }

  getValidationErrors(): string[] {
    const result = [];
    if (this.detectedCycles())
      result.push('Possible cycle detected.');
    if (this.detectedLoop())
      result.push('Loop detected: tag is referencing itself.');
    return result;
  }

  private detectedCycles(): boolean {
    return intersectionOf(this.allSuccessors, this.allAncestors).length !== 0;
  }

  private detectedLoop(): boolean {
    if (this.selectedTag == null)
      return false;
    return this.parents.includes(this.selectedTag.name)
        || this.children.includes(this.selectedTag.name);
  }

  private clearValues() {
    this.selectedTag = null;
    this.newName = null;
    this.description = null;
    this.children = [];
    this.parents = [];
  }

  reloadPopupData() {
    this.newName = this.selectedTag?.name ?? null;
    this.description = this.selectedTag?.description ?? null;
    this.children = this.selectedTag?.children ?? [];
    this.parents = this.selectedTag?.parents ?? [];
    this.allSuccessors = this.findAllSuccessors();
    this.allAncestors = this.findAllAncestors();
  }

  @Watch('selectedTag')
  onSelectedTagChange() {
    this.reloadPopupData();
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
      pushUnique(pool, tag.children);
    }
    return pool;
  }

  findAllAncestors(): string[] {
    const pool = Array.from(this.parents);
    for(let i = 0; i < pool.length; ++i) {
      const tag = this.findTagByName(pool[i]);
      pushUnique(pool, tag.parents);
    }
    return pool;
  }

  private findTagByName(name: string): ITag {
    const result = this.allTags.find(tag => tag.name === name);
    if (result == null)
      throw new Error(`Tag named '${name}' does not exist.`);
    return result;
  }
}
</script>
