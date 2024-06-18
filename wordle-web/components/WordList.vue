<template>
  <v-bottom-sheet v-model="modelValue">
    <v-card>
      <v-card-item v-for="word in props.wordsList" :key="word">
        <v-card-title>{{ word }}</v-card-title>
      </v-card-item>
      <v-card-actions class="mx-auto"> </v-card-actions>
    </v-card>
  </v-bottom-sheet>
</template>

<script setup lang="ts">
const modelValue = defineModel<boolean>({ default: false });

const props = defineProps<{
  wordsList: string[];
}>();
</script>
