<template>
  <v-container grid-list-md>
    <v-layout row wrap>
      <v-flex xs12>
        <v-card>
            <v-card-title primary-title>
              <div>
                <h4 class="headline mb-0">Hi {{investorName}} </h4>
                <div>This companies are waiting for your decision</div>
              </div>
              <v-spacer></v-spacer>
              <v-card-actions>
              
              
                <v-btn text-color="primary">
                  <v-icon color="success">add_circle</v-icon> 
                  <span text-color="primary">Add company</span>
                </v-btn>
            </v-card-actions>
            </v-card-title>
            
          </v-card>
      </v-flex>
     
    </v-layout>
  </v-container>
</template>

<script>


  export default {
    props:{
      investorName: String
    },
    components: {
    },
    
    
  }
</script>
