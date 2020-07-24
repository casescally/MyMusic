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
      <div className="songInfo">
        <button
          className="songPlayButton"
          onClick={function () {
            const player = document.getElementById("songPlayer");
            const audioPlayer = player.parentElement;
            player.src = `${song.url}`;
            audioPlayer.load();
          }}
        >
          <img
            className="playButtonIcon"
            src="https://firebasestorage.googleapis.com/v0/b/hifi-ed258.appspot.com/o/images%2FPlayButton3.png?alt=media&token=16374b88-23e6-4c1a-843a-ed22878773f2"
            alt="playButtonIcon"
          ></img>
        </button>
        <img className="coverImage" src={song.songCoverUrl}></img>
      </div>
    </section>
  );
};
