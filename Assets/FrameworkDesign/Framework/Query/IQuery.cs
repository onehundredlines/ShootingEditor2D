namespace FrameworkDesign
{
    public interface IQuery<TResult> : ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanSentQuery
    {
        TResult Do();
    }
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do() => OnDo();
        protected abstract T OnDo();
        private IArchitecture mArchitecture;
        public void SetArchitecture(IArchitecture architecture) => mArchitecture = architecture;
        public IArchitecture GetArchitecture() => mArchitecture;
    }
}