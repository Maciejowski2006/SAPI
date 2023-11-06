using SAPI.API;

namespace SAPI.Auth;

public class Auth : IExtensionBase
{
	public void Init()
	{
		Database.Init();
	}
}