#region Header
/*
 ************************************************************************************
 Name: IValuesFacade
 Description: Interface  for all the Values operations 
 Created On:  28-sep-2018
 Created By:  Uday Kiran
 Last Modified On: 
 Last Modified By: 
 Last Modified Reason: 
 ************************************************************************************
 */
#endregion

using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace OnePointRestAPI.Facade
{
    interface IPostTicketingRequestFacade : IFacade
    {
        // method declarations goes here
        
        dynamic PostTicketingRequest(dynamic value);
        dynamic SearchPostTicketingRequest(dynamic value);
        dynamic MarkAsRead(dynamic value);
        dynamic PostTicketingRequestReIssue(dynamic value);
        
    }
}