## 🚧 Notice 🚧

This project is in a very raw and experimental state. It is not production-ready and may contain bugs, missing features, and breaking changes as development progresses.

This project was primarily created to support [the main LumexUI project](https://github.com/LumexUI/lumexui), and functionality is added only as needed to fulfill its requirements. As a result, it may lack certain features or general-purpose usability.

If you choose to explore or contribute, please be aware that stability is not guaranteed, and updates may be infrequent or focused on specific needs.

## Installation

1. Register the service in the DI:
   
```csharp
// Program.cs

using LumexUI.Motion.Extensions;

builder.Services.AddLumexMotion();
```

2. Globally add the necessary usings:

```razor
@* _Imports.razor *@

@using LumexUI.Motion
@using LumexUI.Motion.Types
```

## API

Since this project is a Blazor wrapper, I mimic the original API, but it’s not fully implemented yet. 
For more details, refer to the [original Motion React documentation](https://motion.dev/docs/react-animation).

```csharp
public record MotionProps
{
    // Animations that are triggered on after render.
    public object? Enter { get; init; }

    // Animations that are triggered before component is removed from the render tree.
    public object? Exit { get; init; }

    // Transition settings for all animations.
    public object? Transition { get; init; }
};
```

The properties are of type `object` purely for simplicity, as they are later serialized into JavaScript objects.
Additionally, this makes it easier to follow the Motion's usage examples.

## Examples

The Motion library is one of the most powerful animation libraries available, allowing you to create almost any animation you want.
Check out the full list of Motion vanilla JavaScript examples [here](https://examples.motion.dev/js).

Below are some of the simplest animation examples to give you an idea of how it works in this library.

#### Simple Fade-In Animation

```razor
@* A component that wraps content for animation. *@
<Motion Enter="@_motionProps.Enter">
    <h1>Hello, world!</h1>
</Motion>

@code {
    private MotionProps _motionProps = new MotionProps
    {
        Enter = new
        {
            Opacity = new float[] { 0, 1 } // Animate opacity from 0 to 1.
        }
    };
}
```

https://github.com/user-attachments/assets/7b86932e-7e5c-422e-959d-a091f11ee4ef

#### Simple Fade-Out Animation

```razor
<button @onclick="@(() => _isVisible = !_isVisible)">
    @(_isVisible ? "Hide" : "Show")
</button>

@* A component that detects when its direct children are removed from the render tree. *@
<AnimatePresence>
    @if( _isVisible )
    {
        <Motion Exit="@_motionProps.Exit">
            <h1>Hello, world!</h1>
        </Motion>
    }
</AnimatePresence>

@code {
    private bool _isVisible = false;

    private MotionProps _motionProps = new MotionProps
    {
        Exit = new
        {
            Opacity = 0 // Animate opacity from initial (1) to 0.
        }
    };
}
```

https://github.com/user-attachments/assets/ee0139fb-0e83-45b4-a095-907c0b2947c8

#### Simple Fade-In-Out Animation

```razor
<button @onclick="@(() => _isVisible = !_isVisible)">
    @(_isVisible ? "Hide" : "Show")
</button>

<AnimatePresence>
    @if( _isVisible )
    {
        <Motion Enter="@_motionProps.Enter" Exit="@_motionProps.Exit">
            <h1>Hello, world!</h1>
        </Motion>
    }
</AnimatePresence>

@code {
    private bool _isVisible = false;

    private MotionProps _motionProps = new MotionProps
    {
        Enter = new
        {
            Opacity = new float[] { 0, 1 } // Animate opacity from 0 to 1.
        },

        Exit = new
        {
            Opacity = 0 // Animate opacity from current (1) to 0.
        }
    };
}
```

https://github.com/user-attachments/assets/e98c272d-977e-4ad9-b919-630db6d84016

Ultimately, the component is actually removed from the DOM.
