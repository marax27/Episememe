import Vue from 'vue';
import Vuetify from 'vuetify/lib';
import colors from 'vuetify/lib/util/colors';

const darkColorPalette = {
    primary: '#7462b5',
    secondary: '#424242',
    accent: '#ffbf00',
    info: '#2c75e3',
    error: '#d60f22',
    warning: '#ff7e00',
    success: '#1ec756'
};

Vue.use(Vuetify);

export default new Vuetify({
    theme: {
        dark: true,
        options: {
            customProperties: true
        },
        themes: {
            dark: darkColorPalette
        }
    }
});
