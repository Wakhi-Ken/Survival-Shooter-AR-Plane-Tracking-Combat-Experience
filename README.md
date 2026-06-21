# AR Shooter Game (Unity)

An Augmented Reality (AR) shooter game built in Unity with enemy spawning, object pooling, and real-time gameplay systems.

---

## Features

- AR plane detection and placement  
- Enemy spawning (melee, shooter, boss)  
- Player shooting system  
- Bullet object pooling  
- Score and kills tracking  
- Game state management (Start, Playing, GameOver, Win)  
- Sound effects and music system  

---

## Game Flow

AR plane placement starts the game.  
GameManager sets the game state to Playing.  
Enemies are spawned on the AR plane.  
Player shoots bullets from a pool.  
Bullets hit enemies and deal damage.  
UI updates score and kills.

---

## Object Pooling

Bullets are reused for better performance.

Player shoots → get bullet from pool → activate → fire → hit enemy → return to pool

---

## Project Structure

Assets/
├── Scripts/
│   ├── Core (GameManager)
│   ├── Gameplay (Enemies, Spawner, Shooting)
│   ├── UI (HUD)
│   ├── Audio (AudioManager)
│   ├── Pooling (BulletPool)

---

## Requirements

- Unity 2021 or newer  
- AR Foundation  
- ARCore / ARKit supported device  

---

## How to Run

1. Clone the repository  
2. Open project in Unity  
3. Install AR Foundation packages  
4. Build and run on a mobile device  

---

## Author

Unity AR Shooter Game Project
