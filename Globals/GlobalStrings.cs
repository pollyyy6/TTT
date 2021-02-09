using System;

namespace Chat
{
    public class GlobalStrings
    {
        public static string GetConnectionStringName()
        {
			String connection = "";
#if DEBUG
			connection = "TTT_Local";
#else
            connection = "TTT_Production";
#endif
			return connection;
		}
	}
}
