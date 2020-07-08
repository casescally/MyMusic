using System;
using System.Collections.Generic;
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
        public string Get(int id)
        {
            return "value";
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
    }
}
