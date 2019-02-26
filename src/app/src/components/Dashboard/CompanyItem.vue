<template>

  <v-container>
    <v-layout row wrap>
      <v-flex xs12 md3>
        <h2>{{ name }}</h2>
        <p>{{ name | url }}</p>
      </v-flex>
      <v-flex xs3 md4>
         
    <div class="kpi-circle">
      <v-progress-circular
      :rotate="360"
      :size="75"
      :width="10"
      :value="(noMetKpis * 100) / 9"
      color="red"
    >
      {{ (noMetKpis * 100) / 9 | round }}%
    </v-progress-circular>
    </div>
    <div class="kpi-circle">
        <v-progress-circular
      :rotate="360"
      :size="75"
      :width="10"
      :value="(missingKpis * 100) / 9"
      color="orange"
    >
      {{ (missingKpis * 100) / 9 | round }}%
    </v-progress-circular>
     </div>
    <div class="kpi-circle">
      <v-progress-circular
      :rotate="360"
      :size="75"
      :width="10"
      :value="niceToHavePercentage"
      color="blue"
    >
      {{ niceToHavePercentage | round }}%
    </v-progress-circular>
    </div>
    <div class="kpi-circle">
      <v-progress-circular
      :rotate="360"
      :size="75"
      :width="10"
      :value="superNiceToHavePercentage"
      color="blue"
    >
      {{ superNiceToHavePercentage | round }}%
    </v-progress-circular>
    </div>
     
      </v-flex>
       <v-flex xs3 md3>
         <div class="rating">
            <h3>{{rating | matchStatus}}</h3>
         </div>
        
      </v-flex>
       <v-flex xs3 md2>
        
      </v-flex>
       <v-flex xs3 md2>
        
      </v-flex>
      <v-flex xs2> </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
export default {
  props: {
    name: String,
    rating: Number,
    noMetKpis: Number,
    missingKpis: Number,
    niceToHavePercentage : Number,
    superNiceToHavePercentage: Number
  },
  filters: {
    url: function(value) {
      if (!value) return "";
      value = value.replace(/\s/g, "").toLowerCase();
      return `http://www.${value}.com`;
    },
    round: function(value) {
      return Math.round(value);
    },
    matchStatus: function (value) {
      switch (value) {
        case 0:
          return "No Match"
          break;
        case 1:
          return "Low"
          break;
        case 2:
          return "Medium-Low"
          break;
        case 3:
          return "Medium"
          break;
        case 4:
          return "Medium-High"
          break;
        case 5:
          return "High"
          break;
        default:
          return "Perfect Match"
          break;
      }
     
    }
  },
  components: {}
};
</script>
<style scoped>
.kpi-circle{
  display: inline-block;
  margin-right: 10px;
  margin-bottom: 3px;
}
.rating{
  margin-top:30px
}
</style>

