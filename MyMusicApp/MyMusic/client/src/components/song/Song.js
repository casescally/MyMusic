import React, { useContext, useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { UserContext } from "../user/UserProvider";
import "./Songs.css";
//import { getUser } from "../../API/userManager";
import ReactAudioPlayer from "react-audio-player";
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
        <ReactAudioPlayer src="" autoPlay controls />
        <img className="coverImage" src={song.songCoverUrl}></img>
      </div>
    </section>
  );
};
