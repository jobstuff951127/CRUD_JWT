import Vue from 'vue';
import Vuetify from 'vuetify/lib';

Vue.use(Vuetify);

export default new Vuetify({
  theme: {
    options: {
      customProperties: true,
    },
    themes: {
      light: {
        primary: '#01579B',
        secondary: '#2f1ef7',
        accent: '#fff',
        error: '#fa2314',
        info: '#0076f5',
        success: '#02ba1a',
        warning: '#edba13',
      },
    },
  },
  icons: {
    iconfont: 'md',
    values: {
      product: 'pencil',
      support: 'mdi-lifebuoy',
      steam: 'mdi-steam-box',
      pc: 'mdi-desktop-classic',
      xbox: 'mdi-xbox',
      playstation: 'mdi-playstation',
      switch: 'mdi-nintendo-switch',
    },
  },
});
