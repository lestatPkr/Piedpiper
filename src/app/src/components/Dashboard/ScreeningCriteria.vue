<template>
    <div>
        
  <v-layout row>
    <v-flex xs12>
      <v-card>
        <v-toolbar color="green" dark>
         

          <v-toolbar-title>Screening criteria</v-toolbar-title>

          <v-spacer></v-spacer>

         
        </v-toolbar>
  <v-subheader>
              <h3>MUST HAVE</h3> 
            </v-subheader>
        <v-list>
          <draggable class="list-group" :list="mustHave"  :options="{ group: 'people' }">
          <template v-for="(item, index) in mustHave">
            <v-list-tile
              :key="index"
              avatar>
              

              <v-list-tile-content>
                
                <v-chip color="green" text-color="white">
                  {{item | kpi}}
                  <v-icon right>star</v-icon>
                </v-chip>
              </v-list-tile-content>
            </v-list-tile>
            
            
          </template>
          </draggable>
        </v-list>
        <v-subheader>
               <h3>SUPER NICE TO HAVE</h3> 
            </v-subheader>
        <v-list >
          
            <draggable class="list-group" :list="niceToHave"  :options="{ group: 'people' }">
          <template v-for="(item, index) in niceToHave" >
            <v-list-tile :key="index"
              
              avatar>
              

              <v-list-tile-content>
                
                <v-chip color="orange" text-color="white">
                  {{item | kpi}}
                  <v-icon right>star</v-icon>
                </v-chip>
              </v-list-tile-content>
            </v-list-tile>
              </template>
            </draggable>
            
        
        </v-list>
         <v-subheader>
            <h3>NICE TO HAVE</h3> 
            </v-subheader>
        <v-list >
           <draggable class="list-group" :list="niceToHave"  :options="{ group: 'people' }">
          <template v-for="(item, index) in superNiceToHave">
            <v-list-tile
              :key="index"
              avatar>
              

              <v-list-tile-content>
                
                <v-chip color="primary" text-color="white">
                  {{item | kpi}}
                  <v-icon right>star</v-icon>
                </v-chip>
              </v-list-tile-content>
            </v-list-tile>
            
            
          </template>
          </draggable>
        </v-list>
      </v-card>
    </v-flex>
  </v-layout>

    </div>
    
</template>

<script>
import draggable from 'vuedraggable'
import { mapState, mapGetters } from "vuex";
import store from "@/store/store";
export default {
    components:{
        draggable
    },
    data(){
        return{
            
        }
    },
    computed: {
      //...mapState("investors", ["dashboard"]),
      mustHave: {
          get() {
              return this.$store.state.investors.dashboard.mustHave
          },
          set(value) {
              this.$store.commit('UPDATE_MH', value)
          }
      },
      niceToHave: {
          get() {
              return this.$store.state.investors.dashboard.niceToHave
          },
          set(value) {
              this.$store.commit('UPDATE_NTH', value)
          }
      },
      superNiceToHave: {
          get() {
              return this.$store.state.investors.dashboard.superNiceToHave
          },
          set(value) {
              this.$store.commit('UPDATE_SNTH', value)
          }
      }
    
    
    },
   
    filters:{
      kpi: function (value) {
      switch (value) {
        case "CEOFullTime":
          return "CEO Full time"
          break;
        case "CTOFullTime":
          return "CTO Full time"
          break;
        case "RoundBetween1MAnd10M":
          return "Round between 1M and 10M"
          break;
        case "CEOWorkedAtHighGrowthStartup":
          return "Source of leads is personal contact"
          break;
        case "CTOPlus2YearsLeadingTechTeams":
          return "CEO repeats entrepeneur"
          break;
        case "PreviousInvestorsFromTopFunds":
          return "More than 50K MRR"
          break;
          case "SourceOfLeadsIsPersonalContact":
          return "CEO worked at high growth startup"
          break;
          case "CEORepeatsEntrepeneur":
          return "CTO +2 years leading tech teams"
          break;
          case "MoreThan50KMonthlyRecurrentRevenue":
          return "Previous investors from top funds"
          break;
       
      }
      }
    }
    
}
</script>

<style>

</style>
