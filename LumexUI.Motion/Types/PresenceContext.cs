namespace LumexUI.Motion.Types;

internal class PresenceContext
{
    private readonly List<Motion> _motions = [];

    private bool _collecting = true;

    public IReadOnlyList<Motion> Motions => _motions.AsReadOnly();
    public event Action? PresenceChanged;

    public void Register( Motion m )
    {
        if( _collecting )
        {
            _motions.Add( m );
        }
    }

    public void Unegister( Motion m )
    {
        _motions.Remove( m );
        NotifyPresenceChanged();
    }

    public void StartCollecting()
    {
        _motions.Clear();
        _collecting = true;
    }

    public void StopCollecting()
    {
        _collecting = false;
    }

    private void NotifyPresenceChanged() => PresenceChanged?.Invoke();
}
