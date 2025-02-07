﻿using Omadiko.Database;
using Omadiko.Entities;
using Omadiko.Entities.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Omadiko.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Product
        public ActionResult Index(int? pSize, int? page,string sortOrder,string searchProduct)
        {
            List<Product> products =Filtering(sortOrder);
            //Filtering
            products = Filter(searchProduct, products);
            //Sorting
            products= Sorting(sortOrder, products);
            int pageSize, pageNumber;
            Pagination(pSize, page, out pageSize, out pageNumber);
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        private static List<Product>Filter(string searchProduct, List<Product> products)
        {
            if (!String.IsNullOrEmpty(searchProduct))
            {
                products = products.Where(x => x.ProductName.ToUpper().Contains(searchProduct.ToUpper())).ToList();
            }
            return products;
        }
        private static void Pagination(int? pSize, int? page, out int pageSize, out int pageNumber)
        {
            pageSize = pSize ?? 6;
            pageNumber = page ?? 1;
        }
        private static List<Product> Sorting(string sortOrder,List<Product>products)
        {
            switch (sortOrder)
            {
                case "PriceDesc":products = products.OrderByDescending(x => x.Price).ToList();break;
                case "PriceAsc":products = products.OrderBy(x => x.Price).ToList();break;
                case "CategoryDesc":products = products.OrderByDescending(x => x.Category.CategoryName).ToList();break;
                case "CategoryAsc": products = products.OrderBy(x => x.Category.CategoryName).ToList();break;
                case "MostPopular":products = products.OrderBy(x => x.Popularity).ToList();break;
                case "LessPopular":products = products.OrderByDescending(x => x.Popularity).ToList();break;
                case "NameAsc":products = products.OrderBy(x => x.ProductName).ToList();break;
                case "NameDesc":products = products.OrderByDescending(x => x.ProductName).ToList();break;
                default:products = products.OrderBy(x => x.Price).ToList();break;
                   
            }
            return products;
        }
        private List<Product> Filtering(string sortOrder)
        {
            var products = db.Products.ToList();
            ViewBag.PD = String.IsNullOrEmpty(sortOrder) ? "PriceDesc" : "";
            ViewBag.PA = sortOrder == "PriceAsc" ? "PriceDesc" : "PriceAsc";
            ViewBag.MP = sortOrder == "MostPopular" ? "LessPopular" : "MostPopular";
            ViewBag.LP = sortOrder == "LessPopular" ? "MostPopular" : "LessPopular";
            ViewBag.NA = sortOrder == "NameAsc" ? "NameDesc" : "NameAsc";
            ViewBag.ND = sortOrder == "NameDesc" ? "NameAsc" : "NameDesc";
            ViewBag.CatDesc = sortOrder == "CategoryDesc" ? "CategoryAsc" : "CategoryDesc";
            ViewBag.CatAsc = sortOrder == "CategoryAsc" ? "CategoryDesc" : "CategoryAsc";
            ViewBag.CurrentSortOrder = sortOrder;

            return products;
        }


        //[HttpGet]
        //public ActionResult ProductInfo()
        //{
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ProductInfo(int? id,FormCollection frc)
        {
           if(id == null)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            Product product = db.Products.Find(id);
            if(product == null)
            {
                return  View("~/Views/Shared/Error.cshtml");
            }
            var mostpopular = db.Products.Where(x => x.Popularity >= 3).OrderByDescending(x => x.Popularity).Take(5).ToList();
            if (ModelState.IsValid)
            {
                
                
                Blog blog = new Blog()
                {
                    ProductId = product.ProductId,
                    CustomerName = User.Identity.Name,
                    CustomerEmail = User.Identity.Name,
                    Comments = frc["comment"]
                };
                db.Blogs.Add(blog);
                db.SaveChanges();



                var blogs = db.Blogs.Where(x => x.Product.ProductId == id).ToList();
                IndexHomeViewModel vm = new IndexHomeViewModel()
                {
                    Blog = blog,
                    Product = product,
                    BestProductsByPopularity = mostpopular,
                    IndividualBlogForProduct = blogs

                };
                return View(vm);
            }

            return RedirectToAction("Index");
        }
         
    }
}