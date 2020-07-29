import React, { useContext, useState } from "react";
import { SongContext } from "./SongProvider";
import Song from "./Song";
import "./Songs.css";


export function SongList(props) {
  const { songs } = useContext(SongContext);

  return (
    <div className="tabPanel">
      <h1>Songs</h1>

      <div className="songs top-space">
        {songs.map((song) => {
          return <Song key={song.id} song={song} />;
        })}
      </div>
    </div>
  );
}
