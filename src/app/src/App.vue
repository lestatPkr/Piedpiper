<template>
  <v-app>
    <v-toolbar app>
      <v-btn icon>
    </v-btn>
       <img height="50"  src="@/assets/pp-logo.png"  />
       
      <v-toolbar-title class="headline text-uppercase">
       <span>piedpiper</span>
        <span class="font-weight-light">VENTURES</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
    
      <v-btn icon>
      <v-icon>apps</v-icon>
    </v-btn>

    <v-btn icon>
      <v-icon>refresh</v-icon>
    </v-btn>

    
    </v-toolbar>

    <v-content>
      <router-view/>
      <template>
  
</template>
    </v-content>
  </v-app>
</template>

<script>

export default {
  name: 'App',
  data () {
    return {
      //
    }
  }
}
</script>
