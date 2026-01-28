<div align="center">
  
# 🐠 Virtual Aquarium

![Language](https://img.shields.io/badge/Language-C%23-blue)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-8.0-purple)
![Platform](https://img.shields.io/badge/Platform-Windows%20Forms-yellow)
![Status](https://img.shields.io/badge/Status-Completed-success)

## 📖 Project Overview
**Virtual Aquarium** is an interactive desktop application that simulates a living aquatic ecosystem. The project is designed to demonstrate advanced concepts of **Object-Oriented Programming (OOP)**, **GDI+ Graphics rendering**, **Data Serialization**, and **Application Architecture** within the .NET environment.

The application allows users to populate an aquarium with various marine species, track their movement statistics, save the simulation state and switch interface languages dynamically.

---
</div>


<div align="center">

## ✨ Key Features

### 🎮 Interactive Simulation
* **Diverse Marine Life:** Supports 8 unique fish types (Shark, Swordfish, Seahorse, Pufferfish, etc.), <br>each with distinct visual characteristics.
* **Smart Spawning:** Implements collision-aware spawning logic that respects the aspect ratio <br>of every fish and prevents UI overlapping.
* **Dynamic Animation:** Smooth movement logic with boundary detection and random speed attributes.
* **Atmospheric Effects:** Includes a particle system for rising air bubbles with varying size, transparency,<br> and realistic sinusoidal movement paths.

### 🛠️ Core Functionality
* **XML Serialization:** Full **Save/Load** capability. The entire state of the aquarium (fish types, positions, speeds)<br> can be saved to an XML file and restored later.
* **Localization (I18n):** Native support for **English** and **Bulgarian**. Changing the language instantly updates<br> all menus, buttons, and real-time counters without restarting the app.
* **Dynamic UI:** The "Add Fish" menu is generated programmatically,<br> featuring icons for every fish type and a "Random" option.

### ⚡ Performance & Optimization
* **High-Performance Rendering:** Runs at a stable framerate using **Double Buffering** to eliminate screen flickering.
* **Asset Manager & Caching:** Implements a dedicated `AssetManager` class. Images are pre-loaded, resized, <br>and cached (including mirrored versions) at startup to minimize runtime memory allocation and CPU usage.
<br>

---

## 🛠️ Tech Stack

* **Language:** C#
* **Framework:** .NET 8.0
* **UI Framework:** Windows Forms (WinForms)
* **Graphics:** GDI+ (System.Drawing) for high-performance rendering.
* **Data Persistence:** XML Serialization (System.Xml) for saving/loading state
* **IDE:** Microsoft Visual Studio 2026

---

## 🏗️ Architecture & Technical Details

The project follows strict **Separation of Concerns** to ensure maintainability and scalability.

| Component | Responsibility | OOP Principle |
| :--- | :--- | :--- |
| **`Fish.cs`** | **Model**. Encapsulates data (Coordinates, Speed, Type, Dimensions). Contains no drawing logic. | *Encapsulation* |
| **`Bubble.cs`** | **Visual Effect**. Encapsulates properties for individual bubbles (Size, Transparency, Physics). | *Abstraction* |
| **`AssetManager.cs`** | **Resource Handler**. A static manager that handles image loading, resizing, and caching. | *Single Responsibility* |
| **`Form1.cs`** | **Controller / View**. Manages user input, the animation timer (`Tick`), and rendering (`OnPaint`). | *Event-Driven Programming* |

### Optimization Strategy
To handle multiple moving objects smoothly, the application avoids `new Bitmap()` calls inside the render loop.<br> Instead, it uses a Dictionary-based caching system:
```csharp
// Example of the caching strategy used in AssetManager
private static Dictionary<int, Image> cachedFishRight = new Dictionary<int, Image>();
private static Dictionary<int, Image> cachedFishLeft = new Dictionary<int, Image>();
```
</div>
<br>

<div align="center">

## 📸 Screenshots

### <img width="1758" height="929" alt="Screenshot 2026-01-28 030606" src="https://github.com/user-attachments/assets/8cb20a58-cde3-4ce2-b43f-fb15ac3da477" />

### <img width="1756" height="893" alt="image" src="https://github.com/user-attachments/assets/605acd94-6805-4728-a5f6-29407d16eddc" />




## 👨‍💻 Author
**Name:** Ivan Tenev Ivanov
<br>
**Faculty Number:** F115436

---

</div>