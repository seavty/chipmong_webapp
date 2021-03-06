﻿using ChipMongWebApp.Models.DB;
using ChipMongWebApp.Models.DTO.User;
using ChipMongWebApp.Utils.Attribute;
using ChipMongWebApp.Utils.Handlers;
using ChipMongWebApp.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChipMongWebApp.Controllers
{
    [Authentication]
    [ErrorLogger]
    public class UserController : Controller
    {
        private readonly UserHandler handler;

        public UserController()
        {
            handler = new UserHandler();
        }

        //--> New
        public ActionResult New()
        {
            return View();
        }

        //-> New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> New(UserNewDTO newDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return Json(await handler.New(newDTO), JsonRequestBehavior.AllowGet);
            }
            catch (HttpException ex)
            {
                //return Json(ConstantHelper.ERROR, JsonRequestBehavior.AllowGet);
                if (ex.Message == ConstantHelper.LOGIN_NAME_EXIST || ex.Message == ConstantHelper.KEY_IN_REQUIRED_FIELD)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //-> View
        public async Task<ActionResult> View(int id)
        {
            var record = await handler.SelectByID(id);
            record.mode = ConstantHelper.MODE_VIEW;
            return View(record);
        }

        //-> Edit
        public async Task<ActionResult> Edit(int id) { return View(await handler.SelectByID(id)); }

        //-> Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(UserEditDTO editDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return Json(await handler.Edit(editDTO), JsonRequestBehavior.AllowGet);

            }
            catch (HttpException ex)
            {
                //return Json(ConstantHelper.ERROR, JsonRequestBehavior.AllowGet);
                if (ex.Message == ConstantHelper.LOGIN_NAME_EXIST || ex.Message == ConstantHelper.KEY_IN_REQUIRED_FIELD)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //-> Find 
        [HttpGet]
        public ActionResult Find() { return View(); }

        //-> Paging
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Paging(UserFindDTO findDTO) { return PartialView(await handler.GetList(findDTO)); }


        //-> Delete
        //??? why if use http delete - resource alway not found ? ???
        [HttpPost]
        public async Task<string> Delete(int id)
        {
            try
            {
                if (await handler.Delete(id))
                {
                    Response.StatusCode = 200;
                    return "ok";
                }
                return null;
            }
            catch (HttpException ex)
            {
                Response.StatusCode = 400;
                return ex.Message;
            }
        }

        //--- *** chnage password **--//
        public ActionResult ChangePassword() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ChangePassword(UserChangePasswordDTO changePasswordDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpException((int)HttpStatusCode.BadRequest, ConstantHelper.KEY_IN_REQUIRED_FIELD);
                Response.StatusCode = 200;
                return Json(await handler.ChangePassword(changePasswordDTO), JsonRequestBehavior.AllowGet);
            }
            catch (HttpException ex)
            {
                
                if (ex.Message == ConstantHelper.INCORRECT_PASSWORD || ex.Message == ConstantHelper.PASSWORD_DOES_NOT_MATCH || ex.Message == ConstantHelper.KEY_IN_REQUIRED_FIELD)
                    Response.StatusCode = 400;
                else
                    Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}