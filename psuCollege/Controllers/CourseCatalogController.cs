using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using psuCollege.BusinessLogic;
using psuCollege.DTOs;
using psuCollege.Models;
using System.Net;

namespace psuCollege.Controllers
{
    [Route("[controller]")] 
    [ApiController]
    public class CourseCatalogController : ControllerBase
    {
        private readonly IConfiguration config;
        private CourseCatalogBL business;
        public CourseCatalogController(IConfiguration _config) {
            this.config = _config;
            business = new CourseCatalogBL(config);
        }

        [HttpGet]
        public async Task<IEnumerable<CourseModel>> GetCourseListAsync()
        {// TODO: Create try catch with http response 
            return  await business.GetCourseList();
        }

        [HttpPost]
        public async Task<int> InsertCourseAsync([FromBody]CourseModel model)
        { // TODO: Create try catch with http response
                return await business.InsertCourseAsync(model); 
        }

        [HttpPut]
        public async Task<int> UpdateCourseAsync([FromBody] CourseModel model)
        { // TODO: Create try catch with http response
            return await business.UpdateCourseAsync(model);
        }

        [HttpDelete("{id}")]
        public async Task<int> DeleteCourseAsync(int id)
        { // TODO: Create try catch with http response
            return await business.DeleteCourseAsync(id);
        }
    } 
  
}
