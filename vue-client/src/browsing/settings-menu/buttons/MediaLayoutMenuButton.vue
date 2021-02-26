<template>
  <BaseSettingsMenuButton
    :label='currentState.label'
    :icon='currentState.icon'
    @click='$emit("click", $event)' />
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import { LayoutModes } from '../../types/LayoutModes';
import BaseSettingsMenuButton from './BaseSettingsMenuButton.vue';

type ButtonState = { label: string; icon: string };
type ButtonStates = { [key in LayoutModes]: ButtonState };

@Component({
  components: {
    BaseSettingsMenuButton
  }
})
export default class MediaLayoutMenuButton extends Vue {

  get currentState(): ButtonState {
    const currentMode = this.$store.state.gallery.layoutMode as LayoutModes;
    return this.states[currentMode];
  }

  private states: ButtonStates = {
    [LayoutModes.OriginalOrFit]: {
      label: 'Original or shrink to fit',
      icon: 'mdi-arrow-collapse-all'
    },
    [LayoutModes.AspectFit]: {
      label: 'Aspect fit',
      icon: 'mdi-aspect-ratio'
    },
    [LayoutModes.AspectFill]: {
      label: 'Aspect fill',
      icon: 'mdi-arrow-expand-all'
    },
  };
}
</script>
