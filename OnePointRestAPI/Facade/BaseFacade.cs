#region Header
/*
 ************************************************************************************
 Name: BaseFacade
 Description: Interface  for all the base operations 
 Created On:  28-sep-2018
 Created By:  Uday Kiran
 Last Modified On: 
 Last Modified By: 
 Last Modified Reason: 
 ************************************************************************************
 */
#endregion
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;

namespace OnePointRestAPI.Facade
{
    public class BaseFacade
    {
        public static readonly ILogger LogManager = UtilsFactory.Logger;
        //Initiallising variable to load any base class instance
    }
}
