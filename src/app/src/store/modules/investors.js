import InvestorsService from '@/services/InvestorsService.js'

export const namespaced = true

export const state = {
  dashboard: {},
  screeningCriteria:{},
  menuOpened: null
}

export const mutations = {
  
  SET_DASHBOARD(state, dashboard) {
    state.dashboard = dashboard;
    state.screeningCriteria = dashboard.screeningCriteria;
  },
  TOGGLE_MENU(state) {
    state.menuOpened = !state.menuOpened;
  },
  UPDATE_MH(state, list) {
    state.dashboard.mustHave = list;
  },
  UPDATE_NTH(state, list) {
    state.dashboard.niceToHave = list;
  },
  UPDATE_SNTH(state, list) {
    state.dashboard.superNiceToHave = list;
  },
}

export const actions = {
  fetchDashboard({commit}, id) {
    
    return InvestorsService.getDashboard(id).then(response => {
      commit('SET_DASHBOARD', response.data)
      return response.data;
    })
  },
  toggleMenu({commit}) {
    commit("TOGGLE_MENU")
  }
}
export const getters = {
  getDashboardById: state => id => {
    return state.dashboard;
  }
}