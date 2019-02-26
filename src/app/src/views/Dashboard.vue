<template>
  <dashboard-header :investorName="dashboard.name"/>
</template>

<script>
import DashboardHeader from "@/components/Dashboard/DashboardHeader";
import { mapState } from 'vuex'
import store from '@/store/store'
  function getPageEvents(routeTo, next) {
  
  store
    .dispatch('investors/fetchDashboard', "9f218330-e2d8-4b71-a6bc-c559a47c3628")
    .then(() => {
      console.log("Success");
      next()
    })
  }
export default {
  components:{
    DashboardHeader
  },
  data() {
    return {
      
    }
  },
  computed:{
      ...mapState('investors', ['dashboard']),
      
  },
  beforeRouteEnter(routeTo, routeFrom, next) {
      getPageEvents(routeTo, next)
  }
    
}
</script>

<style>

</style>

