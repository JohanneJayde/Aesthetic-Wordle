<template>
  <v-container class="ma-3 mx-auto">
    <v-row>
      <v-col cols="12">
        <v-sheet class="pa-3 text-h4 text-center bg-primary rounded">
          How to Play Aesthetic Wordle
        </v-sheet>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12" lg="6">
        <v-card>
          <v-col cols="12" md="12">
            <v-card-title class="font-weight-bold text-center">
              The Color Codes
            </v-card-title>
            <v-card-item>
              <v-col
                class="pa-2 d-flex ga-4 align-center justify-center flex-wrap"
              >
                <LetterResult
                  v-for="letter in key.letters"
                  :key="letter.char"
                  :letter="letter"
                  :clickable="false"
                  class="d-flex align-center justify-center pa-4"
                  style="width: 150px"
                />
              </v-col>
            </v-card-item>
          </v-col>
          <v-col cols="12" md="12">
            <v-card-title class="text-wrap font-weight-bold text-center">
              For example, if the word is "DAISY" and you guess "BASED":
            </v-card-title>
            <v-card-item>
              <GameBoardGuess :guess="exampleWord" />
            </v-card-item>
          </v-col>
        </v-card>
      </v-col>
      <v-col cols="12" lg="6">
        <v-card>
          <v-card-title class="text-wrap font-weight-bold text-center">
            Button Controls
          </v-card-title>
          <v-card-text>
            <v-icon>mdi-timer</v-icon>
            This will start the clock and time you as you play the game. This is
            used in our leaderboard!
          </v-card-text>
          <v-card-text>
            <v-icon>mdi-book</v-icon>
            This feature is a word bank that filters words to help you guess if
            you get stuck. Use it wisely!
          </v-card-text>
          <v-card-text>
            <v-icon> mdi-flag-variant</v-icon>
            This will restart your game if you wish to reset and start over.
          </v-card-text>
          <v-card-text>
            <v-icon> mdi-account</v-icon>
            Save your name for our leaderboard. Have fun!
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-text class="text-body-1">
            There are two variations of the game you can play -
            <b>Daily Wordle</b> and <b>Random Wordle.</b> Simply choose your
            desired game mode. We have many features on the right during the
            game to help.
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { Letter, LetterState } from "~/scripts/letter";
import { Word } from "~/scripts/word";

useHead({
  title: "How to Play",
  meta: [{ name: "description", content: "Cool site!" }],
});

const exampleWord = new Word({ word: "BASED" });
exampleWord.letters = [
  new Letter("B", LetterState.Wrong),
  new Letter("A", LetterState.Correct),
  new Letter("S", LetterState.Misplaced),
  new Letter("E", LetterState.Wrong),
  new Letter("D", LetterState.Misplaced),
];

const key = {
  letters: [
    new Letter("Correct", LetterState.Correct),
    new Letter("Misplaced", LetterState.Misplaced),
    new Letter("Wrong", LetterState.Wrong),
  ],
};
</script>
