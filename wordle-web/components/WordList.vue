<template>
  <v-bottom-sheet v-model="modelValue">
    <v-card>
      <v-card-item>
        <v-row v-for="word in game?.filterValidWords()" :key="word" no-gutters>
          <v-col>
            <v-btn class="w-100" height="50" @click="addGuess(word)">{{
              word
            }}</v-btn>
          </v-col>
        </v-row>
      </v-card-item>
      <v-card-actions>
        <v-btn @click="modelValue = false">Close</v-btn>
      </v-card-actions>
    </v-card>
  </v-bottom-sheet>
</template>

<script setup lang="ts">
import type { Game } from "~/scripts/game";

const modelValue = defineModel<boolean>({ default: false });
const game: Game | undefined = inject("GAME");

function addGuess(word: string) {
  game?.guess.clear();
  for (let i = 0; i < word.length; i++) {
    game?.addLetter(word[i].toUpperCase());
  }
  modelValue.value = false;
}
</script>
