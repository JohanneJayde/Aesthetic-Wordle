import { LetterState, type Letter } from "./letter";
import { Word } from "./word";
import Axios from "axios";
import type { GameStateDto } from "~/Models/GameStateDto";
import TokenService from "./tokenService";

export class Game {
  public maxAttempts: number;
  public guesses: Word[] = [];
  public guessIndex: number = 0;
  public gameState: GameState = GameState.Playing;
  public guessedLetters: Letter[] = [];
  public isBusy: boolean = false;
  public playerNmae: string = "";
  private _secretWordId: number = -1;
  private _solution: string | null = null;
  public wordsList: string[] = [];
  private tokenService: TokenService = new TokenService();

  private set secretWordId(value: number) {
    this._secretWordId = value;
  }
  public get secretWordId(): number {
    return this._secretWordId;
  }

  public get solution(): string | null {
    return this._solution;
  }

  constructor(maxAttempts: number = 6) {
    this.maxAttempts = maxAttempts;
    this.isBusy = true;
    this.gameState = GameState.Initializing;
  }

  public async startNewGame(date?: string | undefined) {
    this.isBusy = true;

    this.guessIndex = 0;
    this.guessedLetters = [];

    // Get a word
    if (date) {
      this.secretWordId = await this.getWordOfTheDayFromApi(date);
    } else {
      this.secretWordId = await this.getRadnomWordFromApi();
    }

    this.guesses = [];
    for (let i = 0; i < this.maxAttempts; i++) {
      this.guesses.push(new Word({ maxNumberOfLetters: 5 }));
    }

    this.gameState = GameState.Playing;
    this.isBusy = false;
  }

  public setWordsList(words: string[]) {
    this.wordsList = words;
  }

  public get guess() {
    return this.guesses[this.guessIndex];
  }

  public setGuessLetters(word: string) {
    this.guess.clear();
    for (let i = 0; i < word.length; i++) {
      this.addLetter(word[i].toUpperCase());
    }
  }

  public removeLastLetter() {
    if (this.gameState === GameState.Playing) {
      this.guess.removeLastLetter();
    }
  }

  public addLetter(letter: string) {
    if (this.gameState === GameState.Playing) {
      this.guess.addLetter(letter);
    }
  }

  public updateGuessedLetters() {
    for (const letter of this.guess.letters) {
      const index = this.guessedLetters.findIndex(
        (existingLetter) => existingLetter.char === letter.char
      );
      if (index !== -1) {
        if (this.guessedLetters[index].state !== LetterState.Correct) {
          if (letter.state !== LetterState.Wrong) {
            this.guessedLetters[index] = letter;
          }
        }
      } else {
        this.guessedLetters.push(letter);
      }
    }
  }

  public async submitGuess(currentTime: number = 0) {
    if (this.gameState !== GameState.Playing) return;
    if (!this.guess.isFilled) return;
    if (!this.isValidWord(this.guess)) {
      this.guess.clear();
      return;
    }
    const state: GameStateDto = await this.validateGuess(
      this.guess.word,
      this.guessIndex + 1,
      this.secretWordId
    );

    for (const [i, letter] of this.guess.letters.entries()) {
      letter.state = state.letterStates[i];
    }

    if (state.solution) {
      this._solution = state.solution;
    }

    this.updateGuessedLetters();

    if (state?.isWin) {
      this.gameState = GameState.Won;
    } else {
      if (this.guessIndex === this.maxAttempts - 1) {
        this.gameState = GameState.Lost;
      } else {
        this.guessIndex++;
      }
    }

    if (this.gameState === GameState.Won || this.gameState === GameState.Lost) {
      this.isBusy = true;

      if (this.tokenService.isLoggedIn()) {
        const config = {
          headers: { Authorization: `Bearer ${this.tokenService.getToken()}` },
        };

        const body = {
          attempts: this.guessIndex + 1,
          isWin: this.gameState === GameState.Won,
          wordId: this.secretWordId,
          seconds: currentTime,
        };

        await Axios.post("/Game/SaveResult", body, config);
      }

      this.isBusy = false;
    }
  }

  public async validateGuess(
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

  public async getWordOfTheDayFromApi(date: string): Promise<number> {
    try {
      let wordUrl = "word/wordOfTheDay/" + date;

      const response = await Axios.get(wordUrl);

      return response.data;
    } catch (error) {
      console.error("Error fetching word of the day:", error);
      return -1; // Probably best to print the error on screen, but this is kind of funny. :)
    }
  }

  public async getRadnomWordFromApi(): Promise<number> {
    let result: number = -1;

    try {
      let wordUrl = "word/randomWord";

      const response = await Axios.get(wordUrl);

      result = response.data;
    } catch (error) {
      console.error("Error fetching random word:", error);
    }

    return result;
  }

  public isValidWord(word: Word): boolean {
    return this.wordsList.includes(word.word.toLowerCase());
  }

  public filterValidWords(): string[] {
    return this.wordsList;
  }
}

export enum GameState {
  Playing,
  Won,
  Lost,
  Initializing,
}
