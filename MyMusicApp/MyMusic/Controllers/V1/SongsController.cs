using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyMusic.Models.Data;

namespace MyMusic.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly IConfiguration _config;

        
        public static IWebHostEnvironment _environment;

        public SongsController(IConfiguration config, IWebHostEnvironment environment)
        {
            _config = config;
            _environment = environment;
        }

        public SqlConnection Connection

        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

                public class FileUploadInterface
        {
            public IFormFile files { get; set; }
        }


        // GET: api/Songs
        [EnableCors("MyPolicy")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)

            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * FROM Songs WHERE ActiveSong = 1";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Song> songs = new List<Song>();

                    try { 
                    while (reader.Read())
                    {
                            Song foundSong = new Song
                            {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Url = reader.GetString(reader.GetOrdinal("Url")),
                            ApplicationUserId = reader.GetString(reader.GetOrdinal("ApplicationUserId")),
                            Genre = reader.GetString(reader.GetOrdinal("Genre")),
                            CoverUrl = reader.GetString(reader.GetOrdinal("CoverUrl")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            ForSale = reader.GetBoolean(reader.GetOrdinal("ForSale")),
                            ImageFileName = reader.GetString(reader.GetOrdinal("ImageFileName"))
                            };
                            songs.Add(foundSong);
                        }
                    } catch (Exception Ex) { }

                    reader.Close();

                    return Ok(songs);
                }
            }
        }

        // GET: api/Songs/5
        [EnableCors("MyPolicy")]
        [HttpGet("{id}", Name = "GetSong")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.Name, s.Url, s.ApplicationUserId, s.Genre, s.CoverUrl, s.Description, s.ForSale, s.ImageFileName, s.ActiveSong, a.UserName, a.FirstName, a.LastName
                                        FROM Songs s
                                        LEFT JOIN AspNetUsers a
                                        ON s.ApplicationUserId = a.Id
                                        WHERE s.Id = @id AND s.ActiveSong = 1";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Song individualSong = null;

                    if (reader.Read())
                    {
                        individualSong = new Song
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Url = reader.GetString(reader.GetOrdinal("Url")),
                            ApplicationUserId = reader.GetString(reader.GetOrdinal("ApplicationUserId")),
                            Genre = reader.GetString(reader.GetOrdinal("Genre")),
                            CoverUrl = reader.GetString(reader.GetOrdinal("CoverUrl")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            ForSale = reader.GetBoolean(reader.GetOrdinal("ForSale")),
                            ImageFileName = reader.GetString(reader.GetOrdinal("ImageFileName"))
                        };
                        reader.Close();

                        return Ok(individualSong);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }

        [HttpPost("files")]
        public async Task<List<string>> PostFile()
        {
            var savedFilePaths = new List<string>();

            if (Request.Form.Files.Count > 0)
            {
                EnsureUploadDirectoryExists();
                foreach (IFormFile file in Request.Form.Files)
                {
                    string savedFilePath = String.Empty;
                    if (file != null && file.Length > 0)
                    {
                        savedFilePath = _environment.WebRootPath + "\\Upload\\"+ Path.GetFileName(file.FileName);
                        using (var fileStream = new FileStream(savedFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        savedFilePaths.Add(savedFilePath);
                    }
                }
            }

            return savedFilePaths;
        }

        private static void EnsureUploadDirectoryExists()
        {
            if (String.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
            }
        }

        // POST: api/Songs
        [HttpPost]
        public void Post([FromForm] Song newSong)
        {
                        using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Songs (Name, Url, ApplicationUserId, Genre, CoverUrl, Description, ForSale, ImageFileName, ActiveSong)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name, @url, @applicationUserId, @genre, @coverUrl, @description, @forSale, @imageFileName, @activeSong)";

                    cmd.Parameters.Add(new SqlParameter("@name", newSong.Name));
                    cmd.Parameters.Add(new SqlParameter("@url", newSong.Url));
                    cmd.Parameters.Add(new SqlParameter("@applicationUserId", newSong.ApplicationUserId));
                    cmd.Parameters.Add(new SqlParameter("@genre", newSong.Genre));
                    cmd.Parameters.Add(new SqlParameter("@coverUrl", newSong.CoverUrl));
                    cmd.Parameters.Add(new SqlParameter("@description", newSong.Description));
                    cmd.Parameters.Add(new SqlParameter("@forSale", newSong.ForSale));
                    cmd.Parameters.Add(new SqlParameter("@imageFileName", newSong.ImageFileName));
                    cmd.Parameters.Add(new SqlParameter("@activeSong", newSong.ActiveSong));

                    int newId = (int)cmd.ExecuteScalar();
                    newSong.Id = newId;

                }
            }
        }

        // PUT: api/Songs/5
        [HttpPut("{id}")]
        public async Task<IActionResult>Put([FromRoute]int id, [FromBody] Song updatedSong)
        {
                       try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Songs
                                            SET Name = @name,
                                            Url = @url,
                                            ApplicationUserId = @applicationUserId,
                                            Genre = @genre,
                                            CoverUrl = @coverUrl,
                                            Description = @description,
                                            ForSale = @forSale,
                                            ImageFileName = @imageFileName,
                                            ActiveSong = @activeSong
                                            WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.Parameters.Add(new SqlParameter("@name", updatedSong.Name));
                    cmd.Parameters.Add(new SqlParameter("@url", updatedSong.Url));
                    cmd.Parameters.Add(new SqlParameter("@applicationUserId", updatedSong.ApplicationUserId));
                    cmd.Parameters.Add(new SqlParameter("@genre", updatedSong.Genre));
                    cmd.Parameters.Add(new SqlParameter("@coverUrl", updatedSong.CoverUrl));
                    cmd.Parameters.Add(new SqlParameter("@description", updatedSong.Description));
                    cmd.Parameters.Add(new SqlParameter("@forSale", updatedSong.ForSale));
                    cmd.Parameters.Add(new SqlParameter("@imageFileName", updatedSong.ImageFileName));
                    cmd.Parameters.Add(new SqlParameter("@activeSong", updatedSong.ActiveSong));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!SongExists(id))
                { 
                    return NotFound();
                    } else
                {
                    throw;
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Songs
                                            SET ActiveSong = 'false'
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
                if (!SongExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
                 private bool SongExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name
                        FROM Songs
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}
