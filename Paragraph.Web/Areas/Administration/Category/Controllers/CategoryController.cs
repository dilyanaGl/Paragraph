using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Paragraph.Web.Areas.Administration.Category
{
    using Services.DataServices;

    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;    

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;           
            
        }

        
        
     
    }
}