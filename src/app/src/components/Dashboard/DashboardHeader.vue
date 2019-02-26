<template>
  <v-container grid-list-md>
    <v-layout row wrap>
      <v-flex xs12>
        <v-card>
          <v-card-title primary-title>
            <div>
              <h4 class="headline mb-0">Hi {{ investorName }}</h4>
              <div>This companies are waiting for your decision</div>
            </div>
            <v-spacer></v-spacer>
            <v-card-actions>
               <v-btn
            color="green"
            dark
            @click.stop="toggleMenu"
          >
            Change criteria
          </v-btn>
            </v-card-actions>
          </v-card-title>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import { mapState, mapActions } from "vuex";
import store from "@/store/store";
export default {
  props: {
    investorName: String
  },
  components: {},
  
  methods:{
    toggleMenu(){
      this.$store
          .dispatch('investors/toggleMenu')
    }
  }
};
</script>
