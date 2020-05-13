<template>
  <v-snackbar
    v-model='isOpen'
    :timeout='8000'>

    {{ errorMessage }}

    <v-btn
      text color='error'
      @click='isOpen = false;'>
      Close
    </v-btn>

  </v-snackbar>
</template>

<script lang='ts'>
import { Component, Vue, Watch } from 'vue-property-decorator';

@Component
export default class ErrorNotification extends Vue {

  isOpen = false;

  get errorMessage(): string | null {
    return this.$store.state.errorMessage;
  }

  @Watch('errorMessage')
  private onErrorMessageChange() {
    if (this.errorMessage) {
      this.isOpen = true;
    }
  }
}
</script>
