import Vue from "vue";
import Vuex from "vuex";
import * as investors from '@/store/modules/investors.js'
Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    investors
  },
  state: {
    
  }
});
