<template>
  <ContentWrapper>
    <v-card>
      <v-card-title>User page</v-card-title>
    </v-card>
  </ContentWrapper>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import ContentWrapper from '../shared/components/content-wrapper/ContentWrapper.vue';

@Component({
  name: 'User',
  components: {
    ContentWrapper
  }
})
export default class User extends Vue {
  $auth: any;
}
</script>
