using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Web.Framework
{
    public abstract class ViewPageBase<TModel> : RazorPage<TModel>
    {
        public String Title { get; set; }

        protected ApplicationUser CurrentUser => new ApplicationUser(Context.User);

    }
}
