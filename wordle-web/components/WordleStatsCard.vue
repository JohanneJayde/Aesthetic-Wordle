<template>
  <v-card elevation="5">
    <v-sheet color="primary" class="py-2">
      <v-row>
        <v-col>
          <V-card-title v-if="isDaily">{{ ordinalDate }}</V-card-title>
          <v-card-title v-else>{{ gameStat.word }}</v-card-title>
        </v-col>
        <v-col class="d-flex justify-end align-center mr-3">
          <v-chip
            v-if="hasPlayed"
            prepend-icon="mdi-check"
            variant="flat"
            size="large"
            class="text-white"
            color="win"
          >
            Completed</v-chip
          >
        </v-col>
      </v-row>
    </v-sheet>
    <v-card-item>
      <v-row>
        <v-col cols="6" lg="12" xl="5" md="12">
          <v-list density="compact">
            <v-list-item class="font-weight-bold"
              >Total Wins: {{ gameStat.totalWins }}</v-list-item
            >
            <v-list-item class="font-weight-bold"
              >Total Losses: {{ gameStat.totalLosses }}</v-list-item
            >
            <v-list-item class="font-weight-bold"
              >Average Seconds:
              {{ gameStat.averageSeconds.toFixed(2) }}</v-list-item
            >
            <v-list-item class="font-weight-bold"
              >User Plays: {{ gameStat.users.length }}</v-list-item
            >
          </v-list>
        </v-col>
        <v-col cols="6" class="d-flex" lg="12" xl="7" md="12">
          <v-col class="d-flex flex-column">
            <span class="font-weight-bold text-center mb-2">Attempts</span>
            <v-progress-circular
              :rotate="360"
              :width="10"
              color="win"
              class="mx-auto font-weight-bold d-flex justify-center align-center"
              size="80"
              :model-value="averageAttempts"
            >
              {{ averageAttempts }}%</v-progress-circular
            >
          </v-col>
          <v-col class="d-flex flex-column">
            <span class="font-weight-bold text-center mb-2">Win %</span>
            <v-progress-circular
              :rotate="360"
              :width="10"
              color="warning"
              class="mx-auto font-weight-bold d-flex justify-center align-center"
              size="80"
              :model-value="winPercentage"
            >
              {{ winPercentage }}%</v-progress-circular
            >
          </v-col>
        </v-col>
      </v-row>
    </v-card-item>
    <v-card-actions>
      <v-btn
        v-if="isDaily"
        class="pa-2 ml-4 mb-3 bg-primary"
        color="white"
        :to="`/Wordle/Daily?date=${formattedDate}`"
        >Play Word</v-btn
      ></v-card-actions
    >
    <v-sheet color="primary" height="5px" class="mb-0" />
  </v-card>
</template>

<script setup lang="ts">
import { addDays } from "date-fns";
import type { GameStats } from "~/Models/GameStas";
import dateUtils from "~/scripts/dateUtils";
import TokenService from "~/scripts/TokenService";

const props = withDefaults(
  defineProps<{
    gameStat: GameStats;
    isDaily: boolean;
    inCurrentGame: boolean;
  }>(),
  {
    isDaily: false,
    inCurrentGame: true,
  }
);

const tokenService = new TokenService();

const winPercentage = computed(() => {
  if (props.gameStat.totalGames === 0) {
    return "0";
  }
  return ((props.gameStat.totalWins / props.gameStat.totalGames) * 100).toFixed(
    2
  );
});

const averageAttempts = computed(() => {
  return ((props.gameStat.averageGuesses / 6) * 100).toFixed(2);
});

const ordinalDate = computed(() => {
  return dateUtils.getFormattedDateWithOrdianl(
    addDays(new Date(props.gameStat.date), 1)
  );
});

const formattedDate = computed(() => {
  return dateUtils.getFormattedDate(addDays(new Date(props.gameStat.date), 1));
});

const hasPlayed = computed(() => {
  return props.gameStat.users.some(
    (user) => user.userName === tokenService.getUserName()
  );
});
</script>
