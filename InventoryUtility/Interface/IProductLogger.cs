
namespace InventoryUtility.Interface
{
    public interface IProductLoggers
    {
        public void LogInformation(string strMessage);
        public void LogError(string strMessage);
        public void LogWarning(string strMessage);
    }
}
