import React from "react";
import { Link, withRouter } from "react-router-dom";
import { getUser, removeUser } from "../API/userManager";

function Header({ history }) {
  const user = getUser();

  const logout = () => {
    removeUser();
    history.push("/");
  };
  console.log(getUser().id);
  return (
    <nav className="header">
      <ul className="nav-items">
        {user ? (
          <>
            <li className="nav-item">Hello {user.username}</li>
            <li className="nav-item" onClick={logout}>
              Log out
            </li>
          </>
        ) : (
          <>
            <li className="nav-item">
              <Link to="/login">Login</Link>
            </li>
            <li className="nav-item">
              <Link to="/register">Register</Link>
            </li>
            {getUser() ? (
              ""
            ) : (
              <li className="nav-item">
                <Link to="/songs/create">Upload Song</Link>
              </li>
            )}
          </>
        )}
      </ul>
    </nav>
  );
}

export default withRouter(Header);
