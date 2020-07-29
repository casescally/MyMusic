import React, { useContext, useState, useEffect } from "react";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import { SongContext } from "../song/SongProvider";
import Song from "../song/Song";
import { UserContext } from "../user/UserProvider";
import { getUser } from "../../API/userManager";
import "react-tabs/style/react-tabs.css";
import { SongList } from "../song/SongList";
import ReactAudioPlayer from "react-audio-player";
import "./Main.css";

export const Main = (props) => {
  const { songs } = useContext(SongContext);
  const [selectedSong, setSelectedSong] = useState({});
  let updateSong = (song) => {
  setSelectedSong(song)
}

console.log('song====>>>>', selectedSong);
  useEffect(() => {
    updateSong()
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
                src={selectedSong && selectedSong.url}
                autoPlay
                controls
              />
              {songs.map((song) => (
                <button type="button" value="button" onClick={updateSong}>
                  Play {song.name}
                </button>
              ))}

              {/* {songs.map((song) => (
                <Song key={song.id} song={song} {...props} />
              ))} */}
            </TabPanel>
            {/* <TabPanel className="tabPanel">
              <article className="events">
                <h3>Events: {currentUsersEvents.length}</h3>

                {currentUsersEvents.map((event) => (
                  <Event key={event.id} event={event} {...props} />
                ))}
              </article>
            </TabPanel> */}
          </Tabs>
        </div>
      </section>
    </div>
  );
};
