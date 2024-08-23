<template>
  <v-container>
    <v-row class="my-3">
      <v-col>
        <v-card class="h-100">
          <v-card-title class="font-weight-bold">
            {{ userData.userName }}'s Stats
          </v-card-title>
          <v-card-text>
            Total Games Played: {{ userData.gameCount }}
          </v-card-text>
          <v-card-text>
            Average Seconds Per Game:
            {{ userData.averageSecondsPerGame }}
          </v-card-text>
        </v-card>
      </v-col>
      <v-col>
        <v-row>
          <v-col cols="auto">
            <v-card width="250">
              <v-card-title class="font-weight-bold text-center">
                Average Attempts
              </v-card-title>
              <v-card-item>
                <v-progress-circular
                  :rotate="360"
                  :width="20"
                  color="warning"
                  class="mx-auto font-weight-bold d-flex justify-center align-center"
                  size="100"
                  :model-value="(userData.averageAttempts / 6) * 100"
                >
                  {{ userData.averageAttempts }}</v-progress-circular
                >
              </v-card-item>
            </v-card>
          </v-col>
          <v-col cols="auto">
            <v-card width="250">
              <v-card-title class="font-weight-bold text-center">
                Win Percentage
              </v-card-title>
              <v-card-item>
                <v-progress-circular
                  :rotate="360"
                  :width="20"
                  color="win"
                  class="mx-auto font-weight-bold d-flex justify-center align-center"
                  size="100"
                  :model-value="
                    (userData.games.filter((g) => g.isWin).length /
                      userData.gameCount) *
                    100
                  "
                >
                  {{
                    (userData.games.filter((g) => g.isWin).length /
                      userData.gameCount) *
                    100
                  }}%</v-progress-circular
                >
              </v-card-item>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
    <v-table>
      <thead>
        <tr class="bg-primary">
          <th>Date Played</th>
          <th>Win</th>
          <th>Attempts</th>
          <th>Seconds</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="game in userData.games" :key="game.id">
          <td>{{ ordinalDate(game.dateAttempted) }}</td>

          <td>
            <v-icon :color="game.isWin ? 'green' : 'red'">{{
              game.isWin ? "mdi-check" : "mdi-close"
            }}</v-icon>
          </td>
          <td>{{ game.attempts }}</td>
          <td>{{ game.seconds }}</td>
        </tr>
      </tbody>
    </v-table>
  </v-container>
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
