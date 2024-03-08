using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using psuCollege.DTOs;
using psuCollege.Models;
using System.Reflection;
using System.Xml.Linq;
using System.Net;

namespace psuCollege.BusinessLogic
{
    public class CourseCatalogBL
    {
        private readonly IConfiguration config;

        public CourseCatalogBL(IConfiguration _config)
        {
            config = _config;
        }

        public async Task<IEnumerable<CourseModel>> GetCourseList()
        {
            using var connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
             
            IEnumerable<CourseCatalogDTO> courseDTO = await connection.QueryAsync<CourseCatalogDTO>("sp_GetAllCourses", commandType: CommandType.StoredProcedure);

            var courseNumber = courseDTO.Select(x => x.ID).Distinct().ToList();

            List<CourseModel> models = new List<CourseModel>();

            foreach (var num in courseNumber)
            {
                var item = courseDTO.Where(x => x.ID == num).ToList().FirstOrDefault();

                var days = courseDTO.Where(x => x.ID == num).Select(y => y.Day.ToLower()).ToList();

                CourseModel model = new CourseModel()
                {
                    ID = item.ID,
                    Name = item.Description,
                    RoomNumber = item.RoomNumber,
                    Professor = new ProfessorModel()
                    {
                        ID = item.ProfessorID,
                        Name = item.Name,
                        Email = item.Email
                    },
                    Days = new DaysModel()
                    {
                        Monday = days.Contains("monday"),
                        Tuesday = days.Contains("tuesday"),
                        Wednesday = days.Contains("wednesday"),
                        Thursday = days.Contains("thursday"),
                        Friday = days.Contains("friday"),
                        Saturday = days.Contains("saturday"),
                        Sunday = days.Contains("sunday"),
                    }
                };

                models.Add(model);
            }

            return models;


        }

        public async Task<int> InsertCourseAsync(CourseModel model)
        {
            using (var connection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
            {  
                    try
                {// add begin transaction commit rollback close
                            DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("description", model.Name);
                        parameters.Add("roomNumber", model.RoomNumber);
                        parameters.Add("name", model.Professor.Name);
                        parameters.Add("email", model.Professor.Email); 
                        parameters.Add("ListDays", model.ListDays);
                       await connection.ExecuteAsync("sp_InsertCourse", parameters, commandType: CommandType.StoredProcedure);
                     
                        return 1;
                    }
                    catch(Exception ex)
                    { 
                        return -1; // return exception
                } 
                    
            } 
         }

        public async Task<int> UpdateCourseAsync(CourseModel model)
        {
            using (var connection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
            { 
                    try
                {// add begin transaction commit rollback close
                    DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("description", model.Name);
                        parameters.Add("roomNumber", model.RoomNumber);
                        parameters.Add("name", model.Professor.Name);
                        parameters.Add("email", model.Professor.Email);
                        parameters.Add("ListDays", model.ListDays);
                        parameters.Add("profesorId", model.Professor.ID); 
                        parameters.Add("courseId", model.ID);

                        await connection.ExecuteAsync("sp_UpdateCourse", parameters, commandType: CommandType.StoredProcedure);
                     
                        return 1;
                    }
                    catch (Exception ex)
                    { 
                        return -1;// return exception
                } 

            }
        }

        public async Task<int> DeleteCourseAsync(int id)
        {
            using (var connection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
            { 
                    try
                    {// add begin transaction commit rollback close
                        DynamicParameters parameters = new DynamicParameters(); 
                        parameters.Add("courseId", id);

                         await connection.ExecuteAsync("sp_DeleteCourse", parameters, commandType: CommandType.StoredProcedure);
                     
                        return 1;
                    }
                    catch (Exception ex)
                    { 
                        return -1; // return exception
                    } 

            }
        }
    }
    
}
