﻿using System;
using System.Linq;
using AbpDesk.Blogging;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace Areas.Blog.Controllers
{
    [Area("Blog")]
    public class PostsController : AbpController
    {
        private readonly IRepository<BlogPost, Guid> _blogPostRepository;

        public PostsController(IRepository<BlogPost, Guid> blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public ActionResult Index()
        {
            var posts = _blogPostRepository.ToList(); //TODO: async..?
            return View(posts);
        }
    }
}
