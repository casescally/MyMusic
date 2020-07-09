using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyMusic.Models.Data;
using MyMusic.Models.ViewModels;

namespace MyMusic.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongImagesController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        public static IWebHostEnvironment _environment;

        public SongImagesController(IConfiguration config, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)

        {
            _config = config;
            _userManager = userManager;
            _environment = environment;
        }

        public SqlConnection Connection

        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: api/SongImages
        [HttpGet]
        [Route("image/get")]
        public IActionResult Get(string imageName)
        {
               // var filePath = _environment.WebRootPath + "\\Upload\\" + imageName;

            var filePath = imageName;

                Byte[] b = System.IO.File.ReadAllBytes(filePath);   // You can use your own method over here.       
            
                return File(b, "image/jpeg");

        }

        // GET: api/SongImages/5
        [HttpGet("{id}", Name = "GetSongImage")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT si.Id, si.SongId, si.ImagePath
                                        FROM SongImages ci
                                        LEFT JOIN Songs s
                                        ON si.SongId = s.Id
                                        WHERE Active = 0";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    SongImage individualSongImage = null;

                    if (reader.Read())
                    {
                    
                        individualSongImage = new SongImage

                            {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            SongId = reader.GetInt32(reader.GetOrdinal("SongId")),
                            ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),

                        };        

                          
                        reader.Close();

                        return Ok(individualSongImage);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }

        // POST: api/SongImages
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SongImage newSongImage)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO SongImages (SongId, ImagePath, Active)
                                        OUTPUT INSERTED.Id
                                        VALUES (@songId, @imagePath, @active)";

                    cmd.Parameters.Add(new SqlParameter("@songId", newSongImage.SongId));
                    cmd.Parameters.Add(new SqlParameter("@imagePath", newSongImage.ImagePath));
                    cmd.Parameters.Add(new SqlParameter("@active", newSongImage.Active));

                    int newId = (int)cmd.ExecuteScalar();
                    newSongImage.Id = newId;
                    return CreatedAtRoute("GetSongImage", new { id = newId}, newSongImage);
                }
            }
        }

        // PUT: api/SongImages/5
        //Update a song image
        [HttpPut("{id}")]
        public async Task<IActionResult>Put([FromRoute]int id, [FromBody] SongImage updatedSongImage)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE SongImages
                                            SET SongId = @songId,
                                            ImagePath = @imagePath,
                                            WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@songId", updatedSongImage.SongId));
                    cmd.Parameters.Add(new SqlParameter("@imagePath", updatedSongImage.ImagePath));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!SongImageExists(id))
                { 
                    return NotFound();
                    } else
                {
                    throw;
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        // Soft delete - sets the active song image boolean to 1
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE SongImages
                                            SET Active = 1
                                            WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                             return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
                        catch (Exception)
            {
                if (!SongImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


         private bool SongImageExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, SongId
                        FROM SongImages
                        WHERE Id = @id
                        AND Active = 1";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }

    }
}
