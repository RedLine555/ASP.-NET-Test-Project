using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeWork3Data;
using HomeWork3Data.DataModel;
using System.IO;
using System.Net;
using System.Configuration;
using HomeWork3.ForbiddenRequest;
using HomeWork3Common.Helpers.Security;
using HomeWork3Business.Interfaces;

namespace HomeWork3.Controllers
{
    public class UserController : Controller
    {
        //private readonly IUserRepository _userRepository;

        private readonly IUserService _userservice;

        public UserController(IUserService userservice)
        {
            _userservice = userservice;
            //serRepository = _userrepository;
            //UsersRep = new UserRepository(new HW3DatabaseEntities());
            //string str = ConfigurationManager.ConnectionStrings["HW3DatabaseADO"].ConnectionString;
            //UsersRep = new ADOUserRepository(str);
        }
        // GET: /User/
        [Authorize]
        public ActionResult Index()
        {
            var users = _userservice.GetAll();
            return View(users.ToList());

        }

        public ActionResult GetImage(int id)
        {
            var img = _userservice.FindByID(id).Image.ImageContent;
            var stream = (img != null) ? new MemoryStream(img.ToArray()) : new MemoryStream();
            return new FileStreamResult(stream, "image/jpeg");
        }

        [AuthorizeForbiddenAttribute(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            User user = _userservice.FindByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _userservice.Delete(id);
            return RedirectToAction("Index");
        }

        [AuthorizeForbiddenAttribute(Roles="Admin")]
        //[Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Models.UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                User user = Mappers.MyMapper.UserViewToModel(userVM);
                _userservice.Add(user);
                return RedirectToAction("Index");
            }

            return View(userVM);
        }

        [AuthorizeForbiddenAttribute(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userservice.FindByID((int)id);
            Models.UserViewModel viewmodel = Mappers.MyMapper.UserModelToView(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Models.UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                User user = Mappers.MyMapper.UserViewToModel(userVM);
                //if (user.Image == null)
                //{
                //    using (User u = UsersRep.FindByID(user.UId))
                //    {
                //        user.Image = u.Image;
                //    }
                //}
                _userservice.Edit(user);
                return RedirectToAction("Index");
            }
            return View(userVM);
        }

        public ActionResult OnEdit()
        {
            if (AjaxRequestExtensions.IsAjaxRequest(Request))
                return Json("You are going to change user info. Are you sure?", JsonRequestBehavior.AllowGet);
            return HttpNotFound();
        }

        public JsonResult IsCorrectLogin(string login)
        {
            return Json(!_userservice.GetAll().Any(x => x.Login == login),
                                                 JsonRequestBehavior.AllowGet);
        }

	}
}