import InvestorsService from '@/services/InvestorsService.js'

export const namespaced = true

export const state = {
  dashboard: {
    investorId: "",
    name: "Oscar 3"
  },
  
}

export const mutations = {
  
  SET_DASHBOARD(state, dashboard) {
    state.dashboard = dashboard;
  }
}

export const actions = {
  fetchDashboard({commit}, id) {
    
    return InvestorsService.getDashboard(id).then(response => {
      commit('SET_DASHBOARD', response.data)
      return response.data;
    })
  }
}
export const getters = {
  getDashboardById: state => id => {
    return state.dashboard;
  }
}