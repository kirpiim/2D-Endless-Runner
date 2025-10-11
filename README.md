#  2D Endless Runner (MonoGame)

A **pixel-style endless runner** game built with **C# and MonoGame**, featuring a playful cat protagonist that jumps, crouches, and dodges obstacles in an infinite world.  
This project demonstrates **game physics, animation handling, asset management, and collision detection** using the MonoGame framework.

---

##  Features

###  Core Gameplay
- **Endless running mechanics** with increasing difficulty  
- **Jump** and **crouch** controls for player movement  
- **Dynamic obstacle generation** (rocks, etc.)  
- **Accurate collision detection** and grounded alignment for all obstacles  
- **Score system** that tracks how long you survive  
- **Restart and game-over mechanics**

###  Visuals & Art
- Retro **pixelated art style**  
- All assets dynamically scaled for different resolutions  
- Smooth **cat animation** using sprite sheets  
- Proper **ground alignment** with vertical offsets (e.g., `rockYOffset`)

###  Technical Highlights
- Developed in **C#** using the **MonoGame framework**  
- Organized with a clean, modular architecture:
  - `Game1.cs` → Main game loop and logic  
  - `Player.cs` → Handles movement, jumping, and crouching  
  - `Obstacle.cs` → Manages spawning and behavior of obstacles  
  - `Content.mgcb` → Asset management and content pipeline  
- Implements **delta time** for consistent movement  
- Efficient rendering using MonoGame’s `SpriteBatch`

---

##  Tools & Technologies

| Tool / Language | Purpose |
|------------------|----------|
| **C# (.NET 8)** | Core programming language |
| **MonoGame** | Game engine / framework |
| **Visual Studio Code** | Main IDE used for development |
| **Git & GitHub** | Version control and repository management |
| **Aseprite / Pixel Studio** *(optional)* | For creating pixel art assets |

---

##  Controls

| Action | Key |
|--------|-----|
| **Jump** | `Space` |
| **Crouch** | `Down Arrow` |
| **Restart** | `R` |
| **Quit** | `Esc` |

---

##  How to Run

### *Prerequisites*
Make sure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [MonoGame Framework](https://www.monogame.net/downloads/)

### *Run the game from terminal (bash or PowerShell)*
```bash
# Clone this repository
git clone https://github.com/<yourusername>/2DEndlessRunner.git
```
### Navigate to the folder
```bash
cd 2DEndlessRunner
```
### Build and run
```bash
dotnet run
```
### Folder Structure
```text 
2DEndlessRunner/
│
├── Content/
│   ├── background.png
│   ├── player_spritesheet.png
│   ├── rock.png
│   └── Content.mgcb
│
├── Game1.cs
├── Player.cs
├── Obstacle.cs
├── Program.cs
├── 2DEndlessRunner.csproj
└── README.md
```

## What I learned

- Implementing physics-like behavior in 2D using delta time
- Managing game states (Start, Playing, Game Over)
- Working with the MonoGame content pipeline
- Handling asset scaling and hitbox precision
- Structuring a small game project cleanly for readability and scalability

## Future Improvements

- Add parallax background scrolling
- Introduce power-ups and enemy types
- Add sound effects and background music
- Include a main menu UI and settings
- Integrate leaderboards or save system
