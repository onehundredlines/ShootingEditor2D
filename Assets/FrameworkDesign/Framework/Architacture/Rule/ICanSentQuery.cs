namespace FrameworkDesign
{
    public interface ICanSentQuery : IBelongToArchitecture
    {
    }
    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ICanSentQuery self, IQuery<TResult> query) => self.GetArchitecture().SendQuery(query);
    }
}