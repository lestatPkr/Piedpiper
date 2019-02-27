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
  updateMustHave({commit, dispatch}, mustHave) {
    var data = {
      investorId: state.dashboard.id,
      mustHave,
      niceToHave: state.dashboard.niceToHave,
      superNiceToHave: state.dashboard.niceToHave
    };
    return InvestorsService.updateScreeningCriteria(data).then(response => {
      commit("UPDATE_MH", mustHave);
      dispatch('fetchDashboard', state.dashboard.id);
      return response.data;
    })
  },
  updateSuperNiceToHave({commit, dispatch}, superNiceToHave) {
    var data = {
      investorId: state.dashboard.id,
      superNiceToHave,
      mustHave: state.dashboard.mustHave,
      niceToHave: state.dashboard.niceToHave
    };
    return InvestorsService.updateScreeningCriteria(data).then(response => {
      commit("UPDATE_SNTH", superNiceToHave);
      dispatch('fetchDashboard', state.dashboard.id);
      return response.data;
    })
  },
  updateNiceToHave({commit, dispatch}, niceToHave) {
    var data = {
      investorId: state.dashboard.id,
      niceToHave,
      mustHave: state.dashboard.mustHave,
      superNiceToHave: state.dashboard.superNiceToHave
    };
    return InvestorsService.updateScreeningCriteria(data).then(response => {
      commit("UPDATE_NTH", niceToHave);
      dispatch('fetchDashboard', state.dashboard.id);
      return response.data;
    })
  },
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