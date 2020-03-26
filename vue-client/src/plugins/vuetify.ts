import Vue from 'vue';
import Vuetify from 'vuetify/lib';
import colors from 'vuetify/lib/util/colors';

const lightColorPalette = {
    primary: colors.purple.darken4,
};

Vue.use(Vuetify);

export default new Vuetify({
    theme: {
        themes: {
            light: lightColorPalette
        }
    }
});
