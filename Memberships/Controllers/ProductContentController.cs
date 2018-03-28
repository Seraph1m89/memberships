using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Memberships.Extensions;
using Microsoft.AspNet.Identity;

namespace Memberships.Controllers
{
    [Authorize]
    public class ProductContentController : Controller
    {
        // GET: ProductModel
        public async Task<ActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = Request.IsAuthenticated ? User.Identity.GetUserId() : null;
            var sections = await SectionExtensions.GetProductSectionAsync(id.Value, userId);
            return View(sections);
        }
    }
}