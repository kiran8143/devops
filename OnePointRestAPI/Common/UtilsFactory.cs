
using OnePointRestAPI.Common.Logger;

namespace OnePointRestAPI.Common
{
    public static class UtilsFactory
    {
        #region IUtilsFactory Members

        private static ILogger _Logger;


        public static ILogger Logger
        {
            get
            {
                if (_Logger == null)
                {
                    _Logger = new LogAdapter();
                }
                return _Logger;
            }
        }

        #endregion IUtilsFactory Members
    }
}
