# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2026-02-16

### Added
- Core tween engine (Tween, Tweener<T>, Sequence, TweenManager)
- 31 easing functions + AnimationCurve custom support
- Type plugins: float, Vector2, Vector3, Color, Quaternion
- VisualElement extension methods:
  - Transform: DOMove, DOMoveX/Y, DORotate, DOScale
  - Position: DOLeft, DOTop, DORight, DOBottom
  - Size: DOWidth, DOHeight, DOSize
  - Color: DOFade, DOColor, DOTextColor
  - Border: DOBorderColor, DOBorderWidth, DOBorderRadius (+ individual sides/corners)
  - Spacing: DOMargin, DOPadding (+ individual sides)
  - Text: DOFontSize, DOLetterSpacing, DOWordSpacing
  - Special: DOPunchPosition/Rotation/Scale, DOShakePosition/Rotation/Scale
- Sequence with Append/Prepend/Join/Insert, intervals, and callbacks
- From() and SetRelative() support
- Loop types: Restart, Yoyo, Incremental
- Auto-kill on VisualElement panel detach (PanelLifecycleTracker)
- Object pooling (TweenPool) with SetCapacity() pre-allocation
- Editor tools: Debug Window, Project Settings provider
- 4 sample scenes: BasicUsage, SequenceDemo, EasingShowcase, AdvancedEffects
- Unit tests: Easing, Tweener, Sequence, Pool
