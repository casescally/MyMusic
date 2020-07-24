import React from "react";
import { Redirect, BrowserRouter as Router, Route } from "react-router-dom";
import ApplicationViews from "./components/ApplicationViews";
import Header from "./components/Header";
import Login from "./components/Login";
import Register from "./components/Register";
import { getUser } from "./API/userManager";
import { SongList } from "./components/song/SongList";
import { Main } from "./components/main/Main";
import { SongProvider } from "./components/song/SongProvider";

import "./App.css";

function App() {
  return (
    <div className="App">
      <Router>
      <SongProvider>     
        <Header />
        <Route exact path="/" render={(props) => <Main {...props} />} />
        <Route
          exact
          path="/Songs"
          render={(props) => <SongList {...props} />}
        />

        <Route
          exact
          path="/login"
          render={() => (
            
            getUser() ? <Redirect to="/" /> : <Login />)}
        />
        <Route
          exact
          path="/register"
          render={() => (getUser() ? <Redirect to="/" /> : <Register />)}
        />
<Route
          render={() =>
            getUser() ? <ApplicationViews /> : <Redirect to="/" />
          }
        /> 
        </SongProvider>
      </Router>
    </div>
  );
}

export default App;
