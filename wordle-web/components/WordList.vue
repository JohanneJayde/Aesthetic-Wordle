<template>
  <v-bottom-sheet v-model="modelValue">
    <v-card>
      <v-card-item>
        <v-data-table
          :items="game?.filterValidWords() || []"
          hide-default-header
        >
          <template #top>
            <v-autocomplete
              :items="game?.filterValidWords()"
              variant="outlined"
              v-model="searchWord"
              :disabled="game?.gameState !== GameState.Playing"
              @update:model-value="addGuess(searchWord)"
            />
          </template>
          <template v-slot:item="{ item }">
            <v-btn
              :disabled="game?.gameState !== GameState.Playing"
              class="w-100"
              height="50"
              @click="addGuess(item)"
              flat
              >{{ item }}</v-btn
            >
          </template>
        </v-data-table>
      </v-card-item>

      <v-card-actions>
        <v-btn color="primary" @click="modelValue = false">Close</v-btn>
      </v-card-actions>
    </v-card>
  </v-bottom-sheet>
</template>

<script setup lang="ts">
import type { Game } from "~/scripts/game";
import { GameState } from "~/scripts/game";

const modelValue = defineModel<boolean>({ default: false });

const searchWord = ref("");

const game: Game | undefined = inject("GAME");

function addGuess(word: string) {
  searchWord.value = "";
  game?.guess.clear();
  for (let i = 0; i < word.length; i++) {
    game?.addLetter(word[i].toUpperCase());
  }
  modelValue.value = false;
}
</script>
