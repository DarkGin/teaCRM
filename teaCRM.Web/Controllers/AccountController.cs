﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using teaCRM.Common;
using teaCRM.Entity;
using teaCRM.Service;
using teaCRM.Service.Impl;
using teaCRM.Web.Filters;

namespace teaCRM.Web.Controllers
{
    public class AccountController : Controller
    {
        #region 登陆

        //
        // GET: /Account/Login 默认登陆页面
        [HttpGet]
        [AutoLogin]
        public ActionResult Login()
        {
            return View();
        }

        //
        // GET: /Account/Login 登陆提交
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            IAccountService accountService = new AccountServiceImpl();
            ResponseMessage rmsg = accountService.Login(fc["type"].ToString(),
                fc["accountType"].ToString(), fc["userName"].ToString(), fc["userPassword"].ToString(),
                fc["remember"].ToString(),
                fc["clientIp"].ToString(), HttpUtility.UrlDecode(fc["clientPlace"].ToString()),
                fc["clientTime"].ToString());
            return Json(rmsg);
        }

//        //
//        // GET: /Account/ValidateLogin 自动登陆检测
//        [HttpPost]
//        public ActionResult ValidateLogin(FormCollection fc)
//        {
//            IAccountService accountService = new AccountServiceImpl();
//            ResponseMessage rmsg = accountService.ValidateAccount("login", fc["type"].ToString(),
//                fc["accountType"].ToString(), fc["userName"].ToString(), fc["userPassword"].ToString());
//            return Json(rmsg);
//        }

        #region 自动提示

        //
        // GET: /Account/UserNameAuto/ 自动提示
        public ActionResult UserNameAuto(string query)
        {
            string[] emails = new string[]
            {
                "126.com", "163.com", "yeah.net", "sina.com", "sina.cn", "qq.com", "vip.qq.com", "sohu.com",
                "live.com", "msn.cn", "gmail.com"
            };
            List<KeyValue> results = new List<KeyValue>();
            if (String.IsNullOrEmpty(query))
            {
                results.Add(new KeyValue() {value = "暂无结果", data = "0"});
                return Json(results, JsonRequestBehavior.AllowGet);
            }

            for (int i = 0; i < emails.Length; i++)
            {
                var email = emails[i];
                KeyValue item = new KeyValue();
                if (query.Contains("@"))//有@才提示
                {
                    string query2 = query.Split('@')[1];
                    if (email.StartsWith(query2))
                    {
                        item.value = query.Split('@')[0] + "@" + email.Trim();
                        item.data = i.ToString();
                    }
                    results.Add(item);
                }
//                else
//                {
//                    item.value = query + "@" + email.Trim();
//                    item.data = i.ToString();
//                }
            }
            results = Utils.RemoveEmptyList(results);
            AutoStruct autoStruct = new AutoStruct();
            autoStruct.query = "Unit";
            autoStruct.suggestions = results;
            return Json(autoStruct, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 返回结果
        /// </summary>
        private class AutoStruct
        {
            public string query { set; get; }
            public List<KeyValue> suggestions { set; get; }
        }

        #endregion

        #endregion

        #region 注册

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View("Register");
        }

        //
        // GET: /Account/EmailRegister

        public ActionResult EmailRegister()
        {
            return View("EmailRegister");
        }

        //
        // GET: /Account/PhoneRegister

        public ActionResult PhoneRegister()
        {
            return View("PhoneRegister");
        }

        #endregion

        #region 退出

        public ActionResult Logout()
        {
            Session.Remove(teaCRMKeys.SESSION_USER_COMPANY_INFO_ID);
            return View("Login");
        }

        #endregion
    }
}