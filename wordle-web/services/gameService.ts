import Axios from "axios";
import type { GameStateDto } from "~/Models/GameStateDto";

export default class GameService {
  public static async validateGuess(
    guess: string,
    attemptNumber: number,
    wordId: number
  ): Promise<GameStateDto> {
    const response = await Axios.post("/Game/Guess", {
      guess: guess,
      attemptNumber: attemptNumber,
      wordId: wordId,
    });

    return response.data;
  }

  public static async submitGame(
    attempts: number,
    wordId: number,
    isWin: boolean,
    seconds: number,
    token: string
  ) {
    const config = {
      headers: { Authorization: `Bearer ${token}` },
    };

    const body = {
      attempts: attempts + 1,
      isWin: isWin,
      wordId: wordId,
      seconds: seconds,
    };

    await Axios.post("/Game/SaveResult", body, config);
  }
}
