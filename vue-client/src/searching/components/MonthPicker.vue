<template>
  <v-menu
    ref='menu'
    v-model='menuIsOpen'
    :close-on-content-click='false'
    :return-value.sync='date'
    transition='scale-transition'
    offset-y
    max-width='290px'
    min-width='290px'>

    <template v-slot:activator='{ on }'>
      <v-text-field
        class='ma-0 pa-0'
        v-model='date'
        :label='label'
        :prepend-icon='icon'
        readonly
        v-on='on'
      ></v-text-field>
    </template>

    <v-date-picker
      v-model='date'
      type='month'
      no-title
      scrollable>

      <v-spacer></v-spacer>
      <v-btn text color='accent' @click='updateDate(null)'>Reset</v-btn>
      <v-btn text color='accent' @click='menuIsOpen = false'>Cancel</v-btn>
      <v-btn text color='accent' @click='updateDate(date)'>OK</v-btn>
    </v-date-picker>

  </v-menu>
</template>

<script lang='ts'>
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class MonthPicker extends Vue {

  @Prop({ default: '' })
  icon!: string;

  @Prop({ default: null })
  label!: string;

  menuIsOpen = false;
  date: string | null = null;

  updateDate(value: string | null) {
    (this.$refs.menu as any).save(value);
    const eventValue = value == null ? null : new Date(value);
    this.$emit('change', eventValue);
  }
}
</script>
