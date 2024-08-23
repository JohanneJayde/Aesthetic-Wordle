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
      <v-row no-gutters>
        <v-col cols="12" class="mb-3">
          <span><b>Win Percentage:</b> {{ winPercentage }}%</span>
          <v-progress-linear
            :model-value="winPercentage"
            color="win"
            height="10"
          />
        </v-col>
        <v-col cols="12" class="mb-3">
          <span
            ><b>Average Attempts:</b>
            {{ gameStat.averageGuesses.toFixed(2) }}</span
          >
          <v-progress-linear
            :model-value="averageAttempts"
            color="warning"
            height="10"
          />
        </v-col>

        <v-col>
          <v-list>
            <span>
              <b
                >Total Wins
                <v-icon icon="mdi-trophy" color="primary" size="small" />:</b
              >
              {{ gameStat.totalWins }} </span
            ><br />
            <span>
              <b
                >Total Losses
                <v-icon icon="mdi-close" color="primary" size="small" />:</b
              >
              {{ gameStat.totalLosses }} </span
            ><br />
            <span>
              <b
                >Average Seconds
                <v-icon icon="mdi-clock" color="primary" size="small" />:</b
              >
              {{ gameStat.averageSeconds.toFixed(2) }} </span
            ><br />
            <span>
              <b
                >User Plays
                <v-icon
                  icon="mdi-account-multiple"
                  color="primary"
                  size="small"
                />:</b
              >
              {{ gameStat.users.length }} </span
            ><br />
          </v-list>
        </v-col>
      </v-row>
    </v-card-item>
    <v-card-actions>
      <v-btn
        v-if="isDaily"
        class="pa-2 ml-2 mb-3 bg-primary"
        color="white"
        :disabled="hasPlayed"
        :to="`/Wordle/Daily?date=${formattedDate}`"
        >Play Word</v-btn
      >
    </v-card-actions>
    <v-sheet color="primary" height="5px" class="mb-0" />
  </v-card>
</template>

<script setup lang="ts">
import { addDays } from "date-fns";
import type { GameStats } from "~/Models/GameStas";
import dateUtils from "~/scripts/dateUtils";
import TokenService from "~/services/tokenService";

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
