import React from "react";
import { Redirect, Route } from "react-router-dom";
import Home from "./Home";
import SongForm from "./song/SongForm";
import { SongProvider } from "./song/SongProvider";
import { getUser } from "../API/userManager";

export default function ApplicationViews() {
  return (
    <>
      <SongProvider>
        <Route
          exact
          path="/register"
          render={() => (getUser() ? <Redirect to="/" /> : <SongForm />)}
        />
      </SongProvider>
    </>
  );
}
