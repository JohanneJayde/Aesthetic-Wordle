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
}
