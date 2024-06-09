<template>
  <v-dialog v-model="modelValue" max-width="1000">
    <v-card>
      <v-sheet color="primary">
        <v-card-title> Settings </v-card-title>
      </v-sheet>
      <v-card-item>
        <ThemePicker color="transparent" />
      </v-card-item>
      <v-card-item class="mx-3">
        <v-card-title class="font-weight-bold">Audio Volume</v-card-title>
        <v-slider
          color="primary"
          min="0"
          max="1"
          show-ticks
          step=".1"
          v-model="audioVolume"
          @update:model-value="updateVolume($event)"
        >
          <template v-slot:append>
            <v-col cols=""></v-col>
            <v-sheet
              hide-details
              single-line
              dense
              outlined
              min-width="20"
              max-width="50"
              class="ml-3"
              >{{ audioVolume * 100 }}</v-sheet
            >
          </template>
        </v-slider>
      </v-card-item>
      <v-card-actions>
        <v-spacer />
        <v-btn
          color="primary"
          variant="elevated"
          justify-center
          @click="modelValue = false"
        >
          Close
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import nuxtStorage from "nuxt-storage";

const modelValue = defineModel<boolean>({ default: false });
const audioVolume = ref(0.5);

const updateVolume = (value: number) => {
  audioVolume.value = value;

  nuxtStorage.localStorage.setData("audioVolume", audioVolume.value);
};

onMounted(async () => {
  const volume = await nuxtStorage.localStorage.getData("audioVolume");
  audioVolume.value = volume ? volume : 0.5;
});
</script>
