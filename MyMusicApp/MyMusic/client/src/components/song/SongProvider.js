import React, { useState, useEffect } from "react";
import { createAuthHeaders } from "../../API/userManager";

/*

    The context is imported and used by individual components

    that need data

*/

export const SongContext = React.createContext();

/*

 This component establishes what data can be used.

 */

export const SongProvider = (props) => {
  const [songs, setSongs] = useState([]);

  const getSongs = () => {
    const authHeader = createAuthHeaders();
    return fetch("https://localhost:5001/api/songs", {
      headers: authHeader,
    })
      .then((res) => res.json())
      .then(setSongs);
  };

  const saveImages = async (files) => {
    const authHeader = createAuthHeaders();
    const formData = new FormData();
    if (files) {
      Array.from(files).forEach((file) => {
        formData.append(file.name, file);
      });
    }

    const response = await fetch("https://localhost:5001/api/songs/files", {
      // content-type header should not be specified!
      method: "POST",
      headers: {
        authHeader,
      },
      body: formData,
      responseType: "text",
    });
    return response.text();
  };

  const saveFile = async (file) => {
    const authHeader = createAuthHeaders();
    const formData = new FormData();
    if (file) {
      formData.append(file.name, file);
    }

    const response = await fetch("https://localhost:5001/api/songs/Songs/audioFile", {
      // content-type header should not be specified!
      method: "POST",
      headers: {
        authHeader,
      },
      body: formData,
      responseType: "text",
    });
    return response.text();
  };

  const addSong = (song) => {
    const authHeader = createAuthHeaders();

    var form_data = new FormData();

    console.log(song);

    for (var key in song) {
      form_data.append(key, song[key]);
    }

    for (var key of form_data.entries()) {
      console.log(key[0] + ", " + key[1]);
    }

    return fetch("https://localhost:5001/api/songs", {
      method: "POST",

      headers: {
        authHeader,
      },

      body: form_data,
    }).then(getSongs);
  };

  const deleteSong = (song) => {
    const authHeader = createAuthHeaders();
    return fetch(`https://localhost:5001/api/songs/${song.id}`, {
      authHeader,
      method: "DELETE",
    }).then(getSongs);
  };

  const updateSong = (song) => {
    const authHeader = createAuthHeaders();
    return fetch(`https://localhost:5001/api/songs/${song.id}`, {
      method: "PUT",

      headers: {
        authHeader,
        "Content-Type": "application/json",
      },

      body: JSON.stringify(song),
    }).then(getSongs);
  };

  /*

        Load all songs when the component is mounted. Ensure that
        an empty array is the second argument to avoid infinite loop.

    */

  useEffect(() => {
    getSongs();
  }, []);

  useEffect(() => {
    //console.log("****  CAR APPLICATION STATE CHANGED  ****")
    //console.log(songs)
  }, [songs]);

  return (
    <SongContext.Provider
      value={{
        songs,
        addSong,
        deleteSong,
        updateSong,
        saveImages,
        saveFile,
      }}
    >
      {props.children}
    </SongContext.Provider>
  );
};
