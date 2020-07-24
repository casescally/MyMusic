import React, { useContext } from "react";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import { SongContext } from "../song/SongProvider";
import Song from "../song/Song";
import { UserContext } from "../user/UserProvider";
import { getUser } from "../../API/userManager";
import "react-tabs/style/react-tabs.css";

export const Main = (props) => {
  const { songs } = useContext(SongContext);

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
            <TabPanel className="tabPanel">
              <article className="songList">
                <h3>Songs: {songs.length}</h3>

                {songs.map((song) => (
                  <Song key={song.id} car={song} {...props} />
                ))}
              </article>
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
