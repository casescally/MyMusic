import React from "react";
import { Redirect, Route } from "react-router-dom";
import Home from "./Home";
import SongForm from "./song/SongForm";
import { SongProvider } from "./song/SongProvider";
import { getUser } from "../API/userManager";
import { Main } from "./main/Main";

export default function ApplicationViews(props) {
  return (
    <>
      <SongProvider>
        <Route
          exact
          path="/songs/create"
          render={() =>
            getUser() ? <SongForm {...props} /> : <Redirect to="/" />
          }
        />
        
      </SongProvider>
    </>
  );
}
