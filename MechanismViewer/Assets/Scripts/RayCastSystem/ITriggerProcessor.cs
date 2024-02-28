namespace RayCastSystem
{
    public interface ITriggerProcessor
    {
        bool CanProcess(IRayCastTrigger trigger);
        void StartProcessing(IRayCastTrigger trigger);
        void AbortProcessing(IRayCastTrigger trigger);
    }
}