import React, { useContext, useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { UserContext } from "../user/UserProvider";
import "./Songs.css";
//import { getUser } from "../../API/userManager";

export default ({ song }) => {
  //const { users } = useContext(UserContext);
  //const songUser = users.find((u) => u.id === song.userId) || {};
  const [songImages, setSongImages] = useState([]);

  useEffect(() => {
    const images = song.imagePath;
    if (images) setSongImages(JSON.parse(images));
  }, [song]);
  return (
    <section className="songSection">
      <div class="divider div-transparent"></div>
      <div className="songInfo">
        {songImages.map((image, i) => (
          <img
            key={i}
            src={`https://localhost:5001/api/SongImages/image/get?imageName=${image}`}
            className="song_image"
            alt="Song"
          />
        ))}
        <h3 className="song__name">
          <Link to={`/songs/${song.id}`} className="songLink">
            {song.name + "    "}
          </Link>
        </h3>
        <img className="coverImage" src={song.songCoverUrl}></img>
      </div>

      <div className="creatorInfo"></div>
    </section>
  );
};
