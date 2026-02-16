# UITKTween

**High-performance tweening library for Unity UI Toolkit.**

DOTween-inspired fluent API with 31 easing functions, sequences, object pooling, and automatic lifecycle management for VisualElement.

## Requirements

- Unity 6 (6000.0+)
- UI Toolkit (com.unity.modules.uielements)

## Installation

### Via Package Manager (UPM)

1. Open Window > Package Manager
2. Click "+" > "Add package from disk..."
3. Select `Packages/com.dsgarage.uitktween/package.json`

### Via Git URL

```
https://github.com/dsgarage/UITKTween.git?path=Packages/com.dsgarage.uitktween
```

## Quick Start

```csharp
using DSGarage.UITKTween;

// Fade out
element.DOFade(0f, 0.5f);

// Move with easing
element.DOMove(new Vector2(100, 200), 0.5f).SetEase(EaseType.OutBack);

// Scale with loop
element.DOScale(1.5f, 0.3f).SetLoops(2, LoopType.Yoyo);

// Rotate
element.DORotate(360f, 1f).SetEase(EaseType.InOutCubic);

// Color
element.DOColor(Color.red, 0.5f);

// Generic tween
UITKTween.To(() => myValue, x => myValue = x, targetValue, 1f);
```

## API Reference

### Extension Methods (VisualElement)

| Category | Methods |
|----------|---------|
| Transform | `DOMove`, `DOMoveX`, `DOMoveY`, `DORotate`, `DOScale` |
| Position | `DOLeft`, `DOTop`, `DORight`, `DOBottom` |
| Size | `DOWidth`, `DOHeight`, `DOSize` |
| Color | `DOFade`, `DOColor`, `DOTextColor` |
| Border | `DOBorderColor`, `DOBorderWidth`, `DOBorderRadius` + individual |
| Spacing | `DOMargin`, `DOPadding` + individual sides |
| Text | `DOFontSize`, `DOLetterSpacing`, `DOWordSpacing` |
| Special | `DOPunchPosition/Rotation/Scale`, `DOShakePosition/Rotation/Scale` |

### Tween Settings (Fluent Chain)

```csharp
element.DOFade(0f, 0.5f)
    .SetEase(EaseType.OutQuad)       // Easing
    .SetDelay(0.3f)                   // Delay before start
    .SetLoops(3, LoopType.Yoyo)      // Loop
    .SetRelative()                    // Relative to current value
    .SetAutoKill(false)               // Keep alive after completion
    .SetId("myTween")                 // ID for global control
    .SetTarget(element)               // Target for filtered operations
    .SetTimeScale(2f)                 // Speed multiplier
    .SetUpdate(UpdateType.Late)       // Update timing
    .From(1f)                         // Animate FROM value
    .OnStart(() => { })               // First frame callback
    .OnUpdate(t => { })               // Every frame (normalized 0-1)
    .OnComplete(() => { })            // Completion callback
    .OnKill(() => { });               // Kill callback
```

### Sequence

```csharp
UITKTween.Sequence()
    .Append(element.DOFade(0f, 0.3f))         // Add to end
    .AppendInterval(0.2f)                       // Wait
    .Append(element.DOFade(1f, 0.3f))          // Add to end
    .Join(element.DOScale(1.2f, 0.3f))         // Play with previous
    .Insert(0.5f, element.DORotate(90f, 0.5f)) // Insert at time
    .AppendCallback(() => Debug.Log("Done"))    // Callback at end
    .SetLoops(2, LoopType.Yoyo);
```

### Global Control

```csharp
UITKTween.PauseAll();
UITKTween.PlayAll();
UITKTween.KillAll();
UITKTween.Kill("myId");
UITKTween.KillTarget(element);
UITKTween.ManualUpdate(deltaTime);  // For UpdateType.Manual
UITKTween.SetCapacity(64, 16);      // Pre-allocate pool
```

### Easing (31 Types)

Linear, InQuad/OutQuad/InOutQuad, InCubic/OutCubic/InOutCubic, InQuart/OutQuart/InOutQuart, InQuint/OutQuint/InOutQuint, InSine/OutSine/InOutSine, InExpo/OutExpo/InOutExpo, InCirc/OutCirc/InOutCirc, InElastic/OutElastic/InOutElastic, InBack/OutBack/InOutBack, InBounce/OutBounce/InOutBounce, Custom (AnimationCurve)

### Loop Types

- `Restart` - Restart from beginning
- `Yoyo` - Reverse direction each loop
- `Incremental` - Add to end value each loop

## Editor Tools

- **Window > UITKTween > Debug Window** - Real-time tween inspector
- **Project Settings > UITKTween** - Default settings

## Samples

Import via Package Manager > UITKTween > Samples:

1. **Basic Usage** - Fade, Move, Scale, Rotate, Color, Chain
2. **Sequence Demo** - Sequential, Parallel, Complex sequences
3. **Easing Showcase** - Visual demo of all 31 easing functions
4. **Advanced Effects** - Punch, Shake, From, Relative, Border, Padding

## Architecture

```
UITKTween (static facade)
    -> TweenManager (manages active tweens)
        -> TweenUpdateRunner (MonoBehaviour singleton, drives updates)
    -> Tween (abstract base)
        -> Tweener<T> (generic value interpolation via ITweenPlugin<T>)
        -> Sequence (timeline-based tween composition)
    -> TweenPool (object pooling for GC reduction)
    -> PanelLifecycleTracker (auto-kill on VisualElement detach)
```

## License

MIT License. See [LICENSE.md](LICENSE.md).
