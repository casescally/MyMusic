import React, { useContext, useState, useEffect } from "react";
import { SongContext } from "./SongProvider";
import { getUser } from "../../API/userManager";
import Dropzone from "react-dropzone";
import "./Songs.css";

export default (props) => {
  const user = getUser();
  const { songs, addSong, saveImages, saveFile, updateSong } = useContext(
    SongContext
  );
  const [song, setSong] = useState({});
  //const [songImages, setSongImages] = useState([]);
  //const [songAudioFile, setNewSongsAudioFile] = useState([]);
  const [songsAudioFile, setSongsAudioFile] = useState([]);
  const [songsAudioFileName, setSongsAudioFileName] = useState([]);
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
      let existingAudioFile = songsAudioFile.filter(
        (song) => !song.startsWith("blob")
      );

      if (songsAudioFile.length) {
        const filePaths = JSON.parse(await saveFile(songsAudioFile));
        existingAudioFile = existingAudioFile.concat(songsAudioFileName);
      }

      if (songsAudioFile.length) {
        const songCoverImage = JSON.parse(await saveFile(songsAudioFile));
        existingAudioFile = songCoverImage;
      }

      updateSong({
        ...song,
        genre: parseInt(song.genre),
        applicationUserId: user.id,
        songAudioFile: JSON.stringify(existingAudioFile),
        //imageFileNames: JSON.stringify(existingImgs),
      }).then(() => {
        props.history.push("/songs");
      });
    } else {
      let filePaths = [];
      if (songsAudioFile.length) {
        console.log("before uploadd upp===>>>", songsAudioFile);
        const uploadedSongFile = await saveFile(songsAudioFile);
        filePaths = JSON.parse(uploadedSongFile);
        console.log("upp==>>>>>", JSON.stringify(filePaths));
      }

      addSong({
        name: "dasdas",
        audioFileName: JSON.stringify(songsAudioFileName),
        applicationUserId: user.id,
        genre: song.genre,
        description: song.description,
        activeSong: true,
        url: "testUrl",
        coverUrl: "testCoverUrl",
        imageFileName: "testImageFileName",
      }).then(() => props.history.push("/songs"));
    }
  };

  useEffect(() => {
    const audioFile = song.songAudioFile;
    if (audioFile) setSongsAudioFile(JSON.parse(audioFile));
  }, [song.songAudioFile]);

  const updateSongsAudioFile = (file) => {
    if (file && file.startsWith("blob")) {
      setSongsAudioFile([file]);
    }

    //console.log('upppdatedddd===>>>>', file, 'newCOV', newSongsCoverFile)
  };

  const handleAddAudioFile = (files) => {
    //console.log("heyyup==>>>", files);
    console.log(files);
    const newFile = files[0];
    //const newFile = files.forEach((file) => URL.createObjectURL(file));
    setSongsAudioFile(newFile);
    //console.log(newFile);
    setSongsAudioFileName(newFile.name);
  };

  const handleRemoveAudioFile = (index) => {
    const audioFileToDelete = songsAudioFile[index];
    let updatedAudioFile = [...songsAudioFile];
    updatedAudioFile.splice(index, 1);
    let updatedSongsAudioFile = [];
    if (audioFileToDelete.startsWith("blob")) {
      let updatedSongsAudioFile = [...songsAudioFile];
      updatedSongsAudioFile.splice(index, 1);
      setSongsAudioFile(updatedSongsAudioFile);
    }
    setSongsAudioFile(updatedSongsAudioFile);
  };
  console.log("file=====>", songsAudioFile);
  console.log("fileNAME===>>>>", songsAudioFile);
  console.log(song);
  return (
    <form className="songForm">
      <h2 className="songForm__title">
        {editMode ? "Update Song" : "Add Song"}
      </h2>
      <fieldset>
        <div className="form-group">
          <label htmlFor="name">Song file: </label>
          <Dropzone
            onChange={handleControlledInputChange}
            //value={song.audioFileName}
            onDrop={(acceptedFiles) => handleAddAudioFile(acceptedFiles)}
          >
            {({ getRootProps, getInputProps }) => (
              <section>
                <div {...getRootProps()}>
                  <input name="audioFileName" {...getInputProps()} />
                  <p>Drag 'n' drop some files here, or click to select files</p>
                </div>
              </section>
            )}
          </Dropzone>
        </div>
      </fieldset>
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
          ></input>
        </div>
      </fieldset>
      <fieldset>
        <div className="form-group" id="songDescription">
          <label htmlFor="songDescription">Description: </label>
          <textarea
            name="songDescription"
            className="form-control"
            id="songDescriptionForm"
            value={song.description}
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
