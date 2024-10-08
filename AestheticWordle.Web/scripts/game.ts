import { LetterState, type Letter } from "./letter";
import { Word } from "./word";
import type { GameStateDto } from "~/Models/GameStateDto";

export class Game {
  public maxAttempts: number;
  public guesses: Word[] = [];
  public guessIndex: number = 0;
  public gameState: GameState = GameState.Playing;
  public guessedLetters: Letter[] = [];
  private _solution: string | null = null;
  public wordsList: string[] = [];

  public get solution(): string | null {
    return this._solution;
  }

  constructor(maxAttempts: number = 6) {
    this.maxAttempts = maxAttempts;
    this.gameState = GameState.Initializing;
  }

  public async startNewGame(words: string[]) {
    this.guessIndex = 0;
    this.guessedLetters = [];
    this.wordsList = words;

    this.guesses = [];
    for (let i = 0; i < this.maxAttempts; i++) {
      this.guesses.push(new Word({ maxNumberOfLetters: 5 }));
    }

    this.gameState = GameState.Playing;
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

  public submitGuess(state: GameStateDto) {
    for (const [i, letter] of this.guess.letters.entries()) {
      letter.state = state.letterStates[i];
    }

    if (state.solution) {
      this._solution = state.solution;
    }

    this.updateGuessedLetters();

    if (state.isWin) {
      this.gameState = GameState.Won;
    } else {
      if (this.guessIndex === this.maxAttempts - 1) {
        this.gameState = GameState.Lost;
      } else {
        this.guessIndex++;
      }
    }
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
