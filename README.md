# Zoo World

A Unity-based ecosystem simulation featuring predators and prey with dynamic behaviors, collision interactions, and population management.

## 🎮 Overview

This project simulates a simple ecosystem where predators hunt prey, animals move autonomously, and population dynamics emerge from their interactions. Built with Unity 2022.3.62f3 and Universal Render Pipeline (URP).

## 📦 Plugins & Tools
- **Cinemachine**: For camera control
- **TextMeshPro**: For high-quality text rendering
- **Unity Object Pooling**: For efficient object management
- **Dotween**: For animations and transitions

## ✨ Features
### Animal System
- **Animal Types**: Prey and Predator with distinct behaviors
- **Movement Behaviors**: Autonomous movement with interruption handling

### Collision System
- **Rule-Based Collisions**: Different outcomes based on animal types, can be extended with new rules
    - **Prey vs Prey**: Bounce back and change direction
    - **Predator vs Prey**: Predator eats prey
    - **Predator vs Predator**: 50/50 chance of one eating the other

### Spawning & Boundaries
- **Dynamic Spawning**: Configurable spawn rates and locations
- **Boundary Monitoring**: Keeps animals within play area

### Key Systems

#### Collision Resolution
The collision system uses a dictionary-based approach with `CollisionPair` keys to map animal type combinations to specific `ICollisionRule` implementations.

#### Factory Pattern
`AnimalFactory` handles the creation and configuration of animal instances from ScriptableObject configs.

#### Signal System
Event-driven architecture in `Core/Signals/` for decoupled communication between systems.

## 📝 Code Architecture

### Design Patterns Used
- **Strategy Pattern**: Collision rules (`ICollisionRule`)
- **Factory Pattern**: Animal creation (`AnimalFactory`)
- **Observer Pattern**: Signal system for events
- **Prototype Pattern**: ScriptableObject configurations for animals, type, size, speed, jump distance etc.
- **Object Pooling**: Efficient reuse of animal instances

### Key Classes
- `Animal`: Core animal entity with behavior management
- `CollisingResolver`: Handles collision detection and rule application
- `AnimalSpawner`: Manages animal population spawning
- `BoundaryMonitor`: Ensures animals stay within bounds

### Testing
- The `AnimalSpawner` has context menu options to spawn animals during runtime for testing purposes.
