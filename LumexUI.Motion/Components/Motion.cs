using LumexUI.Motion.Types;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace LumexUI.Motion;

/// <summary>
/// 
/// </summary>
public class Motion : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public string As { get; set; } = "div";

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public object? Enter { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public object? Exit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public object? Transition { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public string? LayoutId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter( CaptureUnmatchedValues = true )]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter] private PresenceContext? PresenceContext { get; set; }

    [Inject] private IJSRuntime JS { get; set; } = default!;

    private ElementReference _ref;
    private MotionProps? _props;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if( Exit is not null && PresenceContext is null )
        {
            throw new InvalidOperationException(
                $"{GetType()} must be wrapped within the '{nameof( AnimatePresence )}' component" +
                $"when the '{nameof( Exit )}' parameter is set." );
        }

        if( Enter is not null ||
            Exit is not null ||
            Transition is not null )
        {
            _props = new MotionProps( Enter, Exit, Transition );
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync( bool firstRender )
    {
        if( string.IsNullOrEmpty( LayoutId ) )
        {
            if( _props is not null )
            {
                await AnimateEnterAsync( _props );
            }
        }
        else
        {
            await AnimateLayoutIdAsync( _props, LayoutId );
        }
    }

    /// <inheritdoc />
    protected override void BuildRenderTree( RenderTreeBuilder builder )
    {
        if( PresenceContext is not null )
        {
            PresenceContext.Register( this );
        }
        else
        {
            Render( builder );
        }
    }

    internal void Render( RenderTreeBuilder builder )
    {
        builder.OpenElement( 0, As );
        builder.AddMultipleAttributes( 1, AdditionalAttributes );
        builder.AddElementReferenceCapture( 2, elementReference => _ref = elementReference );
        builder.AddContent( 3, ChildContent );
        builder.CloseElement();
    }

    private ValueTask AnimateEnterAsync( MotionProps props )
    {
        return JS.InvokeVoidAsync( "motionInterop.animateEnter", _ref, props );
    }

    private ValueTask AnimateExitAsync( MotionProps props )
    {
        return JS.InvokeVoidAsync( "motionInterop.animateExit", _ref, props );
    }

    private ValueTask AnimateLayoutIdAsync( MotionProps? props, string layoutId )
    {
        return JS.InvokeVoidAsync( "motionInterop.animateLayoutId", _ref, layoutId, props );
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if( PresenceContext is not null && _props is not null )
        {
            await AnimateExitAsync( _props );
            PresenceContext.Unegister( this );
        }
    }
}