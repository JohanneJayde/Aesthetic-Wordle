export default class TokenService {
  private tokenKey: string = "token";

  public setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  public getToken(): string {
    return localStorage.getItem(this.tokenKey) ?? "";
  }

  public isLoggedIn(): boolean {
    if (this.isExpired()) {
      this.logout();
      return false;
    }

    return this.getToken() !== "";
  }

  public isExpired(): boolean {
    const token = this.getToken();
    if (token === "") {
      return true;
    }
    const expiration = JSON.parse(atob(token.split(".")[1])).exp;
    return Date.now() >= expiration * 1000;
  }

  public logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  }

  public getUserName() {
    const token = this.getToken();
    if (token === "") {
      return "";
    }
    return JSON.parse(atob(token.split(".")[1])).userName;
  }

  public getUserId() {
    const token = this.getToken();
    if (token === "") {
      return "";
    }
    return JSON.parse(atob(token.split(".")[1])).userId;
  }

  public canDeleteAndAdd() {
    const token = this.getToken();
    if (token === "") {
      return false;
    }
    const birthday = JSON.parse(atob(token.split(".")[1])).BirthDate;
    const masterOfTheUniverse = JSON.parse(
      atob(token.split(".")[1])
    ).MasterOfTheUniverse;
    const year = new Date().getFullYear();
    const birthYear = new Date(birthday).getFullYear();

    return year - birthYear >= 21 && masterOfTheUniverse;
  }

  public generateTokenHeader() {
    return { Authorization: `Bearer ${this.getToken()}` };
  }
}
