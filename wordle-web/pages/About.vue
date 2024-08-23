import { useDisplay } from 'vuetify/lib/framework.mjs';
<template>
  <v-container>
    <v-card
      color="secondary"
      class="mx-autopa-8 rounded text-center"
      elevation="4"
    >
      <v-img
        :src="logoPath"
        alt="Logo"
        max-width="240"
        max-height="130"
        class="mx-auto"
      />

      <v-card-text
        :class="[
          $vuetify.display.smAndDown ? 'text-body-1' : '',
          'text-h6 text-cente ma-2',
        ]"
      >
        Wordle is a poetic journey through language, where each guess paints a
        delicate picture of meaning on the canvas of possibility, revealing the
        hidden hues of words until the final stroke unveils the masterpiece of
        understanding. Just start guessing and the magic begins!
      </v-card-text>
      <v-card-actions class="d-flex justify-center mb-1">
        <v-btn
          color="white"
          class="bg-primary pa-2 px-5"
          to="/"
          text="Go Home"
        />
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { useTheme } from "vuetify";

useHead({
  title: "About Us | Aesthetic Wordle",
});

const theme = useTheme();

const logoPath = computed(() => {
  return theme.global.name.value === "light" ||
    theme.global.name.value === "dark"
    ? "logo_Standard.svg"
    : "logo_" + theme.global.name.value.replace("Dark", "") + ".svg";
});
</script>
