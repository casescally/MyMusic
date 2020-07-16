import React, { useContext, useState, useEffect } from "react";
import { SongContext } from "./SongProvider";
import { getUser } from "../../API/userManager";
import "./Songs.css";
import ThumbnailGallery from "../thumbnail-gallery/Thumbnail-Gallery";

export default (props) => {
  const user = getUser();
  const { songs, addSong, saveImages, saveImage, updateSong } = useContext(
    SongContext
  );
  const [song, setSong] = useState({});
  const [songImages, setSongImages] = useState([]);
  const [songCoverImage, setSongCoverImage] = useState([]);
  const [newSongsFiles, setNewSongsFiles] = useState([]);
  const [newSongsCoverFile, setNewSongsCoverFile] = useState([]);
  const editMode = props.match.params.hasOwnProperty("songId");

  const handleControlledInputChange = (event) => {

    const newSong = Object.assign({}, song);
    newSong[event.target.name] = event.target.value;
    setSong(newSong);
  };

  const setDefaults = () => {
    if (editMode) {
      const songId = parseInt(props.match.params.songId);
      const selectedSong = songs.find((a) => a.id === songId) || {};
      setSong(selectedSong);
    }
  };

  useEffect(() => {
    setDefaults();
  }, [songs]);


  const constructNewSong = async () => {
    if (editMode) {
      //Filter the song images that are not blob data
      let existingImgs = songImages.filter((song) => !song.startsWith("blob"));
      let existingCoverImg = songCoverImage.filter(
        (coverImg) => !coverImg.startsWith("blob")
      );
      //let existingCoverImg = songPageCoverUrl
      if (newSongsFiles.length) {
        const filePaths = JSON.parse(await saveImages(newSongsFiles));
        existingImgs = existingImgs.concat(filePaths);
      }

      if (newSongsCoverFile.length) {
        const songCoverImage = JSON.parse(await saveImages(newSongsCoverFile));
        existingCoverImg = songCoverImage;
      }


      updateSong({
        ...song,
        genre: parseInt(song.genre),
        applicationUserId: user.id,
        songPageCoverUrl: JSON.stringify(existingCoverImg),
        imageFileNames: JSON.stringify(existingImgs),
      }).then(() => {
        props.history.push("/songs");
      });
    } else {

      let filePaths = [];

      if (newSongsFiles.length) {
        console.log("before uploadd upp===>>>", newSongsFiles);
        const uploadedSongImages = await saveImages(newSongsFiles);
        filePaths = JSON.parse(uploadedSongImages);
        coverIMG = filePaths[0];
        console.log("upp==>>>>>", JSON.stringify(filePaths));
      }


      addSong({
        name: song.name,
        applicationUserId: user.id,
        genre: song.genre,
        songPageCoverUrl: JSON.stringify([coverIMG]),
        imageFileNames: JSON.stringify(filePaths),
        songDescription: song.songDescription,
        activeSong: true,
      }).then(() => props.history.push("/songs"));
    }
  };

  useEffect(() => {
    const images = song.imageFileNames;
    if (images) setSongImages(JSON.parse(images));
  }, [song.imageFileNames]);


  const updateSongsCoverImage = (file) => {

if (file && file.startsWith("blob")) {
  
    setNewSongsCoverFile([file]);
}

    //console.log('upppdatedddd===>>>>', file, 'newCOV', newSongsCoverFile)
}

  const handleAddImages = (files) => {
    //console.log("heyyup==>>>", files);
    const newImgs = files.map((file) => URL.createObjectURL(file));
    setSongImages([...newImgs, ...songImages]);
    setNewSongsFiles(files.concat(newSongsFiles));
  };

  const handleRemoveImage = (index) => {
    const imageToDelete = songImages[index];
    let updatedImages = [...songImages];
    updatedImages.splice(index, 1);

    if (imageToDelete.startsWith("blob")) {
      let updatedSongFiles = [...newSongsFiles];
      updatedSongFiles.splice(index, 1);
      setNewSongsFiles(updatedSongFiles);
    }
    setSongImages(updatedImages);
  };

  console.log(songImages);
  return (
    <form className="songForm">
      <h2 className="songForm__title">{editMode ? "Update Song" : "Add Song"}</h2>
      <fieldset>
        <div className="form-group">
          <label htmlFor="name">Song name: </label>
          <input
            type="text"
            name="name"
            required
            autoFocus
            className="form-control"
            proptype="varchar"
            placeholder="Song name"
            defaultValue={song.name}
            onChange={handleControlledInputChange}
          />
        </div>
      </fieldset>
      <fieldset>
        <div className="form-group">
          <label htmlFor="genre">Genre: </label>
          <input
            name="genre"
            required
            className="form-control"
            proptype="varchar"
            placeholder="Genre"
            defaultValue={song.genre}
            onChange={handleControlledInputChange}
          >
          </input>
        </div>
      </fieldset>
      <fieldset>
        <div className="form-group" id="songDescription">
          <label htmlFor="songDescription">Description: </label>
          <textarea
            name="songDescription"
            className="form-control"
            id="songDescriptionForm"
            value={song.songDescription}
            onChange={handleControlledInputChange}
          ></textarea>
        </div>
      </fieldset>
      <button
        type="submit"
        id="addSongButton"
        onClick={(evt) => {
          evt.preventDefault();
          constructNewSong();
        }}
        className="btn btn-primary"
      >
        {editMode ? "Save Updates" : "Add Song"}
      </button>
    </form>
  );
};
