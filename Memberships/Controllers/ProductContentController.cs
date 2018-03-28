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

        public async Task<ActionResult> Content(int? productId, int? itemId)
        {
            if (productId == null || itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await SectionExtensions.GetContentAsync(productId.Value, itemId.Value);
            return View(model);
        }
    }
}