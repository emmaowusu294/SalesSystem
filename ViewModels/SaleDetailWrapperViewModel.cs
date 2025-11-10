// In /ViewModels/SaleDetailWrapperViewModel.cs
namespace SalesSystem.ViewModels
{
    /// <summary>
    // This is a "wrapper" to hold the two viewmodels
    // our "Details" page needs.
    /// </summary>
    public class SaleDetailWrapperViewModel
    {
        public SaleDetailViewModel Receipt { get; set; }
        public SaleAddItemViewModel AddItemForm { get; set; }
    }
}