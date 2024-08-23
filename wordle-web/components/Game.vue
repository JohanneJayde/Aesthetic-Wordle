<template>
  <v-container class="mt-2">
    <v-progress-linear
      v-if="game.gameState === GameState.Initializing"
      color="primary"
      indeterminate
    />

    <v-sheet v-else color="transparent">
      <div
        v-if="isDaily"
        class="font-text text-primary text-center text-wrap font-weight-bold mb-3"
      >
        Daily Wordle: {{ formattedDate }}
      </div>
      <div
        v-else
        class="font-text text-primary text-center text-wrap font-weight-bold mb-3"
      >
        Random Wordle
      </div>
      <v-row>
        <v-col lg="4" cols="1">
          <v-sheet
            v-if="$vuetify.display.smAndDown"
            class="position-fixed left-0 pa-1 rounded-e-lg bg-primary elevation-4"
            width="60"
          >
            <v-icon size="small" icon="mdi-timer" />
            {{ stopwatch.getCurrentTime() }}
          </v-sheet>
        </v-col>
        <v-col lg="4" cols="10" class="mb-3">
          <GameBoardGuess
            v-for="(guess, i) of game.guesses"
            :key="i"
            :guess="guess"
          />
        </v-col>
        <v-col lg="4" cols="1" v-if="$vuetify.display.mdAndUp" class="my-3">
          <v-row class="mb-1 justify-center">
            <v-sheet
              class="pa-2"
              color="primary"
              rounded
              elevation="4"
              min-width="200px"
              height="40px"
            >
              <v-icon icon="mdi-timer" />
              <strong> Current Time:</strong> {{ stopwatch.getCurrentTime() }}
            </v-sheet>
          </v-row>
          <v-row class="mb-1 justify-center">
            <v-sheet
              color="primary"
              min-width="200px"
              height="40px"
              v-ripple
              class="mx-auto pa-2 cursor-pointer"
              elevation="4"
              rounded
              @click="showWordsList = !showWordsList"
            >
              <v-icon icon="mdi-book" />
              <strong> Words List:</strong> {{ validWordsNum }}
            </v-sheet>
          </v-row>
          <v-row class="mb-1 justify-center">
            <v-sheet
              @click="isGameOver = true"
              color="primary"
              min-width="200px"
              height="40px"
              v-ripple
              class="mx-auto pa-2 cursor-pointer font-weight-bold"
              elevation="4"
              rounded
            >
              <v-icon icon="mdi-flag-variant" />
              {{ game.gameState === GameState.Playing ? "Give Up" : "Results" }}
            </v-sheet>
          </v-row>
        </v-col>
      </v-row>
      <v-bottom-navigation
        v-if="!$vuetify.display.mdAndUp"
        v-model="itemSelect"
        grow
      >
        <v-btn value="showWordsList">
          <v-icon icon="mdi-book" />
          {{ validWordsNum }}
        </v-btn>

        <v-btn value="showResult">
          <v-icon icon="mdi-flag-variant" />
          {{ game.gameState === GameState.Playing ? "Give Up" : "Results" }}
        </v-btn>
      </v-bottom-navigation>

      <Keyboard @keyup="handleClick" />

      <v-dialog v-model="isGameOver" class="mx-auto" max-width="500">
        <v-card
          :color="gameStateColor"
          tile
          class="pa-5 text-center text-white"
          rounded
        >
          <v-card-title class="text-h5 text-wrap">
            {{ gameMessage }}
          </v-card-title>
          <v-card-text v-if="game.gameState !== GameState.Playing" class="my-3">
            The word was: <strong>{{ game.solution?.toUpperCase() }}</strong>
          </v-card-text>
          <v-card-text v-else class="my-3">
            You still have <strong>{{ 6 - game.guessIndex }}</strong> attempts
            left..
          </v-card-text>
          <v-card-actions class="mx-auto">
            <v-btn variant="outlined" @click="closeGameDialog">
              <v-icon size="large" class="mr-2"> mdi-restart </v-icon> Restart
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>

      <WordList @keyup.stop v-model="showWordsList" :wordsList="wordsList" />
    </v-sheet>
  </v-container>
</template>

<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap");

.font-text {
  font-family: "Press Start 2P", sans-serif;
}
</style>

<script setup lang="ts">
import { Game, GameState } from "../scripts/game";
import { Stopwatch } from "~/scripts/stopwatch";
import nuxtStorage from "nuxt-storage";
import Axios from "axios";
import { addDays } from "date-fns";
import dateUtils from "~/scripts/dateUtils";
import {
  playClickSound,
  playEnterSound,
  playLoseSound,
  playWinSound,
} from "../scripts/soundUtils";
import type { WordDto } from "~/Models/WordDto";
import { WordService } from "~/Services/WordService";

const props = withDefaults(
  defineProps<{
    isDaily: boolean;
  }>(),
  {
    isDaily: false,
  }
);

const route = useRoute();
const gameMessage = ref("");
const gameStateColor = ref("");
const showWordsList = ref(false);
const isGameOver = ref(false);
const itemSelect = ref("");
const date = route.query.date?.toString();
const volumne = ref(0.5);
const wordsList = ref<string[]>([]);

const wordId = await getWordId();

function getWordId() {
  if (props.isDaily) {
    return WordService.getWordOfTheDayFromApi(date!);
  } else {
    return WordService.getRadnomWordFromApi();
  }
}

const game = reactive(new Game());
const validWordsNum = computed(() => game.filterValidWords().length);
provide("GAME", game);
const stopwatch = ref(new Stopwatch());

const option = ref<string | null>();

game.startNewGame(wordId);

function closeGameDialog() {
  isGameOver.value = false;
  setTimeout(() => {
    game.startNewGame(wordId);
  }, 300);
  stopwatch.value.reset();
  stopwatch.value.start();
}

const formattedDate = computed(() => {
  return dateUtils.getFormattedDateWithOrdianl(addDays(new Date(date!), 1));
});

watch(itemSelect, () => {
  if (itemSelect.value === "showWordsList") {
    showWordsList.value = true;
  } else if (itemSelect.value === "showResult") {
    isGameOver.value = true;
  }

  itemSelect.value = "";
});

watch(
  () => game.gameState,
  (newState) => {
    switch (newState) {
      case GameState.Won:
        gameMessage.value = "Congratulations! You won! 🥳";
        gameStateColor.value = "win";
        playWinSound(volumne.value);
        stopwatch.value.stop();
        isGameOver.value = true;
        break;

      case GameState.Lost:
        gameMessage.value = "You lost! Better luck next time! 😭";
        gameStateColor.value = "lose";
        playLoseSound(volumne.value);
        stopwatch.value.stop();
        isGameOver.value = true;
        break;

      case GameState.Playing:
        gameMessage.value = "Giving up already? 🤔";
        gameStateColor.value = "play";
        isGameOver.value = false;
        break;
    }
  }
);

async function handleClick(value: string) {
  if (value === "ENTER") {
    let currentGuessIndex = game.guessIndex;
    await game.submitGuess(stopwatch.value.getCurrentTime());

    if (currentGuessIndex !== game.guessIndex) {
      playEnterSound(volumne.value);
    }
  } else if (value === "👈" || value === "BACKSPACE") {
    playClickSound(volumne.value);
    game.removeLastLetter();
  } else {
    playClickSound(volumne.value);
    game.addLetter(value);
  }
}

onMounted(async () => {
  volumne.value =
    (await nuxtStorage.localStorage.getData("audioVolume")) ?? 0.5;

  stopwatch.value.start();

  Axios.get("Word/FullWordsList")
    .then((res) => res.data)
    .then((data) => {
      wordsList.value = data.items.map((word: WordDto) =>
        word.word.toLowerCase()
      );
      game.setWordsList(wordsList.value);
    });
  if (props.isDaily) {
    option.value = date;
  } else {
    option.value = null;
  }
});
</script>
