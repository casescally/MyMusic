﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class PlaylistsController : ControllerBase
    {

                private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlaylistsController(IConfiguration config, UserManager<ApplicationUser> userManager)

        {
            _config = config;
            _userManager = userManager;
        }

        public SqlConnection Connection

        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: api/Playlists
        [HttpGet]
        public async Task<IActionResult> Get()
        {
  using (SqlConnection conn = Connection)

            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =

                        @"SELECT pl.Id AS Id, pl.ApplicationUserId, pl.SongIds AS SongIds
                          FROM PLaylists pl
                          LEFT JOIN AspNetUsers a
                          ON pl.ApplicationUserId = a.Id";
                
                SqlDataReader reader = cmd.ExecuteReader();

                    List<Playlist> playlists = new List<Playlist>();

                    try {

                    while (reader.Read())
                    {
                            Playlist foundPlaylist = new Playlist

                            {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            SongIds = reader.GetString(reader.GetOrdinal("SongIds")),
                            ApplicationUserId = reader.GetString(reader.GetOrdinal("ApplicationUserId")),

                        };        

                            playlists.Add(foundPlaylist);
                    }
                    } catch (Exception ex) { }
                       
                    reader.Close();

                    return Ok(playlists);
                    
                }
            }
        }

        // GET: api/Playlists/5
        [HttpGet("{id}", Name = "GetPlaylist")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
 using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT pl.Id, pl.Name, pl.ApplicationUserId, pl.SongIds, a.Id AS UserId, a.UserName, a.FirstName, a.LastName
                                        FROM Playlists pl
                                        LEFT JOIN AspNetUsers a
                                        ON pl.ApplicationUserId = a.Id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Playlist individualPlaylist = null;

                    if (reader.Read())
                    {
                    
                        individualPlaylist = new Playlist

                            {

                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ApplicationUserId = reader.GetString(reader.GetOrdinal("ApplicationUserId")),
                            SongIds = reader.GetString(reader.GetOrdinal("SongIds"))

                        };

                        reader.Close();

                        return Ok(individualPlaylist);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }

        // POST: api/Playlists
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Playlist newPlaylist)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Playlists (ApplicationUserId, SongIds, Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@applicationUserId, @songIds, @name)";

                    cmd.Parameters.Add(new SqlParameter("@applicationUserId", newPlaylist.ApplicationUserId));
                    cmd.Parameters.Add(new SqlParameter("@name", newPlaylist.Name));
                    cmd.Parameters.Add(new SqlParameter("@songIds", newPlaylist.SongIds));

                    int newId = (int)cmd.ExecuteScalar();
                    newPlaylist.Id = newId;
                    return CreatedAtRoute("GetPlaylist", new { id = newId}, newPlaylist);
                }
            }
        }

        // PUT: api/Playlists/5
        [HttpPut("{id}")]
        public async Task<IActionResult>Put([FromRoute]int id, [FromBody] Playlist updatedPlaylist)
        {
             try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE MyMusic.dbo.Playlists
                                            SET Name = @name,
                                            SongIds = @songIds
                                            WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@Name", updatedPlaylist.Name));
                    cmd.Parameters.Add(new SqlParameter("@SongIds", updatedPlaylist.SongIds));

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
                if (!PlaylistExists(id))
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
        public async Task<IActionResult>Delete([FromRoute] int id)
        {
                  try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Playlists
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
                if (!PlaylistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
                 private bool PlaylistExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name
                        FROM Playlist
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}
