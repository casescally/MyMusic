import React, { useContext, useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { UserContext } from "../user/UserProvider";
import "./Songs.css";
//import { getUser } from "../../API/userManager";

export default ({ song }) => {
  return (
    <section className="songSection">
      <div className="songInfo">{song && song.name}</div>
    </section>
  );
};
