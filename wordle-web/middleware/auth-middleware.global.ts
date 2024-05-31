import TokenService from "~/scripts/tokenService";

export default defineNuxtRouteMiddleware((to, from) => {
  const tokenService = new TokenService();

  console.log("here");
  switch (to.path) {
    case "/test":
      if (!tokenService.isLoggedIn()) {
        return navigateTo("/403");
      }
      break;
  }
});
