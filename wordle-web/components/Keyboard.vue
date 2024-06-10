<template>
  <v-row
    class="justify-center"
    v-for="(keyboardRow, rowIndex) in keyboardLetterRows"
    :key="rowIndex"
    dense
  >
    <v-col
      cols="auto"
      v-for="letter in keyboardRow"
      :key="letter.char"
      :class="'ml-1 px-0'"
    >
      <LetterResult
        :letter="letter"
        :clickable="true"
        :is-keyboard="true"
        @click="handleLetterClick(letter)"
      />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { Game } from "~/scripts/game";
import { Letter } from "~/scripts/letter";
import { playClickSound, playEnterSound } from "~/scripts/soundUtils";
import nuxtStorage from "nuxt-storage";

const game: Game | undefined = inject("GAME");
const emit = defineEmits<{
  (e: "keyup", value: string): string;
}>();
const volume = ref(0.5);

const keyboardLetterRows = computed(() => {
  let keyboardLetterRows: Letter[][] = [];

  const keyboardRows = [
    ["Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P"],
    ["A", "S", "D", "F", "G", "H", "J", "K", "L"],
    ["ENTER", "Z", "X", "C", "V", "B", "N", "M", "👈"],
  ];

  for (let keyboardRow of keyboardRows) {
    let keyboardLetterRow: Letter[] = [];
    for (let key of keyboardRow) {
      keyboardLetterRow.push(
        game.guessedLetters.find((letter) => letter.char === key) ??
          new Letter(key)
      );
    }
    keyboardLetterRows.push(keyboardLetterRow);
  }

  return keyboardLetterRows;
});

function onKeyup(event: KeyboardEvent) {
  console.log(event.key);
  if (
    event.key.match(/[A-z]/) &&
    event.key.length !== 1 &&
    event.key !== "Enter" &&
    event.key !== "Backspace"
  )
    return;

  emit("keyup", event.key.toUpperCase());
}

const handleLetterClick = (letter: Letter) => {
  playClickSound(volume.value);
  if (letter.char === "ENTER") {
    playEnterSound(volume.value);
  }
  emit("keyup", letter.char);
};

onMounted(async () => {
  window.addEventListener("keyup", onKeyup);

  const storedVolume = await nuxtStorage.localStorage.getData("audioVolume");
  volume.value = storedVolume ? storedVolume : 0;
});

onUnmounted(() => {
  window.removeEventListener("keyup", onKeyup);
});
</script>
