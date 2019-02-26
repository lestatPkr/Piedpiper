import InvestorsService from '@/services/InvestorsService.js'

export const namespaced = true

export const state = {
  dashboard: {},
  menuOpened: null
}

export const mutations = {
  
  SET_DASHBOARD(state, dashboard) {
    state.dashboard = dashboard;
  },
  TOGGLE_MENU(state) {
    state.menuOpened = !state.menuOpened;
  }
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