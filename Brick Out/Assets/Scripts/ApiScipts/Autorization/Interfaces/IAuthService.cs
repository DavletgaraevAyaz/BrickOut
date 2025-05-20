using System;
using System.Collections;
using System.Threading.Tasks;

public interface IAuthService
{
    IEnumerator LoginAsync(string username, string password, Action<bool, string> callback);
}
