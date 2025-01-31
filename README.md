# 🎮 Rocketman Game

A physics-based mobile game where players control a gliding ball through an endless obstacle course of platforms. Master the art of launching, gliding, and landing to achieve the highest score possible!

## 🌟 Features

- **Dynamic Launch System**
  - Intuitive pull-back mechanism for launching the ball
  - Power-based launch system: pull strength directly affects launch distance
  - Smooth camera transitions following the ball's trajectory

- **Advanced Movement Controls**
  - Real-time gliding controls with touch input
  - Dynamic wing deployment system
  - Rotation and direction control during flight
  - Custom physics implementation without Unity's built-in physics system

- **Endless Platform System**
  - Two types of platforms with unique properties:
    - Rectangular platforms (X unit bounce height)
    - Cylindrical platforms (2X unit bounce height)
  - Platform pooling system for optimal performance
  - Procedurally generated endless forward progression
  - Variable platform heights for added challenge

## 🔧 Technical Specifications

### Requirements
- Unity 2022.3.55
- Mobile device with touch input capability

### Architecture Highlights
- Custom physics implementation
- Object pooling system for platform management
- Optimized for mobile performance
- Touch input system for precise control
- Scriptale Object Observer Pattern for handling game states, and events
- Poison Disc Sampling for randomized platform placement

## 🎯 Gameplay Mechanics

### Launch Phase
1. Ball starts attached to a launch stick
2. Pull back to adjust launch power
3. Release to launch the ball forward

### Flight Phase
1. Tap and hold screen to deploy wings and glide
2. Slide finger left/right to control gliding direction
3. Release to retract wings and enter falling state

### Platform Interaction
- Rectangle platforms: X unit bounce
- Cylinder platforms: 2X unit bounce
- Miss platforms = Game Over
- Quick restart system without scene reloading

## 🚀 Development Setup

1. Clone the repository
```bash
git clone [repository-url]
```

2. Open project in Unity 2022.3.55

3. Open the main scene from: `Assets/Scenes/MainGame`

## 🎨 Asset Information

The project includes:
- Character model with animations
- Launch stick model and animations
- Wing deployment animations
- Basic platform models

## 🔍 Code Structure

```
Assets/
├── Art/
│   ├── 2D Casual UI/
│   ├── Animations/
│   ├── Materials/
│   ├── Models/
│   └── Textures/
├── Input/
├── SOs/
│   ├── Events/
│   │   ├── GameEvents/
│   │   └── UIEvents/
│   ├── Variables/
│   │   ├── GameVariables/
│   │   └── UIVariables/
├── Scenes/
├── Scripts/
│   ├── Managers/
│   ├── LevelGeneration/
│   ├── Player/
│   ├── SOs/
│   ├── Stick/
├── Settings/
├── Prefabs/
└── TextMeshPro/
```

## ⚡ Performance Optimization

- Object pooling for platform reuse
- Optimized platform generation system
- Efficient memory management
- No scene reloading for restarts

---

Built with Unity 2022.3.55
