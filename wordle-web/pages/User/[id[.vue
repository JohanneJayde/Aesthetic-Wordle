<template>
  <h3>{{ userData.userName }}</h3>
  <h3>{{ userData.averageAttempts }}</h3>
  <h3>{{ userData.averageSecondsPerGame }}</h3>
  <h3>{{ userData.gameCount }}</h3>
  <v-table>
    <thead>
      <tr>
        <th>Win</th>
        <th>Attempts</th>
        <th>Seconds</th>
        <th>Date</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="game in userData.games" :key="game.id">
        <td>{{ game.isWin }}</td>
        <td>{{ game.attempts }}</td>
        <td>{{ game.seconds }}</td>
        <td>{{ ordinalDate(game.dateAttempted) }}</td>
      </tr>
    </tbody>
  </v-table>
</template>

<script setup lang="ts">
import Axios from "axios";
import dateUtils from "~/scripts/dateUtils";

const route = useRoute();

const userId = route.params.id;

function ordinalDate(date: string): string {
  return dateUtils.getFormattedDateWithOrdianl(new Date(date));
}

const res = await Axios.get("/User/Get?userId=" + userId);

const userData = res.data;
</script>
