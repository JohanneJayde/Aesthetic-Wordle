<template>
  <v-bottom-sheet v-model="modelValue">
    <v-card>
      <v-card-item>
        <v-virtual-scroll
          :items="game?.filterValidWords() || []"
          :item-height="50"
          height="300"
        >
          <template v-slot="{ item }">
            <v-row no-gutters>
              <v-col>
                <v-btn
                  :disabled="game?.gameState !== GameState.Playing"
                  class="w-100"
                  height="50"
                  @click="addGuess(item)"
                  flat
                  >{{ item }}</v-btn
                >
              </v-col>
            </v-row>
          </template>
        </v-virtual-scroll>
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
const game: Game | undefined = inject("GAME");

function addGuess(word: string) {
  game?.guess.clear();
  for (let i = 0; i < word.length; i++) {
    game?.addLetter(word[i].toUpperCase());
  }
  modelValue.value = false;
}
</script>
