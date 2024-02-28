using RayCastSystem.Highlight.Outline;

namespace RayCastSystem.Highlight
{
    public interface IHighlightTrigger : IRayCastTrigger
    {
        OutlineData OutlineData { get; }
    }
}