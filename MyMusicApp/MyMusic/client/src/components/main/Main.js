import React, { useContext, useState, useEffect } from "react";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import { SongContext, getSongFile } from "../song/SongProvider";
import Song from "../song/Song";
import { UserContext } from "../user/UserProvider";
import { getUser } from "../../API/userManager";
import "react-tabs/style/react-tabs.css";
import { SongList } from "../song/SongList";
import ReactAudioPlayer from "react-audio-player";
import "./Main.css";
//import {getSongFile} from "../song/SongProvider"
//import { createAuthHeaders } from "../../API/userManager";
import butterchurn from "butterchurn";
import butterchurnPresets from "butterchurn-presets";

export const Main = (props) => {
  const { songs } = useContext(SongContext);
  const [selectedSong, setSelectedSong] = useState({});
  const [songFile, setSongFile] = useState([]);
  const [songFileUrl, setSongFileUrl] = useState([]);
  let userMediaCallback = (stream) => {


    
    let audioContext = new AudioContext();
    let audioNode = audioContext.createMediaStreamSource(stream);

    //React.createElement('div');
    // const canvas = React.createElement;

    //  React.render(
    //    canvas('div', null, 'Hello World'),
    //    document.getElementById('root')
    //  );

    //const canvas = document.querySelector(boldElement)

      const canvas = document.getElementsByClassName("mainProfileSection");
    
    
    const visualizer = butterchurn.createVisualizer(audioContext, canvas, {
      width: 800,
      height: 600,
    });

    // get audioNode from audio source or microphone

    visualizer.connectAudio(audioNode);

    // load a preset

    const presets = butterchurnPresets.getPresets();
    const preset =
      presets["Flexi, martin + geiss - dedicated to the sherwin maxawow"];

    visualizer.loadPreset(preset, 0.0); // 2nd argument is the number of seconds to blend presets

    // resize visualizer

    visualizer.setRendererSize(1600, 1200);

    // render a frame

    visualizer.render();
  };

  function handleClick(e) {
    e.preventDefault();
    let theSong = songs.find((song) => song.name === e.target.id);
    setSelectedSong(theSong);
    //console.log("current playing song====>>>>", selectedSong);

    const getSongFile = () => {
      return fetch(`http://127.0.0.1:8887/${selectedSong.url}`, {})
        .then((res) => res.blob())
        .then((blob) => {
          const mp3 = new Blob([blob], { type: "audio/mp3" });
          var blobUrl = URL.createObjectURL(blob);
          setSongFileUrl(blobUrl);
          setSongFile(mp3);
        });
    };
    getSongFile();

    let myStream = new MediaStream();
    if (selectedSong&&myStream.active) {
      
      const reactAudioPlayer = document.getElementById("audioPlayer");
      console.log(myStream);
      reactAudioPlayer.captureStream(myStream);
      userMediaCallback(myStream)

    }
    getSongFile(selectedSong.url);
  }

  useEffect(() => {
    console.log("selected", selectedSong);
    console.log("song file====>>>>", songFile);
    console.log("song file URL====>>>>", songFileUrl);
  });

  return (
    <div className="profile top-space">
      <section className="userProfile">
        <div className="mainProfileSection">
          <Tabs>
            <TabList className="custom-tabs">
              <Tab>Songs</Tab>
              <Tab>Merch</Tab>
              {/* <Tab>Liked Cars</Tab> */}
            </TabList>
            <TabPanel className="tabPanel" id="songTab">
              <ReactAudioPlayer
                src={songFile && songFileUrl}
                autoPlay="false"
                controls
                controlsList="nodownload"
                id="audioPlayer"
              />
              {songs.map((song) => (
                <div>
                  {song.name}
                  <button
                    type="button"
                    value="button"
                    id={song.name}
                    onClick={(e) => handleClick(e)}
                  >
                    Play
                  </button>
                </div>
              ))}

              {/* {songs.map((song) => (
                <Song key={song.id} song={song} {...props} />
              ))} */}
            </TabPanel>
            <TabPanel className="tabPanel">
              <article className="merch"></article>
            </TabPanel>
          </Tabs>
        </div>
      </section>
    </div>
  );
};
