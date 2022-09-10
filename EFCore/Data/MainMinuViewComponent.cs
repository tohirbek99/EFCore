using Microsoft.AspNetCore.Mvc;

namespace EFCore.Data
{
    public class MainMinuViewComponent : ViewComponent
    {
        private readonly DataContext context;

        public MainMinuViewComponent(DataContext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

        }
    }
}
