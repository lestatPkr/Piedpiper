<template>
  <div>
     <v-layout justify-center>
         
        </v-layout>
    <dashboard-header :investorName="dashboard.name" />
    <v-container grid-list-md>
      <v-layout row wrap>
        <v-flex xs12>
          <v-card>
            <v-card-text>
              <div v-for="company in dashboard.companies" :key="company.id">
                <company-item :name="company.name" :rating="company.matchStatus"
                :noMetKpis="company.noMetKpis"
                :missingKpis="company.missingKpis"
                :niceToHavePercentage="company.niceToHavePercentage"
                :superNiceToHavePercentage="company.superNiceToHavePercentage"/>
              </div>
            </v-card-text>
          </v-card>
        </v-flex>
      </v-layout>
    </v-container>
    <v-navigation-drawer 
        v-model="menuOpened"
        absolute
        temporary
        floating
        
      >
        <screening-criteria />
      </v-navigation-drawer>

  </div>
</template>

<script> 
import DashboardHeader from "@/components/Dashboard/DashboardHeader";
import CompanyItem from "@/components/Dashboard/CompanyItem.vue";
import ScreeningCriteria from '@/components/Dashboard/ScreeningCriteria.vue';
import { mapState } from "vuex";
import store from "@/store/store";
function getPageEvents(routeTo, next) {
  store
    .dispatch(
      "investors/fetchDashboard",
      "35efeb92-dd31-4227-ac48-50ffe030d011"
    )
    .then(() => {
      console.log("Success");
      next();
    });
}
export default {
  components: {
    DashboardHeader,
    CompanyItem,
    ScreeningCriteria
  },
  data() {
    return {};
  },
  computed: {
    ...mapState("investors", ["dashboard", "menuOpened"])
  },
  beforeRouteEnter(routeTo, routeFrom, next) {
    getPageEvents(routeTo, next);
  }
};
</script>

<style></style>
