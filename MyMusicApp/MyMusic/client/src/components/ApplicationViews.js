import React from "react";
import { Route } from "react-router-dom";
import Home from "./Home";
import SongForm from "./song/SongForm";
import { SongProvider } from "./song/SongProvider";
export default function ApplicationViews() {
  return (
    <>
      <SongProvider>
        <Route exact path="/" render={() => <Home />} />

        <Route
          exact
          path="/songs/create"
          render={(props) => <SongForm {...props} />}
        />
      </SongProvider>
    </>
  );
}
