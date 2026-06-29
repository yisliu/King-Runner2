 # Changelog

All notable changes to King Runner are documented here.

## [0.2.0] - 2026-06-28

### Added
- Female playable character with full animations and collision
- Island levels (Isle_01_LP 1 and Isle_01_LP 2)
- Game over screen with fade-in animation
- Ship boarding sequence — player walks to the ship and shrinks into it before the level transitions
- Coin spend animation that plays during the boarding sequence
- Jump buffer so inputs just before landing still register
- Camera FOV zoom effect on speed changes with particle burst
- Low-time warning — timer pulses red when time is running low
- Animated score counter using DOTween
- Coin progress bar with animated fill
- Camera target system — characters can define a `CameraTarget` child object for precise camera placement

### Changed
- Player physics overhauled — now uses real gravity, ground check sphere, and rotation-based steering
- Camera setup now finds the player by tag at runtime, works across all character prefabs
- Level transition now supports both a direct scene reference and a level cooker fallback
- Settings panel removed from the main menu for the initial release

### Fixed
- Player collision now correctly finds the level cooker for both island and standard levels
- Camera no longer snaps in front of characters whose root pivot faces a non-standard direction

---

## [0.1.0] - Initial Release

### Added
- Core infinite runner gameplay with procedural chunk generation
- Five handcrafted levels: Snow, Desert, Forest, Dark Forest, Island
- Coin collection system with a threshold that unlocks the ship exit
- Ship hub level select scene with cinematic camera intro
- Start screen with animated main menu
- Win screen
- Settings panel (volume slider, difficulty buttons)
- Basic score tracking with high score saved to PlayerPrefs