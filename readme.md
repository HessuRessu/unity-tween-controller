# Generic Tween controller for unity

Lightweight, allocation-friendly tweening baseline for Unity.

This package provides a small but extensible tween framework designed for UI and gameplay utilities where:
- Simplicity is preferred over large feature sets
- GC allocations must be avoided
- Tweens are driven from a central controller
- New tween types can be added with minimal boilerplate

The system is intentionally straightforward and does **not** try to implement full-featured tween library. Instead, it focuses on clarity, determinism, and tight integration into code-driven projects providing extenable skeleton for tween-like animation.  

## Compatibility

This package has been implemented and tested with **Unity v6000.0** and **Universal Render Pipeline v17.0.4**.

## Features

- Centralized `TweenController` singleton
- Pooling of tween bindings (no per-tween allocations)
- Strongly typed tween bindings
- Generic base classes for easy extension
- Built-in bindings for common cases:
  - CanvasGroup alpha
  - UI Graphic color
  - Local scale
  - Material float mutation
  - Delayed trigger (timer-only tween)

## Installation (UPM)

Add this package through your project's `manifest.json`:

```json
{
  "dependencies": {
    "com.pihkura.unity-tween-controller": "https://github.com/HessuRessu/unity-tween-controller.git"
  }
}
```

Or you can add this repository directly as a Git package:

1. Open **Unity → Window → Package Manager**  
2. Click **+ → Add package from git URL**  
3. Paste:  
   `https://github.com/hessuressu/unity-tween-controller.git`

## Basic Concept

A **TweenBinding** represents a single running tween instance.

- The `TweenController` owns all bindings
- Each frame, it updates active bindings
- Completed bindings are recycled into a reserve pool

You never manually create bindings using `new` in gameplay code.
Instead, you request them from the controller:

```csharp
var tween = TweenController.Instance.Get<CanvasGroupAlphaTweenBinding>();
```

## Minimal Usage Example

Fade a `CanvasGroup` from 0 → 1 in 0.3 seconds:

```csharp
var tween = TweenController.Instance.Get<CanvasGroupAlphaTweenBinding>();

tween.Bind(
    canvasGroup,
    0.3f,
    new TweenContext
    {
        BaseValue = 0f,
        TargetValue = 1f
    }
);

tween.onComplete = () =>
{
    Debug.Log("Fade complete");
};
```

## Cancel Tweens

Cancel all tweens targeting a specific object:

```csharp
TweenController.Instance.Cancel(canvasGroup);
```

Optionally suppress completion callbacks:

```csharp
TweenController.Instance.Cancel(canvasGroup, invokeOnComplete: false);
```

## Built-in Tween Bindings

### CanvasGroupAlphaTweenBinding
Fades `CanvasGroup.alpha`.

Context fields used:
- `BaseValue`
- `TargetValue`

### ColorTweenBinding
Interpolates `Graphic.color`.

Context fields used:
- `BaseColor`
- `TargetColor`

### ScaleTweenBinding
Interpolates `GameObject.transform.localScale`.

Context fields used:
- `BaseScale`
- `TargetScale`
- `Influence` (per-axis weight 0..1)

### MaterialMutationTweenBinding
Interpolates a float shader parameter on a UI Image material.

Context fields used:
- `TweenKey`
- `BaseValue`
- `TargetValue`

### LineRendererAlphaTweenBinding
Interpolates the alpha channel of a LineRenderer's color gradient over time.

Common use cases:
- Fade-in / fade-out of paths and roads
- Territory or zone border highlighting
- Selection outlines
- Any procedural line-based visual that requires smooth alpha animation

Context fields used:
- `BaseValue` (starting alpha)
- `TargetValue` (final alpha)

### DelayedTriggerTweenBinding
Timer-only tween. Does not mutate target.
Useful as a delayed callback.

Context fields used: none

## Creating Custom Tween Bindings

Derive from `TweenBinding<T>`:

```csharp
public class MyCustomTween : TweenBinding<MyComponent>
{
    public override void OnBind()
    {
        // Initialize starting state
    }

    public override bool Update(float deltaTime)
    {
        Timer += deltaTime;

        float t = Mathf.Clamp01(Timer / Duration);

        // Apply interpolation here

        return Timer >= Duration;
    }

    public override void Cancel()
    {
        // Optional: apply final state
    }
}
```

Then request it normally:

```csharp
var tween = TweenController.Instance.Get<MyCustomTween>();
```

## Architecture Overview

```
TweenController
 ├─ Active Bindings
 ├─ Reserve Pool
 └─ Update Loop

TweenBindingBase
 └─ TweenBinding<T>
      └─ Concrete Binding
```

Design goals:
- No reflection
- No dynamic allocations per frame
- Clear ownership
- Easy to debug

## Performance Characteristics

- No GC allocations during normal tween usage
- O(n) update where n = active tweens
- Pool reuse avoids churn

Suitable for UI-heavy projects and utility animations.

## Limitations (By Design)

- No timelines
- No sequences
- No built-in easing curves (can be added in bindings)
- No editor tooling

This keeps the system predictable and small.

## Testing
Includes EditMode unit tests using Unity Test Framework.

## Additional Notes

If you want additional helpers (easing library, chaining, sequences, etc.), they can be layered on top without changing the core architecture.

## License

MIT