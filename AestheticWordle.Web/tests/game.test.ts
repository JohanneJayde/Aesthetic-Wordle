import { beforeEach, expect, test } from "vitest";
import { Game } from "~/scripts/game";
import AxiosMockAdapter from "axios-mock-adapter";
import axios from "axios";

let mock: AxiosMockAdapter;

beforeEach(() => {
  mock = new AxiosMockAdapter(axios);
});

test("game", async () => {
  const game = new Game();
  mock.onGet("/word/randomWord").reply(200, { wordId: 1 });

  await game.startNewGame();
  expect(game.guessIndex).toBe(0);
});
