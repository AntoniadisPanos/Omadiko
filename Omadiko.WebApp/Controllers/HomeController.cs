﻿using Omadiko.Database;
using Omadiko.Entities;
using Omadiko.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;



namespace Omadiko.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            IndexHomeViewModel vm = new IndexHomeViewModel()
            {
                Articles = db.Articles.OrderBy(x => x.DateTime).Take(5).ToList(),
                BestProductsByPopularity = db.Products.Where(x => x.Popularity >= 3).OrderByDescending(x => x.Popularity).Take(5).ToList(),
                AllProducts = db.Products.ToList(),
                AllCategories = db.Categories.ToList(),
                GetDays=Product.GetDays()
            };
            
            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(FormCollection frc, int? id)
        {           
                Customer customer = new Customer()
                {
                    CustomerEmail=frc["Customeremail"],
                    CustomerFirstName=frc["CustomerFirstName"]
                };
                db.Customers.Add(customer);
                db.SaveChanges();
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    Message msg = new Message()
                    {

                        UserId = db.Users.Max(x => x.Id),
                        UserName = User.Identity.Name,
                        Text = frc["message"],
                        When = DateTime.Now.Date
                    };
                    db.Messages.Add(msg);
                    db.SaveChanges();
                }               
            }
            else
            {
                Message msg = new Message()
                {
                    UserName = customer.CustomerEmail,
                    CustomerId =db.Customers.Max(x=>x.CustomerId),
                    Text = frc["message"],
                    When = DateTime.Now.Date
                };
                db.Messages.Add(msg);
                db.SaveChanges();
            }                            
            return View();
        }                                         
        public ActionResult OurStory()
        {

            IndexHomeViewModel vm = new IndexHomeViewModel()
            {
                Articles = db.Articles.OrderBy(x => x.DateTime).Take(5).ToList(),
                BestProductsByPopularity = db.Products.Where(x => x.Popularity >= 3).OrderByDescending(x => x.Popularity).Take(5).ToList(),
                AllProducts = db.Products.ToList(),
                AllCategories = db.Categories.ToList(),
                GetDays = Product.GetDays()
            };

            return View(vm);
        }
        public ActionResult Blog()
        {
            var article = db.Articles.ToList();
            return View(article);
        }

        


       
        public ActionResult SingleBlog(FormCollection frc,int?id )
        {

            if (id == null)
            {
                return View("~/Views/Shared/Error.cshtml");

            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                 return View("~/Views/Shared/Error.cshtml");
            }
            if (ModelState.IsValid)
            {
                Comment comment = new Comment()
                {
                    ArticleId = article.ArticleId,
                    CustomerName = User.Identity.Name,
                    CustomerEmail = User.Identity.Name,
                    Comments = frc["comment"],
                };
                db.Entry(comment).State=System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                var articlepercomment = db.Comments.Where(x => x.Article.ArticleId == id).ToList();
                ArticleCommentsUserViewModel vm = new ArticleCommentsUserViewModel()
                {
                    Article = article,
                    Comment = comment,
                    Comments = articlepercomment,

                };

                return View(vm);
                
            }
            else
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            
           
                                                             
        }
        public ActionResult OurBeer()
        {           
            IndexHomeViewModel vm = new IndexHomeViewModel()
            {
                BestProductsByPopularity = db.Products.Where(x => x.Popularity >= 3).OrderByDescending(x => x.Popularity).Take(10).ToList(),
                Articles = db.Articles.Take(2).ToList(),
                TwoProductsForPage = db.Products.Take(2).ToList(),   
                AllProducts=db.Products.ToList(),
                AllCategories=db.Categories.ToList(),
                GetDays = Product.GetDays()

            }; 
            return View(vm);
        }
       


    }
}