<template>
  <BaseSettingsMenuButton
    label='Revise'
    icon='mdi-square-edit-outline'
    @click='$emit("click", $event)' />
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import BaseSettingsMenuButton from './BaseSettingsMenuButton.vue';

@Component({
  components: {
    BaseSettingsMenuButton
  }
})
export default class RevisionMenuButton extends Vue {}
</script>
