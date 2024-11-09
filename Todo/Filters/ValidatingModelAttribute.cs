using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Todo.Filters
{
    public class ValidatingModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the model is valid
            if (!context.ModelState.IsValid)
            {
                // Try to get the referer URL from the HTTP request
                var refererUrl = context.HttpContext.Request.Headers["Referer"].ToString();

                // If there is a referer URL, redirect to that URL, else redirect to a default page
                if (!string.IsNullOrEmpty(refererUrl))
                {
                    context.Result = new RedirectResult(refererUrl);
                }
                else
                {
                    // Optional: Handle the case where there's no referer (fallback to a default page or error)
                    context.Result = new RedirectToActionResult("Index", "Home", null); // Redirect to a default page
                }
            }
        }
    }
}
