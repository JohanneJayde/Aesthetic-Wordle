<template>
  <v-container>
    <v-progress-linear
      v-if="isDailyWordlesLoading"
      class="mx-auto"
      color="primary"
      height="10"
      indeterminate
      rounded
      width="75%"
    />
    <div v-else>
      <div class="text-h4 my-5 font-weight-bold text-primary">
        Lastest Wordles
      </div>
      <v-row>
        <v-col
          v-for="(gameStat, i) in gameStats"
          :key="i"
          cols="12"
          sm="12"
          md="6"
          lg="4"
          xl="3"
        >
          <WordleStatsCard
            :gameStat="gameStat"
            :isDaily="true"
            :inCurrentGame="false"
          />
        </v-col>
      </v-row>
    </div>
  </v-container>
</template>

<script setup lang="ts">
import Axios from "axios";
import { format } from "date-fns";
import WordleStatsCard from "~/components/WordleStatsCard.vue";
import type { GameStats } from "~/Models/GameStas";

const isDailyWordlesLoading = ref(true);
const date = ref("");

useHead({
  title: "Lastest Wordles | Aesthetic Wordle",
});

const gameStats = ref<GameStats[]>([]);

onMounted(() => {
  const formatDate = format(new Date(), "MM-dd-yyyy");
  date.value = formatDate;
  Axios.get("game/LastTenWordOfTheDayStats/" + formatDate)
    .then((res: { data: any }) => res.data)
    .then((data: any) =>
      data.map((data: any) => ({
        totalGames: data.totalTimesPlayed,
        totalWins: data.totalWins,
        totalLosses: data.totalLosses,
        averageSeconds: data.averageSeconds,
        date: data.date,
        word: data.word,
        averageGuesses: data.averageGuesses,
        users: data.users,
      }))
    )
    .then((gameStatData: GameStats[]) => {
      isDailyWordlesLoading.value = false;
      gameStats.value = gameStatData;
    });
});
</script>
