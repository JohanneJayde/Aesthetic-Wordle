<template>
  <v-dialog v-model="modelValue" width="500">
    <v-card>
      <v-sheet color="primary mb-3">
        <v-card-title class="text-wrap">Sign in</v-card-title>
      </v-sheet>
      <v-alert
        v-if="errorMessage"
        type="error"
        title-date-format="Function"
        rounded
        class="mx-3"
      >
        {{ errorMessage }}
      </v-alert>
      <v-tabs v-model="currentPage" align-tabs="center">
        <v-tab>Sign In</v-tab>
        <v-tab>Register</v-tab>
      </v-tabs>
      <v-tabs-window v-model="currentPage">
        <v-tabs-window-item>
          <v-card-text>
            <v-form v-model="validateSignIN" @submit.prevent>
              <v-text-field
                v-model="email"
                :rules="emailRule"
                @keyup.stop
                label="Email"
                type="email"
                variant="outlined"
              />
              <v-col />
              <v-text-field
                v-model="password"
                @keyup.stop
                label="Password"
                :type="showPassword ? 'text' : 'password'"
                :append-inner-icon="
                  showPassword ? 'mdi-eye-off-outline' : 'mdi-eye-outline'
                "
                variant="outlined"
                @click:append-inner="showPassword = !showPassword"
              />
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="primary" variant="tonal" @click="close">
              Cancel
            </v-btn>
            <v-btn color="primary" variant="flat" type="submit" @click="signIn">
              Sign In
            </v-btn>
          </v-card-actions>
        </v-tabs-window-item>
        <v-tabs-window-item>
          <v-card-text>
            <v-form v-model="validateRegister" @submit.prevent>
              <v-text-field
                v-model="email"
                @keyup.stop
                label="Email"
                :rules="emailRule"
                type="email"
                variant="outlined"
              />
              <v-text-field
                v-model="userName"
                @keyup.stop
                label="Username"
                variant="outlined"
              />
              <v-text-field
                v-model="password"
                @keyup.stop
                label="Password"
                :type="showPassword ? 'text' : 'password'"
                :append-inner-icon="
                  showPassword ? 'mdi-eye-off-outline' : 'mdi-eye-outline'
                "
                :rules="passwordRule"
                variant="outlined"
                @click:append-inner="showPassword = !showPassword"
              />
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="primary" variant="tonal" @click="close">
              Cancel
            </v-btn>
            <v-btn
              color="primary"
              variant="flat"
              type="submit"
              @click="register"
            >
              Register
            </v-btn>
          </v-card-actions>
        </v-tabs-window-item>
      </v-tabs-window>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import axios from "axios";
import TokenService from "~/scripts/tokenService";
import { useRouter } from "vue-router";

const router = useRouter();
const tokenService = new TokenService();

const modelValue = defineModel<boolean>({ default: false });
const showPassword = ref(false);
const userName = ref("");
const password = ref("");
const email = ref("");
const errorMessage = ref("");
const currentPage = ref();
const validateSignIN = ref(false);
const validateRegister = ref(false);

const emailRule = [
  (v: string) => !!v || "E-mail is required",
  (v: string) => /.+@.+\..+/.test(v) || "E-mail must be valid",
];

const passwordRule = [
  (v: string) => !!v || "Password is required",
  (v: string) => v.length >= 8 || "Password must be at least 8 characters",
  (v: string) => /[A-Z]/.test(v) || "Password must contain an uppercase letter",
  (v: string) => /[a-z]/.test(v) || "Password must contain a lowercase letter",
  (v: string) => /\d/.test(v) || "Password must contain a number",
  (v: string) => /\W/.test(v) || "Password must contain a special character",
];

watch(
  () => currentPage.value,
  () => {
    errorMessage.value = "";
    email.value = "";
    password.value = "";
    userName.value = "";
  }
);

function signIn() {
  errorMessage.value = "";
  if (!validateSignIN.value) return;

  axios
    .post("/Token/GetToken", {
      email: email.value,
      password: password.value,
    })
    .then((response) => {
      tokenService.setToken(response.data.token);
      modelValue.value = false;
      router.push("/");
    })
    .catch((error) => {
      errorMessage.value = error.response.data;
    });
}

function register() {
  if (!validateRegister.value) return;

  axios
    .post("/Account/Register/", {
      username: userName.value,
      password: password.value,
      email: email.value,
    })
    .then(() => {
      signIn();
    })
    .catch((error) => {
      errorMessage.value = error.response.data;
    });
}

function close() {
  userName.value = "";
  password.value = "";
  modelValue.value = false;
}
</script>
