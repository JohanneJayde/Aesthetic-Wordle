import { Letter, LetterState } from "./letter";

export class Word {
  public letters: Letter[];

  constructor(wordOptions: WordOptions) {
    if (wordOptions.word) {
      this.letters = wordOptions.word.split("").map((char) => new Letter(char));
    } else if (wordOptions.maxNumberOfLetters) {
      this.letters = [];
      for (let i = 0; i < wordOptions.maxNumberOfLetters; i++) {
        this.letters.push(new Letter(""));
      }
    } else {
      throw new Error(
        "WordOptions must have either maxNumberOfLetters or word"
      );
    }
  }

  public addLetter(newLetter: string): void {
    for (const letter of this.letters) {
      if (!letter.char) {
        letter.char = newLetter;
        break;
      }
    }
  }

  public removeLastLetter(): void {
    for (let i = this.letters.length - 1; i >= 0; i--) {
      if (this.letters[i].char) {
        this.letters[i].char = "";
        break;
      }
    }
  }
  public get isFilled(): boolean {
    return this.letters.every((letter) => letter.char);
  }

  public get word(): string {
    return this.letters.map((x) => x.char).join("");
  }

  public clear() {
    for (const letter of this.letters) {
      letter.char = "";
    }
  }
}

class WordOptions {
  maxNumberOfLetters?: number = 0;
  word?: string | null = null;
}
