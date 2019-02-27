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
  SET_MENU(state, value) {
    state.menuOpened = value;
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
  updateScreeningCriteria({commit, dispatch}){
    var data = {
      investorId: state.dashboard.investorId,
      superNiceToHave: state.dashboard.superNiceToHave,
      mustHave: state.dashboard.mustHave,
      niceToHave: state.dashboard.niceToHave
    };
    return InvestorsService.updateScreeningCriteria(data).then(response => {
      
      dispatch('fetchDashboard', state.dashboard.id);
      return response.data;
    })
  },
  updateMustHave({commit, dispatch}, mustHave) {
    commit("UPDATE_MH", mustHave);
    // var data = {
    //   investorId: state.dashboard.investorId,
    //   mustHave: mustHave,
    //   niceToHave: state.dashboard.niceToHave,
    //   superNiceToHave: state.dashboard.superNiceToHave
    // };
    // console.log(data)
    // return InvestorsService.updateScreeningCriteria(data).then(response => {
    
    //   dispatch('fetchDashboard', state.dashboard.id);
    //   return response.data;
    // })
  },
  updateSuperNiceToHave({commit, dispatch}, superNiceToHave) {
    commit("UPDATE_SNTH", superNiceToHave);
    // var data = {
    //   investorId: state.dashboard.investorId,
    //   superNiceToHave,
    //   mustHave: state.dashboard.mustHave,
    //   niceToHave: state.dashboard.niceToHave
    // };
    // return InvestorsService.updateScreeningCriteria(data).then(response => {
    //   dispatch('fetchDashboard', state.dashboard.id);
    //   return response.data;
    // })
  },
  updateNiceToHave({commit, dispatch}, niceToHave) {
    commit("UPDATE_NTH", niceToHave);
    // var data = {
    //   investorId: state.dashboard.investorId,
    //   niceToHave,
    //   mustHave: state.dashboard.mustHave,
    //   superNiceToHave: state.dashboard.superNiceToHave
    // };
    // return InvestorsService.updateScreeningCriteria(data).then(response => {
     
    //   dispatch('fetchDashboard', state.dashboard.investorId);
    //   return response.data;
    // })
  },
  fetchDashboard({commit}, id) {
    return InvestorsService.getDashboard(id).then(response => {
      if(response.data.investorId){
        commit('SET_DASHBOARD', response.data)
      }
      
      return response.data;
    })
  },
  toggleMenu({commit}) {
    commit("TOGGLE_MENU")
  }
}
export const getters = {
  getDashboardById: state => id => {
    console.log(id)
    return state.dashboard;
  }
}