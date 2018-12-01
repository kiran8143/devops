namespace OnePointRestAPI.Facade
{
    internal interface ITripDetailsFacade : IFacade
    {
        dynamic SearchTripDetails(string value);
    }
}