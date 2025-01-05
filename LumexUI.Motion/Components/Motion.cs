using LumexUI.Motion.Types;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace LumexUI.Motion;

/// <summary>
/// 
/// </summary>
public class Motion : ComponentBase
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

    [Inject] private IJSRuntime JS { get; set; } = default!;

    private ElementReference _ref;
    private MotionProps? _props;

    protected override void OnParametersSet()
    {
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
                await AnimateAsync( _props );
            }
        }
        else
        {
            await AnimateAsync( _props, LayoutId );
        }
    }

    /// <inheritdoc />
    protected override void BuildRenderTree( RenderTreeBuilder builder )
    {
        builder.OpenElement( 0, As );
        builder.AddMultipleAttributes( 1, AdditionalAttributes );
        builder.AddElementReferenceCapture( 2, elementReference => _ref = elementReference );
        builder.AddContent( 3, ChildContent );
        builder.CloseElement();
    }

    private ValueTask AnimateAsync( MotionProps props )
    {
        return JS.InvokeVoidAsync( "motionInterop.animateEnter", _ref, props );
    }

    private ValueTask AnimateAsync( MotionProps? props, string layoutId )
    {
        return JS.InvokeVoidAsync( "motionInterop.animateLayoutId", _ref, layoutId, props );
    }

    private ValueTask AnimateAsync( string layoutId )
    {
        return JS.InvokeVoidAsync( "motionInterop.animateLayoutId", _ref, layoutId, new { Enter }, Transition );
    }
}