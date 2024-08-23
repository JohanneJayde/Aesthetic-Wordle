<template>
  <v-dialog v-model="modelValue" width="500" @update:model-value="close">
    <v-card>
      <v-sheet color="primary mb-3">
        <v-card-title class="text-wrap">{{
          currentPage === 0 ? "Sign in" : "Register"
        }}</v-card-title>
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
      <v-tabs-window v-model="currentPage" class="mt-3">
        <v-tabs-window-item>
          <v-form v-model="validateSignIN" @submit.prevent>
            <v-col>
              <v-text-field
                v-model="email"
                :rules="emailRule"
                @keyup.stop
                label="Email"
                type="email"
                variant="outlined"
              />
            </v-col>
            <v-col>
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
            </v-col>
          </v-form>
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
          <v-form v-model="validateRegister" @submit.prevent>
            <v-col>
              <v-text-field
                v-model="email"
                @keyup.stop
                label="Email"
                :rules="emailRule"
                type="email"
                variant="outlined"
              />
            </v-col>
            <v-col>
              <v-text-field
                v-model="userName"
                @keyup.stop
                :rules="[() => !!userName || 'Username is required']"
                label="Username"
                variant="outlined"
              />
            </v-col>
            <v-col>
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
            </v-col>
            <v-col>
              <v-text-field
                v-model="passwordConfirm"
                @keyup.stop
                label="Password"
                :type="showPassword ? 'text' : 'password'"
                :append-inner-icon="
                  showPasswordConfirm
                    ? 'mdi-eye-off-outline'
                    : 'mdi-eye-outline'
                "
                :rules="confirmPasswordRule"
                variant="outlined"
                @click:append-inner="showPasswordConfirm = !showPasswordConfirm"
              />
            </v-col>
          </v-form>

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
import TokenService from "~/scripts/TokenService";
import { useRouter } from "vue-router";

const router = useRouter();
const tokenService = new TokenService();

const modelValue = defineModel<boolean>({ default: false });
const showPassword = ref(false);
const userName = ref("");
const password = ref("");
const showPasswordConfirm = ref(false);
const passwordConfirm = ref("");
const email = ref("");
const errorMessage = ref("");
const currentPage = ref();
const validateSignIN = ref(false);
const validateRegister = ref(false);

const emailRule = [
  (emailValue: string) => !!emailValue || "E-mail is required",
  (emailValue: string) =>
    /.+@.+\..+/.test(emailValue) || "E-mail must be valid",
];

const passwordRule = [
  (passwordValue: string) => !!passwordValue || "Password is required",
  (passwordValue: string) =>
    passwordValue.length >= 8 || "Password must be at least 8 characters",
  (passwordValue: string) =>
    /[A-Z]/.test(passwordValue) || "Password must contain an uppercase letter",
  (passwordValue: string) =>
    /[a-z]/.test(passwordValue) || "Password must contain a lowercase letter",
  (passwordValue: string) =>
    /\d/.test(passwordValue) || "Password must contain a number",
  (passwordValue: string) =>
    /\W/.test(passwordValue) || "Password must contain a special character",
];

const confirmPasswordRule = [
  (passwordValue: string) => !!passwordValue || "Password is required",
  (passwordValue: string) =>
    passwordValue === password.value || "Passwords do not match",
];

watch(
  () => currentPage.value,
  () => {
    errorMessage.value = "";
    email.value = "";
    password.value = "";
    userName.value = "";
    passwordConfirm.value = "";
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
      close();
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
  email.value = "";
  passwordConfirm.value = "";
  currentPage.value = 0;
  modelValue.value = false;
}
</script>
