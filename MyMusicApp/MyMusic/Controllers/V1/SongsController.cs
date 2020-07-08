using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)

            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * FROM Songs";
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
        [HttpGet("{id}", Name = "GetSong")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.Name, s.Url, s.ApplicationUserId, s.Genre, s.CoverUrl, s.Description, s.ForSale, s.ImageFileName, a.FirstName, a.LastName, a.StreetAddress, a.ProfilePicturePath, a.ProfileBackgroundPicturePath, a.Description, a.ProfileHeader, a.ActiveUser
                                        FROM Songs s
                                        LEFT JOIN AspNetUsers a
                                        ON c.ApplicationUserId = a.Id
                                        WHERE c.Id = @id AND c.activeCar = 'true'";

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
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Songs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
