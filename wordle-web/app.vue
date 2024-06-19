<template>
  <v-app v-if="themeLoaded" class="full-page-gradient">
    <v-app-bar class="bg-primary" :elevation="2">
      <template v-slot:prepend>
        <v-app-bar-nav-icon variant="text" @click="drawer = !drawer" />
      </template>
      <v-img
        :src="logoPath"
        alt="Logo"
        min-width="130"
        min-height="60"
        max-width="150"
        max-height="80"
        @click="$router.push('/')"
        style="cursor: pointer"
      />
      <v-app-bar-title> </v-app-bar-title>

      <v-btn v-if="$vuetify.display.smAndUp" @click="showLoginLogOut">
        {{ tokenService.isLoggedIn() ? tokenService.getUserName() : "Log In" }}
      </v-btn>
      <v-btn
        v-else
        @click="showLoginLogOut"
        :icon="tokenService.isLoggedIn() ? 'mdi-account' : 'mdi-login'"
      />

      <v-app-bar-nav-icon
        icon="mdi-help-circle"
        @click="$router.push('/Instructions')"
      />
      <v-app-bar-nav-icon icon="mdi-cog" @click="showSettingsDialog = true" />
    </v-app-bar>
    <v-navigation-drawer
      v-model="drawer"
      location="left"
      color="secondary"
      temporary
    >
      <v-list>
        <v-list-item
          v-for="item in [
            'Daily Wordle',
            'Random Wordle',
            'About',
            'Leaderboard',
            'Instructions',
            'Latest Wordles',
            'Word Editor',
          ]"
          :key="item"
          @click="
            if (item === 'Daily Wordle') {
              $router.push(
                '/Wordle/Daily?date=' + dateUtils.getFormattedDate(new Date())
              );
            } else if (item === 'Random Wordle') {
              $router.push('/Wordle/Random');
            } else $router.push('/' + item.replaceAll(' ', ''));
          "
        >
          <v-list-item-title class="text-button">
            {{ item }}
          </v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-main> <NuxtPage /> </v-main>
    <SettingsDialogue v-model="showSettingsDialog" />
    <SignInDialog v-model="showSignInDialog" />
    <ConfirmDialog
      v-model="showConfirmDialog"
      confirm-message="Are you sure you want to logout?"
      confirmTitle="Log Out"
      confirmAction="Log Out"
      @updated="logout"
    />
  </v-app>
</template>

<script setup lang="ts">
import { useTheme } from "vuetify";
import nuxtStorage from "nuxt-storage";
import dateUtils from "./scripts/dateUtils";
import TokenService from "./scripts/tokenService";

const router = useRouter();

const tokenService = new TokenService();

useHead({
  title: "Aesthetic Wordle",
  meta: [{ name: "description", content: "Cool site!" }],
});

const theme = useTheme();
const showSettingsDialog = ref(false);
const drawer = ref(false);
const showSignInDialog = ref(false);
const showConfirmDialog = ref(false);

const logoPath = computed(() => {
  return theme.global.name.value === "light" ||
    theme.global.name.value === "dark" ||
    theme.global.name.value === undefined
    ? "/logo_Standard.svg"
    : "/logo_" + theme.global.name.value.replace("Dark", "") + ".svg";
});

const themeLoaded = ref(false);

function showLoginLogOut() {
  if (localStorage.getItem("token")) {
    showConfirmDialog.value = true;
  } else {
    showSignInDialog.value = true;
  }
}

function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("user");

  router.push("/");
}

onMounted(async () => {
  var defaultTheme =
    (await nuxtStorage.localStorage.getData("theme")) ?? "light";
  theme.global.name.value = defaultTheme;
  themeLoaded.value = true;
});
</script>

<style scoped lang="scss">
@import url("https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap");

.font-text {
  font-size: 1.2;
  font-family: "Press Start 2P", sans-serif;
}
</style>
