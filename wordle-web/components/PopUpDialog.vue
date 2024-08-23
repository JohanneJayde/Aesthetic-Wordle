<template>
  <v-dialog v-model="modelValue" class="mx-auto" max-width="500">
    <v-card :color="color" tile class="pa-5 text-center text-white" rounded>
      <v-card-title class="text-h5 text-wrap">
        {{ title }}
      </v-card-title>
      <v-card-text class="my-3">
        <slot name="content">
          {{ message }}
        </slot>
      </v-card-text>
      <v-card-actions class="mx-auto">
        <v-btn
          :prepend-icon="actionIcon"
          variant="outlined"
          @click="emits('closePopUp')"
        >
          {{ action }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
defineProps<{
  title: string;
  message?: string;
  color: string;
  action: string;
  actionIcon: string;
}>();

const emits = defineEmits<{
  (e: "closePopUp"): void;
}>();

const modelValue = defineModel<boolean>({ default: false });
</script>
