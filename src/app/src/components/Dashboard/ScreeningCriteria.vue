<template>
    <div>
        
  <v-layout row>
    <v-flex xs12>
      <v-card>
        <v-toolbar color="cyan" dark>
          <v-toolbar-side-icon></v-toolbar-side-icon>

          <v-toolbar-title>Screening criteria</v-toolbar-title>

          <v-spacer></v-spacer>

         
        </v-toolbar>
 
        <v-list two-line>
          <template v-for="(item, index) in list1">
            <draggable class="list-group" :list="list1" :key="index" :options="{ group: 'people' }">
            <v-list-tile
              :key="item.title"
              avatar>
              

              <v-list-tile-content>
                <v-list-tile-title v-html="item"></v-list-tile-title>
                <!-- <v-list-tile-sub-title v-html="item.subtitle"></v-list-tile-sub-title> -->
              </v-list-tile-content>
            </v-list-tile>
            </draggable>
            
          </template>
        </v-list>
        <v-list two-line>
          <template v-for="(item, index) in list2">
            <draggable class="list-group" :list="list2" :key="index" :options="{ group: 'people' }">
            <v-list-tile
              :key="item.title"
              avatar>
              

              <v-list-tile-content>
                <v-list-tile-title v-html="item"></v-list-tile-title>
                <!-- <v-list-tile-sub-title v-html="item.subtitle"></v-list-tile-sub-title> -->
              </v-list-tile-content>
            </v-list-tile>
            </draggable>
            
          </template>
        </v-list>
        <v-list two-line>
          <template v-for="(item, index) in list3">
            <draggable class="list-group" :list="list3" :key="index" :options="{ group: 'people' }">
            <v-list-tile
              :key="item.title"
              avatar>
              

              <v-list-tile-content>
                <v-list-tile-title v-html="item"></v-list-tile-title>
                <!-- <v-list-tile-sub-title v-html="item.subtitle"></v-list-tile-sub-title> -->
              </v-list-tile-content>
            </v-list-tile>
            </draggable>
            
          </template>
        </v-list>
      </v-card>
    </v-flex>
  </v-layout>

    </div>
    
</template>

<script>
import draggable from 'vuedraggable'
export default {
    components:{
        draggable
    },
    data(){
        return{
            list1:[
                "Texto 1",
                "Texto 2",
                "Texto 3"
            ],
            list2:[
                "Texto 4",
                "Texto 5",
                "Texto 6"
            ],
            list3:[
                "Texto 7",
                "Texto 8",
                "Texto 9"
            ]
        }
    },
    
}
</script>

<style>

</style>
