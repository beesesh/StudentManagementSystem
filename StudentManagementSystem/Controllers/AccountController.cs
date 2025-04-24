using StudentManagementSystem.Models;
using System.Linq;
using System.Web.Mvc;
using StudentManagementSystem.Models;
using System.Web.UI.WebControls;

namespace YourNamespace.Controllers
{
    public class AccountController : Controller
    {
        private readonly schoolEntities context;

        public AccountController()
        {
            context = new schoolEntities();
        }

        // GET: Account/Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users model)
        {
            if (ModelState.IsValid)
            {
                var user = context.users
                    .FirstOrDefault(a => a.username == model.Username && a.password == model.Password);

                if (user != null)
                {
                    // Store user information in session or cookie
                    Session["UserId"] = user.id;
                    Session["Username"] = user.username;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }

        // GET: Account/Signup
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(user model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = context.users.FirstOrDefault(u => u.username == model.username);
                if (existingUser == null)
                {
                    context.users.Add(model);
                  int a =  context.SaveChanges();
                    if (a > 0)
                    {

                        ViewBag.InsertMessage = "<script>alert('Registered Successfully')></script>";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.InsertMessage = "<script>alert('Register unsuccessfull.')></script>";
                    }
                    

                    // Redirect to login after successful signup
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Username already exists.");
                }
            }
            return View(model);
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout2()
        {
            // Clear the user's session
            Session.Clear();

            // Sign out the user
            System.Web.Security.FormsAuthentication.SignOut();

            // Redirect to the login page or home page
            return RedirectToAction("Login", "Account");
        }
    }
}
