namespace CRUD_.Net_Core_Web_API.Interfaces
{
    public interface IAuth<TClass>
    {
        TClass Authenticate(string username, string password);

    }
}
