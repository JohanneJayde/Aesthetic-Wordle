import Axios from "axios";

export default class WordService {
  private static controllerEndPoint: string = "word";

  public static async getWordOfTheDayFromApi(date: string): Promise<number> {
    let result: number = -1;

    try {
      let wordUrl = this.controllerEndPoint + "/wordOfTheDay/" + date;

      const response = await Axios.get(wordUrl);

      result = response.data;
    } catch (error) {
      console.error("Error fetching word of the day:", error);
    }

    return result;
  }

  public static async getRadnomWordFromApi(): Promise<number> {
    let result: number = -1;
    try {
      let wordUrl = this.controllerEndPoint + "/randomWord";

      const response = await Axios.get(wordUrl);

      result = response.data;
    } catch (error) {
      console.error("Error fetching random word:", error);
    }

    return result;
  }
}
