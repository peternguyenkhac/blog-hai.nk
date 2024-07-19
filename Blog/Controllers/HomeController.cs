using Blog.Data;
using Blog.Models;
using Blog.Utils;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogContext _blogContext;
        private IValidator<Post> _postValidator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(BlogContext blogContext, IWebHostEnvironment webHostEnvironment, IValidator<Post> postValidator)
        {
            _blogContext = blogContext;
            _webHostEnvironment = webHostEnvironment;
            _postValidator = postValidator;
        }

        public async Task<IActionResult> Index(string searchString = "")
        {
            var query = _blogContext.Posts.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Title.Contains(searchString));
            }
            var posts = await query.ToListAsync();
            ViewBag.SearchString = searchString;
            return View(posts);
        }

        [Route("CreatePost")]
        public IActionResult CreatePost()
        {
            BuildOptions();
            return View();
        }

        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> CreatePost(Post post, IFormFile imageUpload)
        {
            ValidationResult result = await _postValidator.ValidateAsync(post);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                BuildOptions();
                return View(post);
            }
            string urlImage = "";
            if(imageUpload != null)
            {
                var uploadDirecotroy = "uploads/";
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, uploadDirecotroy);
                var fileName = imageUpload.FileName;
                var filePath = Path.Combine(uploadPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageUpload.CopyTo(fileStream);
                }

                urlImage = "/" + uploadDirecotroy + fileName;
            }
            post.Image = urlImage;
            await _blogContext.Posts.AddAsync(post);
            await _blogContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Route("EditPost/{id}")]
        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _blogContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return RedirectToAction("Error");
            }
            BuildOptions();
            return View(post);
        }


        [HttpPost]
        [Route("EditPost/{id}")]
        public async Task<IActionResult> EditPost(int id, Post updatedPost, IFormFile imageUpload)
        {
            ValidationResult result = await _postValidator.ValidateAsync(updatedPost);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                BuildOptions();
                return View(updatedPost);
            }
            string urlImage = "";
            if (imageUpload != null)
            {
                var uploadDirecotroy = "uploads/";
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, uploadDirecotroy);
                var fileName = imageUpload.FileName;
                var filePath = Path.Combine(uploadPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageUpload.CopyTo(fileStream);
                }

                urlImage = "/" + uploadDirecotroy + fileName;
            }
            if (!String.IsNullOrEmpty(urlImage))
            {
                updatedPost.Image = urlImage;
            }
            _blogContext.Update(updatedPost);
            await _blogContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Route("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _blogContext.Posts.FirstAsync(p => p.Id == id);
            _blogContext.Posts.Remove(post);
            await _blogContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void BuildOptions()
        {
            ViewBag.Categories = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Du lịch", Text = "Du lịch" },
                new SelectListItem { Value = "Ẩm thực", Text = "Ẩm thực" },
                new SelectListItem { Value = "Giải trí", Text = "Giải trí" }
            }, "Value", "Text");


            ViewBag.Positions = new List<string> {"Việt Nam", "Trung Quốc", "Châu Á", "Châu Âu", "Châu Mỹ"};
        }
    }
}
