using System.ComponentModel;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace LumexUI.Motion.Internal;

/// <remarks>
/// For internal use only.
/// </remarks>
[EditorBrowsable( EditorBrowsableState.Never )]
public class _Defer : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree( RenderTreeBuilder builder )
    {
        builder.AddContent( 0, ChildContent );
    }
}
