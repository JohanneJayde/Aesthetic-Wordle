<template>Welcome {{ username }}</template>

<script setup lang="ts">
import TokenService from "~/scripts/TokenService";

const tokenService = new TokenService();

const username = computed(() =>
  tokenService.getUserName() === "" ? "Guest" : tokenService.getUserName()
);
</script>
