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
    [Parameter( CaptureUnmatchedValues = true )]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject] private IJSRuntime JS { get; set; } = default!;

    private ElementReference _ref;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync( bool firstRender )
    {
        await EnterAsync();
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

    private ValueTask EnterAsync()
    {
        return JS.InvokeVoidAsync( "motionInterop.animate", _ref, new { Enter }, Transition );
    }
}