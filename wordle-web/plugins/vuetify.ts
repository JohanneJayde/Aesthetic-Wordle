/// import this after install `@mdi/font` package
import "@mdi/font/css/materialdesignicons.css";
import colors from "vuetify/lib/util/colors"; // Corrected import path

import "vuetify/styles";
import { createVuetify, type ThemeDefinition } from "vuetify";

const SapphireDeepSeaDive: ThemeDefinition = {
  dark: false,
  colors: {
    primary: colors.indigo.accent2,
    secondary: colors.indigo.accent1,
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.lighten1,
    unknown: colors.grey.lighten3,
    background: colors.blue.lighten4,
  },
};
const SapphireDeepSeaDiveDark: ThemeDefinition = {
  dark: true,
  colors: {
    primary: colors.indigo.accent2,
    secondary: colors.indigo.accent1,
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.darken2,
    unknown: colors.grey.darken3,
    background: "#121212",
  },
};

const OpalOpulence: ThemeDefinition = {
  dark: false,
  colors: {
    primary: colors.blue.lighten4,
    secondary: colors.pink.lighten4,
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.lighten1,
    unknown: colors.grey.lighten3,
    background: colors.blueGrey.lighten5,
  },
};
const OpalOpulenceDark: ThemeDefinition = {
  dark: true,
  colors: {
    primary: colors.blue.lighten4,
    secondary: colors.pink.lighten4,
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.darken2,
    unknown: colors.grey.darken3,
  },
};

const EmeraldIsle: ThemeDefinition = {
  dark: false,
  colors: {
    primary: "#388E3C",
    secondary: "#8bc34a",
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.lighten1,
    unknown: colors.grey.lighten3,
    background: colors.lime.accent1,
  },
};
const EmeraldIsleDark: ThemeDefinition = {
  dark: true,
  colors: {
    primary: "#388E3C",
    secondary: "#8bc34a",
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.darken2,
    unknown: colors.grey.darken3,
    background: "#121212",
  },
};

const RubyRoyale: ThemeDefinition = {
  dark: false,
  colors: {
    primary: "#D50000",
    secondary: colors.red.accent3,
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.lighten1,
    unknown: colors.grey.lighten3,
    background: colors.red.lighten2,
  },
};
const RubyRoyaleDark: ThemeDefinition = {
  dark: true,
  colors: {
    primary: "#D50000",
    secondary: colors.red.accent3,
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.darken2,
    unknown: colors.grey.darken3,
    background: "#121212",
  },
};
const AmethystTwilightMist: ThemeDefinition = {
  dark: false,
  colors: {
    primary: "#4A148C",
    secondary: "#AA00FF",
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.lighten1,
    unknown: colors.grey.lighten3,
    background: colors.purple.lighten4,
  },
};
const AmethystTwilightMistDark: ThemeDefinition = {
  dark: true,
  colors: {
    primary: "#4A148C",
    secondary: "#AA00FF",
    lose: colors.red.lighten1,
    win: colors.green.accent3,
    play: colors.blue.lighten1,
    correct: colors.green.accent4,
    misplaced: colors.yellow.darken1,
    wrong: colors.grey.darken2,
    unknown: colors.grey.darken3,
    background: "#121212",
  },
};

export default defineNuxtPlugin((app) => {
  const vuetify = createVuetify({
    theme: {
      defaultTheme: "dark",
      themes: {
        SapphireDeepSeaDive,
        EmeraldIsle,
        AmethystTwilightMist,
        RubyRoyale,
        OpalOpulence,
        SapphireDeepSeaDiveDark,
        EmeraldIsleDark,
        AmethystTwilightMistDark,
        RubyRoyaleDark,
        OpalOpulenceDark,
        light: {
          dark: false,
          colors: {
            primary: colors.pink.accent2,
            secondary: colors.pink.accent1,
            accent: colors.pink.accent3,
            lose: colors.red.lighten1,
            win: colors.green.accent3,
            play: colors.blue.lighten1,
            correct: colors.green.accent4,
            misplaced: colors.yellow.darken1,
            wrong: colors.grey.lighten1,
            unknown: colors.grey.lighten3,
            background: colors.pink.lighten4,
          },
        },
        dark: {
          dark: true,
          colors: {
            primary: colors.pink.accent2,
            secondary: colors.pink.accent1,
            accent: colors.pink.accent3,
            lose: colors.red.lighten1,
            win: colors.green.accent3,
            play: colors.blue.lighten1,
            correct: colors.green.accent4,
            misplaced: colors.yellow.darken1,
            wrong: colors.grey.darken2,
            unknown: colors.grey.darken3,
            background: "#121212",
          },
        },
      },
    },
  });
  app.vueApp.use(vuetify);
});
