export function playClickSound(volumeLevel?: number): void {
  if (process.client) {
    const audio = new Audio("/click.mp3");
    audio.volume = volumeLevel ?? 0.9;
    audio.play();
  }
}

export function playWinSound(volumeLevel?: number): void {
  if (process.client) {
    const audio = new Audio("/won.mp3");
    audio.volume = volumeLevel ?? 0.9;
    audio.play();
  }
}

export function playLoseSound(volumeLevel?: number): void {
  if (process.client) {
    const audio = new Audio("/lose.mp3");
    audio.volume = volumeLevel ?? 0.9;
    audio.play();
  }
}

export function playEnterSound(volumeLevel?: number): void {
  if (process.client) {
    const audio = new Audio("/success.mp3");
    audio.volume = volumeLevel ?? 0.9;
    audio.play();
  }
}
